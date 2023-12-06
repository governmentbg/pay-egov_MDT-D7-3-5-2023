using System;
using System.Linq;
using System.Web;
using SAML2;
using SAML2.Schema.Protocol;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Net.Http;
using System.Security.Cryptography;

namespace EPayments.Common.Saml
{
    public class SamlHelper
    {
        public static XmlDocument GenerateKEPAuthnRequest(string providerName, string providerId, string returnUri, string targetUri, string extServiceId, string extProviderId, string metadataUri)
        {
            Saml20AuthnRequest req = new Saml20AuthnRequest();

            req.ProtocolBinding = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST";
            req.Request.ProviderName = providerName;
            req.Request.Issuer.SPProvidedID = providerId;
            req.Request.Issuer.Value = metadataUri;
            req.Request.IssueInstant = DateTime.Now;
            req.Request.Destination = targetUri;
            req.Request.ForceAuthn = false;
            req.Request.IsPassive = false;
            req.Request.AssertionConsumerServiceUrl = returnUri;

            XmlElement ext = GetExtensions(extServiceId, extProviderId);
            req.Request.Extensions = new Extensions();
            req.Request.Extensions.Any = new XmlElement[] { ext };
            var namespaces = new XmlSerializerNamespaces(SamlSerialization.XmlNamespaces);
            namespaces.Add("egovbga", "urn:bg:egov:eauth:2.0:saml:ext");
            //namespaces.Add("ds", "http://www.w3.org/2000/09/xmldsig#");
            var resp = SamlSerialization.Serialize(req.Request, namespaces);

            //remove the declaration
            if (resp.FirstChild is XmlDeclaration)
            {
                resp.RemoveChild(resp.FirstChild);
            }
            return resp;
        }

        private static XmlElement GetExtensions(string service, string provider)
        {
            var extensionStr = String.Format(@"<egovbga:RequestedService><egovbga:Service>{0}</egovbga:Service><egovbga:Provider>{1}</egovbga:Provider><egovbga:LevelOfAssurance>SUBSTANTIAL</egovbga:LevelOfAssurance></egovbga:RequestedService>", service, provider);
            var doc = new XmlDocument();
            using (var sr = new StringReader(extensionStr))
            using (var xtr = new XmlTextReader(sr) { Namespaces = false })
            {
                doc.Load(xtr);
            }
            return doc.DocumentElement;
        }

        protected static HttpResponseMessage GetIdpMetadata()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string url = AppSettings.EPaymentsWeb_EAuthIdpMetadata;
            HttpClient client = new HttpClient();
            return client.GetAsync(url).Result;
        }

        public static string SignXmlDocument(XmlDocument xmlDocument, string pathToCertificate, string certPassword)
        {
            if (string.IsNullOrEmpty(pathToCertificate))
            {
                throw new ArgumentNullException("pathToCertificate");
            }

            if (!Path.IsPathRooted(pathToCertificate))
            {
                var dirname = HttpContext.Current.Server.MapPath("~/");
                pathToCertificate = Path.Combine(dirname, pathToCertificate);
            }
            if (!File.Exists(pathToCertificate))
            {
                throw new ArgumentOutOfRangeException("pathToCertificate");
            }

            X509Certificate2 cert;
            try
            {
                byte[] bytes = System.IO.File.ReadAllBytes(pathToCertificate);
#if DEBUG
                cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(bytes, certPassword);
#else
                cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(bytes, certPassword, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
#endif
            }
            catch
            {
                return null;
            }

            //get the reference to be signed
            string reference = xmlDocument.DocumentElement.GetAttribute("ID");
            var signedDocument = SignDocument(xmlDocument, cert, reference);
            var signedDocumentString = SignedDocumentToString(signedDocument);
            bool verified = VerifySignature(signedDocumentString, "Signature");

            if (!verified)
            {
                //ElmahLogger.Instance.Info("Can not verify signed AuthNRequest xml");
            }
            return signedDocumentString;
        }

        private static XmlDocument SignDocument(XmlDocument doc, X509Certificate2 cert, string referenceId)
        {
            SignedXml xml = new SignedXml(doc)
            {
                SignedInfo =
                {
                    CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl,
                    SignatureMethod = SignedXml.XmlDsigRSASHA1Url
                },
                SigningKey = cert.PrivateKey
            };


            Reference reference = new Reference();
            reference.Uri = "#" + referenceId;
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform());
            xml.AddReference(reference);

            xml.KeyInfo = new KeyInfo();
            var keyInfoData = new KeyInfoX509Data(cert);
            keyInfoData.AddIssuerSerial(cert.Issuer, cert.SerialNumber);
            keyInfoData.AddSubjectName(cert.Subject);
            xml.KeyInfo.AddClause(keyInfoData);
            xml.ComputeSignature();

            var signatureXml = xml.GetXml();

            if (doc.DocumentElement != null)
            {
                XmlNodeList elementsByTagName = doc.DocumentElement.GetElementsByTagName("Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
                XmlNode parentNode = elementsByTagName[0].ParentNode;
                if (parentNode != null)
                {
                    parentNode.InsertAfter(doc.ImportNode(signatureXml, true), elementsByTagName[0]);
                }
            }
            return doc;
        }

        private static string SignedDocumentToString(XmlDocument doc)
        {
            // Save the signed XML document to a file specified 
            // using the passed string.
            StringBuilder sb = new StringBuilder();
            using (TextWriter sw = new StringWriter(sb))
            {
                using (XmlTextWriter xmltw = new XmlTextWriter(sw))
                {
                    doc.WriteTo(xmltw);
                }
            }
            return sb.ToString();

        }

        private static Boolean VerifySigningInternal(String xmlString, string signatureTag, bool verifyCertificate, string certificateThumbprint, bool validateCertificateExpirationDate)
        {
            // Create a new XML document.
            XmlDocument xmlDocument = new XmlDocument();

            // Format using white spaces.
            //xmlDocument.PreserveWhitespace = true;

            // Load the passed XML file into the document. 
            xmlDocument.LoadXml(xmlString);

            // Create a new SignedXml object and pass it y
            // the XML document class.
            SignedXml signedXml = new SignedXml(xmlDocument);

            // Find the "Signature" node and create a new 
            // XmlNodeList object.
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName(signatureTag);

            // Load the signature node.
            signedXml.LoadXml((XmlElement)nodeList[0]);

            if (verifyCertificate)
            {
                KeyInfoX509Data x509data = signedXml.Signature.KeyInfo.OfType<KeyInfoX509Data>().First();
                X509Certificate2 signingCertificate = (X509Certificate2)x509data.Certificates[0];

                if (signingCertificate.Thumbprint.ToLower().Trim() != certificateThumbprint.ToLower().Trim())
                {
                    return false;
                }

                if (validateCertificateExpirationDate)
                {
                    if (DateTime.Now < signingCertificate.NotBefore || DateTime.Now > signingCertificate.NotAfter)
                    {
                        return false;
                    }
                }
            }

            return signedXml.CheckSignature();
        }

        public static Boolean VerifySignatureAndCertificate(String xmlString, string signatureTag, string certificateThumbprint, bool validateCertificateExpirationDate)
        {
            return VerifySigningInternal(xmlString, signatureTag, true, certificateThumbprint, validateCertificateExpirationDate);
        }

        public static Boolean VerifySignature(String xmlString, string signatureTag)
        {
            return VerifySigningInternal(xmlString, signatureTag, false, null, false);
        }

        public static EAuthLoginDataDO ParseEAuthResponse(string SamlResponse, string responseSignCertificateThumbprint, bool validateCertificateExpirationDate)
        {         
            var eAuthLoginDataDO = new EAuthLoginDataDO();
            
            if (AppSettings.EPaymentsWeb_EAuthSkipped)
            {
                eAuthLoginDataDO.ResponseStatus = EAuthResponseStatus.Success;
                eAuthLoginDataDO.Egn = "9011118326";
                eAuthLoginDataDO.Name = "Демо";
                return eAuthLoginDataDO;
            }
            
            //ElmahLogger.Instance.Info("Certificate auth response: ", SamlResponse);
            if (string.IsNullOrEmpty(SamlResponse))
            {
                throw new ArgumentNullException("SamlResponse");
            }

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            try
            {
                doc.LoadXml(SamlResponse);
            }
            catch
            {
                //ElmahLogger.Instance.Error(ex, "Can not load samlresponse object!");
                eAuthLoginDataDO.ResponseStatus = EAuthResponseStatus.InvalidResponseXML;
                return eAuthLoginDataDO;
            }

            var pathToCertificate = AppSettings.EPaymentsWeb_EAuthRequestSignCertificatePath;
            if (string.IsNullOrEmpty(pathToCertificate))
            {
                throw new ArgumentNullException("pathToCertificate");
            }

            if (!Path.IsPathRooted(pathToCertificate))
            {
                var dirname = HttpContext.Current.Server.MapPath("~/");
                pathToCertificate = Path.Combine(dirname, pathToCertificate);
            }
            if (!File.Exists(pathToCertificate))
            {
                throw new ArgumentOutOfRangeException("pathToCertificate");
            }

            var encryptedAssertionNote = doc.DocumentElement.GetElementsByTagName("saml2:EncryptedAssertion")[0];
            var doc2 = new XmlDocument();
            doc2.PreserveWhitespace = true;
            doc2.AppendChild(doc2.ImportNode(encryptedAssertionNote, true));

            X509Certificate2 spCert = new X509Certificate2(pathToCertificate, AppSettings.EPaymentsWeb_EAuthRequestSignCertificatePass);
            Saml20EncryptedAssertion encryptedAssertion = new Saml20EncryptedAssertion((RSA)spCert.PrivateKey, doc2);
            encryptedAssertion.Decrypt();

            var metadataResponse = GetIdpMetadata();
            if (!metadataResponse.IsSuccessStatusCode)
            {
                eAuthLoginDataDO.ResponseStatus = EAuthResponseStatus.InvalidMetadata;
                return eAuthLoginDataDO;
            }

            var res = metadataResponse.Content.ReadAsStringAsync().Result;
            XmlDocument xmlMetadataDocument = new XmlDocument();
            xmlMetadataDocument.LoadXml(res);
            var metadataKeyInfo = xmlMetadataDocument.GetElementsByTagName("ds:X509Certificate")[0];
            var IdpCert = new X509Certificate2(Convert.FromBase64String(metadataKeyInfo.InnerXml));

            SignedXml signedXml = new SignedXml(encryptedAssertion.Assertion);

            XmlNodeList nodeList = encryptedAssertion.Assertion.GetElementsByTagName("ds:Signature");
            signedXml.LoadXml((XmlElement)nodeList[0]);

            signedXml.KeyInfo = new KeyInfo();
            var keyInfoData = new KeyInfoX509Data(IdpCert, X509IncludeOption.WholeChain);
            keyInfoData.AddIssuerSerial(IdpCert.Issuer, IdpCert.SerialNumber);
            keyInfoData.AddSubjectName(IdpCert.Subject);
            signedXml.KeyInfo.AddClause(keyInfoData);

            var valid = signedXml.CheckSignature();

            if (!valid)
            {
                eAuthLoginDataDO.ResponseStatus = EAuthResponseStatus.InvalidSignature;
                return eAuthLoginDataDO;
            }
 
            var responseElement = doc.DocumentElement;

            var samlNS = new XmlNamespaceManager(doc.NameTable);
            samlNS.AddNamespace("egovbga", "urn:bg:egov:eauth:2.0:saml:ext");
            samlNS.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            samlNS.AddNamespace("saml2", "urn:oasis:names:tc:SAML:2.0:assertion");
            samlNS.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");
            samlNS.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            var statusCode = responseElement.SelectSingleNode("//samlp:Status/samlp:StatusCode", samlNS);
            if (statusCode != null)
            {
                var statusCodeValue = statusCode.Attributes["Value"].Value;
                var innerStatusCode = statusCode.SelectSingleNode("samlp:StatusCode", samlNS);
                if (innerStatusCode != null)
                {
                    statusCodeValue = innerStatusCode.Attributes["Value"].Value;
                }
                var statusMessage = responseElement.SelectSingleNode("//samlp:Status/samlp:StatusMessage", samlNS);
                eAuthLoginDataDO.ResponseStatusMessage = statusMessage != null ? HttpUtility.HtmlDecode(statusMessage.InnerText) : string.Empty;
                eAuthLoginDataDO.ResponseStatus = GetResponseStatusFromCode(statusCodeValue, eAuthLoginDataDO.ResponseStatusMessage);
            }

            if (eAuthLoginDataDO.ResponseStatus != EAuthResponseStatus.Success)
            {
                return eAuthLoginDataDO;
            }

            var attributes = encryptedAssertion.Assertion.DocumentElement.SelectSingleNode("//saml2:AttributeStatement", samlNS);
            if (attributes != null)
            {
                var latinName = attributes.SelectSingleNode("saml2:Attribute[@Name='urn:egov:bg:eauth:2.0:attributes:personName']/saml2:AttributeValue", samlNS);
                if (latinName != null) eAuthLoginDataDO.Name = latinName.InnerText;

                //get egn
                var egn = attributes.SelectSingleNode("saml2:Attribute[@Name='urn:egov:bg:eauth:2.0:attributes:personIdentifier']/saml2:AttributeValue", samlNS);
                if (egn != null)
                {
                    if (String.IsNullOrEmpty(egn.InnerText.Split('-')[1]))
                    {
                        eAuthLoginDataDO.ResponseStatus = EAuthResponseStatus.MissingEGN;
                        return eAuthLoginDataDO;
                    }
                    eAuthLoginDataDO.Egn = egn.InnerText.Split('-')[1];
                }
            }
            return eAuthLoginDataDO;
        }

        private static EAuthResponseStatus GetResponseStatusFromCode(string statusCode, string statusMessage)
        {
            switch (statusCode)
            {
                case "urn:oasis:names:tc:SAML:2.0:status:AuthnFailed":
                    if (statusMessage.Trim().ToLower() == "отказан от потребител")
                        return EAuthResponseStatus.CanceledByUser;
                    else if (statusMessage.Trim().ToLower() == "not_detected_qes")
                        return EAuthResponseStatus.NotDetectedQES;
                    else
                        return EAuthResponseStatus.AuthenticationFailed;
                case "urn:oasis:names:tc:SAML:2.0:status:Success":
                    return EAuthResponseStatus.Success;
            }

            return EAuthResponseStatus.AuthenticationFailed;
        }

        public static string GenerateSaml2ArtifactResolve(string artifactStr)
        {
            var resolve = new SAML2.Saml20ArtifactResolve();
            resolve.Artifact = artifactStr;

            //get the element
            var samlElement = resolve.Resolve;
            //get xml document
            var xml = GetXmlDocumentFromSaml(samlElement);
            var artifactResolveString = xml.DocumentElement.OuterXml;
            //return soap packed message
            return WrapInSoapMessage(artifactResolveString);
        }

        public static XmlDocument GetXmlDocumentFromSaml<T>(T samlObj)
        {
            XmlDocument document = new XmlDocument
            {
                PreserveWhitespace = true
            };
            document.LoadXml(SamlSerialization.SerializeToXmlString<T>(samlObj));
            return document;
        }

        public static string WrapInSoapMessage(string body)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            builder.AppendLine("<SOAP-ENV:Body>");
            builder.AppendLine(body);
            builder.AppendLine("</SOAP-ENV:Body>");
            builder.AppendLine("</SOAP-ENV:Envelope>");
            return builder.ToString();
        }

        public static string SendWebRequest(string message, string URL)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/soap+xml; charset=utf-8");
                client.Headers.Add("SOAPAction", URL);
                var result = client.UploadString(URL, message);
                return result;
            }
        }

        public static DcElectronicIdentityInfo ParseSaml2Result(string result, out Status status)
        {
            var soapdoc = new XmlDocument();
            soapdoc.LoadXml(result);
            var soapNS = new XmlNamespaceManager(soapdoc.NameTable);
            soapNS.AddNamespace("soapenv", soapdoc.DocumentElement.GetNamespaceOfPrefix("soapenv"));

            var samlElement = soapdoc.DocumentElement.SelectSingleNode("//soapenv:Body", soapNS).FirstChild;
            var samlNS = new XmlNamespaceManager(soapdoc.NameTable);
            samlNS.AddNamespace("pr", "urn:person:eid.egov.bg");
            samlNS.AddNamespace("saml2", "urn:oasis:names:tc:SAML:2.0:assertion");
            samlNS.AddNamespace("saml2p", "urn:oasis:names:tc:SAML:2.0:protocol");
            var statusCode = samlElement.SelectSingleNode("//saml2p:Status/saml2p:StatusCode/@Value", samlNS).Value;
            var statusMessage = HttpUtility.HtmlDecode(samlElement.SelectSingleNode("//saml2p:Status/saml2p:StatusMessage", samlNS).InnerText);
            status = new Status() { StatusCode = new StatusCode() { Value = statusCode }, StatusMessage = statusMessage };

            if (statusCode == SAML2.Saml20Constants.StatusCodes.Success)
            {
                var personData = new DcElectronicIdentityInfo();
                personData.IsValid = true;
                var personNode = samlElement.SelectSingleNode("//pr:Person", samlNS);
                if (personNode == null) return null;

                personData.EGN = personNode.SelectSingleNode("//pr:Egn", samlNS).InnerText;
                personData.GivenName = personNode.SelectSingleNode("//pr:Name/pr:GivenName", samlNS).InnerText;
                personData.MiddleName = personNode.SelectSingleNode("//pr:Name/pr:MiddleName", samlNS).InnerText;
                personData.FamilyName = personNode.SelectSingleNode("//pr:Name/pr:FamilyName", samlNS).InnerText;
                personData.GivenNameLat = personNode.SelectSingleNode("//pr:NameLat/pr:GivenName", samlNS).InnerText;
                personData.MiddleNameLat = personNode.SelectSingleNode("//pr:NameLat/pr:MiddleName", samlNS).InnerText;
                personData.FamilyNameLat = personNode.SelectSingleNode("//pr:NameLat/pr:FamilyName", samlNS).InnerText;

                //personData.Spin = personNode.SelectSingleNode("//pr:Spin", samlNS).InnerText;
                //var birthDate = personNode.SelectSingleNode("//pr:DateOfBirth", samlNS);
                //DateTime date;
                //if (birthDate != null && DateTime.TryParse(birthDate.InnerText, out date))
                //{
                //    personData.DateOfBirth = date;
                //}
                //else
                //{
                //    personData.DateOfBirth = EgnHelper.GetBirthDateFromEgn(personData.EGN);
                //}

                //var phoneElement = samlElement.SelectNodes("//saml2:Attribute[@Name='phoneNumber']", samlNS);
                //if (phoneElement != null && phoneElement.Count > 0)
                //{
                //    personData.PhoneNumber = phoneElement[0].FirstChild.InnerText;
                //}
                //var addressElement = samlElement.SelectNodes("//saml2:Attribute[@Name='address']", samlNS);
                //if (addressElement != null && addressElement.Count > 0)
                //{
                //    personData.Address = addressElement[0].FirstChild.InnerText;
                //}

                return personData;
            }
            return null;
        }

        public static readonly string eIDSuccessStatus = "urn:oasis:names:tc:SAML:2.0:status:Success";

    }
}
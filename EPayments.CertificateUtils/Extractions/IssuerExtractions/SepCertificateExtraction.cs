﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EPayments.CertificateUtils.Extractions.IssuerExtractions
{
    internal class SepCertificateExtraction : CertificateExtraction
    {
        private const string IssuerName = "SEP Bulgaria";

        public override bool MatchCertificateType(X509Certificate2 x509certificate)
        {
            return x509certificate.Issuer.Contains(IssuerName);
        }

        protected override bool IsPersonal(X509Certificate2 x509certificate)
        {
            return
                this.MatchRegexInCertificatePolicies(x509certificate, this.IsPersonalRegex1) ||
                this.MatchRegexInCertificatePolicies(x509certificate, this.IsPersonalRegex2) ||
                this.MatchRegexInCertificatePolicies(x509certificate, this.IsPersonalRegex3);
        }

        protected override bool IsCompany(X509Certificate2 x509certificate)
        {
            return this.MatchRegexInCertificatePolicies(x509certificate, this.IsCompanyRegex);
        }

        protected override string GetPersonalIdentifier(X509Certificate2 x509certificate)
        {
            if (this.EgnRegex1.IsMatch(x509certificate.Subject))
            {
                Match match = this.EgnRegex1.Match(x509certificate.Subject);
                return match.Groups["egn"].Value.Trim();
            }
            else if (this.EgnRegex2.IsMatch(x509certificate.Subject))
            {
                Match match = this.EgnRegex2.Match(x509certificate.Subject);
                return match.Groups["egn"].Value.Trim();
            }
            else
            {
                return String.Empty;
            }
        }

        protected override string GetName(X509Certificate2 x509certificate)
        {
            if (this.NameRegex.IsMatch(x509certificate.Subject))
            {
                Match match = this.NameRegex.Match(x509certificate.Subject);
                return match.Groups["name"].Value.Trim();
            }
            else
            {
                return String.Empty;
            }
        }

        protected override List<string> GetPolicyOidsForPersonalKUKEP()
        {
            return new List<string>
            {
                @"1.3.6.1.4.1.30299.3.1.3"
            };
        }

        protected override List<string> GetPolicyOidsForCompanyKUKEP()
        {
            return new List<string>
            {
                @"1.3.6.1.4.1.30299.3.1.4"
            };
        }

        #region Regex expressions

        private readonly Regex NameRegex = new Regex(@"CN=(?<name>.*?),", RegexOptions.Compiled);

        private readonly Regex EgnRegex1 = new Regex(@"UID=EGN(?<egn>\d+)", RegexOptions.Compiled);
        private readonly Regex EgnRegex2 = new Regex(@"OID.0.9.2342.19200300.100.1.1=EGN(?<egn>\d+)", RegexOptions.Compiled);

        private readonly Regex IsPersonalRegex1 = new Regex(@"1.3.6.1.4.1.30299.2.1.1", RegexOptions.Compiled);
        private readonly Regex IsPersonalRegex2 = new Regex(@"1.3.6.1.4.1.30299.2.1.3", RegexOptions.Compiled);
        private readonly Regex IsPersonalRegex3 = new Regex(@"1.3.6.1.4.1.30299.2.5.1", RegexOptions.Compiled);

        private readonly Regex IsCompanyRegex = new Regex(@"1.3.6.1.4.1.30299.2.1.2", RegexOptions.Compiled);

        #endregion
    }
}

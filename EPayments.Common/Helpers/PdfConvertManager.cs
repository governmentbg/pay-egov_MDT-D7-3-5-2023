using iTextSharp.text.pdf;
using SautinSoft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPayments.Common.Helpers
{
    public class PdfConvertManager
    {
        static readonly string PDF_METAMORPHOSIS_SERIAL = "10024747414";
        static readonly string PDF_SECURITY_PASSWORD = "epayments@abbaty@encryption";

        static readonly object rtfLocker = new object();
        static readonly object txtLocker = new object();
        static readonly object htmlLocker = new object();
        static readonly object docxLocker = new object();

        public static byte[] Convert(byte[] input, ref string mimeType)
        {
            byte[] pdf;
            switch (mimeType)
            {
                case MimeTypeFileExtension.MIME_TEXT_RTF:
                    {
                        pdf = ConvertRtfToPdf(input);
                        if (pdf != null)
                            mimeType = MimeTypeFileExtension.MIME_APPLICATION_PDF;
                        break;
                    }
                case MimeTypeFileExtension.MIME_TEXT_PLAIN:
                    {
                        pdf = ConvertTxtToPdf(input);
                        if (pdf != null)
                            mimeType = MimeTypeFileExtension.MIME_APPLICATION_PDF;
                        break;
                    }
                case MimeTypeFileExtension.MIME_TEXT_HTML:
                    {
                        pdf = ConvertHtmlToPdf(input);
                        if (pdf != null)
                            mimeType = MimeTypeFileExtension.MIME_APPLICATION_PDF;
                        break;
                    }
                default:
                    {
                        return input;
                    }
            }

            return EncryptPdf(pdf);
        }

        private static byte[] ConvertHtmlToPdf(byte[] input)
        {
            try
            {
                lock (htmlLocker)
                {
                    PdfMetamorphosis pdfConverter = new PdfMetamorphosis();
                    pdfConverter.SetSerial(PDF_METAMORPHOSIS_SERIAL);

                    pdfConverter.UnicodeOptions.FontsDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts);

                    byte[] pdf = pdfConverter.HtmlToPdfConvertStringToByte(UTF8Encoding.UTF8.GetString(input));

                    return pdf;
                }
            }
            catch
            {
                return null;
            }
        }

        private static byte[] ConvertTxtToPdf(byte[] input)
        {
            try
            {
                lock (txtLocker)
                {
                    PdfMetamorphosis pdfConverter = new PdfMetamorphosis();
                    pdfConverter.SetSerial(PDF_METAMORPHOSIS_SERIAL);
                    byte[] pdf = pdfConverter.RtfToPdfConvertByte(input);

                    return pdf;
                }
            }
            catch
            {
                return null;
            }
        }

        private static byte[] ConvertRtfToPdf(byte[] input)
        {
            try
            {
                lock (rtfLocker)
                {
                    MemoryStream rtfMemoryStream = new MemoryStream(input);
                    MemoryStream pdfMemoryStream = new MemoryStream();

                    PdfMetamorphosis pdfConverter = new PdfMetamorphosis();
                    pdfConverter.SetSerial(PDF_METAMORPHOSIS_SERIAL);
                    RichTextBox rtfParser = new RichTextBox();

                    rtfParser.LoadFile(rtfMemoryStream, RichTextBoxStreamType.RichText);
                    rtfParser.SaveFile(pdfMemoryStream, RichTextBoxStreamType.RichText);

                    byte[] rtf = pdfMemoryStream.ToArray();
                    byte[] pdf = pdfConverter.RtfToPdfConvertByte(rtf);

                    return pdf;
                }
            }
            catch
            {
                return null;
            }
        }

        private static byte[] EncryptPdf(byte[] input)
        {
            using (MemoryStream inputStream = new MemoryStream(input))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    try
                    {
                        PdfReader reader = new PdfReader(inputStream);
                        PdfEncryptor.Encrypt(reader, outputStream, true, null, PDF_SECURITY_PASSWORD,
                            PdfWriter.ALLOW_PRINTING &
                            ~PdfWriter.ALLOW_COPY &
                            ~PdfWriter.ALLOW_ASSEMBLY &
                            PdfWriter.ALLOW_DEGRADED_PRINTING &
                            ~PdfWriter.ALLOW_FILL_IN &
                            ~PdfWriter.ALLOW_MODIFY_ANNOTATIONS &
                            ~PdfWriter.ALLOW_MODIFY_CONTENTS &
                            ~PdfWriter.ALLOW_SCREENREADERS);

                        return outputStream.ToArray();
                    }
                    catch
                    {
                        return input;
                    }
                }
            }
        }
    }
}

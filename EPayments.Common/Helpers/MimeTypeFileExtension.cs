﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPayments.Common.Helpers
{
    public class MimeTypeFileExtension
    {
        public static string GetFileExtenstionByMimeType(string mimeType)
        {
            Dictionary<string, string> mimeTypes =
                new Dictionary<string, string>();

            InitializeMimeTypeFileExtensionDictionary(mimeTypes);

            if (!mimeTypes.ContainsValue(mimeType))
                return string.Empty;

            return mimeTypes.FirstOrDefault(e => e.Value == mimeType).Key;
        }

        private static void InitializeMimeTypeFileExtensionDictionary(Dictionary<string, string> mimeTypes)
        {
            mimeTypes.Add(".xul", MIME_APPLICATION_VND_MOZZILLA_XUL_XML);
            mimeTypes.Add(".json", MIME_APPLICATION_JSON);
            mimeTypes.Add(".ice", MIME_X_CONFERENCE_X_COOLTALK);
            mimeTypes.Add(".movie", MIME_VIDEO_X_SGI_MOVIE);
            mimeTypes.Add(".avi", MIME_VIDEO_X_MSVIDEO);
            mimeTypes.Add(".wmv", MIME_VIDEO_X_MS_WMV);
            mimeTypes.Add(".m4u", MIME_VIDEO_VND_MPEGURL);
            mimeTypes.Add(".mxu", MIME_VIDEO_VND_MPEGURL);
            mimeTypes.Add(".htc", MIME_TEXT_X_COMPONENT);
            mimeTypes.Add(".etx", MIME_TEXT_X_SETEXT);
            mimeTypes.Add(".wmls", MIME_TEXT_VND_WAP_WMLSCRIPT);
            mimeTypes.Add(".wml", MIME_TEXT_VND_WAP_XML);
            mimeTypes.Add(".tsv", MIME_TEXT_TAB_SEPARATED_VALUES);
            mimeTypes.Add(".sgm", MIME_TEXT_SGML);
            mimeTypes.Add(".sgml", MIME_TEXT_SGML);
            mimeTypes.Add(".css", MIME_TEXT_CSS);
            mimeTypes.Add(".ifb", MIME_TEXT_CALENDAR);
            mimeTypes.Add(".ics", MIME_TEXT_CALENDAR);
            mimeTypes.Add(".wrl", MIME_MODEL_VRLM);
            mimeTypes.Add(".vrlm", MIME_MODEL_VRLM);
            mimeTypes.Add(".silo", MIME_MODEL_MESH);
            mimeTypes.Add(".mesh", MIME_MODEL_MESH);
            mimeTypes.Add(".msh", MIME_MODEL_MESH);
            mimeTypes.Add(".iges", MIME_MODEL_IGES);
            mimeTypes.Add(".igs", MIME_MODEL_IGES);
            mimeTypes.Add(".rgb", MIME_IMAGE_X_RGB);
            mimeTypes.Add(".ppm", MIME_IMAGE_X_PORTABLE_PIXMAP);
            mimeTypes.Add(".pgm", MIME_IMAGE_X_PORTABLE_GRAYMAP);
            mimeTypes.Add(".pbm", MIME_IMAGE_X_PORTABLE_BITMAP);
            mimeTypes.Add(".pnm", MIME_IMAGE_X_PORTABLE_ANYMAP);
            mimeTypes.Add(".ico", MIME_IMAGE_X_ICON);
            mimeTypes.Add(".ras", MIME_IMAGE_X_CMU_RASTER);
            mimeTypes.Add(".wbmp", MIME_IMAGE_WAP_WBMP);
            mimeTypes.Add(".djv", MIME_IMAGE_VND_DJVU);
            mimeTypes.Add(".djvu", MIME_IMAGE_VND_DJVU);
            mimeTypes.Add(".svg", MIME_IMAGE_SVG_XML);
            mimeTypes.Add(".ief", MIME_IMAGE_IEF);
            mimeTypes.Add(".cgm", MIME_IMAGE_CGM);
            mimeTypes.Add(".bmp", MIME_IMAGE_BMP);
            mimeTypes.Add(".xyz", MIME_CHEMICAL_X_XYZ);
            mimeTypes.Add(".pdb", MIME_CHEMICAL_X_PDB);
            mimeTypes.Add(".ra", MIME_AUDIO_X_PN_REALAUDIO);
            mimeTypes.Add(".ram", MIME_AUDIO_X_PN_REALAUDIO);
            mimeTypes.Add(".m3u", MIME_AUDIO_X_MPEGURL);
            mimeTypes.Add(".aifc", MIME_AUDIO_X_AIFF);
            mimeTypes.Add(".aif", MIME_AUDIO_X_AIFF);
            mimeTypes.Add(".aiff", MIME_AUDIO_X_AIFF);
            mimeTypes.Add(".mp3", MIME_AUDIO_MPEG);
            mimeTypes.Add(".mp2", MIME_AUDIO_MPEG);
            mimeTypes.Add(".mp1", MIME_AUDIO_MPEG);
            mimeTypes.Add(".mpga", MIME_AUDIO_MPEG);
            mimeTypes.Add(".kar", MIME_AUDIO_MIDI);
            mimeTypes.Add(".mid", MIME_AUDIO_MIDI);
            mimeTypes.Add(".midi", MIME_AUDIO_MIDI);
            mimeTypes.Add(".dtd", MIME_APPLICATION_XML_DTD);
            mimeTypes.Add(".xsl", MIME_APPLICATION_XML);
            mimeTypes.Add(".xml", MIME_APPLICATION_XML);
            mimeTypes.Add(".xslt", MIME_APPLICATION_XSLT_XML);
            mimeTypes.Add(".xht", MIME_APPLICATION_XHTML_XML);
            mimeTypes.Add(".xhtml", MIME_APPLICATION_XHTML_XML);
            mimeTypes.Add(".src", MIME_APPLICATION_X_WAIS_SOURCE);
            mimeTypes.Add(".ustar", MIME_APPLICATION_X_USTAR);
            mimeTypes.Add(".ms", MIME_APPLICATION_X_TROFF_MS);
            mimeTypes.Add(".me", MIME_APPLICATION_X_TROFF_ME);
            mimeTypes.Add(".man", MIME_APPLICATION_X_TROFF_MAN);
            mimeTypes.Add(".roff", MIME_APPLICATION_X_TROFF);
            mimeTypes.Add(".tr", MIME_APPLICATION_X_TROFF);
            mimeTypes.Add(".t", MIME_APPLICATION_X_TROFF);
            mimeTypes.Add(".texi", MIME_APPLICATION_X_TEXINFO);
            mimeTypes.Add(".texinfo", MIME_APPLICATION_X_TEXINFO);
            mimeTypes.Add(".tex", MIME_APPLICATION_X_TEX);
            mimeTypes.Add(".tcl", MIME_APPLICATION_X_TCL);
            mimeTypes.Add(".sv4crc", MIME_APPLICATION_X_SV4CRC);
            mimeTypes.Add(".sv4cpio", MIME_APPLICATION_X_SV4CPIO);
            mimeTypes.Add(".sit", MIME_APPLICATION_X_STUFFIT);
            mimeTypes.Add(".swf", MIME_APPLICATION_X_SHOCKWAVE_FLASH);
            mimeTypes.Add(".shar", MIME_APPLICATION_X_SHAR);
            mimeTypes.Add(".sh", MIME_APPLICATION_X_SH);
            mimeTypes.Add(".cdf", MIME_APPLICATION_X_NETCDF);
            mimeTypes.Add(".nc", MIME_APPLICATION_X_NETCDF);
            mimeTypes.Add(".latex", MIME_APPLICATION_X_LATEX);
            mimeTypes.Add(".skm", MIME_APPLICATION_X_KOAN);
            mimeTypes.Add(".skt", MIME_APPLICATION_X_KOAN);
            mimeTypes.Add(".skd", MIME_APPLICATION_X_KOAN);
            mimeTypes.Add(".skp", MIME_APPLICATION_X_KOAN);
            mimeTypes.Add(".js", MIME_APPLICATION_X_JAVASCRIPT);
            mimeTypes.Add(".hdf", MIME_APPLICATION_X_HDF);
            mimeTypes.Add(".gtar", MIME_APPLICATION_X_GTAR);
            mimeTypes.Add(".spl", MIME_APPLICATION_X_FUTURESPLASH);
            mimeTypes.Add(".dvi", MIME_APPLICATION_X_DVI);
            mimeTypes.Add(".dxr", MIME_APPLICATION_X_DIRECTOR);
            mimeTypes.Add(".dir", MIME_APPLICATION_X_DIRECTOR);
            mimeTypes.Add(".dcr", MIME_APPLICATION_X_DIRECTOR);
            mimeTypes.Add(".csh", MIME_APPLICATION_X_CSH);
            mimeTypes.Add(".cpio", MIME_APPLICATION_X_CPIO);
            mimeTypes.Add(".pgn", MIME_APPLICATION_X_CHESS_PGN);
            mimeTypes.Add(".vcd", MIME_APPLICATION_X_CDLINK);
            mimeTypes.Add(".bcpio", MIME_APPLICATION_X_BCPIO);
            mimeTypes.Add(".rm", MIME_APPLICATION_VND_RNREALMEDIA);
            mimeTypes.Add(".ppt", MIME_APPLICATION_VND_MSPOWERPOINT);
            mimeTypes.Add(".mif", MIME_APPLICATION_VND_MIF);
            mimeTypes.Add(".grxml", MIME_APPLICATION_SRGS_XML);
            mimeTypes.Add(".gram", MIME_APPLICATION_SRGS);
            mimeTypes.Add(".smil", MIME_APPLICATION_RDF_SMIL);
            mimeTypes.Add(".smi", MIME_APPLICATION_RDF_SMIL);
            mimeTypes.Add(".rdf", MIME_APPLICATION_RDF_XML);
            mimeTypes.Add(".ogg", MIME_APPLICATION_X_OGG);
            mimeTypes.Add(".oda", MIME_APPLICATION_ODA);
            mimeTypes.Add(".dmg", MIME_APPLICATION_OCTET_STREAM);
            mimeTypes.Add(".lzh", MIME_APPLICATION_OCTET_STREAM);
            mimeTypes.Add(".so", MIME_APPLICATION_OCTET_STREAM);
            mimeTypes.Add(".lha", MIME_APPLICATION_OCTET_STREAM);
            mimeTypes.Add(".dms", MIME_APPLICATION_OCTET_STREAM);
            mimeTypes.Add(".bin", MIME_APPLICATION_OCTET_STREAM);
            mimeTypes.Add(".mathml", MIME_APPLICATION_MATHML_XML);
            mimeTypes.Add(".cpt", MIME_APPLICATION_MAC_COMPACTPRO);
            mimeTypes.Add(".hqx", MIME_APPLICATION_MAC_BINHEX40);
            mimeTypes.Add(".jnlp", MIME_APPLICATION_JNLP);
            mimeTypes.Add(".ez", MIME_APPLICATION_ANDREW_INSET);
            mimeTypes.Add(".txt", MIME_TEXT_PLAIN);
            mimeTypes.Add(".ini", MIME_TEXT_PLAIN);
            mimeTypes.Add(".c", MIME_TEXT_PLAIN);
            mimeTypes.Add(".h", MIME_TEXT_PLAIN);
            mimeTypes.Add(".cpp", MIME_TEXT_PLAIN);
            mimeTypes.Add(".cxx", MIME_TEXT_PLAIN);
            mimeTypes.Add(".cc", MIME_TEXT_PLAIN);
            mimeTypes.Add(".chh", MIME_TEXT_PLAIN);
            mimeTypes.Add(".java", MIME_TEXT_PLAIN);
            mimeTypes.Add(".csv", MIME_TEXT_PLAIN);
            mimeTypes.Add(".bat", MIME_TEXT_PLAIN);
            mimeTypes.Add(".cmd", MIME_TEXT_PLAIN);
            mimeTypes.Add(".asc", MIME_TEXT_PLAIN);
            mimeTypes.Add(".rtf", MIME_TEXT_RTF);
            mimeTypes.Add(".rtx", MIME_TEXT_RICHTEXT);
            mimeTypes.Add(".html", MIME_TEXT_HTML);
            mimeTypes.Add(".htm", MIME_TEXT_HTML);
            mimeTypes.Add(".zip", MIME_APPLICATION_ZIP);
            mimeTypes.Add(".rar", MIME_APPLICATION_X_RAR_COMPRESSED);
            mimeTypes.Add(".gzip", MIME_APPLICATION_X_GZIP);
            mimeTypes.Add(".gz", MIME_APPLICATION_X_GZIP);
            mimeTypes.Add(".tgz", MIME_APPLICATION_TGZ);
            mimeTypes.Add(".tar", MIME_APPLICATION_X_TAR);
            mimeTypes.Add(".gif", MIME_IMAGE_GIF);
            mimeTypes.Add(".jpeg", MIME_IMAGE_JPEG);
            mimeTypes.Add(".jpg", MIME_IMAGE_JPEG);
            mimeTypes.Add(".jpe", MIME_IMAGE_JPEG);
            mimeTypes.Add(".tiff", MIME_IMAGE_TIFF);
            mimeTypes.Add(".tif", MIME_IMAGE_TIFF);
            mimeTypes.Add(".png", MIME_IMAGE_PNG);
            mimeTypes.Add(".au", MIME_AUDIO_BASIC);
            mimeTypes.Add(".snd", MIME_AUDIO_BASIC);
            mimeTypes.Add(".wav", MIME_AUDIO_X_WAV);
            mimeTypes.Add(".mov", MIME_VIDEO_QUICKTIME);
            mimeTypes.Add(".qt", MIME_VIDEO_QUICKTIME);
            mimeTypes.Add(".mpeg", MIME_VIDEO_MPEG);
            mimeTypes.Add(".mpg", MIME_VIDEO_MPEG);
            mimeTypes.Add(".mpe", MIME_VIDEO_MPEG);
            mimeTypes.Add(".abs", MIME_VIDEO_MPEG);
            mimeTypes.Add(".doc", MIME_APPLICATION_MSWORD);
            mimeTypes.Add(".xls", MIME_APPLICATION_VND_MSEXCEL);
            mimeTypes.Add(".eps", MIME_APPLICATION_POSTSCRIPT);
            mimeTypes.Add(".ai", MIME_APPLICATION_POSTSCRIPT);
            mimeTypes.Add(".ps", MIME_APPLICATION_POSTSCRIPT);
            mimeTypes.Add(".pdf", MIME_APPLICATION_PDF);
            mimeTypes.Add(".exe", MIME_APPLICATION_OCTET_STREAM);
            mimeTypes.Add(".dll", MIME_APPLICATION_OCTET_STREAM);
            mimeTypes.Add(".class", MIME_APPLICATION_OCTET_STREAM);
            mimeTypes.Add(".jar", MIME_APPLICATION_JAVA_ARCHIVE);
        }

        public const string MIME_APPLICATION_ANDREW_INSET = "application/andrew-inset";
        public const string MIME_APPLICATION_JSON = "application/json";
        public const string MIME_APPLICATION_ZIP = "application/zip";
        public const string MIME_APPLICATION_X_GZIP = "application/x-gzip";
        public const string MIME_APPLICATION_TGZ = "application/tgz";
        public const string MIME_APPLICATION_MSWORD = "application/msword";
        public const string MIME_APPLICATION_POSTSCRIPT = "application/postscript";
        public const string MIME_APPLICATION_PDF = "application/pdf";
        public const string MIME_APPLICATION_JNLP = "application/jnlp";
        public const string MIME_APPLICATION_MAC_BINHEX40 = "application/mac-binhex40";
        public const string MIME_APPLICATION_MAC_COMPACTPRO = "application/mac-compactpro";
        public const string MIME_APPLICATION_MATHML_XML = "application/mathml+xml";
        public const string MIME_APPLICATION_OCTET_STREAM = "application/octet-stream";
        public const string MIME_APPLICATION_ODA = "application/oda";
        public const string MIME_APPLICATION_RDF_XML = "application/rdf+xml";
        public const string MIME_APPLICATION_JAVA_ARCHIVE = "application/java-archive";
        public const string MIME_APPLICATION_RDF_SMIL = "application/smil";
        public const string MIME_APPLICATION_SRGS = "application/srgs";
        public const string MIME_APPLICATION_SRGS_XML = "application/srgs+xml";
        public const string MIME_APPLICATION_VND_MIF = "application/vnd.mif";
        public const string MIME_APPLICATION_VND_MSEXCEL = "application/vnd.ms-excel";
        public const string MIME_APPLICATION_VND_MSPOWERPOINT = "application/vnd.ms-powerpoint";
        public const string MIME_APPLICATION_VND_RNREALMEDIA = "application/vnd.rn-realmedia";
        public const string MIME_APPLICATION_X_BCPIO = "application/x-bcpio";
        public const string MIME_APPLICATION_X_CDLINK = "application/x-cdlink";
        public const string MIME_APPLICATION_X_CHESS_PGN = "application/x-chess-pgn";
        public const string MIME_APPLICATION_X_CPIO = "application/x-cpio";
        public const string MIME_APPLICATION_X_CSH = "application/x-csh";
        public const string MIME_APPLICATION_X_DIRECTOR = "application/x-director";
        public const string MIME_APPLICATION_X_DVI = "application/x-dvi";
        public const string MIME_APPLICATION_X_FUTURESPLASH = "application/x-futuresplash";
        public const string MIME_APPLICATION_X_GTAR = "application/x-gtar";
        public const string MIME_APPLICATION_X_HDF = "application/x-hdf";
        public const string MIME_APPLICATION_X_JAVASCRIPT = "application/x-javascript";
        public const string MIME_APPLICATION_X_KOAN = "application/x-koan";
        public const string MIME_APPLICATION_X_LATEX = "application/x-latex";
        public const string MIME_APPLICATION_X_NETCDF = "application/x-netcdf";
        public const string MIME_APPLICATION_X_OGG = "application/x-ogg";
        public const string MIME_APPLICATION_X_SH = "application/x-sh";
        public const string MIME_APPLICATION_X_SHAR = "application/x-shar";
        public const string MIME_APPLICATION_X_SHOCKWAVE_FLASH = "application/x-shockwave-flash";
        public const string MIME_APPLICATION_X_STUFFIT = "application/x-stuffit";
        public const string MIME_APPLICATION_X_SV4CPIO = "application/x-sv4cpio";
        public const string MIME_APPLICATION_X_SV4CRC = "application/x-sv4crc";
        public const string MIME_APPLICATION_X_TAR = "application/x-tar";
        public const string MIME_APPLICATION_X_RAR_COMPRESSED = "application/x-rar-compressed";
        public const string MIME_APPLICATION_X_TCL = "application/x-tcl";
        public const string MIME_APPLICATION_X_TEX = "application/x-tex";
        public const string MIME_APPLICATION_X_TEXINFO = "application/x-texinfo";
        public const string MIME_APPLICATION_X_TROFF = "application/x-troff";
        public const string MIME_APPLICATION_X_TROFF_MAN = "application/x-troff-man";
        public const string MIME_APPLICATION_X_TROFF_ME = "application/x-troff-me";
        public const string MIME_APPLICATION_X_TROFF_MS = "application/x-troff-ms";
        public const string MIME_APPLICATION_X_USTAR = "application/x-ustar";
        public const string MIME_APPLICATION_X_WAIS_SOURCE = "application/x-wais-source";
        public const string MIME_APPLICATION_VND_MOZZILLA_XUL_XML = "application/vnd.mozilla.xul+xml";
        public const string MIME_APPLICATION_XHTML_XML = "application/xhtml+xml";
        public const string MIME_APPLICATION_XSLT_XML = "application/xslt+xml";
        public const string MIME_APPLICATION_XML = "application/xml";
        public const string MIME_APPLICATION_XML_DTD = "application/xml-dtd";
        public const string MIME_IMAGE_BMP = "image/bmp";
        public const string MIME_IMAGE_CGM = "image/cgm";
        public const string MIME_IMAGE_GIF = "image/gif";
        public const string MIME_IMAGE_IEF = "image/ief";
        public const string MIME_IMAGE_JPEG = "image/jpeg";
        public const string MIME_IMAGE_TIFF = "image/tiff";
        public const string MIME_IMAGE_PNG = "image/png";
        public const string MIME_IMAGE_SVG_XML = "image/svg+xml";
        public const string MIME_IMAGE_VND_DJVU = "image/vnd.djvu";
        public const string MIME_IMAGE_WAP_WBMP = "image/vnd.wap.wbmp";
        public const string MIME_IMAGE_X_CMU_RASTER = "image/x-cmu-raster";
        public const string MIME_IMAGE_X_ICON = "image/x-icon";
        public const string MIME_IMAGE_X_PORTABLE_ANYMAP = "image/x-portable-anymap";
        public const string MIME_IMAGE_X_PORTABLE_BITMAP = "image/x-portable-bitmap";
        public const string MIME_IMAGE_X_PORTABLE_GRAYMAP = "image/x-portable-graymap";
        public const string MIME_IMAGE_X_PORTABLE_PIXMAP = "image/x-portable-pixmap";
        public const string MIME_IMAGE_X_RGB = "image/x-rgb";
        public const string MIME_AUDIO_BASIC = "audio/basic";
        public const string MIME_AUDIO_MIDI = "audio/midi";
        public const string MIME_AUDIO_MPEG = "audio/mpeg";
        public const string MIME_AUDIO_X_AIFF = "audio/x-aiff";
        public const string MIME_AUDIO_X_MPEGURL = "audio/x-mpegurl";
        public const string MIME_AUDIO_X_PN_REALAUDIO = "audio/x-pn-realaudio";
        public const string MIME_AUDIO_X_WAV = "audio/x-wav";
        public const string MIME_CHEMICAL_X_PDB = "chemical/x-pdb";
        public const string MIME_CHEMICAL_X_XYZ = "chemical/x-xyz";
        public const string MIME_MODEL_IGES = "model/iges";
        public const string MIME_MODEL_MESH = "model/mesh";
        public const string MIME_MODEL_VRLM = "model/vrml";
        public const string MIME_TEXT_PLAIN = "text/plain";
        public const string MIME_TEXT_RICHTEXT = "text/richtext";
        public const string MIME_TEXT_RTF = "text/rtf";
        public const string MIME_TEXT_HTML = "text/html";
        public const string MIME_TEXT_CALENDAR = "text/calendar";
        public const string MIME_TEXT_CSS = "text/css";
        public const string MIME_TEXT_SGML = "text/sgml";
        public const string MIME_TEXT_TAB_SEPARATED_VALUES = "text/tab-separated-values";
        public const string MIME_TEXT_VND_WAP_XML = "text/vnd.wap.wml";
        public const string MIME_TEXT_VND_WAP_WMLSCRIPT = "text/vnd.wap.wmlscript";
        public const string MIME_TEXT_X_SETEXT = "text/x-setext";
        public const string MIME_TEXT_X_COMPONENT = "text/x-component";
        public const string MIME_VIDEO_QUICKTIME = "video/quicktime";
        public const string MIME_VIDEO_MPEG = "video/mpeg";
        public const string MIME_VIDEO_VND_MPEGURL = "video/vnd.mpegurl";
        public const string MIME_VIDEO_X_MSVIDEO = "video/x-msvideo";
        public const string MIME_VIDEO_X_MS_WMV = "video/x-ms-wmv";
        public const string MIME_VIDEO_X_SGI_MOVIE = "video/x-sgi-movie";
        public const string MIME_X_CONFERENCE_X_COOLTALK = "x-conference/x-cooltalk";
    }
}

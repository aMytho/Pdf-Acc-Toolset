namespace Pdf_Acc_Toolset.Services.Pdf
{
    public static class PdfMetadata
    {
        /// <summary>
        /// Returns the title of the PDF.
        /// </summary>
        /// <returns></returns>
        public static string GetTitle()
        {
            try {
                return PdfManager.GetDocument().GetPdfDocument().GetDocumentInfo().GetTitle();
            } catch(Exception) {
                return null;
            }
        }

        /// <summary>
        /// Returns the language of the PDF
        /// </summary>
        /// <returns></returns>
        public static string GetLang()
        {
            try {
                return PdfManager.GetDocument().GetPdfDocument().GetCatalog().GetLang().ToString();
            } catch(Exception) {
                return null;
            }
        }

        /// <summary>
        /// Returns the amount of pages. If the amount cannot be found, returns -1
        /// </summary>
        /// <returns></returns>
        public static int GetPageCount()
        {
            try {
                return PdfManager.GetDocument().GetPdfDocument().GetNumberOfPages();
            } catch(Exception) {
                return -1;
            }
        }

        /// <summary>
        /// Returns the version of the PDF
        /// </summary>
        /// <returns></returns>
        public static string GetPdfVersion()
        {
            try {
                return PdfManager.GetDocument().GetPdfDocument().GetPdfVersion().ToString();
            } catch(Exception) {
                return null;
            }
        }

        /// <summary>
        /// Returns the standard of the PDF (A or UA or none)
        /// </summary>
        /// <returns></returns>
        public static string GetPdfStandard()
        {
            try {
                return PdfManager.GetDocument().GetPdfDocument().GetReader().GetPdfAConformanceLevel().GetConformance();
            } catch(Exception) {
                return null;
            }
        }
    }
}
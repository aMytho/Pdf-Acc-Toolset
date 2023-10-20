using System.Text;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;

namespace Pdf_Acc_Toolset.Services.Pdf;

public static class PdfFormat {
    public static PdfVersion GetPdfVersion(Stream file)
    {
        // Load the PDF
        PdfReader reader = new(file);
        return GetVersionFromReader(reader);
    }

    public static PdfVersion GetPdfVersion(Stream file, string password)
    {
        try
        {
            // Unlock the PDF and get the reader
            PdfReader reader = PdfGuard.UnlockPdf(file, password);
            // Return the version
            return GetVersionFromReader(reader);
        }
        catch (BadPasswordException)
        {
            Console.WriteLine("Failed to unlock the PDF with the provided password.");
            throw;
        }
    }

    private static PdfVersion GetVersionFromReader(PdfReader reader)
    {
        // Get the version
        PdfDocument document = new(reader);
        PdfVersion version = document.GetPdfVersion();
        Console.WriteLine($"PDF Version: {version}");
        // Close document
        document.Close();

        return version;
    }

    public static Encoding GetEncoding(PdfVersion version)
    {
        // PDF ver 1.6 and lower use latin 1 encoding
        if (version.CompareTo(PdfVersion.PDF_1_6) <= 0 ) {
            return Encoding.Latin1;
        }

        // Every version after that uses unicode
        return Encoding.Unicode;
    }
}
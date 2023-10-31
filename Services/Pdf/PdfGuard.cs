// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using System.Text;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;

namespace Pdf_Acc_Toolset.Services.Pdf;

public static class PdfGuard
{
    /// <summary>
    /// Runs the required checks. Returns a list of checks that failed.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static List<PdfCheck> RunRequiredChecks(PdfReader reader)
    {
        List<PdfCheck> failedChecks = new();

        bool passwordExists = IsPasswordProtected(reader);
        if (passwordExists)
        {
            failedChecks.Add(PdfCheck.Password);
            // If its password protected, we can't check the other values.
            return failedChecks;
        }

        bool modificationAllowed = IsModificationAllowed(reader);
        if (!modificationAllowed)
        {
            failedChecks.Add(PdfCheck.Modification);
        }

        return failedChecks;
    }

    public static bool RunOptionalChecks(PdfReader reader)
    {
        return IsScreenReaderAllowed(reader);
    }

    private static bool IsPasswordProtected(PdfReader reader)
    {
        try
        {
            PdfDocument tempPDF = new(reader);
            tempPDF.Close();
            return false;
        }
        catch (BadPasswordException)
        {
            return true;
        }
        // Any other error is not caught here
    }

    private static bool IsModificationAllowed(PdfReader reader)
    {
        return PdfEncryptor.IsModifyContentsAllowed(reader.GetCryptoMode());
    }

    private static bool IsScreenReaderAllowed(PdfReader reader)
    {
        return PdfEncryptor.IsScreenReadersAllowed((int)reader.GetPermissions());
    }

    /// <summary>
    /// Returns a reader if the PDF is valid. If not, throws BadPasswordException
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    private static PdfReader GetReaderIfPasswordValid(Stream file, byte[] password)
    {
        // Tries to open the document with the given password
        PdfReader reader = new(file, new ReaderProperties().SetPassword(password));
        PdfDocument tempPDF = new(reader);
        tempPDF.Close();

        // If it didn't throw an exception, its the right password.
        // Since we used up the reader we have to make another one
        file.Position = 0;
        return new(file, new ReaderProperties().SetPassword(password));
    }

    public static PdfReader UnlockPdf(Stream file, string password)
    {
        // At this stage we don't know the PDF version so we don't know the encoding
        // Instead, we try both UTF and Latin1. If both fail, throw exception

        // We first try Latin1
        try
        {
            byte[] passwordInLatin1 = Encoding.Latin1.GetBytes(password);
            PdfReader reader = GetReaderIfPasswordValid(file, passwordInLatin1);

            Console.WriteLine("PDF unlocked in Latin1 format.");
            // Reset stream and return version
            file.Position = 0;
            return reader;
        }
        catch (BadPasswordException)
        {
            // Not latin 1
            // Reset the stream so we can try again
            file.Position = 0;
        }

        // Now try UTF
        try
        {
            byte[] passwordInUnicode = Encoding.Unicode.GetBytes(password);
            PdfReader reader = GetReaderIfPasswordValid(file, passwordInUnicode);

            Console.WriteLine("PDF unlocked in unicode format.");
            // Reset stream and return version
            file.Position = 0;
            return reader;
        }
        catch (BadPasswordException)
        {
            // Not latin 1
            // Reset the stream if the user tries another password
            file.Position = 0;
        }

        throw new BadPasswordException("The password was incorrect in both formats.");
    }

}

public enum PdfCheck
{
    Password,
    Modification,
    ScreenReader
}
// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;
using iText.Layout;
using Pdf_Acc_Toolset.Services.Pdf;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services;

public class PdfManager
{
    /// <summary>
    /// Store the active PDF
    /// </summary>
    private static Document document;

    public static MemoryStream outFile;

    /// <summary>
    /// Name of the exported PDF as set by the user.
    /// </summary>
    public static string filename;

    public static bool pdfDownloadable = false;
    public static bool hasDownloaded = false;
    private static PdfVersion pdfVersion;

    public static ImportOperation<PdfReader> GetReaderFromFile(Stream file)
    {
        // load the file, close the stream when complete
        PdfReader reader = new(file);

        // Return the file
        return new ImportOperation<PdfReader>(reader, true);
    }

    /// <summary>
    /// Gets a PDF reader from a file with a set password
    /// </summary>
    /// <param name="file"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static ImportOperation<PdfReader> GetReaderFromFile(Stream file, string password)
    {
        // Convert the password to byte array
        PdfVersion version = PdfFormat.GetPdfVersion(file, password);
        byte[] passwordBytes = PdfFormat.GetEncoding(version).GetBytes(password);

        // load the file, close the stream when complete
        PdfReader reader = new(file, new ReaderProperties().SetPassword(passwordBytes));

        // Return the file
        return new ImportOperation<PdfReader>(reader, true);
    }

    public static PdfImportConfig GetMetadata(PdfReader reader)
    {
        // Open the pdf in memory to get its metadata (if any)
        PdfDocument tempPDF = new(reader);

        // Create the metadata holder
        PdfImportConfig metadata = new();

        // Set the properties from the temp PDF document and the reader if they exist
        metadata.Title = tempPDF.GetDocumentInfo().GetTitle();
        if (tempPDF.GetCatalog().GetLang() != null)
        {
            metadata.Lang = tempPDF.GetCatalog().GetLang().ToString();
        }

        // Store the PDF version for future use
        // At this point the user is likely going to complete the import
        pdfVersion = tempPDF.GetPdfVersion();

        // Removes the temp PDF from memory
        tempPDF.Close();

        // Create a blank writer config
        metadata.WriterConfig = new();

        // Return the metadata
        return metadata;
    }

    public static void SetOutputFile(Stream input, PdfImportConfig conf)
    {
        // Create a new out stream
        outFile = new();
        // Adjust metadata (if needed)
        if (conf.Standard != null && conf.Standard.Equals("UA"))
        {
            // TO-DO: add the other standards
            conf.WriterConfig.AddUAXmpMetadata();
        }

        // Create reader properties
        ReaderProperties readerProperties = new();

        // Add a password if it exists
        if (conf.Password != null)
        {
            byte[] passwordBytes = PdfFormat.GetEncoding(pdfVersion).GetBytes(conf.Password);
            readerProperties = new ReaderProperties().SetPassword(passwordBytes);
        }

        // Load the file from disk, apply config
        PdfWriter writer = new(outFile, conf.WriterConfig);
        PdfDocument pdf = new(new PdfReader(input, readerProperties), writer);

        // Store the document in the manager
        document = new Document(pdf);
        // Store the filename
        filename = conf.Filename;

        // Enable tagging (or load existing tags)
        pdf.SetTagged();
        // Set lang
        pdf.GetCatalog().SetLang(new PdfString(conf.Lang));
        // Set the title if one exists
        if (conf.Title != null && conf.Title.Length > 0)
        {
            pdf.GetDocumentInfo().SetTitle(conf.Title);
        }
        // Display the document title instead of file name (acc)
        pdf.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));

        // Set role map for custom tags
        TagUtil.SetRoleMap(document.GetPdfDocument().GetStructTreeRoot().GetRoleMap());

        // Store the PDF version (may have changed since GetMetadata was called)
        pdfVersion = pdf.GetPdfVersion();

        // Allow it to be downloaded
        pdfDownloadable = true;
        hasDownloaded = false;
    }

    /// <summary>
    /// Returns the active document
    /// </summary>
    /// <returns></returns>
    public static Document GetDocument()
    {
        if (document.GetPdfDocument().IsClosed())
        {
            Console.WriteLine("Attempted to access a closed document");
            NotificationUtil.Inform(
                NotificationType.Error,
                "The PDF is closed. Import a new PDF to make changes"
            );
            return null;
        }
        return document;
    }

    public static TagTreePointer GetTagRoot()
    {
        TagTreePointer original = document.GetPdfDocument().GetTagStructureContext().GetAutoTaggingPointer().MoveToRoot();
        // Get the element for the pointer
        PdfStructElem elem = original.GetContext().GetPointerStructElem(original);
        // Return a new pointer
        return original.GetContext().CreatePointerForStructElem(elem);
    }

    /// <summary>
    /// Save the PDF, close all, move on in life...
    /// </summary>
    public static void Save()
    {
        document.Close();
        // Prevent duplicate downloads
        pdfDownloadable = false;
        // Update error message if they try to download after close
        hasDownloaded = true;
    }

    /// <summary>
    /// Removes the memory stream and the PDF data.
    /// Run after calling Save()
    /// </summary>
    public static void Close()
    {
        document = null;
        outFile = null;
    }
}

public struct ImportOperation<T>
{
    // The data from the operation
    public T Data;
    // Did the operation succeed?
    public bool success = false;

    // Operation may have succeeded, either way the data is able to be returned
    public ImportOperation(T Data, bool success)
    {
        this.Data = Data;
        this.success = success;
    }
}

public struct PdfImportConfig
{
    /// <summary>
    /// Title of the PDF
    /// </summary>
    public string Title;
    /// <summary>
    /// Lang of the PDF. Some ISO, idk
    /// </summary>
    public string Lang;
    /// <summary>
    /// PDf standard. Probaly UA or nothing
    /// </summary>
    public string Standard;
    /// <summary>
    /// The filename of the PDF
    /// </summary>
    public string Filename;

    /// <summary>
    /// The user password (for reading)
    /// </summary>
    public string Password;
    public WriterProperties WriterConfig;
}
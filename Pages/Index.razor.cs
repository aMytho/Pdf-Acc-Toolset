// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using Microsoft.AspNetCore.Components.Forms;
using Pdf_Acc_Toolset.Pages.Import;
using Pdf_Acc_Toolset.Services;
using Pdf_Acc_Toolset.Services.Pdf;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Pages;

public partial class Index
{
    private bool showImport = false;
    private bool showReimportWarning = false;
    private bool showPassword = false;
    private bool showGetStarted = true;
    private string filename;
    private string password;

    private PdfImportConfig import;
    private MemoryStream ms;

    /// <summary>
    /// Requests an uploaded file and sends it to the PDF manager
    /// </summary>
    private async void UploadFile(InputFileChangeEventArgs e)
    {
        Console.WriteLine("User beginning import process.");

        // Validate that only 1 PDF was uploaded
        bool uploadValid = IsUploadValid(e);
        if (!uploadValid)
        {
            return;
        }

        // Hide the import/password box if user left previous import process
        this.showPassword = false;
        this.showImport = false;
        StateHasChanged();

        // Store the filename
        this.filename = e.File.Name;
        // The upload is valid, load it into memory
        this.ms = await SetMemoryStream(e);

        // Check PDF validity (password, editable, etc.)
        bool pdfValid = IsPdfValid();
        if (!pdfValid)
        {
            return;
        }

        // Get a new reader since the last one was used by the validator
        var reader = PdfManager.GetReaderFromFile(ms);
        ShowImportComponent(reader.Data);
    }

    private bool IsUploadValid(InputFileChangeEventArgs e)
    {
        // Validate the PDF, accept only 1 file, must be a pdf
        if (e.FileCount != 1 || !e.File.Name.EndsWith("pdf"))
        {
            NotificationUtil.Inform(NotificationType.Error,
                "Import Failed. Ensure that a single PDF was selected."
            );
            return false;
        }

        Console.WriteLine("User upload is valid.");
        return true;
    }

    private bool IsPdfValid()
    {
        // Get a reader from the file stream
        var reader = PdfManager.GetReaderFromFile(ms);
        // Handle failed reads
        if (!reader.success)
        {
            return false;
        }

        Console.WriteLine("Import Reader Created");

        // Validate PDF. Checks for issues within the PDF spec
        List<PdfCheck> failedChecks = PdfGuard.RunRequiredChecks(reader.Data);

        // Handle each failed check
        foreach (var check in failedChecks)
        {
            HandleFailedCheck(check);
        }

        // Reset the file stream since we used it
        ms.Position = 0;

        return failedChecks.Count == 0;
    }

    private async Task<MemoryStream> SetMemoryStream(InputFileChangeEventArgs e)
    {
        // Open a stream for the file
        try
        {
            Console.WriteLine($"The file is {e.File.Size} bytes.");
            // Create a new stream the size of the file
            MemoryStream fileStream = new((int)e.File.Size);
            // Load the file into the stream
            await e.File.OpenReadStream(e.File.Size).CopyToAsync(fileStream);
            // Return to 0 so the PDF reader can use it later
            fileStream.Position = 0;

            return fileStream;
        }
        catch (IOException)
        {
            NotificationUtil.Inform(NotificationType.Error,
                "Import Failed. Ensure that a valid PDF was selected"
            );
            throw;
        }
        catch (Exception)
        {
            NotificationUtil.Inform(NotificationType.Error,
                "Unknown PDF import error."
            );
            throw;
        }
    }

    private void HandleFailedCheck(PdfCheck check)
    {
        switch (check)
        {
            case PdfCheck.Password:
                // SHow password check component
                NotificationUtil.Inform(NotificationType.Warning,
                    "A password is required to open this document"
                );
                break;
            case PdfCheck.Modification:
                NotificationUtil.Inform(NotificationType.Warning,
                    "This PDF has modification disabled. A password is required to edit the document."
                );
                break;
        }
        // Either of these require that a password be entered.
        // When we find one I can't change, these go back in switch statement
        showPassword = true;
        StateHasChanged();
    }

    private void OnImportSet(ImportSettings.ImportSelection import)
    {
        NotificationUtil.Inform(NotificationType.Success,
            "Import Complete! You may begin the edit process."
        );

        // Move to the next section
        this.showImport = false;

        // based on the user settings, set the metadata for the output file
        this.import.Title = import.Title;
        this.import.Lang = import.Lang;
        this.import.Standard = import.Standard;
        this.import.Filename = import.Filename;

        Console.WriteLine("Saved import settings.");

        this.ms.Position = 0;
        // Set output
        PdfManager.SetOutputFile(this.ms, this.import);
        // Remove our local version from memory
        this.ms.Dispose();

        Console.WriteLine("Writable file is ready");

        // Switch to the tools page
        Navigation.NavigateTo("/tools");
    }

    private void OnPasswordSet(string password)
    {
        // Create a new reader with the password
        this.ms.Position = 0;
        try
        {
            var reader = PdfManager.GetReaderFromFile(this.ms, password);
            // If we got this far, the password is correct!
            // Hide component, save password
            this.showPassword = false;
            this.password = password;

            ShowImportComponent(reader.Data);
            NotificationUtil.Inform(NotificationType.Success, "Password valid!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NotificationUtil.Inform(NotificationType.Error, "Invalid Password.");
            return;
        }
    }

    private void ShowImportComponent(iText.Kernel.Pdf.PdfReader reader)
    {
        // Request metadata for the PDF. We pass this to the import component later
        this.import = PdfManager.GetMetadata(reader);
        // Add filename from upload
        this.import.Filename = this.filename;
        this.import.Password = password;

        // Clear any existing tasks from previous PDFs
        if (!TaskManager.IsEmpty())
        {
            TaskManager.RemoveTasks();
        }

        // Request import settings from user
        this.showImport = true;
        // Hide import file component
        this.showGetStarted = false;
        StateHasChanged();
    }

    /// <summary>
    /// Returns an import selection from the metadata (type conversion)
    /// </summary>
    /// <returns></returns>
    private ImportSettings.ImportSelection ConvertMetadataToImportDefault()
    {
        return new ImportSettings.ImportSelection(this.import.Title, this.import.Lang, this.import.Standard,
        this.import.Filename);
    }

    protected override void OnInitialized()
    {
        // If the user navigated back to the toolbox, show the right page.
        if (PdfManager.outFile != null)
        {
            this.showImport = false;
            this.showReimportWarning = true;
        }
        base.OnInitialized();
    }
}
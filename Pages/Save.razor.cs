// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Pdf_Acc_Toolset.Services;
using Pdf_Acc_Toolset.Services.Pdf;
using Pdf_Acc_Toolset.Services.UI;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Pages;

public partial class Save
{
    private PdfInfo info = new();
    private int taskIndex;
    
    [Inject]
    private NavigationManager navigation { get; set; }

    private bool canDownload = false;

    public List<AccessibilityTask> tasks = new();
    protected override void OnInitialized()
    {
        // Get the tasks in the queue
        tasks = TaskManager.GetAccessibilityTasks();

        // Get the PDF info
        info.lang = PdfMetadata.GetLang();
        info.title = PdfMetadata.GetTitle();
        info.standard = PdfMetadata.GetPdfStandard();
        info.version = PdfMetadata.GetPdfVersion();
        info.pageCount = PdfMetadata.GetPageCount();
        info.tasksRan = TaskManager.GetTasksRanCounter();
        info.tasksInQueue = TaskManager.GetTasksInQueue().Count();
        info.filename = PdfManager.filename;

        // Correct any blank or invalid fields
        info = AddPdfInfoDefaults(info);

        // Allow/disallow downloads
        canDownload = PdfManager.pdfDownloadable;
    }

    private PdfInfo AddPdfInfoDefaults(PdfInfo info)
    {
        if (info.title == null || info.title.Length == 0) {
            info.title = "None";
        }

        if (info.standard == null || info.standard.Length == 0) {
            info.standard = "None";
        }

        if (info.lang == null || info.lang.Length == 0) {
            info.lang = "None";
        }

        Console.WriteLine(info);

        return info;
    }

    private async void SavePdf()
    {
        // If the user already downloaded the PDF, stop them from trying again
        if (!canDownload) {
            NotificationUtil.Inform(
                NotificationType.Error, "You cannot download a PDF once it has been saved."
            );
            return;
        }

        // Close Tag Inspector
        TagInspectorService.NotifyPdfClose();

        // Save the PDF
        PdfManager.Save();
        Console.WriteLine("Saving the PDF");
        // Download the PDF
        await DownloadPdf();

        // Show the complete screen
        navigation.NavigateTo("/complete");
    }

    private void RunQueueAndSavePdf()
    {
        if (canDownload) {
            // Run the queued tasks
            TaskManager.RunQueuedTasks();
            // Save the PDF
            SavePdf();
        } else {
            NotificationUtil.Inform(
                NotificationType.Error, "You cannot download a PDF once it has been saved."
            );
        }
    }

    private async Task DownloadPdf()
    {
        // Generate a file
        byte[] data = PdfManager.outFile.ToArray();
        Console.WriteLine("Exporting PDF...");
        // Send the data to JS to actually download the file
        await JSRuntime.InvokeVoidAsync(
            "BlazorDownloadFile", CorrectFilename(PdfManager.filename),
            "application/octet-stream", data
        );
        NotificationUtil.Inform(NotificationType.Success, "Starting Export...");

        // Reset all PDF data
        PdfManager.Close();
        // Remove any tasks
        TaskManager.RemoveTasks();
    }

    // Handle bad filenames
    private string CorrectFilename(string filename)
    {
        // No file name
        if (filename == null) {
            return "Default_Name.pdf";
        }

        // No file extension
        if (!filename.EndsWith(".pdf")) {
            return filename + ".pdf";
        }

        // Good user, good boy!
        return filename;
    }

    private struct PdfInfo
    {
        public string filename;
        public string version;
        public string title;
        public string standard;
        public string lang;
        public int tasksRan;
        public int tasksInQueue;
        public int pageCount;
    }
}
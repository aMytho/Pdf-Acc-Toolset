// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using Microsoft.AspNetCore.Components;
using Pdf_Acc_Toolset.Services;
using Pdf_Acc_Toolset.Services.UI;


namespace Pdf_Acc_Toolset.Pages.Inspector;

public partial class TagInspector
{
    [Parameter]
    public EventCallback<int> OnCloseInspector { get; set; }

    private bool pdfVisible = false;

    protected override void OnInitialized()
    {
        TagInspectorService.PdfReady += OnPdfReady;
        TagInspectorService.PdfClosed += OnPdfClosed;

        if (PdfManager.outFile != null) {
            pdfVisible = true;
        }
        base.OnInitialized();
    }

    private void OnPdfReady()
    {
        Console.WriteLine("Tag View Created");
        this.pdfVisible = true;
    }

    private void OnPdfClosed(object sender, EventArgs e)
    {
        this.pdfVisible = false;
    }

    private void CloseInspector()
    {
        this.OnCloseInspector.InvokeAsync();
    }

    private iText.Kernel.Pdf.Tagutils.TagTreePointer GetTagTree()
    {
        return PdfManager.GetTagRoot();
    }
}
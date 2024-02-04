
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

        if (PdfManager.outFile != null) {
            pdfVisible = true;
        }
        base.OnInitialized();
    }

    private void OnPdfReady()
    {
        Console.WriteLine("idk");
        this.pdfVisible = true;
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
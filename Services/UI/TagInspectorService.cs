
namespace Pdf_Acc_Toolset.Services.UI;

public class TagInspectorService
{
    public static event Action PdfReady;

    public static void NotifyPdfReady()
    {
        PdfReady?.Invoke();
    }
}
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace Pdf_Acc_Toolset.Services.Tools.Canvas;

public class PdfColorFinder : PdfCanvasEditor
{
    /// <summary> 
    /// These operators may indicate that a color change has occurred 
    /// </summary>
    private readonly List<string> TEXT_SHOWING_OPERATORS = ["Tj", "'", "\"", "TJ"];

    private List<Color> fillColors = [];
    private List<Color> strokeColors = [];

    public PdfColorFinder() : base() {}

    public override void ModifyCanvas(PdfCanvasProcessor processor, PdfLiteral pdfLiteral, IList<PdfObject> operands)
    {
        string operatorString = pdfLiteral.ToString();
        
        // Only check fill operators
        if (TEXT_SHOWING_OPERATORS.Contains(operatorString))
        {
            // Gets the color of the content
            Color currentFillColor = GetGraphicsState().GetFillColor();
            Color currentStrokeColor = GetGraphicsState().GetStrokeColor();

            // Store the colors
            fillColors.Add(currentFillColor);
            strokeColors.Add(currentStrokeColor);
        }

        // Copy the page without making any changes
        Write(processor, pdfLiteral, operands);
    }

    
    public void Find(PdfDocument document) {
        for (int i = 1; i < document.GetNumberOfPages() + 1; i++)
        {
            EditPage(document, i);
        }

        // Remove duplicates (if any)
        RemoveDuplicateColors();
    }

    private void RemoveDuplicateColors()
    {
        // Remove duplicates by converting to a Hash Set and then to a list.
        // Hash Sets can't have duplicates at all.
        fillColors = new HashSet<Color>(fillColors).ToList();
        strokeColors = new HashSet<Color>(strokeColors).ToList();
    }

    public List<Color> GetFillColors() {
        return fillColors;
    }

    public List<Color> GetStrokeColors() {
        return strokeColors;
    }
}
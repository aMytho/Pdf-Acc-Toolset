// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset

using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace Pdf_Acc_Toolset.Services.Tools.Canvas;

public class PdfColorEditor : PdfCanvasEditor
{
    /// <summary> 
    /// These operators may indicate that a color change has occurred 
    /// </summary>
    private readonly List<string> TEXT_SHOWING_OPERATORS = ["Tj", "'", "\"", "TJ"];

    /// <summary> 
    /// The color of content in the document 
    /// </summary>
    private Color currentColor;
    /// <summary> 
    /// Color specified by the user to be replaced 
    /// </summary>
    private readonly Color colorToFind;

    private readonly Color replacementColor;
    
    // Replacing either fill or stroke
    private readonly ColorPart colorPart;

    // Keeps track of how many colors were replaced, logged to console on completion.
    private int colorMatches = 0;

    public PdfColorEditor(Color find, Color replace, ColorPart part) : base()
    {
        this.colorToFind = find;
        this.replacementColor = replace;
        this.colorPart = part;
        Console.WriteLine($"Target color is {find.GetColorValue()}");
        Console.WriteLine($"Replacement color is {replace.GetColorValue()}");
    }

    public void Edit(PdfDocument document) {
        for (int i = 1; i < document.GetNumberOfPages() + 1; i++)
        {
            EditPage(document, i);
        }

        Console.WriteLine($"Color Matches: {colorMatches}");
    }

    /// <summary>
    /// Changes the color
    /// </summary>
    public override void ModifyCanvas(PdfCanvasProcessor processor, PdfLiteral pdfLiteral, IList<PdfObject> operands)
    {
        string operatorString = pdfLiteral.ToString();
        // Only check a color if there is a related operator
        if (TEXT_SHOWING_OPERATORS.Contains(operatorString))
        {
            if (currentColor == null)
            {
                // Gets the color of the content
                Color nextColor;
                if (colorPart == ColorPart.Fill) {
                    nextColor = GetGraphicsState().GetFillColor();
                } else {
                    nextColor = GetGraphicsState().GetStrokeColor();
                }

                // If the color matches, start the replacement process
                if (colorToFind.Equals(nextColor))
                {
                    // Add one to the counter
                    colorMatches++;
                    // Set the current color
                    currentColor = nextColor;

                    // Replace it
                    List<PdfObject> list = GetColorList(replacementColor);
                    Write(processor, GetPdfLiteral("rg"), list);
                }
            }
        }
        else if (currentColor != null)
        {
            if (currentColor is DeviceCmyk)
            {
                List<PdfObject> list = GetColorList(replacementColor);
                Write(processor, GetPdfLiteral("k"), list);
            }
            else if (currentColor is DeviceGray)
            {
                List<PdfObject> list = GetColorList(replacementColor);
                Write(processor, GetPdfLiteral("g"), list);
            }
            else
            {
                List<PdfObject> list = GetColorList(replacementColor);
                Write(processor, GetPdfLiteral("rg"), list);
            }

            // Reset. Allows for more colors to be replaced
            currentColor = null;
        }

        Write(processor, pdfLiteral, operands);
    }

    /// <summary> 
    /// Gets a color list based on the color space
    /// </summary>
    private List<PdfObject> GetColorList(Color color)
    {
        List<PdfObject> list = new();
        float[] values = color.GetColorValue();
        if (color is DeviceCmyk)
        {
            list.Add(new PdfNumber(values[0]));
            list.Add(new PdfNumber(values[1]));
            list.Add(new PdfNumber(values[2]));
            list.Add(new PdfNumber(values[3]));
            list.Add(GetPdfLiteral("k"));
        }
        else if (color is DeviceGray)
        {
            list.Add(new PdfNumber(values[0]));
            list.Add(GetPdfLiteral("g"));
        }
        else
        {
            list.Add(new PdfNumber(values[0]));
            list.Add(new PdfNumber(values[1]));
            list.Add(new PdfNumber(values[2]));
            list.Add(GetPdfLiteral("rg"));
        }

        return list;
    }

    // Based on the color part, return the right literal
    private PdfLiteral GetPdfLiteral(string literal)
    {
        if (colorPart == ColorPart.Stroke) {
            // Stroke PDF literals are always uppercase
            return new PdfLiteral(literal.ToUpper());
        }

        // If fill, return literal as is
        return new PdfLiteral(literal);
    }
}

public enum ColorPart
{
    Fill,
    Stroke
}
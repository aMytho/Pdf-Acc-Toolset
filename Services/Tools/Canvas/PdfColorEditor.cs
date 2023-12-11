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

    public PdfColorEditor(Color find, Color replace) : base()
    {
        this.colorToFind = find;
        this.replacementColor = replace;
        Console.WriteLine($"Target color is {find.GetColorValue()}");
        Console.WriteLine($"Replacement color is {replace.GetColorValue()}");
    }

    public void Edit(PdfDocument document) {
        for (int i = 1; i < document.GetNumberOfPages() + 1; i++)
        {
            EditPage(document, i);
        }
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
                Color currentFillColor = GetGraphicsState().GetFillColor();

                // If the color matches, start the replacement process
                if (colorToFind.Equals(currentFillColor))
                {
                    Console.WriteLine("Found a color match");
                    // Set the current color
                    currentColor = currentFillColor;

                    // Replace it
                    List<PdfObject> list = GetColorList(replacementColor);
                    Write(processor, new PdfLiteral("rg"), list);
                }
            }
        }
        else if (currentColor != null)
        {
            if (currentColor is DeviceCmyk)
            {
                List<PdfObject> list = GetColorList(replacementColor);
                Write(processor, new PdfLiteral("k"), list);
            }
            else if (currentColor is DeviceGray)
            {
                List<PdfObject> list = GetColorList(replacementColor);
                Write(processor, new PdfLiteral("g"), list);
            }
            else
            {
                List<PdfObject> list = GetColorList(replacementColor);
                Write(processor, new PdfLiteral("rg"), list);
            }

            // Reset. Allows for more colors to be replaced
            currentColor = null;
        }

        Write(processor, pdfLiteral, operands);
    }

    /// <summary> 
    /// Gets a color list based on the color space
    /// </summary>
    private static List<PdfObject> GetColorList(Color color)
    {
        List<PdfObject> list = new();
        float[] values = color.GetColorValue();
        if (color is DeviceCmyk)
        {
            list.Add(new PdfNumber(values[0]));
            list.Add(new PdfNumber(values[1]));
            list.Add(new PdfNumber(values[2]));
            list.Add(new PdfNumber(values[3]));
            list.Add(new PdfLiteral("k"));
        }
        else if (color is DeviceGray)
        {
            list.Add(new PdfNumber(values[0]));
            list.Add(new PdfLiteral("g"));
        }
        else
        {
            list.Add(new PdfNumber(values[0]));
            list.Add(new PdfNumber(values[1]));
            list.Add(new PdfNumber(values[2]));
            list.Add(new PdfLiteral("rg"));
        }

        return list;
    }

}
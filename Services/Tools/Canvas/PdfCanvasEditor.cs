// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset

using System.Collections.ObjectModel;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;

namespace Pdf_Acc_Toolset.Services.Tools.Canvas;

/// <summary> 
/// Base low-level PDF editor 
/// </summary>
public abstract class PdfCanvasEditor : PdfCanvasProcessor
{
    /// <summary> 
    /// Holds output canvas and related resources 
    /// </summary>
    protected PdfCanvas canvas;

    public PdfCanvasEditor() : base(new ContentListener()) {}
    public PdfCanvasEditor(Collection<EventType> collection) : base(new ContentListener(collection)) {}


    /// <summary>
    /// Edits a page by a given number
    /// </summary>
    public void EditPage(PdfDocument pdfDocument, int pageNumber)
    {
        // Get the current page and resources
        PdfPage page = pdfDocument.GetPage(pageNumber);
        PdfResources pdfResources = page.GetResources();

        // Create a new canvas to make changes on
        PdfCanvas pdfCanvas = new(new PdfStream(), pdfResources, pdfDocument);
        // Make the changes
        ProcessPageContent(page.GetContentBytes(), pdfResources, pdfCanvas);

        // Overwrites the old page with the new data
        page.Put(PdfName.Contents, pdfCanvas.GetContentStream());
    }

    /// <summary> 
    /// Begins the processing. The event listener will handle each change
    /// </summary>
    public void ProcessPageContent(byte[] contentBytes, PdfResources resources, PdfCanvas canvas)
    {
        this.canvas = canvas;
        ProcessContent(contentBytes, resources);
        this.canvas = null;
    }

    /// <summary> 
    /// Copies a page to the "new" document. 
    /// </summary>
    public void Write(PdfCanvasProcessor processor, PdfLiteral pdfOperator, IList<PdfObject> operands)
    {
        PdfOutputStream pdfOutputStream = canvas.GetContentStream().GetOutputStream();
        int index = 0;

        // Copy each object to the new page
        foreach (PdfObject obj in operands)
        {
            pdfOutputStream.Write(obj);
            if (operands.Count > ++index) {
                pdfOutputStream.WriteSpace();
            } else {
                pdfOutputStream.WriteNewLine();
            }
        }
    }

    /// <summary> 
    /// Modifies the canvas. These are the actual changes done to the PDF.
    /// </summary>
    public abstract void ModifyCanvas(PdfCanvasProcessor processor, PdfLiteral pdfLiteral, IList<PdfObject> operands);

    /// <summary> 
    /// Overrides PdfContentStreamProcessor methods 
    /// </summary>
    public override IContentOperator RegisterContentOperator(string operatorString, IContentOperator pdfOperator)
    {
        ContentOperatorWrapper wrapper = new ContentOperatorWrapper(this);
        wrapper.SetOriginalOperator(pdfOperator);
        IContentOperator formerOperator = base.RegisterContentOperator(operatorString, wrapper);
        if (formerOperator is ContentOperatorWrapper)
        {
            return ((ContentOperatorWrapper)formerOperator).GetOriginalOperator();
        }
        else
        {
            return formerOperator;
        }
    }
}
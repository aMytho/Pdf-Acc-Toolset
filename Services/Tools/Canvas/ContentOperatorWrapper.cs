// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace Pdf_Acc_Toolset.Services.Tools.Canvas;

/// <summary>
/// A content operator class to wrap all content operators to forward the invocation to the editor
/// </summary>
class ContentOperatorWrapper : IContentOperator
{
    private readonly PdfCanvasEditor editor;
    private IContentOperator originalOperator = null;

    public ContentOperatorWrapper(PdfCanvasEditor editor)
    {
        this.editor = editor;
    }

    public IContentOperator GetOriginalOperator()
    {
        return originalOperator;
    }

    public void SetOriginalOperator(IContentOperator originalOperator)
    {
        this.originalOperator = originalOperator;
    }


    public void Invoke(PdfCanvasProcessor processor, PdfLiteral pdfLiteral, IList<PdfObject> operands)
    {
        if (originalOperator != null && !"Do".Equals(pdfLiteral.ToString()))
        {
            originalOperator.Invoke(processor, pdfLiteral, operands);
        }

        // Make the change. Some child class will implement this for us
        editor.ModifyCanvas(processor, pdfLiteral, operands);
    }
}
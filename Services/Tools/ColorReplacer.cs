// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset

using iText.Kernel.Colors;
using iText.Layout;
using Pdf_Acc_Toolset.Services.Tools.Canvas;

namespace Pdf_Acc_Toolset.Services.Tools;

public class ColorReplacer : AccessibilityTask
{
    private readonly PdfColorEditor editor;

    public ColorReplacer(Document document, Selection.Selection selection, Color find, Color replace) : base(document, selection)
    {
        this.editor = new(find, replace);
        this.Name = "Color Replacer";
    }

    public override void Run()
    {
        editor.Edit(Document.GetPdfDocument());
        
        // Mark as complete
        this.TaskComplete = true;
    }
}
// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset

using iText.Kernel.Colors;
using iText.Layout;
using Pdf_Acc_Toolset.Services.Tools.Canvas;

namespace Pdf_Acc_Toolset.Services.Tools;

public class ColorReplacer : AccessibilityTask
{
    private readonly PdfColorEditor editor;
    private readonly Color target;
    private readonly Color replacement;

    public ColorReplacer(Document document, Selection.Selection selection, Color find, Color replace, ColorPart part) : base(document, selection)
    {
        this.target = find;
        this.replacement = replace;
        this.editor = new(find, replace, part);
        this.Name = "Color Replacer";
    }

    public override void Run()
    {
        editor.Edit(Document.GetPdfDocument());
        
        // Mark as complete
        this.TaskComplete = true;
    }

    public Color GetTarget()
    {
        return target;
    }

    public Color GetReplacement()
    {
        return replacement;
    }
}
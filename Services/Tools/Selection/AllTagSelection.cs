// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using iText.Kernel.Pdf.Tagutils;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services.Tools.Selection;

/// <summary> 
/// This selection returns every tag
/// </summary>
public class AllTag : Selection
{
    public AllTag(TagTreePointer input, InsertionPoint ins) : base()
    {
        this.inputTags = input;
        this.point = ins;
    }

    public override void FindElements()
    {
        // List which will contain all tags
        List<TagTreePointer> matchingTags = TagUtil.GetAllTags(this.inputTags);

        // Save the selection
        this.selection = matchingTags;
        this.HandleTopInsertionPoint();
        this.foundSelection = true;
        Console.WriteLine($"Selection items: {this.selection.Count}");
    }

    private void HandleTopInsertionPoint()
    {
        // If the insertion point is before/after the selection, strip out the document root
        // We can't insert above or after the root element
        if (this.point == InsertionPoint.BeforeSelection || this.point == InsertionPoint.AfterSelection) {
            this.selection = this.selection.Skip(1).ToList();
        }
    }
}
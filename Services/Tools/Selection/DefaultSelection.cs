// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using iText.Kernel.Pdf.Tagutils;

namespace Pdf_Acc_Toolset.Services.Tools.Selection;

public class DefaultSelection : Selection
{
    public DefaultSelection(TagTreePointer input, InsertionPoint ins, int limit = -1) : base()
    {
        this.inputTags = input;
        this.limit = limit;
        this.point = ins;
    }

    public override void FindElements()
    {
        // List which will be returned with the tags
        List<TagTreePointer> matchingTags = new(1)
            {
                // The default selection is just the root tag
                this.inputTags
            };

        // Save the selection
        this.selection = matchingTags;
        this.foundSelection = true;
        Console.WriteLine($"Selection items: {this.selection.Count}");
    }
}
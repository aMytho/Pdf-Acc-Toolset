// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;
using iText.Layout;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services.Tools;

public class TagShifter : AccessibilityTask
{
    private TagType tagToShift;

    public TagShifter(Document document, Selection.Selection selection, TagType tagType) : base(document, selection)
    {
        this.Name = "Tag Shifter";
        this.tagToShift = tagType;
    }

    public override void Run()
    {
        // Based on the selection, find where to start
        Selection.FindElements();
        // Check the selection
        if (!Selection.FoundSelection())
        {
            NotificationUtil.Inform(NotificationType.Warning,
                $"The task {this.Name} did not find a selection. Cancelling Task"
            );
            return;
        }
        // Get the selection
        List<TagTreePointer> selectionTargets = Selection.GetSelection();

        string tagToBeShifted = TagUtil.GetTagByEnum(tagToShift, true);
        foreach (TagTreePointer pointer in selectionTargets)
        {
            IList<string> kidsRoles = pointer.GetKidsRoles();
            Console.WriteLine(kidsRoles.Count);

            // Keep track of the elements moved because the index will change
            int elementsMoved = 0;
            for (int i = 0; i < kidsRoles.Count; i++)
            {
                // If this kid is content (MCR), skip
                if (kidsRoles[i].Equals("MCR")) continue;

                // Get a copy of the current (parent) tag
                PdfStructElem elem = pointer.GetContext().GetPointerStructElem(pointer);
                TagTreePointer pointerCopy = pointer.GetContext().CreatePointerForStructElem(elem);
                
                string pointerCopyRole = TagUtil.ConvertRole(new PdfName(pointerCopy.GetRole()));
                
                // Move to the current kid. Account for previously moved kids
                pointer.MoveToKid(i - elementsMoved);
                string role = TagUtil.ConvertRole(new PdfName(kidsRoles[i]));
                
                if (role.Equals(tagToBeShifted) && role.Equals(pointerCopyRole)) {
                    // Move kid up
                    PdfStructElem kidElem = pointerCopy.GetContext().GetPointerStructElem(pointerCopy);
                    TagTreePointer kidPointerCopy = pointerCopy.GetContext().CreatePointerForStructElem(kidElem);
                    
                    // Move pointer up
                    kidPointerCopy.MoveToParent();
                    // Move kid tag to parent of above pointer
                    pointer.Relocate(kidPointerCopy);

                    elementsMoved++;
                }

                // Return back to the parent
                pointer.MoveToPointer(pointerCopy);
            }
            // Mark as complete
            TaskComplete = true;
        }
    }

    public TagType getTagToShift()
    {
        return this.tagToShift;
    }
}
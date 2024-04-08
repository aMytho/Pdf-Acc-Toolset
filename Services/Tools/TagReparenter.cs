// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;
using iText.Layout;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services.Tools;

public class TagReparenter : AccessibilityTask
{
    private string tagToReparent;
    private bool deleteTag;
    private TagTreePointer tagRoot;

    public TagReparenter(Document document, Selection.Selection selection, TagType tagType, bool deleteTag) : base(document, selection)
    {
        this.Name = "Tag Reparent";
        this.tagToReparent = TagUtil.GetTagByEnum(tagType);
        this.deleteTag = deleteTag;
        this.tagRoot = PdfManager.GetTagRoot();
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

        foreach (TagTreePointer pointer in selectionTargets)
        {
            // Create a new pointer for tree traversal
            PdfStructElem elem = pointer.GetContext().GetPointerStructElem(pointer);
            TagTreePointer pointerCopy = pointer.GetContext().CreatePointerForStructElem(elem);

            // Create a new pointer for deleting the parent
            TagTreePointer pointerCopy2 = pointer.GetContext().CreatePointerForStructElem(elem);

            // Try to find a matching parent. If found, move to it
            TagTreePointer targetTagFound = FindTargetParent(pointerCopy);
            if (targetTagFound != null) {
                Console.WriteLine("Reparenting tag!");
                // Move the second copy up one tag.
                // This is used for empty parent deletion below
                pointerCopy2.MoveToParent();

                // Move the tag to the new parent
                pointer.Relocate(targetTagFound);
            }

            // If deleteTag is true and the parent tag now has no children, delete it
            try {
                if (!deleteTag) continue;
                
                if (pointerCopy2.GetKidsRoles().Count == 0) {
                    pointerCopy2.RemoveTag();
                }
            } catch (Exception ex) {
                Console.WriteLine("Error deleting empty parent after Tag Reparent");
                Console.WriteLine(ex);
            }
        }
        // Mark as complete
        TaskComplete = true;
    }

    private TagTreePointer FindTargetParent(TagTreePointer pointer)
    {
        pointer.MoveToParent();

        // If the current tag is the matching tag, return it
        if (IsTargetType(pointer)) {
            return pointer;
        }
        
        // If root, failed to find a match
        if (IsRoot(pointer)) {
            return null;
        }

        // Keep checking parent tags
        return FindTargetParent(pointer);
    }

    private bool IsTargetType(TagTreePointer pointer)
    {
        return pointer.GetRole().Equals(tagToReparent);
    }

    private bool IsRoot(TagTreePointer pointer) {
        return pointer.IsPointingToSameTag(tagRoot);
    }

    public bool GetWillDeleteTag() {
        return deleteTag;
    }
}
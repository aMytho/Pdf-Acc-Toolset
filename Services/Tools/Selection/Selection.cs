// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using iText.Kernel.Pdf.Tagutils;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services.Tools.Selection;

public abstract class Selection
{
    protected int limit;

    protected TagTreePointer inputTags;

    protected List<TagTreePointer> selection;

    protected bool foundSelection = false;

    protected InsertionPoint point = InsertionPoint.FirstChild;

    public Selection() { }

    public abstract void FindElements();

    public bool FoundSelection()
    {
        return foundSelection;
    }

    public List<TagTreePointer> GetSelection()
    {
        return selection;
    }

    public InsertionPoint GetInsertion()
    {
        return point;
    }

    /// <summary>
    /// Moves a TagPointer to its new position based on the insertion value.
    /// Run this while the Accessibility Task is running to prevent unwanted changes to the tree.
    /// The pointer is modified so this method has no return val
    /// </summary>
    /// <param name="pointer"></param>
    public void MoveSelectionToInsertion(TagTreePointer pointer)
    {
        switch (point)
        {
            case InsertionPoint.FirstChild:
                // Set the next insertion to the last child
                pointer.SetNextNewKidIndex(0);
                break;
            case InsertionPoint.LastChild:
                // Default behavior, do nothing
                break;
            case InsertionPoint.BeforeSelection:
                // Get current index in parent list
                int indexBefore = pointer.GetIndexInParentKidsList();
                // If the tag is the root or something is really wrong
                if (indexBefore == -1)
                {
                    // If this happens we don't move up the tree!
                    Console.Error.WriteLine(
                        "Warning: Attempted to move tag above root or move to flushed tag"
                    );
                    Console.Error.WriteLine("The problem tag is: " + pointer.GetRole());
                    NotificationUtil.Inform(
                        NotificationType.Warning,
                        "Attempted to move to an invalid tag location. Selection will not be moved."
                    );
                }
                else
                {
                    // Move up and set new index
                    pointer.MoveToParent();
                    pointer.SetNextNewKidIndex(indexBefore);
                }
                break;
            case InsertionPoint.AfterSelection:
                // Get the current position
                int indexAfter = pointer.GetIndexInParentKidsList();
                pointer.MoveToParent();
                // Set the position + 1 for after
                pointer.SetNextNewKidIndex(indexAfter + 1);
                break;
            default:
                break;
        }
    }
}

public enum InsertionPoint
{
    BeforeSelection,
    AfterSelection,
    FirstChild,
    LastChild
}

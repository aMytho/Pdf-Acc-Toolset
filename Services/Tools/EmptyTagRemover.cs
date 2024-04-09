// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using iText.Kernel.Pdf.Tagutils;
using iText.Layout;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services.Tools;

public class EmptyTagRemover : AccessibilityTask
{
    public EmptyTagRemover(Document document, Selection.Selection selection) : base(document, selection)
    {
        this.Name = "Empty Tag Remover";
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
            string role = pointer.GetRole();
            IList<string> kidsRoles = pointer.GetKidsRoles();

            // Skip non-empty tags
            if (kidsRoles.Count > 0) continue;

            // Skip content tags
            if (role.Equals("MCR") || role.Equals("Link - OBJR") || role.Equals("Artifact")) continue;

            // Delete the tag
            pointer.RemoveTag();
        }
        // Mark as complete
        TaskComplete = true;
    }
}
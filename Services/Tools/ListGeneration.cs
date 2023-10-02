using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Pdf;
using iText.Layout;
using Pdf_Acc_Toolset.Services.Util;
using Pdf_Acc_Toolset.Services.Tools.Selection;

namespace Pdf_Acc_Toolset.Services.Tools
{
    public class ListGeneration : AccessibilityTask
    {
        private string Title;
        private int ListItemAmount;
        private bool AddLabels;

        public ListGeneration(Document document, Selection.Selection selection, string title, int listItemAount, bool addLabels): base(document, selection)
        {
            this.Title = title;
            this.ListItemAmount = listItemAount;
            this.AddLabels = addLabels;
            this.Name = "List Generator";
        }

        public override void Run()
        {
            // Based on the selection, find where to start
            Selection.FindElements();
            // Check the selection
            if (!Selection.FoundSelection()) {
                NotificationUtil.Inform(NotificationType.Warning, "The task " + this.Name + " did not find a selection. Cancelling Task");
                return;
            }
            // Get the selection
            List<TagTreePointer> selectionTargets = Selection.GetSelection();

            // For each selection target, run the list generation
            foreach (TagTreePointer pointer in selectionTargets) {
                // Add the tag in the specified insertion (handled elsewhere)
                pointer.AddTag("L");

                // Set the title if it exists
                if (this.Title != null && this.Title.Length > 0)
                {
                    // Get the PDF dictionary for the list, add it to tree. PdfName.T represents the title
                    pointer.GetContext().GetPointerStructElem(pointer).Put(PdfName.T, new PdfString(this.Title));
                }

                // For each item to be generated, add the required items
                for (int i = 0; i < this.ListItemAmount; i++)
                {
                    // Add the list item.
                    pointer.AddTag("LI");
                    // Adds the list item body
                    pointer.AddTag("LBody");
                    if (this.AddLabels)
                    {
                        // Needs a label. Since we are in the item body, we need to move up a level
                        pointer.MoveToParent();
                        // Add the label before the list item body
                        pointer.AddTag(0, "Lbl");
                    }
                    // Return to the list parent element for the next iteration
                    pointer.MoveToParent().MoveToParent();
                }

                // Complete, move back to root
                // TO DO - Move this back to where it started
                pointer.MoveToRoot();
                // Mark as complete
                this.TaskComplete = true;
            }

        }
    }
}

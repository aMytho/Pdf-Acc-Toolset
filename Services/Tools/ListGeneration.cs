using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Pdf;
using iText.Layout;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services.Tools
{
    public class ListGeneration : AccessibilityTask
    {
        private string title;
        private int listItemAmount;
        private bool addLabels;

        public ListGeneration(Document document, Selection.Selection selection, string title, int listItemAount, bool addLabels): base(document, selection)
        {
            this.title = title;
            this.listItemAmount = listItemAount;
            this.addLabels = addLabels;
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
                // Update the selection insertion point if necessary
                Selection.MoveSelectionToInsertion(pointer);
                // Add the tag in the specified insertion (handled elsewhere)
                pointer.AddTag("L");

                // Set the title if it exists
                if (this.title != null && this.title.Length > 0)
                {
                    // Get the PDF dictionary for the list, add it to tree. PdfName.T represents the title
                    pointer.GetContext().GetPointerStructElem(pointer).Put(PdfName.T, new PdfString(this.title));
                }

                // For each item to be generated, add the required items
                for (int i = 0; i < this.listItemAmount; i++)
                {
                    // Add the list item.
                    pointer.AddTag("LI");
                    // Adds the list item body
                    pointer.AddTag("LBody");
                    if (this.addLabels)
                    {
                        // Needs a label. Since we are in the item body, we need to move up a level
                        pointer.MoveToParent();
                        // Add the label before the list item body
                        pointer.AddTag(0, "Lbl");
                    }
                    // Return to the list parent element for the next iteration
                    pointer.MoveToParent().MoveToParent();
                }

                // Mark as complete
                this.TaskComplete = true;
            }

        }

        /// <summary>
        /// Returns the amount of list items to be generated
        /// </summary>
        /// <returns></returns>
        public int GetListItemCount()
        {
            return listItemAmount;
        }

        /// <summary>
        /// Returns a bool that represents wether labels will be added
        /// </summary>
        /// <returns></returns>
        public bool GetLabels()
        {
            return addLabels;
        }
    }
}

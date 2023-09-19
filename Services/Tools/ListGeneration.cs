using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Pdf;
using iText.Layout;

namespace Pdf_Acc_Toolset.Services.Tools
{
    public class ListGeneration : AccessibilityTask
    {
        private string Title;
        private int ListItemAmount;
        private bool AddLabels;

        public ListGeneration(Document document, string title, int listItemAount, bool addLabels) : base(document)
        {
            this.Title = title;
            this.ListItemAmount = listItemAount;
            this.AddLabels = addLabels;
            this.Name = "List Generator";
        }

        public override void Run()
        {
            // Get the tag pointer. It starts at the root of the tag tree.
            TagTreePointer tags = this.Document.GetPdfDocument().GetTagStructureContext().GetAutoTaggingPointer();
            // Add the parent list element to the beginning of the tag tree
            tags.AddTag(0, "L");

            // Set the title if it exists
            if (this.Title != null && this.Title.Length > 0)
            {
                // Get the PDF dictionary for the list, add it to tree. PdfName.T represents the title
                tags.GetContext().GetPointerStructElem(tags).Put(PdfName.T, new PdfString(this.Title));
            }

            // For each item to be generated, add the required items
            for (int i = 0; i < this.ListItemAmount; i++)
            {
                // Add the list item.
                tags.AddTag("LI");
                // Adds the list item body
                tags.AddTag("LBody");
                if (this.AddLabels)
                {
                    // Needs a label. Since we are in the item body, we need to move up a level
                    tags.MoveToParent();
                    // Add the label before the list item body
                    tags.AddTag(0, "Lbl");
                }
                // Return to the list parent element for the next iteration
                tags.MoveToParent().MoveToParent();
            }

            // Complete, move back to root
            tags.MoveToRoot();
            // Mark as complete
            this.TaskComplete = true;
        }
    }
}

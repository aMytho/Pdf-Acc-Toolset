using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Pdf;
using iText.Layout;

namespace Pdf_Acc_Toolset.Services.Tools
{
    public class TableGeneration : AccessibilityTask
    {
        private string Title;
        private int Rows;
        private int Cols;

        public TableGeneration(Document document, string title, int rows, int cols) : base(document)
        {
            this.Title = title;
            this.Rows = rows;
            this.Cols = cols;
            this.Name = "Table Generator";
        }

        public override void Run()
        {
            // Get the tag pointer. It starts at the root of the tag tree.
            TagTreePointer tags = this.Document.GetPdfDocument().GetTagStructureContext().GetAutoTaggingPointer();

            // Add the parent table element to the beginning of the tag tree
            tags.AddTag(0, "Table");

            // Set the ID if it exists
            if (this.Title != null && this.Title.Length > 0)
            {
                // Get the PDF dictionary for the table, add it to tree. PdfName.T represents the title
                tags.GetContext().GetPointerStructElem(tags).Put(PdfName.T, new PdfString(this.Title));
            }

            // For each row to be generated, add the required columns
            for (int i = 0; i < this.Rows; i++)
            {
                // Add the table row
                tags.AddTag("TR");
                // Adds the table column cells
                for (int j = 0; j < this.Cols; j++)
                {
                    // If this is the first row, add header cells
                    if (i == 0)
                    {
                        tags.AddTag("TH");
                    }
                    else
                    {
                        // Its not a header row, so we add normal data cells.
                        // Alternatively, if column headers are enabled the first column will be a header cell
                        if (j == 0)
                        {
                            tags.AddTag("TH");
                        }
                        else
                        {
                            // No row header, use normal cell
                            tags.AddTag("TD");
                        }
                    }

                    // Return to the parent row for the next iteration (add more columns)
                    tags.MoveToParent();
                }

                // Return to the parent table element for the next iteration (add more rows)
                tags.MoveToParent();
            }

            // Complete. Move back to root in case of other operations
            tags.MoveToRoot();
        }
    }
}

// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagutils;
using iText.Layout;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services.Tools;

public class TableGeneration : AccessibilityTask
{
    private string title;
    private int rows;
    private int cols;

    public TableGeneration(Document document, Selection.Selection selection, string title, int rows, int cols) : base(document, selection)
    {
        this.title = title;
        this.rows = rows;
        this.cols = cols;
        this.Name = "Table Generator";
    }

    public override void Run()
    {
        // Based on the selection, find where to start
        Selection.FindElements();
        // Check the selection
        if (!Selection.FoundSelection())
        {
            NotificationUtil.Inform(NotificationType.Warning, "The task " + this.Name + " did not find a selection. Cancelling Task");
            return;
        }
        // Get the selection
        List<TagTreePointer> selectionTargets = Selection.GetSelection();

        // For each selection target, run the list generation
        foreach (TagTreePointer pointer in selectionTargets)
        {
            // Update the selection insertion point if necessary
            Selection.MoveSelectionToInsertion(pointer);

            // Add the parent table element to the beginning of the tag tree
            pointer.AddTag("Table");

            // Set the ID if it exists
            if (this.title != null && this.title.Length > 0)
            {
                // Get the PDF dictionary for the table, add it to tree. PdfName.T represents the title
                pointer.GetContext().GetPointerStructElem(pointer).Put(PdfName.T, new PdfString(this.title));
            }

            // For each row to be generated, add the required columns
            for (int i = 0; i < this.rows; i++)
            {
                // Add the table row
                pointer.AddTag("TR");
                // Adds the table column cells
                for (int j = 0; j < this.cols; j++)
                {
                    // If this is the first row, add header cells
                    if (i == 0)
                    {
                        pointer.AddTag("TH");
                    }
                    else
                    {
                        // Its not a header row, so we add normal data cells.
                        // Alternatively, if column headers are enabled the first column will be a header cell
                        if (j == 0)
                        {
                            pointer.AddTag("TH");
                        }
                        else
                        {
                            // No row header, use normal cell
                            pointer.AddTag("TD");
                        }
                    }

                    // Return to the parent row for the next iteration (add more columns)
                    pointer.MoveToParent();
                }

                // Return to the parent table element for the next iteration (add more rows)
                pointer.MoveToParent();
            }

            // Mark as complete
            TaskComplete = true;
        }
    }

    /// <summary>
    /// Returns the amount of rows to be generated
    /// </summary>
    /// <returns></returns>
    public int GetRowCount()
    {
        return rows;
    }

    /// <summary>
    /// Returns the amount of columns to be generated
    /// </summary>
    /// <returns></returns>
    public int GetColumnCount()
    {
        return cols;
    }
}

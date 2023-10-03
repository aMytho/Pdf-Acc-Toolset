using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagutils;
using iText.Layout;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services.Tools
{
    public class TagGeneration : AccessibilityTask
    {
        /// <summary>
        /// The group title for the elements to be generated. May be empty
        /// </summary>
        private string Title;
        /// <summary>
        /// The amount of tags to generate
        /// </summary>
        private int Count;

        /// <summary>
        /// The type of tag to generate
        /// </summary>
        private TagType Tag;

        public TagGeneration(Document document, Selection.Selection selection, string title, TagType tag, int count) : base(document, selection)
        {
            this.Title = title;
            this.Tag = tag;
            this.Count = count;
            this.Name = "Tag Generator";
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
                
                // Get the role for our enum (string representation of the enum)
                string tagRole = TagUtil.GetTagByEnum(Tag);

                // Add a title/grouping div if a title was requested
                if (Title != null && Title.Length > 0)
                {
                    // Add the div
                    pointer.AddTag("Div");
                    // Set its title
                    pointer.GetContext().GetPointerStructElem(pointer).Put(PdfName.T, new PdfString(Title));
                }

                // For each count, add the tag to the tree
                int i = 0;
                while (i < Count)
                {
                    AddTag(pointer, tagRole);
                    i++;
                }

                // Mark as complete
                TaskComplete = true;
            }
        }

        private void AddTag(TagTreePointer tree, string role) {
            // Add the tag, return to root
            tree.AddTag(role);
            tree.MoveToParent();
        }
    }
}
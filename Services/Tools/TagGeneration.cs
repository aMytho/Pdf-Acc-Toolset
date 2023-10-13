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
        private string title;
        /// <summary>
        /// The amount of tags to generate
        /// </summary>
        private int count;

        /// <summary>
        /// The type of tag to generate
        /// </summary>
        private TagType tag;

        public TagGeneration(Document document, Selection.Selection selection, string title, TagType tag, int count) : base(document, selection)
        {
            this.title = title;
            this.tag = tag;
            this.count = count;
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
                string tagRole = TagUtil.GetTagByEnum(tag);

                // Add a title/grouping div if a title was requested
                if (title != null && title.Length > 0)
                {
                    // Add the div
                    pointer.AddTag("Div");
                    // Set its title
                    pointer.GetContext().GetPointerStructElem(pointer).Put(PdfName.T, new PdfString(title));
                }

                // For each count, add the tag to the tree
                int i = 0;
                while (i < count)
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

        /// <summary>
        /// Returns the amount of tags to be generated
        /// </summary>
        /// <returns></returns>
        public int GetTagCount()
        {
            return count;
        }

        /// <summary>
        /// Returns the type of tag to be generated
        /// </summary>
        /// <returns></returns>
        public TagType GetTagType()
        {
            return tag;
        }
    }
}
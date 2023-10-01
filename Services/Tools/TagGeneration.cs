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
            // Get the tag pointer
            TagTreePointer tags = Document.GetPdfDocument().GetTagStructureContext().GetAutoTaggingPointer();
            
            // Get the role for our enum (string representation of the enum)
            string tagRole = TagUtil.GetTagByEnum(Tag);

            // Add a title/grouping div if a title was requested
            if (Title != null && Title.Length > 0)
            {
                // Add the div
                tags.AddTag(0, "Div");
                // Set its title
                tags.GetContext().GetPointerStructElem(tags).Put(PdfName.T, new PdfString(Title));
            }

            // For each count, add the tag to the tree
            int i = 0;
            while (i < Count)
            {
                AddTag(tags, tagRole);
                i++;
            }

            // Return to root
            tags.MoveToRoot();

            // Mark as complete
            TaskComplete = true;
        }

        private void AddTag(TagTreePointer tree, string role) {
            // Add the tag, return to root
            tree.AddTag(0, role);
            tree.MoveToParent();
        }
    }
}
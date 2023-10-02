using iText.Kernel.Pdf.Tagutils;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services.Tools.Selection
{
    public class TagSelection : Selection
    {
        protected TagType tagType;
        public TagSelection(TagTreePointer input, InsertionPoint ins, TagType tagType, int limit = -1) : base()
        {
            this.inputTags = input;
            this.limit = limit;
            this.point = ins;
            this.tagType = tagType;
        }

        public override void FindElements()
        {
            // Create new empty list, request all tags of requested type
            List<TagTreePointer> matchingTags = new();
            List<TagTreePointer> nodes = TagUtil.GetTagsByType(tagType, inputTags);
            
            // Add each node
            foreach (TagTreePointer node in nodes) {
                // Make sure that we are not violating the limit
                if (matchingTags.Count + 1 > this.limit) {
                    break;
                }

                // Add the pointer
                matchingTags.Add(node);
            }
            
            // Set result
            this.selection = matchingTags;
            Console.WriteLine("Selection items: " + this.selection.Count);
            if (this.selection.Count > 0) {
                this.foundSelection = true;
            }

            this.MoveSelectionToInsertion();
        }
    }
}
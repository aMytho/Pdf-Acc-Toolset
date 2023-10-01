
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;

namespace Pdf_Acc_Toolset.Services.Tools.Selection
{
    public class AttributeSelection : Selection
    {
        private Attribute atr;
        private string val;

        public AttributeSelection(TagTreePointer input, InsertionPoint ins, Attribute atr, string val, int limit = -1) : base() {
            this.inputTags = input;
            this.atr = atr;
            this.val = val;
            this.limit = limit;
            this.point = ins;
        }

        public override void FindElements()
        {
            // List which will be returned with the tags
            List<TagTreePointer> matchingTags = new();

            // Start searching
            CheckChildElement(inputTags);

            // Recursive function to check all tags in the tag tree
            void CheckChildElement(TagTreePointer children)
            {
                Console.WriteLine("The tag: " + children.GetRole());
                Console.WriteLine("The tag: " + children.GetProperties().GetActualText());
                // Check the current child
                if (CheckTag(children))
                {
                    // Get the element for the pointer
                    PdfStructElem elem = children.GetContext().GetPointerStructElem(children);
                    // Make a new pointer for the elem. Circular way, but pretty neat!
                    matchingTags.Add(children.GetContext().CreatePointerForStructElem(elem));
                }

                // Check if it has kids
                // Get each child
                IList<IStructureNode> childKids = children.GetContext().GetPointerStructElem(children).GetKids();

                // More kids to check
                if (childKids != null && childKids.Count > 0) {
                    Console.WriteLine("am child " + childKids.Count);
                    // Make a new pointer for the current tag
                    for (int i = 0; i < childKids.Count; i++)
                    {
                        Console.WriteLine(childKids[i].GetType() == typeof(PdfStructElem));
                        // If the kid is null or content, skip it
                        if (childKids[i] == null || childKids[i].GetType() != typeof(PdfStructElem)) {
                            continue;
                        }

                        // Get the element for the pointer
                        PdfStructElem elem = children.GetContext().GetPointerStructElem(children);
                        // Make a new pointer for the elem.
                        TagTreePointer childPointer = children.GetContext().CreatePointerForStructElem(elem);
                        CheckChildElement(childPointer.MoveToKid(i));
                    }
                }
            }

            // Set result
            this.selection = matchingTags;
            if (this.selection.Count > 0) {
                this.foundSelection = true;
            }
        }

        private bool CheckTag(TagTreePointer tag)
        {
            Console.WriteLine(atr + " is insertion");
            if (atr == Attribute.Id) {
                // Get the ID
                byte[] buffer = tag.GetProperties().GetStructureElementId();
                // Handle it being null (no id)
                if (buffer == null) {
                    return false;
                }

                // convert it to a string
                try {
                    string id = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    // Check if its a match
                    return id.Equals(val);
                } catch(Exception e) {
                    Console.WriteLine("Error converting tag ID to string");
                    Console.WriteLine(e);
                    return false;
                }
            } else if (atr == Attribute.Title) {
                // Get the title of the tag
                // PDFName.T is the title attribute
                PdfString title = tag.GetContext().GetPointerStructElem(tag).GetPdfObject().GetAsString(PdfName.T);
                // Check if its a match
                return title != null && title.ToString().Equals(val);
            }

            // False by default, handles any other case
            return false;
        }
    }

    public enum Attribute
    {
        Title,
        Id
    }
}
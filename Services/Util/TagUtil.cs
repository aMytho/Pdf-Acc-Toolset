using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Pdf;

namespace Pdf_Acc_Toolset.Services.Util
{
    internal class TagUtil
    {
        private static PdfDictionary roleMap;
        public static void SetRoleMap(PdfDictionary dict) { roleMap = dict; }

        public static List<IStructureNode> GetTagsByType(TagType tagType, TagTreePointer pointer)
        {
            string tagToFind = GetTagByEnum(tagType);

            // Create a copy of the current tag
            PdfStructElem currentTag = pointer.GetContext().GetPointerStructElem(pointer);

            // List which will be returned with the tags
            List<IStructureNode> matchingTags = new();

            // Check if the current tag is the one we are looking for.
            // We return the elem itself and any kids that match the description.
            if (ConvertRole(currentTag.GetRole()) == CombineTagName(tagToFind))
            {
                matchingTags.Add(currentTag);
            }

            // Get each child
            IList<IStructureNode> children = currentTag.GetKids();

            // Check each child
            if (children != null && children.Count > 0)
            {
                CheckChildElement(children);
            }

            // Recursive function to check all tags in the tag tree
            void CheckChildElement(IList<IStructureNode> children)
            {
                foreach (IStructureNode child in children)
                {
                    // Check the element itself
                    if (ConvertRole(child.GetRole()) == CombineTagName(tagToFind))
                    {
                        matchingTags.Add(child);
                    }

                    // Check kids
                    if (child.GetKids() != null && child.GetKids().Count > 0)
                    {
                        // Element has kids. Check them
                        CheckChildElement(child.GetKids());
                    }
                }
            }

            // Return result
            return matchingTags;
        }

        public static string CombineTagName(string tagName)
        {
            // In the tag tree, roles (tagnames) have a starting "/". *Sigh*
            return "/" + tagName;
        }

        public static string ConvertRole(PdfName tagName)
        {
            // Converts a role. Some tags are mapped to other tags, this function returns the new value
            if (roleMap.Get(tagName) == null)
            {
                // Its a normal tag, no mapping
                return tagName.ToString();
            }
            else
            {
                // Its a mapped tag, return its real value
                return roleMap.Get(tagName).ToString();
            }

        }

        public static string GetTagByEnum(TagType tagType)
        {
            return tagType switch
            {
                TagType.Document => "Document",
                TagType.Part => "Part",
                TagType.Div => "Div",
                TagType.Art => "Art",
                TagType.Sect => "Sect",
                TagType.P => "P",
                TagType.H1 => "H1",
                TagType.H2 => "H2",
                TagType.H3 => "H3",
                TagType.H4 => "H4",
                TagType.H5 => "H5",
                TagType.H6 => "H6",
                TagType.L => "L",
                TagType.Li => "LI",
                TagType.Lbl => "Lbl",
                TagType.LBody => "LBody",
                TagType.BlockQuote => "BlockQuote",
                TagType.Caption => "Caption",
                TagType.Index => "Index",
                TagType.TOC => "TOC",
                TagType.TOCI => "TOCI",
                TagType.Table => "Table",
                TagType.TR => "TR",
                TagType.TD => "TD",
                TagType.TH => "TH",
                TagType.BibEntry => "BibEntry",
                TagType.Quote => "Quote",
                TagType.Span => "Span",
                TagType.Code => "Code",
                TagType.Figure => "Figure",
                TagType.Form => "Form",
                TagType.Formula => "Formula",
                TagType.Link => "Link",
                TagType.Note => "Note",
                TagType.Reference => "Reference",
                _ => throw new ArgumentOutOfRangeException(nameof(tagType), tagType, "Invalid tag type"),
            };
        }

        public static string GetTagByEnum(TagType tagType, bool combineTag)
        {
            string tag = GetTagByEnum(tagType);
            if (combineTag)
                return "/" + tag;
            return tag;
        }
    }

    public enum TagType
    {
        // Root tag
        Document,
        // Various dividers for content. Purely for the remediatior, has no effect on accessibility
        Part, Div, Art, Sect,
        // Paragraph
        P,
        // Headers
        H1, H2, H3, H4, H5, H6,
        // Lists
        L, Li, Lbl, LBody,
        BlockQuote,
        Caption,
        Index,
        // Table of contents (not an actual table)
        TOC, TOCI,
        // Actual Tables :)
        Table, TR, TD, TH,
        BibEntry,
        Quote,
        Span,
        Code,
        // Images
        Figure,
        Form,
        Formula,
        Link,
        Note,
        Reference,
    }
}

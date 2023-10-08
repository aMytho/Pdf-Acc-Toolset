using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagutils;
using iText.Layout;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services.Tools
{
    public class AttributeModifier : AccessibilityTask
    {
        private Selection.Attribute attribute;

        private AttributeAction action;

        private string value;

        public AttributeModifier(Document document, Selection.Selection selection, Selection.Attribute attr, AttributeAction act, string val): base(document, selection)
        {
            this.attribute = attr;
            this.action = act;
            this.value = val;
            this.Name = "Attribute Modifier";
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

                if (action == AttributeAction.Add) {
                    Add(attribute, pointer);
                } else if (action == AttributeAction.Edit) {
                    Edit(attribute, pointer);
                } else {
                    Remove(attribute, pointer);
                }
            }
        }

        private void Add(Selection.Attribute attr, TagTreePointer tag)
        {
            switch (attr)
            {
                case Tools.Selection.Attribute.Title:
                    byte[] buffer = tag.GetProperties().GetStructureElementId();
                    if (buffer == null || buffer.Length == 0) {
                        Edit(attr, tag);
                    }
                    break;
                case Tools.Selection.Attribute.Id:
                    byte[] id = tag.GetProperties().GetStructureElementId();
                    if (id == null || id.Length == 0) {
                        Edit(attr, tag);
                    }
                break;
                case Tools.Selection.Attribute.ActualText:
                    string actText = tag.GetProperties().GetActualText();
                    if (actText == null || actText.Length == 0) {
                        Edit(attr, tag);
                    }
                break;
                case Tools.Selection.Attribute.AltText:
                    string alt = tag.GetProperties().GetAlternateDescription();
                    if (alt == null || alt.Length == 0) {
                        Edit(attr, tag);
                    }
                break;
            }
        }

        private void Edit(Selection.Attribute attr, TagTreePointer tag)
        {
            switch (attr)
            {
                case Tools.Selection.Attribute.Title:
                    tag.GetContext().GetPointerStructElem(tag).Put(PdfName.T, new PdfString(value));
                break;
                case Tools.Selection.Attribute.Id:
                    tag.GetProperties().SetStructureElementIdString(value);
                break;
                case Tools.Selection.Attribute.ActualText:
                    tag.GetProperties().SetActualText(value);
                break;
                case Tools.Selection.Attribute.AltText:
                    tag.GetProperties().SetAlternateDescription(value);                    
                break;
            }
        }

        private void Remove(Selection.Attribute attr, TagTreePointer tag) {
            switch (attr)
            {
                case Tools.Selection.Attribute.Title:
                    tag.GetContext().GetPointerStructElem(tag).Put(PdfName.T, new PdfString(""));
                break;
                case Tools.Selection.Attribute.Id:
                    tag.GetProperties().SetStructureElementIdString(null);
                break;
                case Tools.Selection.Attribute.ActualText:
                    tag.GetProperties().SetActualText(null);
                break;
                case Tools.Selection.Attribute.AltText:
                    tag.GetProperties().SetAlternateDescription(null);
                break;
            }
        }
    }

    /// <summary>
    /// The action to perform on each tag attribute
    /// </summary>
    public enum AttributeAction
    {
        /// <summary>
        /// Adds attribute if no data is present
        /// </summary>
        Add,
        /// <summary>
        /// Sets a modifier value
        /// </summary>
        Edit,
        /// <summary>
        /// Removes an attribute
        /// </summary>
        Remove
    }
}
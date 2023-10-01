
using iText.Kernel.Pdf.Tagutils;
using Pdf_Acc_Toolset.Services.Tools.Selection;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Pages.Selection
{
    public static class SelectionValidator
    {
        public static bool Validate(Selection.SelectionModel model)
        {
            // Check limit
            if (model.Limit < 1) {
                NotificationUtil.Inform(NotificationType.Error, "Invalid limit. Limit must be greater than 1");
                return false;
            }

            // Based on selection, check the data entered
            if (model.selection.type == SelectionType.SSelectionType.Tag) {
                if (!Enum.IsDefined(typeof(TagType), model.selection.tag))
                {
                    NotificationUtil.Inform(NotificationType.Error, "Invalid tag. Select a valid tag for targeting.");
                    return false;
                }
            } else if (model.selection.type == SelectionType.SSelectionType.Attribute) {
                if (model.selection.attrWithVal.val == null || model.selection.attrWithVal.val.Length < 1) {
                    NotificationUtil.Inform(NotificationType.Error, "Invalid attribute value. You must provide a value");
                    return false;
                }
            }

            // Probably fine
            return true;
        }

        public static Services.Tools.Selection.Selection CreateSelection(Selection.SelectionModel model, TagTreePointer tag)
        {
            return model.selection.type switch
            {
                SelectionType.SSelectionType.Attribute or SelectionType.SSelectionType.Tag => new AttributeSelection(tag, model.InsertionPoint, model.selection.attrWithVal.attr, model.selection.attrWithVal.val, model.Limit),
                _ => null,
            };
        }
    }
}
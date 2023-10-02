
using iText.Kernel.Pdf.Tagutils;
using Pdf_Acc_Toolset.Services.Tools.Selection;
using Pdf_Acc_Toolset.Services.Util;
using Pdf_Acc_Toolset.Services;

namespace Pdf_Acc_Toolset.Pages.Selection
{
    public static class SelectionValidator
    {
        /// <summary>
        /// Validates a selection model. Shows warnings to the user if invalid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Validate(Selection.SelectionModel model)
        {
            // Check limit
            if (model.Limit < 1) {
                NotificationUtil.Inform(NotificationType.Error, "Invalid limit. Limit must be greater than 1");
                return false;
            }

            // Based on selection, check the data entered
            // Handle tag errors
            if (model.selection.type == SelectionType.SSelectionType.Tag) {
                Console.WriteLine(model.selection.type + " is the type");
                Console.WriteLine(Enum.IsDefined(typeof(TagType), model.selection.tag));
                if (!Enum.IsDefined(typeof(TagType), model.selection.tag))
                {
                    NotificationUtil.Inform(NotificationType.Error, "Invalid tag. Select a valid tag for targeting.");
                    return false;
                }
            }
            // Handle attribute errors
            if (model.selection.type == SelectionType.SSelectionType.Attribute) {
                if (model.selection.attrWithVal.val == null || model.selection.attrWithVal.val.Length < 1) {
                    NotificationUtil.Inform(NotificationType.Error, "Invalid attribute value. You must provide a value");
                    return false;
                }
            }

            if (
                model.selection.type == SelectionType.SSelectionType.Default
                || model.selection.type == SelectionType.SSelectionType.Tag
                )
            {
                // Prevent inserting adjacent to the document root
                if (
                    (model.InsertionPoint == Services.Tools.Selection.InsertionPoint.BeforeSelection
                    || model.InsertionPoint == Services.Tools.Selection.InsertionPoint.AfterSelection)
                    && (model.selection.type == SelectionType.SSelectionType.Default
                    || model.selection.tag == TagType.Document)
                    )
                {
                    NotificationUtil.Inform(NotificationType.Error,
                    "You cannot use Before Selection or After Selection on the Document Root. Try a child selection instead!");
                    return false;
                }
            }

            // Probably fine
            return true;
        }

        /// <summary>
        /// Based on the model type, create the proper selection class
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tag">The root tag</param>
        /// <returns></returns>
        public static Services.Tools.Selection.Selection CreateSelection(Selection.SelectionModel model, TagTreePointer tag)
        {
            return model.selection.type switch
            {
                SelectionType.SSelectionType.Default => new DefaultSelection(tag, model.InsertionPoint, model.Limit),
                SelectionType.SSelectionType.Attribute => new AttributeSelection(tag, model.InsertionPoint, model.selection.attrWithVal.attr, model.selection.attrWithVal.val, model.Limit),
                SelectionType.SSelectionType.Tag => new TagSelection(tag, model.InsertionPoint, model.selection.tag, model.Limit),
                _ => null
            };
        }

        public static bool CheckSelectionMatches(Selection.SelectionModel selectionModel)
        {
            // Validate selection
            if (!Validate(selectionModel))
            {
                return false;
            }

            // Create selection class from model
            TagTreePointer pointer = PdfManager.GetTagRoot();
            Services.Tools.Selection.Selection selection = CreateSelection(selectionModel, pointer);
            if (selection == null)
            {
                NotificationUtil.Inform(NotificationType.Error, "No selection class matches the input.");
                return false;
            }

            // See if any matches are found
            selection.FindElements();
            if (selection.FoundSelection())
            {
                NotificationUtil.Inform(
                    NotificationType.Success, "Found " + selection.GetSelection().Count + " Element(s)"
                );
                return true;
            } else {
                NotificationUtil.Inform(NotificationType.Warning, "No elements found!");
                return false;
            }
        }
    }
}
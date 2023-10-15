using iText.Layout;
using Pdf_Acc_Toolset.Services.Tools.Selection;

namespace Pdf_Acc_Toolset.Services
{
    /// <summary>
    /// An accessibility task. This will modify the PDF in some way.
    /// </summary>
    public abstract class AccessibilityTask
    {
        /// <summary>
        /// The friendly name of the task. Displayed in UI
        /// </summary>
        public string Name;

        /// <summary>
        /// The PDF document. Primarily used for modifications to the PDF
        /// </summary>
        public Document Document;

        /// <summary>
        /// Is the task complete?
        /// </summary>
        public bool TaskComplete = false;

        /// <summary>
        /// The selection for the task. May contain multiple items
        /// </summary>
        public Selection Selection;

        public AccessibilityTask(Document document, Selection selection)
        {
            this.Document = document;
            this.Selection = selection;
        }

        /// <summary>
        /// Run the Accessibility Task
        /// </summary>
        public abstract void Run();
    }
}

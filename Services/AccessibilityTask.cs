using iText.Layout;

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

		public AccessibilityTask(Document document)
		{
			this.Document = document;
		}

		public abstract void Run();
	}
}

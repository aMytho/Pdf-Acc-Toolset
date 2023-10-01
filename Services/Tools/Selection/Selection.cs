
using iText.Kernel.Pdf.Tagutils;

namespace Pdf_Acc_Toolset.Services.Tools.Selection
{
    public abstract class Selection
    {
        private string name;

        protected int limit;

        protected TagTreePointer inputTags;

        protected List<TagTreePointer> selection;

        protected bool foundSelection = false;

        protected InsertionPoint point = InsertionPoint.FirstChild;

        public Selection() {}

        public abstract void FindElements();

        public bool FoundSelection()
        {
            return foundSelection;
        }

        public List<TagTreePointer> GetSelection()
        {
            return selection;
        }

        public InsertionPoint GetInsertion()
        {
            return point;
        }
    }

    public enum InsertionPoint {
        BeforeSelection,
        AfterSelection,
        FirstChild,
        LastChild
    }
}
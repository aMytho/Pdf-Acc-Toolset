
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

        protected void MoveSelectionToInsertion()
        {
            foreach (TagTreePointer pointer in selection) {
                switch (point)
                {
                    case InsertionPoint.FirstChild:
                        // Set the next insertion to the last child
                        pointer.SetNextNewKidIndex(0);
                    break;
                    case InsertionPoint.LastChild:
                        // Default behavior, do nothing
                    break;
                    case InsertionPoint.BeforeSelection:
                        // TO DO
                    break;
                    case InsertionPoint.AfterSelection:
                        int indexAfter = pointer.GetIndexInParentKidsList();
                        Console.WriteLine(indexAfter);
                        Console.WriteLine(pointer.GetRole());
                        pointer.MoveToParent();
                        Console.WriteLine(pointer.GetRole());
                        pointer.SetNextNewKidIndex(indexAfter + 1);
                    break;
                    default:
                    break;
                }
            }
        }
    }

    public enum InsertionPoint {
        BeforeSelection,
        AfterSelection,
        FirstChild,
        LastChild
    }
}
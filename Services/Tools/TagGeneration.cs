using iText.Layout;
using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services.Tools
{
    public class TagGeneration : AccessibilityTask
    {
        private string Title;
        private int Count;
        private TagType Tag;

        public TagGeneration(Document document, string title, TagType tag, int count) : base(document)
        {
            this.Title = title;
            this.Tag = tag;
            this.Count = count;
            this.Name = "Table Generator";
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}
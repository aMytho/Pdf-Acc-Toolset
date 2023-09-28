
namespace Pdf_Acc_Toolset.Services.UI
{
    public class TocService
    {
        /// <summary>
        /// Emits when the TOC changes
        /// </summary>
        public static EventHandler<List<TocItem>> tocChanged;

        /// <summary>
        /// Sets the new TOC
        /// </summary>
        /// <param name="list"></param>
        public static void SetToc(List<TocItem> list) {
            tocChanged.Invoke(null, list);
        }
    }

    public struct TocItem
    {
        public string id;
        public int header;
        public string label;

        public TocItem(string id, int header, string label)
        {
            this.id = id;
            this.header = header;
            this.label = label;
        }
    }
}
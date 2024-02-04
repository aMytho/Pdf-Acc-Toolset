// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
namespace Pdf_Acc_Toolset.Services.UI;

public class TagInspectorService
{
    public static event Action PdfReady;
    public static EventHandler<UserTagSelection> TagSelected { get; set; }

    public static void NotifyPdfReady()
    {
        PdfReady?.Invoke();
    }

    public static void NotifyTagSelection(string tagType, string altText, string actualText, string id, string title)
    {
        TagSelected.Invoke(null, new UserTagSelection(tagType, altText, actualText, id, title));
    }
}

public class UserTagSelection(string tagType, string altText, string actualText, string id, string title)
{
    public string TagType = tagType ?? "None";
    public string AltText = altText ?? "None";
    public string ActualText = actualText ?? "None";
    public string ID = id ?? "None";
    public string Title = title ?? "None";
}
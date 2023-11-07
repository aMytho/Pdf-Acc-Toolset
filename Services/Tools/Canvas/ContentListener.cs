// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset

using System.Collections.ObjectModel;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace Pdf_Acc_Toolset.Services.Tools.Canvas;

/// <summary> 
/// A listener which will emit when an event occurs
/// </summary>
public class ContentListener : IEventListener
{
    private readonly Collection<EventType> events;

    /// <summary>
    /// Creates a listener for all types
    /// </summary>
    public ContentListener()
    {
        this.events = new Collection<EventType>() {
            EventType.BEGIN_TEXT, EventType.RENDER_TEXT, EventType.END_TEXT, EventType.RENDER_IMAGE,
            EventType.RENDER_PATH, EventType.CLIP_PATH_CHANGED
        };
    }

    /// <summary> 
    /// Creates a listener for the specified types 
    /// </summary>
    public ContentListener(Collection<EventType> events) {
        this.events = events;
    }

    public void EventOccurred(IEventData data, EventType type) { }

    ICollection<EventType> IEventListener.GetSupportedEvents()
    {
        return events;
    }
}
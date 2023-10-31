// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
namespace Pdf_Acc_Toolset.Services.Util;

/// <summary>
/// A util class for emitting notifications.
/// </summary>
public static class NotificationUtil
{
    /// <summary>
    /// Activates when an notification should occur
    /// </summary>
    public static event EventHandler<Notification> NotificationEvent;

    /// <summary>
    /// Standard message with a timer and no required user interaction
    /// </summary>
    /// <param name="type"></param>
    /// <param name="message"></param>
    public static void Inform(NotificationType type, string message)
    {
        Notification notif = new(type, message);
        NotificationEvent.Invoke(null, notif);
    }
}

/// <summary>
/// Possible notification types
/// </summary>
public enum NotificationType
{
    Error,
    Info,
    Success,
    Warning
}

/// <summary>
/// A notification event
/// </summary>
public struct Notification
{
    public NotificationType type;
    public string message;

    public Notification(NotificationType type, string message)
    {
        this.type = type;
        this.message = message;
    }
}

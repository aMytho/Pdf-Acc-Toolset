using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services
{
    public class TaskManager
    {
        /// <summary>
        /// List of tasks
        /// </summary>
        private static readonly List<AccessibilityTask> Tasks = new();

        public static void RunQueuedTasks()
        {
            Console.WriteLine("Running all tasks");
            foreach (AccessibilityTask task in Tasks)
            {
                task.Run();
            }
            Console.WriteLine("Task Queue Complete");
            NotificationUtil.Inform(NotificationType.Success, "Task Queue Complete");
        }

        public static void RunTask(int index) {
            try
            {
                Tasks[index].Run();
                Console.WriteLine("Task at index: " + index + " ran successfully!");
                NotificationUtil.Inform(NotificationType.Success, "Task ran successfully");
            }
            catch (Exception)
            {
                Console.WriteLine("Attempted to run task at index: " + index + ". FAILED");
            }
        }

        public static void AddTask(AccessibilityTask task)
        {
            Tasks.Add(task);
            Console.WriteLine("Task Created!");
            NotificationUtil.Inform(NotificationType.Success, "Task Created!");
        }

        public static List<AccessibilityTask> GetAccessibilityTasks()
        {
            return Tasks;
        }

        /// <summary>
        /// Removes a task with the specified index
        /// </summary>
        /// <param name="index"></param>
        public static void RemoveTask(int index) {
            try {
                Tasks.RemoveAt(index);
                Console.WriteLine("Removing task at index: " + index);
                NotificationUtil.Inform(NotificationType.Success, "Task Removed");
            } catch (ArgumentOutOfRangeException) {
                Console.WriteLine("Error: Tried to remove a task with an invalid index.");
                NotificationUtil.Inform(NotificationType.Error, "Task not removed.");
            }
        }

        public static void ReOrderTask(int toBeMoved, MoveTask direction) {

        }

        public static void ModifyTaskStatus(int index, bool markComplete) {
            try
            {
                Tasks[index].TaskComplete = markComplete;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to modify the status of a task at index: " + index);
            }
        }
    }

    /// <summary>
    /// Moving a task up or down the list...
    /// </summary>
    public enum MoveTask {
        Up,
        Down
    }
}

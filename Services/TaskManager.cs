using Pdf_Acc_Toolset.Services.Util;

namespace Pdf_Acc_Toolset.Services
{
    public class TaskManager
    {
        /// <summary>
        /// List of tasks
        /// </summary>
        private static readonly List<AccessibilityTask> Tasks = new();

        private static int TasksRan = 0;

        public static void RunQueuedTasks()
        {
            Console.WriteLine("Running all tasks");
            foreach (AccessibilityTask task in Tasks)
            {
                // Only run tasks that are not complete
                if (!task.TaskComplete) {
                    task.Run();
                    TasksRan++;
                }
            }
            Console.WriteLine("Task Queue Complete");
            NotificationUtil.Inform(NotificationType.Success, "Task Queue Complete");
        }

        public static void RunTask(int index)
        {
            try
            {
                Tasks[index].Run();
                TasksRan++;
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
            NotificationUtil.Inform(NotificationType.Info, "Task Created!");
        }

        public static List<AccessibilityTask> GetAccessibilityTasks()
        {
            return Tasks;
        }

        /// <summary>
        /// Removes a task with the specified index
        /// </summary>
        /// <param name="index"></param>
        public static void RemoveTask(int index)
        {
            try {
                Tasks.RemoveAt(index);
                Console.WriteLine("Removing task at index: " + index);
                NotificationUtil.Inform(NotificationType.Success, "Task Removed");
            } catch (ArgumentOutOfRangeException) {
                Console.WriteLine("Error: Tried to remove a task with an invalid index.");
                NotificationUtil.Inform(NotificationType.Error, "Task not removed.");
            }
        }

        public static bool MoveTask(int toBeMoved, MoveTask direction)
        {
            try {
                // Check the direction to move
                if (direction == Services.MoveTask.Up) {
                    // Move up the queue
                    if (toBeMoved - 1 < 0) {
                        // Can't move before 0
                        throw new Exception("Cannot move task any higher in the queue.");
                    }
                    // Swap places
                    AccessibilityTask temp = Tasks[toBeMoved - 1];
                    Tasks[toBeMoved - 1] = Tasks[toBeMoved];
                    Tasks[toBeMoved] = temp;
                } else {
                    // Move down the queue
                    if (toBeMoved + 1 > Tasks.Count - 1) {
                        throw new Exception("Cannot move task any lower in the queue");
                    }
                    // Swap
                    AccessibilityTask temp = Tasks[toBeMoved + 1];
                    Tasks[toBeMoved + 1] = Tasks[toBeMoved];
                    Tasks[toBeMoved] = temp;
                }
                return true;
            } catch(Exception e) {
                Console.WriteLine(e.Message);
                NotificationUtil.Inform(NotificationType.Error, e.Message ?? "Task Move Error.");
                return false;
            }
        }

        public static void ModifyTaskStatus(int index, bool markComplete)
        {
            try
            {
                Tasks[index].TaskComplete = markComplete;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to modify the status of a task at index: " + index);
            }
        }

        public static int GetTasksRanCounter() {
            return TasksRan;
        }

        public static IEnumerable<AccessibilityTask> GetTasksInQueue()
        {
            return Tasks.Where(task => task.TaskComplete == false);
        }

        public static void RemoveTasks()
        {
            Tasks.Clear();
            TasksRan = 0;

            NotificationUtil.Inform(NotificationType.Info, "Task Queue Cleared");
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

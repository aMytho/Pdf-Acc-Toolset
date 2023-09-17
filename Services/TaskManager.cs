using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			foreach (AccessibilityTask task in Tasks)
			{
				task.Run();
			}
		}

		public static void AddTask(AccessibilityTask task)
		{
			Tasks.Add(task);
		}

		public static List<AccessibilityTask> GetAccessibilityTasks()
		{
			return Tasks;
		}
	}
}

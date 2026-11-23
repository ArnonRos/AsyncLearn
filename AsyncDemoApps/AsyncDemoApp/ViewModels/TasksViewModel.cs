using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        private const int MaxRunningTasks = 4;

        private int _taskCounter;
        private int _completedTasksCount;

        public ObservableCollection<TaskItemViewModel> Tasks { get; } = new();

        public int CompletedTasksCount
        {
            get => _completedTasksCount;
            set
            {
                if (_completedTasksCount != value)
                {
                    _completedTasksCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand StartTaskCommand { get; }
        public ICommand ClearAllCommand { get; }
        public ICommand CancelAllCommand { get; }

        public TasksViewModel()
        {
            Title = "משימות אסינכרוניות";
            StartTaskCommand = new Command(async () => await StartNewTaskAsync());
            ClearAllCommand = new Command(async () => await ClearAllAsync());
            CancelAllCommand = new Command(CancelAllTasks);
        }

        private async Task StartNewTaskAsync()
        {
            int runningCount = Tasks.Count(t => t.IsRunning);
            if (runningCount >= MaxRunningTasks)
            {
                await ShowAlertAsync(
                    "הגבלת משימות",
                    $"כבר רצות {MaxRunningTasks} משימות במקביל. המתן לסיום אחת מהן.",
                    "אישור");
                return;
            }

            _taskCounter++;
            TaskItemViewModel newTask = new TaskItemViewModel(_taskCounter);
            Tasks.Add(newTask);

            bool completed = await newTask.RunAsync();
            if (completed)
            {
                CompletedTasksCount++;
            }
        }

        private void CancelAllTasks()
        {
            foreach (TaskItemViewModel task in Tasks)
            {
                if (task.IsCancelEnabled)
                {
                    task.CancelCommand.Execute(null);
                }
            }
        }

        private async Task ClearAllAsync()
        {
            bool confirmed = await ShowConfirmAsync(
                "ניקוי הרשימה",
                "האם אתה בטוח שברצונך לנקות את כל המשימות?",
                "כן",
                "לא");

            if (!confirmed)
            {
                return;
            }

            CancelAllTasks();
            Tasks.Clear();
        }
    }
}

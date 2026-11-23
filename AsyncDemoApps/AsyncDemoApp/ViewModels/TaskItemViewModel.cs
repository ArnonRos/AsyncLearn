using System.Windows.Input;

namespace ViewModels
{
    public class TaskItemViewModel : BaseViewModel
    {
        private readonly CancellationTokenSource _cts = new();
        private readonly int _delayMs = Random.Shared.Next(20, 101);
        private int _taskId;
        private double _progress;
        private string _statusText = "ממתין...";
        private bool _isCancelEnabled = true;

        public int TaskId
        {
            get => _taskId;
            set
            {
                if (_taskId != value)
                {
                    _taskId = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusText
        {
            get => _statusText;
            set
            {
                if (_statusText != value)
                {
                    _statusText = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsCancelEnabled
        {
            get => _isCancelEnabled;
            set
            {
                if (_isCancelEnabled != value)
                {
                    _isCancelEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsRunning { get; private set; }

        public ICommand CancelCommand { get; }

        public TaskItemViewModel(int taskId)
        {
            TaskId = taskId;
            StatusText = $"משימה {taskId}: ממתין...";
            CancelCommand = new Command(CancelTask);
        }

        private void CancelTask()
        {
            _cts.Cancel();
            IsCancelEnabled = false;
        }

        public async Task<bool> RunAsync()
        {
            IsRunning = true;
            try
            {
                for (int i = 1; i <= 100; i++)
                {
                    if (_cts.Token.IsCancellationRequested)
                    {
                        StatusText = $"משימה {TaskId}: בוטלה";
                        return false;
                    }

                    Progress = i / 100.0;
                    StatusText = $"משימה {TaskId}: רצה... {i}%";
                    await Task.Delay(_delayMs);
                }

                StatusText = $"משימה {TaskId}: הסתיימה בהצלחה";
                return true;
            }
            finally
            {
                IsRunning = false;
                IsCancelEnabled = false;
                _cts.Dispose();
            }
        }
    }
}

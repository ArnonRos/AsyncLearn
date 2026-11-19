using System.Windows.Input;

namespace ViewModels
{
    public class AsyncDemoViewModel : BaseViewModel
    {
        private double _progressValue;
        private string _statusMessage = "בחר אופן הרצה כדי לראות את ההבדל";

        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                if (_progressValue != value)
                {
                    _progressValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand StartAsyncCommand { get; }
        public ICommand StartSyncCommand { get; }

        public AsyncDemoViewModel()
        {
            Title = "הדגמת אסינכרוניות";
            StartAsyncCommand = new Command(async () => await RunAsyncWork());
            StartSyncCommand = new Command(RunSyncWork);
        }

        // 1. פעולה אסינכרונית תקינה (לא חוסמת את המסך)
        private async Task RunAsyncWork()
        {
            IsLoading = true;
            ProgressValue = 0;

            for (int i = 1; i <= 10; i++)
            {
                StatusMessage = $"רץ אסינכרונית... {i * 10}% (נסה להקליד למטה!)";
                await Task.Delay(500); // משחרר את ה-UI Thread בזמן ההמתנה
                ProgressValue = i / 10.0;
            }

            StatusMessage = "הפעולה האסינכרונית הסתיימה בהצלחה!";
            IsLoading = false;
        }

        // 2. פעולה סינכרונית שגויה (תוקעת את האפליקציה לגמרי)
        private void RunSyncWork()
        {
            IsLoading = true;
            ProgressValue = 0;

            for (int i = 1; i <= 10; i++)
            {
                StatusMessage = $"רץ סינכרונית... {i * 10}%";
                Thread.Sleep(500); // חוסם את ה-UI Thread! המסך קפוא ולא מתעדכן עד הסוף
                ProgressValue = i / 10.0;
            }

            StatusMessage = "הפעולה הסינכרונית הסתיימה (המסך היה תקוע 5 שניות)";
            IsLoading = false;
        }
    }
}

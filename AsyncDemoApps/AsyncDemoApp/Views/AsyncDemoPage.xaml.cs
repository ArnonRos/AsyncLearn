using ViewModels;

namespace Views
{
    public partial class AsyncDemoPage : ContentPage
    {
        public AsyncDemoPage() : this(new AsyncDemoViewModel())
        {
        }

        public AsyncDemoPage(AsyncDemoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}

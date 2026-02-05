using System.Windows;
using System.Windows.Input;
using calc.ViewModel;

namespace calc.View
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            
            // Add keyboard event handler
            this.PreviewKeyDown += Window1_PreviewKeyDown;
        }
        
        private void Window1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as CalculatorViewModel;
            if (viewModel == null) return;
            
            if (e.Key == Key.Up)
            {
                viewModel.ShowPreviousAnswer();
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                viewModel.ShowNextAnswer();
                e.Handled = true;
            }
        }
    }
}

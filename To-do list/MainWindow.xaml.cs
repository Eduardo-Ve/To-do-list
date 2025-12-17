using System.Windows;
using System.Windows.Input;
using To_do_list.ViewModels;

namespace To_do_list
{
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            // Guardar al cerrar la ventana
            Closing += (s, e) => ViewModel.SaveOnExit();
        }

        // ✅ Window Control Buttons
        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // ✅ Note Editor Events - Guardar cuando pierde el foco
        private void NoteEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            ViewModel.TouchSelected();
        }

        // ✅ Todo Events
        private void TodoCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            ViewModel.TouchSelected();
        }

        private void TodoText_LostFocus(object sender, RoutedEventArgs e)
        {
            ViewModel.TouchSelected();
        }

        // ✅ Permitir Enter para agregar todo
        private void NewTodoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ViewModel.AddTodoCommand.CanExecute(null))
            {
                ViewModel.AddTodoCommand.Execute(null);
                e.Handled = true;
            }
        }

        private void NewTodoTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}

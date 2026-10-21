using Laboranforderung.Daten;
using Laboranforderung.ViewModels;
using Laboranforderung.ViewModels.Editor;
using Laboranforderung.ViewModels.FileMangement;
using Laboranforderung.ViewModels.Main;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Laboranforderung
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMessageReplyer
    {
        private ViewModels.Main.MainWindowViewModel? _viewModel;
  
        public MainWindow()
        {
            DataContextChanged += MainWindow_DataContextChanged;
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel viewmodel = new MainWindowViewModel();
            DataContext = viewmodel;
            FileManager fileManager = FileManager.Instance;
            fileManager.MessageReplyer = this;

            viewmodel.Datensammlung= fileManager.DisplayClone;
           
            fileManager.PropertyChanged += (s, args) =>
            {
                if (args.PropertyName == nameof(FileManager.DisplayClone))
                {
                    _viewModel!.Datensammlung = fileManager.DisplayClone;
                }
            };
        }

        private void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                // Detach event handlers from the old ViewModel
                _viewModel.ToastRequested -= Vm_ToastRequested;
            }

            _viewModel = DataContext as ViewModels.Main.MainWindowViewModel;

            if (_viewModel != null)
            {
                // Attach event handlers to the new ViewModel
                _viewModel.ToastRequested += Vm_ToastRequested;
            }
        }


        private void Vm_ToastRequested(string message)
        {
            var toast = CreateToast(message);
            ToastHost.Children.Add(toast);

            // Fade-In
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));
            toast.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            // Auto-Remove nach 3 Sekunden
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            timer.Tick += (_, _) =>
            {
                timer.Stop();

                // Fade-Out
                var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
                fadeOut.Completed += (_, _) => ToastHost.Children.Remove(toast);
                toast.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            };
            timer.Start();
        }

        private Border CreateToast(string message)
        {
            return new Border
            {
                Background = Brushes.Black,
                Opacity = 0,
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 10),
                Child = new TextBlock
                {
                    Text = message,
                    Foreground = Brushes.White,
                    FontSize = 14
                }
            };
        }

        private void OptionenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OptionenWindow optionenWindow = new OptionenWindow();
            optionenWindow.Owner = this; // Set the owner to the main window
            FileManager.Instance.MessageReplyer = optionenWindow; // Set the MessageReplyer to the OptionenWindow
            optionenWindow.ShowDialog();
            FileManager.Instance.MessageReplyer = this; // Reset the MessageReplyer back to the main window
        }

        private void ProfileBearbeitenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //bool aenderungenErlaubt = Properties.Settings.Default.AllowChanges;
            //if(!aenderungenErlaubt)
            //{
            //    MessageBox.Show("Änderungen sind in den Einstellungen nicht erlaubt.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}

            EditorWindow editorWindow= new EditorWindow();
       


            editorWindow.Owner = this;
            FileManager.Instance.MessageReplyer = editorWindow;
            editorWindow.ShowDialog();
            FileManager.Instance.MessageReplyer = this;
        }

        public MessageBoxResult ReplyMessage(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return MessageBox.Show(this, messageBoxText, caption, button, icon);
        }

    }
}

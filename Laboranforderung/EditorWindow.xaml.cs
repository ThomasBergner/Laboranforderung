using Laboranforderung.Daten;
using Laboranforderung.ViewModels;
using Laboranforderung.ViewModels.Editor;
using Laboranforderung.ViewModels.FileMangement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Laboranforderung
{
    /// <summary>
    /// Interaktionslogik für EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window, IMessageReplyer
    {
        private static readonly TimeSpan DragStartDelay = TimeSpan.FromMilliseconds(200);
        private readonly Stopwatch m_DragDelayTimer = new Stopwatch();

        private Point m_DragStartPoint;
        private object? m_DragSourceItem;
        private EditorViewModel? _viewModel;

        public EditorWindow()
        {
            DataContextChanged += EditorWindow_DataContextChanged;
            InitializeComponent();
            EditorViewModel model = new EditorViewModel();
            //model.ReadDatensammlung(new DemoDatensammlung());
            this.DataContext = model;
            this.Closing += EditorWindow_Closing;
        }

        private void EditorWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_viewModel != null)
            {
                FileManager.Instance.EditValue = _viewModel.GetDatensammlung();
                if(FileManager.Instance.EditorWindow_Closing_Cancelled())
                {
                    e.Cancel = true;
                }
                else
                {     
                    _viewModel.GruppenentryMoved -= viewModel_GruppenentryMoved;
                    _viewModel.ProfilentryMoved -= viewModel_ProfilentryMoved;
                }
            }
        }

        private void EditorWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.GruppenentryMoved -= viewModel_GruppenentryMoved;
                _viewModel.ProfilentryMoved -= viewModel_ProfilentryMoved;
            }

            if (e.NewValue is EditorViewModel viewModel)
            {
                _viewModel = viewModel;
                _viewModel.GruppenentryMoved += viewModel_GruppenentryMoved; ;
                _viewModel.ProfilentryMoved += viewModel_ProfilentryMoved;
            }
            else
            {
                _viewModel = null;
            }
        }

        private void viewModel_ProfilentryMoved(object? sender, EventArgs e)
        {
            if (DataContext is EditorViewModel viewModel)
            {
                if (viewModel.Profilentry != null)
                {
                    ProfilListBox.ScrollIntoView(viewModel.Profilentry);
                    ProfilListBox.Focus();
                }

            }
        }

        private void viewModel_GruppenentryMoved(object? sender, EventArgs e)
        {
            if (DataContext is EditorViewModel viewModel)
            {
                if (viewModel.Gruppenentry != null)
                {
                    GruppenListBox.ScrollIntoView(viewModel.Gruppenentry);
                    GruppenListBox.Focus();
                }

            }
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ListBox listBox)
            {
                return;
            }

            m_DragStartPoint = e.GetPosition(listBox);
            m_DragSourceItem = GetItemAtPosition(listBox, m_DragStartPoint);

            if (m_DragSourceItem != null)
            {
                m_DragDelayTimer.Restart();
            }
            else
            {
                m_DragDelayTimer.Reset();
            }
        }

        private void ListBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is not ListBox listBox)
            {
                return;
            }

            if (e.LeftButton != MouseButtonState.Pressed || m_DragSourceItem == null)
            {
                return;
            }

            if (!m_DragDelayTimer.IsRunning || m_DragDelayTimer.Elapsed < DragStartDelay)
            {
                return;
            }

            Point currentPosition = e.GetPosition(listBox);
            if (Math.Abs(currentPosition.X - m_DragStartPoint.X) < SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(currentPosition.Y - m_DragStartPoint.Y) < SystemParameters.MinimumVerticalDragDistance)
            {
                return;
            }

            if (m_DragSourceItem is Gruppenlaborwertitem || m_DragSourceItem is Gruppenitem || m_DragSourceItem is Profillaborwertitem || m_DragSourceItem is Profilitem)
            {
                DragDrop.DoDragDrop(listBox, m_DragSourceItem, DragDropEffects.Move);
            }

            m_DragDelayTimer.Reset();
            m_DragSourceItem = null;
        }

        private void ListBox_DragOver(object sender, DragEventArgs e)
        {
            if (sender is not ListBox listBox)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            bool allowDrop = (listBox == GruppenListBox && (e.Data.GetDataPresent(typeof(Gruppenitem)) || e.Data.GetDataPresent(typeof(Gruppenlaborwertitem)))) ||
                             (listBox == ProfilListBox && (e.Data.GetDataPresent(typeof(Profillaborwertitem)) || e.Data.GetDataPresent(typeof(Gruppenlaborwertitem)) || e.Data.GetDataPresent(typeof(Profilitem))));

            e.Effects = allowDrop ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        private void GruppenListBox_Drop(object sender, DragEventArgs e)
        {
            if (DataContext is not EditorViewModel viewModel)
            {
                return;
            }

            if (!e.Data.GetDataPresent(typeof(Gruppenitem)) && !e.Data.GetDataPresent(typeof(Gruppenlaborwertitem)) || sender is not ListBox listBox)
            {
                return;
            }

            Gruppenentry? source = (Gruppenentry?)e.Data.GetData(typeof(Gruppenitem));
            source = source ?? (Gruppenentry?)e.Data.GetData(typeof(Gruppenlaborwertitem));

            if (source == null)
            {
                return;
            }

            var target = GetItemAtPosition(listBox, e.GetPosition(listBox)) as Gruppenentry;

            if (target != null && source != target)
            {
                viewModel.HandleGruppenDrop(source, target);
            }
        }

        private void ProfilListBox_Drop(object sender, DragEventArgs e)
        {
            if (DataContext is not EditorViewModel viewModel)
            {
                return;
            }

            if (sender is not ListBox listBox)
            {
                return;
            }

            var target = GetItemAtPosition(listBox, e.GetPosition(listBox)) as Profilentry;
            if (target == null)
            {
                return;
            }

            if (e.Data.GetDataPresent(typeof(Profillaborwertitem)))
            {
                var source = (Profillaborwertitem)e.Data.GetData(typeof(Profillaborwertitem));
                viewModel.HandleProfilDrop(source, target);
                return;
            }
            if (e.Data.GetDataPresent(typeof(Profilitem)))
            {
                var source = (Profilitem)e.Data.GetData(typeof(Profilitem));
                viewModel.HandleProfilDrop(source, target);
                return;
            }


            if (e.Data.GetDataPresent(typeof(Gruppenlaborwertitem)))
            {
                var source = (Gruppenlaborwertitem)e.Data.GetData(typeof(Gruppenlaborwertitem));
                viewModel.HandleGruppenLaborwertDropToProfil(source, target);
            }
        }

        private static object? GetItemAtPosition(ListBox listBox, Point point)
        {
            var hitTest = VisualTreeHelper.HitTest(listBox, point);
            if (hitTest?.VisualHit == null)
            {
                return null;
            }

            DependencyObject current = hitTest.VisualHit;
            while (current != null && current is not ListBoxItem)
            {
                current = VisualTreeHelper.GetParent(current);
            }

            return (current as ListBoxItem)?.DataContext;
        }

        private void SchließenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public MessageBoxResult ReplyMessage(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
           return MessageBox.Show(messageBoxText, caption, button, icon);
        }

        private void DruckenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(_viewModel == null)
            {
                return;
            }
            FileManager.Instance.EditValue = _viewModel.GetDatensammlung();
            PrintWindow printWindow = new PrintWindow();
            printWindow.ShowDialog();
        }
    }
}

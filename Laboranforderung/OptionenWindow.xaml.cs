using Laboranforderung.Daten;
using Laboranforderung.ViewModels;
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
    /// Interaktionslogik für OptionenWindow.xaml
    /// </summary>
    public partial class OptionenWindow : Window, IMessageReplyer
    {
        public OptionenWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.OptionenViewModel();
        }


        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public MessageBoxResult ReplyMessage(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return MessageBox.Show(this, messageBoxText, caption, button, icon);
        }


    }
}

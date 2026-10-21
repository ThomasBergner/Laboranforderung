using Laboranforderung.Daten;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Laboranforderung.ViewModels
{
    public interface IMessageReplyer
    {
        public MessageBoxResult ReplyMessage(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Laboranforderung.Commands
{
    public interface IListCommands
    {
        public ICommand Add { get; }
        public ICommand Edit { get; }
        public ICommand Delete { get; }
        public ICommand MoveUp { get; }
        public ICommand MoveDown { get; }
    }
}

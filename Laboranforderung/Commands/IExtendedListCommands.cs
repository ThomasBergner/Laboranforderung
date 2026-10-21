using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Laboranforderung.Commands
{
    public interface IExtendedListCommands: IListCommands
    {        
        public ICommand AddKategorie { get; }
    }
}

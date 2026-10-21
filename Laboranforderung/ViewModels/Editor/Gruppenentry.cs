using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Laboranforderung.ViewModels.Editor
{
    public abstract class Gruppenentry : INotifyPropertyChanged
    {
        #region Fields
        private bool m_IsExpanded;
        private bool m_HasChildren;
        #endregion
       
        #region Constructor
        #endregion
 
        #region Properties
        public abstract string Name { get; set; }
        public virtual int Indent => 0;
        public bool HasChildren
        {
            get => m_HasChildren; set
            {
                if (m_HasChildren != value)
                {
                    m_HasChildren = value;
                    NotifyPropertyChanged();
                }

            }
        }
        public bool IsExpanded
        {
            get => m_IsExpanded;
            set
            {
                if (m_IsExpanded != value)
                {
                    m_IsExpanded = value;
                    NotifyPropertyChanged();
                    OnIsExpandedChanged();
                }
            }
        }

        #endregion

        #region Methods
        internal virtual void OnIsExpandedChanged() { }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        internal void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

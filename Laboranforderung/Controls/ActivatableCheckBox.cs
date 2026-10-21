using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Laboranforderung.Controls
{
    public class ActivatableCheckBox : CheckBox
    {
        public static readonly DependencyProperty IsActivatableProperty =
            DependencyProperty.Register(
                nameof(IsActivatable),
                typeof(bool),
                typeof(ActivatableCheckBox),
                new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Steuert, ob der Benutzer den CheckState per Maus/Tastatur ändern darf.
        /// Programmgesteuerte Änderungen über IsChecked bleiben möglich.
        /// </summary>
        public bool IsActivatable
        {
            get => (bool)GetValue(IsActivatableProperty);
            set => SetValue(IsActivatableProperty, value);
        }

        protected override void OnToggle()
        {
            if (!IsActivatable)
            {
                return;
            }

            base.OnToggle();
        }
    }
}

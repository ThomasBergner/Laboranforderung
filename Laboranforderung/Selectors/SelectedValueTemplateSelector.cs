using Laboranforderung.Daten;
using Laboranforderung.ViewModels.Editor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Laboranforderung.Selectors
{
    public class SelectedValueTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? GruppeTemplate { get; set; }
        public DataTemplate? ProfilTemplate { get; set; }
        public DataTemplate? WertTemplate { get; set; }
        public DataTemplate? EmptyTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return EmptyTemplate!;

            if (item is Gruppenitem)
                return GruppeTemplate!;
            if (item is Profilitem)
                return ProfilTemplate!;
            if (item is Gruppenlaborwertitem)
                return WertTemplate!;

            return EmptyTemplate!;
        }
    }

}

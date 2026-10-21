using Laboranforderung.Daten;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboranforderung.ViewModels.FileMangement
{
    public interface IDatensammlungEditSource
    {
        public Datensammlung GetEditedDatensammlung();
        public void SetEditDatensammlung(Datensammlung datensammlung);
    }
}

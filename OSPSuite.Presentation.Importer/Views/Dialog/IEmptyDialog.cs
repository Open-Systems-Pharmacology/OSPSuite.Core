using OSPSuite.Presentation.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Views.Dialog
{
   public interface IEmptyDialog
   {
      TPresenter Show<TPresenter>(int width, int height) where TPresenter : IPresenter;
   }
}

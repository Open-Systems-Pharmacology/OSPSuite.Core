using System;
using System.ComponentModel;
using OSPSuite.Presentation.Presenters.Main;

namespace OSPSuite.Presentation.Views
{
   public interface IMainView : IShell, IView<IMainViewPresenter>
   {
      event CancelEventHandler Closing;
      event Action Loading;

      /// <summary>
      ///    sets if the child view can be selected or not
      /// </summary>
      bool AllowChildActivation { set; }
   }
}
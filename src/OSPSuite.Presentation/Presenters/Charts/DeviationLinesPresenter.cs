using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IDeviationLinesPresenter : IDisposablePresenter, IPresenter<IDeviationLinesView>
   {
      float GetFoldValue();
   }

   public class DeviationLinesPresenter : AbstractDisposablePresenter<IDeviationLinesView, IDeviationLinesPresenter>,
      IDeviationLinesPresenter
   {
      public DeviationLinesPresenter(IDeviationLinesView view) : base(view)
      {
      }

      public float GetFoldValue()
      {
         _view.Display();
         return _view.Canceled ? 0 : _view.GetFoldValue();
      }
   }
}
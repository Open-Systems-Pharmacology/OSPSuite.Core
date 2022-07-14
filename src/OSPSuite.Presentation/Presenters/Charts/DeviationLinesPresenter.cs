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
   }

   public class DeviationLinesPresenter : AbstractDisposablePresenter<IDeviationLinesView, IDeviationLinesPresenter>,
      IDeviationLinesPresenter
   {
      public DeviationLinesPresenter(IDeviationLinesView view) : base(view)
      {
      }
   }
}
using OSPSuite.Utility.Format;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public class DataColumnFormatter : IFormatter<DataColumn>
   {
      private readonly ICurveSettingsPresenter _curveSettingsPresenter;

      public DataColumnFormatter(ICurveSettingsPresenter curveSettingsPresenter)
      {
         _curveSettingsPresenter = curveSettingsPresenter;
      }

      public string Format(DataColumn col)
      {
         return _curveSettingsPresenter.CurveNameDefinition(col);
      }
   }
}
using System.Data;
using OSPSuite.Utility.Format;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationMatrixView : IView<IParameterIdentificationMatrixPresenter>
   {
      void BindTo(DataTable dataTable, double maxValue);
      void DeleteBinding();
      IFormatter<double> NumberFormatter { get; set; }
   }
}
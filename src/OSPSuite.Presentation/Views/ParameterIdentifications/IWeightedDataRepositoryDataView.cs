using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ObservedData;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IWeightedDataRepositoryDataView : IBaseDataRepositoryDataView<IWeightedDataRepositoryDataPresenter>
   {
      void DisplayColumnReadOnly(System.Data.DataColumn column);
      void SelectRow(int rowIndex);
   }
}

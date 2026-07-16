using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.ObservedData;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IImportPreviewView : IView<IImportPreviewPresenter>
   {
      void SetDataSetNames(IEnumerable<string> names);
      void SetNamingConventions(IEnumerable<string> options, string selected = null);
      void SetNamingConventionKeys(IEnumerable<string> keys);
      void AddChartView(IView chartView);
      void AddDataView(IDataRepositoryDataView dataView);
      string SelectedSeparator { get; }
      bool SelectingDataSetsEnabled { get; set; }
      void SetErrorMessage(string errorMessage);
   }
}

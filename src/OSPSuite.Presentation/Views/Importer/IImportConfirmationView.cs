﻿using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.ObservedData;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IImportConfirmationView : IView<IImportConfirmationPresenter> //IModalView<IImportConfirmationPresenter>
   {
      void SetDataSetNames(IEnumerable<string> names);
      void SetNamingConventions(IEnumerable<string> options, string selected = null);
      void SetNamingConventionKeys(IEnumerable<string> keys);
      void ShowSelectedDataSet(DataRepository dataRepository);
      void AddChartView(IView chartView);
      void AddDataView(IDataRepositoryDataView dataView);
      string SelectedSeparator { get; }
      void DisableSelectingDataSets(string errorMessage);
      void EnableSelectingDataSets();
   }
}

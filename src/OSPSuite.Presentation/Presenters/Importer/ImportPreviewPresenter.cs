using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImportPreviewPresenter : AbstractPresenter<IImportPreviewView, IImportPreviewPresenter>, IImportPreviewPresenter
   {
      private readonly IDataRepositoryChartPresenter _chartPresenter;
      private readonly IDataRepositoryDataPresenter _dataPresenter;
      private string _lastNamingPattern = "";
      private List<string> _conventions;
      private IReadOnlyList<string> _keys;

      public ImportPreviewPresenter(IImportPreviewView view,
         IDataRepositoryChartPresenter chartPresenter, IDataRepositoryDataPresenter dataPresenter) : base(view)
      {
         _chartPresenter = chartPresenter;
         _chartPresenter.LogLinSelectionEnabled = true;
         _dataPresenter = dataPresenter;
         View.AddChartView(_chartPresenter.BaseView);
         View.AddDataView(_dataPresenter.View);
      }
      public void DataSetSelected(int index)
      {
         OnDataSetSelected.Invoke(this, new DataSetSelectedEventArgs { Index = index });
      }

      public void PlotDataRepository(DataRepository dataRepository)
      {
         _chartPresenter.EditObservedData(dataRepository);
         _dataPresenter.EditObservedData(dataRepository);
      }

      private void addDefaultNamingConvention(string selectedNamingConvention)
      {
         var conventions = _conventions.ToList();
         var separator = _view.SelectedSeparator;
         if (string.IsNullOrEmpty(separator))
            separator = Constants.ImporterConstants.NAMING_PATTERN_SEPARATORS.First();
         conventions.Insert(0, string.Join(separator, _keys.Select(k => $"{{{k}}}")));
         _view.SetNamingConventions(conventions, selectedNamingConvention);
         _lastNamingPattern = selectedNamingConvention ?? conventions.First();
      }

      public void TriggerNamingConventionChanged(string namingConvention)
      {
         _lastNamingPattern = namingConvention;
         OnNamingConventionChanged.Invoke(this, new NamingConventionChangedEventArgs {NamingConvention = namingConvention});
      }

      public void SetKeys(IReadOnlyList<string> keys)
      {
         View.SetNamingConventionKeys(keys);
         _keys = keys;
      }

      public void SetNamingConventions(IReadOnlyList<string> namingConventions, string selectedNamingConvention)
      {
         if (namingConventions == null)
            throw new NullNamingConventionsException();

         _conventions = namingConventions.ToList();

         if (_conventions.Count == 0)
            throw new EmptyNamingConventionsException();

         if (selectedNamingConvention == null && !_lastNamingPattern.IsNullOrEmpty())
            selectedNamingConvention = _lastNamingPattern;

         addDefaultNamingConvention(selectedNamingConvention);

         OnNamingConventionChanged.Invoke(this, new NamingConventionChangedEventArgs { NamingConvention = _lastNamingPattern });
      }

      public void ImportData()
      {
         OnImportData.Invoke(this, new EventArgs());
      }

      public void SetDataSetNames(IEnumerable<string> names)
      {
         _view.SetDataSetNames(names);
      }

      public event EventHandler<EventArgs> OnImportData = delegate { };

      public event EventHandler<DataSetSelectedEventArgs> OnDataSetSelected = delegate { };

      public event EventHandler<NamingConventionChangedEventArgs> OnNamingConventionChanged = delegate { };
      
      public void SetViewingStateToError(string invalidExceptionMessage)
      {
         _view.SetErrorMessage(invalidExceptionMessage);
         _view.SelectingDataSetsEnabled = false;
      }

      public void SetViewingStateToNormal()
      {
         _view.SelectingDataSetsEnabled = true;
      }
   }
}

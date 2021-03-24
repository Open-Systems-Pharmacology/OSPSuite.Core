using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Presenters.ObservedData;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImportConfirmationPresenter : AbstractPresenter<IImportConfirmationView, IImportConfirmationPresenter>, IImportConfirmationPresenter
   {
      private readonly IDataRepositoryChartPresenter _chartPresenter;
      private readonly IDataRepositoryDataPresenter _dataPresenter;
      private string _lastNamingPattern = "";

      public ImportConfirmationPresenter(IImportConfirmationView view,
         IDataRepositoryChartPresenter chartPresenter, IDataRepositoryDataPresenter dataPresenter) : base(view)
      {
         _chartPresenter = chartPresenter;
         _chartPresenter.LogLinSelectionEnabled = true;
         _dataPresenter = dataPresenter;
         View.AddChartView(_chartPresenter.BaseView);
         View.AddDataView(_dataPresenter.View);
         _dataPresenter.DisableEdition();
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

      public void TriggerNamingConventionChanged(string namingConvention)
      {
         _lastNamingPattern = namingConvention;
         OnNamingConventionChanged.Invoke(this, new NamingConventionChangedEventArgs {NamingConvention = namingConvention});
      }

      public void SetKeys(IEnumerable<string> keys)
      {
         View.SetNamingConventionKeys(keys);
      }

      public void SetNamingConventions (IEnumerable<string> namingConventions)
      {
         if (namingConventions == null)
            throw new NullNamingConventionsException();

         var conventions = namingConventions.ToList();

         if (conventions.Count == 0)
            throw new EmptyNamingConventionsException();

         _view.SetNamingConventions(conventions);
         _lastNamingPattern = conventions.First();
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
   }
}

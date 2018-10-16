using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Tasks.Starters;
using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Presenters
{
   public interface IDataRepositoryTestPresenter : IPresenter<IDataRepositoryTestView>
   {
      void Edit(DataRepository repository);
   }

   public class DataRepositoryTestPresenter : AbstractCommandCollectorPresenter<IDataRepositoryTestView, IDataRepositoryTestPresenter>, IDataRepositoryTestPresenter
   {
      private readonly IDataRepositoryDataPresenter _dataPresenter;
      private readonly IDataRepositoryChartPresenter _chartPresenter;
      private readonly IDataRepositoryMetaDataPresenter _metaDataPresenter;

      public DataRepositoryTestPresenter(IDataRepositoryTestView view, IDataRepositoryDataPresenter dataPresenter, IDataRepositoryChartPresenter chartPresenter, IDataRepositoryMetaDataPresenter metaDataPresenter, IImportObservedDataTask importObservedDataTask) : base(view)
      {
         _dataPresenter = dataPresenter;
         _chartPresenter = chartPresenter;
         _metaDataPresenter = metaDataPresenter;

         _subPresenterManager.Add(_dataPresenter);
         _subPresenterManager.Add(_chartPresenter);
         _subPresenterManager.Add(_metaDataPresenter);

         _view.AddChartView(_chartPresenter.BaseView);
         _view.AddDataView(_dataPresenter.BaseView);
         _view.AddMetaDataView(_metaDataPresenter.BaseView);

         InitializeWith(new OSPSuiteMacroCommand<OSPSuiteExecutionContext>());
         Edit(importObservedDataTask.ImportObservedData());
      }

      public void Edit(DataRepository repository)
      {
         _chartPresenter.EditObservedData(repository);
         _dataPresenter.EditObservedData(repository);
         _metaDataPresenter.EditObservedData(repository);
      }
   }

   public class ObservedDataConfiguration : IObservedDataConfiguration
   {
      public IEnumerable<string> PredefinedValuesFor(string metaData)
      {
         return emptyList();
      }

      public IReadOnlyList<string> DefaultMetaDataCategories => emptyList();

      private IReadOnlyList<string> emptyList()
      {
         return Enumerable.Empty<string>().ToList();
      }

      public IReadOnlyList<string> ReadOnlyMetaDataCategories => emptyList();
      public bool MolWeightEditable => true;
      public bool MolWeightVisible => true;
   }
}
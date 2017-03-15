using System.Collections.Generic;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Views.ObservedData;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public interface IEditMultipleDataRepositoriesMetaDataPresenter : IDisposablePresenter
   {
      /// <summary>
      ///    Edits metadata from multiple repositories
      /// </summary>
      /// <param name="dataRepositories">The repositories containing metadata to add/delete/modify</param>
      /// <returns>The command used to modify the repositories</returns>
      ICommand Edit(IEnumerable<DataRepository> dataRepositories);
   }

   public class EditMultipleDataRepositoriesMetaDataPresenter :
      AbstractDisposableContainerPresenter<IEditMultipleDataRepositoriesMetaDataView, IEditMultipleDataRepositoriesMetaDataPresenter>, IEditMultipleDataRepositoriesMetaDataPresenter
   {
      private readonly IDataRepositoryMetaDataPresenter _metaDataPresenter;
      private readonly ICommandTask _commandTask;
      private readonly IOSPSuiteExecutionContext _context;

      public EditMultipleDataRepositoriesMetaDataPresenter(IEditMultipleDataRepositoriesMetaDataView view, IDataRepositoryMetaDataPresenter metaDataPresenter,
         ICommandTask commandTask, IOSPSuiteExecutionContext context)
         : base(view)
      {
         _metaDataPresenter = metaDataPresenter;
         _commandTask = commandTask;
         _context = context;
         _view.SetDataEditor(_metaDataPresenter.View);
         AddSubPresenters(_metaDataPresenter);
      }

      public ICommand Edit(IEnumerable<DataRepository> dataRepositories)
      {
         _metaDataPresenter.EditObservedData(dataRepositories);
         _view.Display();

         if (!_view.Canceled)
            return _macroCommand;

         _commandTask.ResetChanges(_macroCommand, _context);
         return new OSPSuiteEmptyCommand<IOSPSuiteExecutionContext>();
      }
   }
}
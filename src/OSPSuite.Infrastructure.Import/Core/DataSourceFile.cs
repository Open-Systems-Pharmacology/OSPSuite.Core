using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Infrastructure.Import.Core
{
   /// <summary>
   ///    Single file containing the data, e.g. excel file or csv file
   /// </summary>
   public interface IDataSourceFile
   {
      string Path { get; }
      IDataFormat Format { get; set; }

      IList<IDataFormat> AvailableFormats { get; set; }

      //Stores what sheet was used to calculate the format
      //so the presenter can actually select such a sheet
      //as active when initialized
      string FormatCalculatedFrom { get; set; }
      DataSheetCollection DataSheets { get; }

      /// <summary>
      ///    Loads the file at <paramref name="path" /> into the data source.
      /// </summary>
      void LoadFromFile(string path);
   }

   public abstract class DataSourceFile : IDataSourceFile
   {
      protected readonly IImportLogger _logger; //ToDo: not sure this is the correct logger implementation - could be we need to write our own
      private readonly IHeavyWorkManager _heavyWorkManager;

      public IDataFormat Format { get; set; }

      private IList<IDataFormat> _availableFormats;

      public IList<IDataFormat> AvailableFormats
      {
         get => _availableFormats;
         set
         {
            _availableFormats = value;
            Format = value.FirstOrDefault();
         }
      }

      public string FormatCalculatedFrom { get; set; }
      public DataSheetCollection DataSheets { get; } = new DataSheetCollection();

      protected DataSourceFile(IImportLogger logger, IHeavyWorkManager heavyWorkManager)
      {
         _logger = logger;
         _heavyWorkManager = heavyWorkManager;
      }

      public string Path { get; private set; }

      protected abstract void DoLoadWork(string path, CancellationToken cancellationToken = default);

      public virtual void LoadFromFile(string path)
      {
         Path = path;
         var cts = new CancellationTokenSource();
         _heavyWorkManager.Start(() =>
         {
            try
            {
               DoLoadWork(path, cts.Token);
            }
            catch (OperationCanceledException)
            {
               //Nothing to do, just not throw exception.
            }
         }, "Importing data...", cts);
      }
   }
}
using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Import;

namespace OSPSuite.Core.Domain.Services
{
   public interface IObservedDataImporter
   {
      /// <summary>
      ///    Imports observed data from a file selected by the user, using the <paramref name="configuration" />
      /// </summary>
      IReadOnlyList<DataRepository> ImportFrom(ImporterConfiguration configuration);

      /// <summary>
      ///    Adds the <paramref name="observedData" /> returned by <see cref="ImportFrom" /> to the project
      /// </summary>
      void AddImportedObservedDataToProject(DataRepository observedData);
   }
}

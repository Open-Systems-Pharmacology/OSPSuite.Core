using System.Collections.Generic;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain.Services
{
   public interface IEditObservedDataTask
   {
      /// <summary>
      /// Command that adds new metadata to an enumeration of data repositories
      /// </summary>
      /// <param name="dataRepositories">The enumeration to apply the new metadata to</param>
      /// <param name="metaDataKeyValue"></param>
      /// <returns>The command which was used to modify the repositories</returns>
      ICommand AddMetaData(IEnumerable<DataRepository> dataRepositories, MetaDataKeyValue metaDataKeyValue);

      /// <summary>
      /// Command that removes metadata from an enumeration of data repositories
      /// </summary>
      /// <param name="dataRepositories">The enumeration to remove metadata from</param>
      /// <param name="metaDataKeyValue">The metadata key and current value that should be removed</param>
      /// <returns>The command which was used to modify the repositories</returns>
      ICommand RemoveMetaData(IEnumerable<DataRepository> dataRepositories, MetaDataKeyValue metaDataKeyValue);

      /// <summary>
      /// Command that modifies metadata in an enumeration of repositories
      /// </summary>
      /// <param name="dataRepositories">The enumeration of repositories to modify the metadata on</param>
      /// <param name="metaDataChanged">The value entity holding old and new values and keys</param>
      /// <returns>The command which was used to modify the repositories</returns>
      ICommand ChangeMetaData(IEnumerable<DataRepository> dataRepositories, MetaDataChanged metaDataChanged);

      /// <summary>
      /// Edits metadata on multiple data repositories at once.
      /// </summary>
      /// <param name="dataRepositories">The data repositories being edited</param>
      void EditMultipleMetaDataFor(IEnumerable<DataRepository> dataRepositories);

      /// <summary>
      /// Updates the value of the molWeight property defined in all <see cref="DataInfo"/>
      /// </summary>
      /// <param name="allDataRepositories">Repositories being edited</param>
      /// <param name="oldMolWeightValue">Old value in core unit</param>
      /// <param name="newMolWeightValue">Value to set in core unit</param>
      ICommand UpdateMolWeight(IEnumerable<DataRepository> allDataRepositories, double oldMolWeightValue, double newMolWeightValue);

      /// <summary>
      /// Checks if the <paramref name="observedData"/>is used by any parameter identification in the project
      /// </summary>
      /// <returns>true if observed data is used by a parameter identification, otherwise false</returns>
      IReadOnlyList<string> ParameterIdentificationsUsingDataRepository(DataRepository observedData);
   }

}
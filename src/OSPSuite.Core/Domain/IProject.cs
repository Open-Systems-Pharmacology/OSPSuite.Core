using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain
{
   public interface IProject : IObjectBase, IWithChartTemplates, IWithCreationMetaData
   {
      /// <summary>
      ///    Full path of project (empty if the project was not saved yet)
      /// </summary>
      string FilePath { get; set; }

      /// <summary>
      ///    Return true if the project has changed since it was saved
      /// </summary>
      bool HasChanged { get; set; }

      /// <summary>
      ///    Returns all the building block of a given type <typeparamref name="T" />
      /// </summary>
      /// <typeparam name="T">Type if building block</typeparam>
      IReadOnlyCollection<T> All<T>() where T : class;

      /// <summary>
      ///    All the observed data defined in the project
      /// </summary>
      IReadOnlyCollection<DataRepository> AllObservedData { get; }

      /// <summary>
      ///    Add an observed data to the project
      /// </summary>
      void AddObservedData(DataRepository dataRepositoryToAdd);

      /// <summary>
      ///    Remove an observed data from the project
      /// </summary>
      void RemoveObservedData(DataRepository dataRepositoryToRemove);

      /// <summary>
      ///    Returns the observed Data with the given id.
      /// </summary>
      /// <param name="dataRepositoryId">Id of the observed data</param>
      DataRepository ObservedDataBy(string dataRepositoryId);

      /// <summary>
      ///    Returns the observed data referenced by <paramref name="usedObservedData"/>
      /// </summary>
      DataRepository ObservedDataBy(UsedObservedData usedObservedData);

      /// <summary>
      ///    List of favorites defined in the project
      /// </summary>
      Favorites Favorites { get; }

      /// <summary>
      ///    Returns all the classifications defined in the the project
      /// </summary>
      IReadOnlyCollection<IClassification> AllClassifications { get; }

      /// <summary>
      ///    Returns all the classifications defined in the the project filtered by the <paramref name="classificationType" />
      /// </summary>
      IReadOnlyCollection<IClassification> AllClassificationsByType(ClassificationType classificationType);

      /// <summary>
      ///    Adds a new classification to the project
      /// </summary>
      void AddClassification(IClassification classification);

      /// <summary>
      ///    Returns all classifiable defined in the project
      /// </summary>
      IReadOnlyCollection<IClassifiable> AllClassifiables { get; }

      /// <summary>
      ///    Returns all classifiable defined in the project filtered by type <typeparamref name="TClassifiable" />
      /// </summary>
      IReadOnlyCollection<TClassifiable> AllClassifiablesByType<TClassifiable>() where TClassifiable : IClassifiable;

      /// <summary>
      ///    Adds a classifiable to the project
      /// </summary>
      void AddClassifiable(IClassifiable classifiable);

      /// <summary>
      ///    Removes the classifiable from the project
      /// </summary>
      void RemoveClassifiable(IClassifiable classifiable);

      /// <summary>
      ///    Removes the classification from the project
      /// </summary>
      void RemoveClassification(IClassification classification);

      /// <summary>
      ///    Retrieves an existing classification based on the parent classification and new classification name as child.
      ///    If the classification does not exist, it is created and added to the project
      /// </summary>
      /// <param name="parent">The parent classification for this new classification</param>
      /// <param name="newClassificationName">The new classification that is being searched for under the parent</param>
      /// <param name="classificationType">The type of classification being searched</param>
      /// <returns>
      ///    The existing classification if an identical one is found,
      ///    otherwise a new one matching the <paramref name="parent" /> and <paramref name="newClassificationName" /> path
      /// </returns>
      IClassification GetOrCreateByPath(IClassification parent, string newClassificationName, ClassificationType classificationType);

      TClassifiable GetOrCreateClassifiableFor<TClassifiable, TSubject>(TSubject subject)
         where TClassifiable : Classifiable<TSubject>, new()
         where TSubject : IWithId, IWithName;

      DisplayUnitsManager DisplayUnits { get; }

      /// <summary>
      ///    Path of the associated working journal (absolute or relative)
      /// </summary>
      string JournalPath { get; set; }

      /// <summary>
      ///    Adds the <paramref name="parameterIdentification" /> to the project
      /// </summary>
      void AddParameterIdentification(ParameterIdentification parameterIdentification);

      /// <summary>
      ///    Returns all <see cref="ParameterIdentification" /> defined in the project
      /// </summary>
      IReadOnlyCollection<ParameterIdentification> AllParameterIdentifications { get; }

      /// <summary>
      ///    Removes the <paramref name="parameterIdentification" /> from the project
      /// </summary>
      void RemoveParameterIdentification(ParameterIdentification parameterIdentification);

      /// <summary>
      ///    Adds the <paramref name="sensitivityAnalysis" /> to the project
      /// </summary>
      void AddSensitivityAnalysis(SensitivityAnalysis sensitivityAnalysis);

      /// <summary>
      ///    Returns all <see cref="SensitivityAnalysis" /> defined in the project
      /// </summary>
      IReadOnlyCollection<SensitivityAnalysis> AllSensitivityAnalyses { get; }

      /// <summary>
      ///    Removes the <paramref name="sensitivityAnalysis" /> from the project
      /// </summary>
      void RemoveSensitivityAnalysis(SensitivityAnalysis sensitivityAnalysis);


      IEnumerable<IParameterAnalysable> AllParameterAnalysables { get; }


      IEnumerable<IUsesObservedData> AllUsersOfObservedData { get; }
   }
}
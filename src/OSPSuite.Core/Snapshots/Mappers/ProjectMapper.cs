using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using ModelDataRepository = OSPSuite.Core.Domain.Data.DataRepository;
using SnapshotDataRepository = OSPSuite.Core.Snapshots.DataRepository;
using ModelParameterIdentification = OSPSuite.Core.Domain.ParameterIdentifications.ParameterIdentification;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public abstract class ProjectMapper<TProject, TSnapshotProject, TProjectContext> : SnapshotMapperBase<TProject, TSnapshotProject, TProjectContext> where TSnapshotProject : new() where TProjectContext : SnapshotContext where TProject : Project
   {
      protected readonly ICreationMetaDataFactory _creationMetaDataFactory;
      protected readonly IOSPSuiteLogger _logger;
      protected readonly Lazy<ISnapshotMapper> _snapshotMapper;
      private readonly IOSPSuiteExecutionContext<TProject> _executionContext;
      protected readonly IClassificationSnapshotTask _classificationSnapshotTask;
      private readonly ParameterIdentificationMapper _parameterIdentificationMapper;

      protected ProjectMapper(ICreationMetaDataFactory creationMetaDataFactory,
         IOSPSuiteLogger logger,
         IOSPSuiteExecutionContext<TProject> executionContext,
         IClassificationSnapshotTask classificationSnapshotTask,
         ParameterIdentificationMapper parameterIdentificationMapper)
      {
         _creationMetaDataFactory = creationMetaDataFactory;
         _logger = logger;
         _executionContext = executionContext;
         _classificationSnapshotTask = classificationSnapshotTask;
         _parameterIdentificationMapper = parameterIdentificationMapper;

         //required to load the snapshot mapper via execution context to avoid circular references
         _snapshotMapper = new Lazy<ISnapshotMapper>(executionContext.Resolve<ISnapshotMapper>);
      }

      protected ISnapshotMapper SnapshotMapper => _snapshotMapper.Value;

      protected Task<Classification[]> MapClassifications<TClassifiable>(TProject project) where TClassifiable : class, IClassifiableWrapper, new() =>
         _classificationSnapshotTask.MapClassificationsToSnapshots<TClassifiable>(project);

      protected async Task<T[]> AwaitAs<T>(IEnumerable<Task<object>> tasks)
      {
         var models = await Task.WhenAll(tasks);
         var array = models.OfType<T>().ToArray();
         return array.Any() ? array : null;
      }

      protected void LogDuplicateEntryError<T>(T subject) where T : class, IWithId, IWithName =>
         _logger.AddError(Error.SnapshotDuplicateEntryByName(subject.Name, _executionContext.TypeFor(subject)));

      protected void AddClassifiableToProject<TClassifiableWrapper, TSubject>(TProject project, TSubject subject,
         Action<TSubject> addToProjectAction, IEnumerable<TSubject> existingInProject) where TClassifiableWrapper : Classifiable<TSubject>, new() where TSubject : class, IWithId, IWithName
      {
         var existing = existingInProject.FindByName(subject.Name);
         if (existing != null)
         {
            LogDuplicateEntryError(subject);
            return;
         }

         addToProjectAction(subject);
         project.GetOrCreateClassifiableFor<TClassifiableWrapper, TSubject>(subject);
      }

      protected Task<TSnapshot[]> MapModelsToSnapshot<TModel, TSnapshot>(IEnumerable<TModel> models, Func<TModel, Task<object>> mapFunc)
      {
         var tasks = models.Select(mapFunc);
         return AwaitAs<TSnapshot>(tasks);
      }

      protected virtual async Task<ParameterIdentification[]> MapParameterIdentificationToSnapshots(IReadOnlyCollection<ModelParameterIdentification> allParameterIdentifications)
      {
         return await _parameterIdentificationMapper.MapToSnapshots(allParameterIdentifications);
      }

      protected Task<SnapshotDataRepository[]> MapObservedDataToSnapshots(IReadOnlyCollection<ModelDataRepository> allObservedData) =>
         MapModelsToSnapshot<ModelDataRepository, SnapshotDataRepository>(allObservedData, SnapshotMapper.MapToSnapshot);

      protected Task<ModelDataRepository[]> ObservedDataFrom(SnapshotDataRepository[] snapshotRepositories, SnapshotContext snapshotContext) =>
         AwaitAs<ModelDataRepository>(MapSnapshotsToModels(snapshotRepositories, snapshotContext));

      protected void AddObservedDataToProject(TProject project, ModelDataRepository repository) =>
         AddClassifiableToProject<ClassifiableObservedData, ModelDataRepository>(project, repository, project.AddObservedData, project.AllObservedData);

      protected void AddParameterIdentificationToProject(TProject project, ModelParameterIdentification parameterIdentification)
      {
         AddClassifiableToProject<ClassifiableParameterIdentification, ModelParameterIdentification>(project, parameterIdentification,
            project.AddParameterIdentification, project.AllParameterIdentifications);
      }

      protected IEnumerable<Task<object>> MapSnapshotsToModels(IEnumerable<object> snapshots, SnapshotContext snapshotContext)
      {
         if (snapshots == null)
            return Enumerable.Empty<Task<object>>();

         return snapshots.Select(x => SnapshotMapper.MapToModel(x, snapshotContext));
      }

      protected Task<ModelParameterIdentification[]> AllParameterIdentificationsFrom(ParameterIdentification[] snapshotParameterIdentifications,
         SnapshotContext snapshotContext)
         => _parameterIdentificationMapper.MapToModels(snapshotParameterIdentifications, snapshotContext);
   }
}
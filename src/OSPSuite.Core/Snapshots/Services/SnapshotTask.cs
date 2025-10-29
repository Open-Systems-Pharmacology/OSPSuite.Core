using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Utility;

namespace OSPSuite.Core.Snapshots.Services
{
   public interface ISnapshotTask<TProject, in TSnapshotProject>
   {
      /// <summary>
      ///    Exports the given <paramref name="modelToExport" /> to snapshot. User will be ask to specify the file where the
      ///    snapshot will be exported
      /// </summary>
      Task ExportModelToSnapshotAsync<T>(T modelToExport) where T : class, IObjectBase;

      /// <summary>
      ///    Exports the given <paramref name="modelToExport" /> to snapshot file <paramref name="fileFullPath" />
      /// </summary>
      Task ExportModelToSnapshotAsync<T>(T modelToExport, string fileFullPath) where T : class, IObjectBase;

      /// <summary>
      ///    Exports the given <paramref name="snapshotObject" /> to file. <paramref name="snapshotObject" /> is already a
      ///    snapshot object and won't be mapped to snapshot
      /// </summary>
      Task ExportSnapshotAsync(IWithName snapshotObject);

      Task<IEnumerable<T>> LoadModelsFromSnapshotFileAsync<T>() where T : class, IObjectBase;

      Task<IEnumerable<T>> LoadSnapshotsAsync<T>(string fileName);

      Task<IEnumerable<T>> LoadModelsFromSnapshotFileAsync<T>(string fileName) where T : class;

      Task<TProject> LoadProjectFromSnapshotFileAsync(string fileName, bool runSimulations = true);

      Task<TProject> LoadProjectFromSnapshotAsync(TSnapshotProject snapshot, bool runSimulations);

      Task<T> LoadSnapshotFromFileAsync<T>(string fileName) where T : IWithName;
   }

   public abstract class SnapshotTask<TProject, TSnapshotProject> : ISnapshotTask<TProject, TSnapshotProject> where TProject : Project where TSnapshotProject : IWithName
   {
      protected readonly IJsonSerializer _jsonSerializer;
      protected readonly ISnapshotMapper _snapshotMapper;
      protected readonly IDialogCreator _dialogCreator;
      protected readonly IObjectTypeResolver _objectTypeResolver;
      protected readonly IOSPSuiteExecutionContext<TProject> _executionContext;

      protected SnapshotTask(IJsonSerializer jsonSerializer, ISnapshotMapper snapshotMapper, IDialogCreator dialogCreator, IObjectTypeResolver objectTypeResolver, IOSPSuiteExecutionContext<TProject> executionContext)
      {
         _jsonSerializer = jsonSerializer;
         _snapshotMapper = snapshotMapper;
         _dialogCreator = dialogCreator;
         _objectTypeResolver = objectTypeResolver;
         _executionContext = executionContext;
      }

      public Task<IEnumerable<T>> LoadModelsFromSnapshotFileAsync<T>() where T : class, IObjectBase
      {
         var fileName = fileNameForSnapshotImport<T>();
         return LoadModelsFromSnapshotFileAsync<T>(fileName);
      }

      public async Task ExportModelToSnapshotAsync<T>(T modelToExport) where T : class, IObjectBase
      {
         if (modelToExport == null)
            return;

         var fileName = fileNameForExport(modelToExport);
         if (string.IsNullOrEmpty(fileName))
            return;

         await ExportModelToSnapshotAsync(modelToExport, fileName);
      }

      public Task ExportModelToSnapshotAsync<T>(T modelToExport, string fileFullPath) where T : class, IObjectBase
      {
         _executionContext.Load(modelToExport);
         return exportSnapshotFor(modelToExport, fileFullPath);
      }

      public async Task ExportSnapshotAsync(IWithName snapshotObject)
      {
         var fileName = fileNameForExport(snapshotObject);
         if (string.IsNullOrEmpty(fileName))
            return;

         await saveSnapshotToFile(snapshotObject, fileName);
      }

      private async Task exportSnapshotFor<T>(T objectToExport, string fileName)
      {
         var snapshot = await _snapshotMapper.MapToSnapshot(objectToExport);
         await saveSnapshotToFile(snapshot, fileName);
      }

      private Task saveSnapshotToFile(object snapshot, string fileName) => _jsonSerializer.Serialize(snapshot, fileName);

      private string fileNameForExport(IWithName objectToExport)
      {
         var message = Captions.SelectSnapshotExportFile(objectToExport.Name, _objectTypeResolver.TypeFor(objectToExport));
         return _dialogCreator.AskForFileToSave(message, Constants.Filter.JSON_FILE_FILTER, Constants.DirectoryKey.REPORT, objectToExport.Name);
      }

      private string fileNameForSnapshotImport<T>()
      {
         var message = Captions.LoadObjectFromSnapshot(_objectTypeResolver.TypeFor<T>());
         return _dialogCreator.AskForFileToOpen(message, Constants.Filter.JSON_FILE_FILTER, Constants.DirectoryKey.REPORT);
      }

      public async Task<TProject> LoadProjectFromSnapshotFileAsync(string fileName, bool runSimulations = true)
      {
         var projectSnapshot = await LoadSnapshotFromFileAsync<TSnapshotProject>(fileName);
         var project = await LoadProjectFromSnapshotAsync(projectSnapshot, runSimulations);
         return projectWithUpdatedProperties(project, FileHelper.FileNameFromFileFullPath(fileName));
      }

      public async Task<TProject> LoadProjectFromSnapshotAsync(TSnapshotProject snapshot, bool runSimulations)
      {
         var project = await ProjectFrom(snapshot, runSimulations);
         return projectWithUpdatedProperties(project, snapshot?.Name);
      }

      protected abstract Task<TProject> ProjectFrom(TSnapshotProject snapshot, bool runSimulations);

      private TProject projectWithUpdatedProperties(TProject project, string name)
      {
         if (project == null)
            return null;

         project.HasChanged = true;
         project.Name = name;
         return project;
      }

      public async Task<T> LoadSnapshotFromFileAsync<T>(string fileName) where T : IWithName
      {
         var snapshots = await LoadSnapshotsAsync<T>(fileName);
         var snapshot = snapshots.FirstOrDefault();

         if (snapshot != null && string.IsNullOrEmpty(snapshot.Name))
            snapshot.Name = FileHelper.FileNameFromFileFullPath(fileName);

         return snapshot;
      }

      public async Task<IEnumerable<T>> LoadSnapshotsAsync<T>(string fileName)
      {
         var snapshots = await loadSnapshot(fileName, typeof(T));
         return snapshots.OfType<T>();
      }

      private async Task<IEnumerable<object>> loadSnapshot(string fileName, Type snapshotType)
      {
         if (string.IsNullOrEmpty(fileName))
            return Array.Empty<object>();

         return await _jsonSerializer.DeserializeAsArray(fileName, snapshotType);
      }

      public async Task<IEnumerable<T>> LoadModelsFromSnapshotFileAsync<T>(string fileName) where T : class
      {
         var snapshotType = _snapshotMapper.SnapshotTypeFor<T>();
         var snapshots = await loadSnapshot(fileName, snapshotType);

         return await loadModelsFromSnapshotsAsync<T>(snapshots);
      }

      private async Task<IEnumerable<T>> loadModelsFromSnapshotsAsync<T>(IEnumerable<object> snapshots)
      {
         if (snapshots == null)
            return Array.Empty<T>();

         //This method is typically called when loading a building block snapshot directly (e.g exported as dev).
         //In this case, we are not supporting any project conversion and we just create one with the current version
         var snapshotContext = GetSnapshotContext();
         var tasks = snapshots.Select(x => _snapshotMapper.MapToModel(x, snapshotContext));
         var models = await Task.WhenAll(tasks);
         return models.OfType<T>();
      }

      protected abstract SnapshotContext GetSnapshotContext();

      protected async Task<T> LoadModelFromSnapshot<T>(object snapshot)
      {
         if (snapshot == null)
            return default;

         var models = await loadModelsFromSnapshotsAsync<T>(new[] { snapshot });
         return models.FirstOrDefault();
      }

      protected abstract TProject GetProject();
   }
}
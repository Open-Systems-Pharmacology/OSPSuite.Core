using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using ModelParameterIdentification = OSPSuite.Core.Domain.ParameterIdentifications.ParameterIdentification;
using SnapshotParameterIdentification = OSPSuite.Core.Snapshots.ParameterIdentification;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class ParameterIdentificationMapper : ObjectBaseSnapshotMapperBase<ModelParameterIdentification, SnapshotParameterIdentification>
   {
      protected readonly ParameterIdentificationConfigurationMapper _parameterIdentificationConfigurationMapper;
      protected readonly OutputMappingMapper _outputMappingMapper;
      protected readonly IdentificationParameterMapper _identificationParameterMapper;
      protected readonly ParameterIdentificationAnalysisMapper _parameterIdentificationAnalysisMapper;
      protected readonly IObjectBaseFactory _objectBaseFactory;
      protected readonly IOSPSuiteLogger _logger;

      public ParameterIdentificationMapper(
         ParameterIdentificationConfigurationMapper parameterIdentificationConfigurationMapper,
         OutputMappingMapper outputMappingMapper,
         IdentificationParameterMapper identificationParameterMapper,
         ParameterIdentificationAnalysisMapper parameterIdentificationAnalysisMapper,
         IObjectBaseFactory objectBaseFactory,
         IOSPSuiteLogger logger
      )
      {
         _parameterIdentificationConfigurationMapper = parameterIdentificationConfigurationMapper;
         _outputMappingMapper = outputMappingMapper;
         _identificationParameterMapper = identificationParameterMapper;
         _parameterIdentificationAnalysisMapper = parameterIdentificationAnalysisMapper;
         _objectBaseFactory = objectBaseFactory;
         _logger = logger;
      }

      public override async Task<SnapshotParameterIdentification> MapToSnapshot(ModelParameterIdentification parameterIdentification)
      {
         var snapshot = await SnapshotFrom(parameterIdentification, x => { x.Simulations = parameterIdentification.AllSimulations.AllNames().ToArray(); });
         snapshot.Configuration = await _parameterIdentificationConfigurationMapper.MapToSnapshot(parameterIdentification.Configuration);
         snapshot.OutputMappings = await _outputMappingMapper.MapToSnapshots(parameterIdentification.AllOutputMappings);
         snapshot.IdentificationParameters = await _identificationParameterMapper.MapToSnapshots(parameterIdentification.AllIdentificationParameters);
         snapshot.Analyses = await _parameterIdentificationAnalysisMapper.MapToSnapshots(parameterIdentification.Analyses);
         return snapshot;
      }

      public override async Task<ModelParameterIdentification> MapToModel(SnapshotParameterIdentification snapshot, SnapshotContext snapshotContext)
      {
         var parameterIdentification = _objectBaseFactory.Create<ModelParameterIdentification>();
         var parameterIdentificationContext = new ParameterIdentificationContext(parameterIdentification, snapshotContext);
         MapSnapshotPropertiesToModel(snapshot, parameterIdentification);

         snapshot.Simulations?.Each(s => { addSimulation(s, parameterIdentification, snapshotContext.Project); });

         await _parameterIdentificationConfigurationMapper.MapToModel(snapshot.Configuration, parameterIdentificationContext);

         var outputMappings = await _outputMappingMapper.MapToModels(snapshot.OutputMappings, parameterIdentificationContext);
         outputMappings?.Each(parameterIdentification.AddOutputMapping);

         var identificationParameters = await _identificationParameterMapper.MapToModels(snapshot.IdentificationParameters, parameterIdentificationContext);
         identificationParameters?.Each(parameterIdentification.AddIdentificationParameter);

         var simulationAnalysis = await _parameterIdentificationAnalysisMapper.MapToModels(snapshot.Analyses, parameterIdentificationContext);
         simulationAnalysis?.Each(parameterIdentification.AddAnalysis);

         parameterIdentification.IsLoaded = true;
         return parameterIdentification;
      }

      private void addSimulation(string simulationName, ModelParameterIdentification parameterIdentification, Project project)
      {
         var simulation = project.All<ISimulation>().FindByName(simulationName);
         if (simulation == null)
         {
            _logger.AddWarning(Error.CouldNotFindSimulation(simulationName));
            return;
         }

         parameterIdentification.AddSimulation(simulation);
      }
   }
}
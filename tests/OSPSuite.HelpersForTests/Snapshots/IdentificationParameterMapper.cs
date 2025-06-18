using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots.Mappers;

namespace OSPSuite.Helpers.Snapshots
{
   public class IdentificationParameterMapper : Core.Snapshots.Mappers.IdentificationParameterMapper
   {
      public IdentificationParameterMapper(ParameterMapper parameterMapper, IIdentificationParameterFactory identificationParameterFactory, IIdentificationParameterTask identificationParameterTask, IOSPSuiteLogger logger) : base(parameterMapper, identificationParameterFactory, identificationParameterTask, logger)
      {
      }

      protected override bool ShouldExportToSnapshot(IParameter parameter)
      {
         return true;
      }

      protected override IModelCoreSimulation SimulationByName(ParameterIdentificationContext parameterIdentificationContext, string simulationName)
      {
         return parameterIdentificationContext.Project.All<ISimulation>().FindByName(simulationName);
      }
   }
}
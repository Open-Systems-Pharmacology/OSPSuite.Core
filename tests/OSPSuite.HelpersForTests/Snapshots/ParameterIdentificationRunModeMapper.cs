using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Helpers.Snapshots
{
   public class ParameterIdentificationRunModeMapper : OSPSuite.Core.Snapshots.Mappers.ParameterIdentificationRunModeMapper
   {
      protected override ParameterIdentificationRunMode RunModeFrom(Core.Snapshots.ParameterIdentificationRunMode snapshot)
      {
         return new StandardParameterIdentificationRunMode();
      }
   }
}
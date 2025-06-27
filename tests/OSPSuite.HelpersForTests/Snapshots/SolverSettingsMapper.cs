using OSPSuite.Core.Domain;

namespace OSPSuite.Helpers.Snapshots
{
   public class SolverSettingsMapper : OSPSuite.Core.Snapshots.Mappers.SolverSettingsMapper
   {
      private readonly ISolverSettingsFactory _solverSettingsFactory;

      public SolverSettingsMapper(ISolverSettingsFactory solverSettingsFactory) : base(solverSettingsFactory.CreateCVODE())
      {
         _solverSettingsFactory = solverSettingsFactory;
      }

      protected override SolverSettings CreateDefault()
      {
         return _solverSettingsFactory.CreateCVODE();
      }
   }
}
namespace OSPSuite.Core.Domain
{
   public interface ISolverSettingsFactory
   {
      SolverSettings CreateCVODE();
      SolverSettings Create(string solverName);
   }

   public class SolverSettingsFactory : ISolverSettingsFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IParameterFactory _parameterFactory;

      public SolverSettingsFactory(IObjectBaseFactory objectBaseFactory, IParameterFactory parameterFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _parameterFactory = parameterFactory;
      }

      public SolverSettings CreateCVODE()
      {
         return Create(Constants.CVODES);
      }

      public SolverSettings Create(string solverName)
      {
         var solverSettings = _objectBaseFactory.Create<SolverSettings>().WithName(solverName);
         solverSettings.Add(newSimulationSolverParameter(Constants.Parameters.ABS_TOL, 1E-9));
         solverSettings.Add(newSimulationSolverParameter(Constants.Parameters.REL_TOL, 1E-4));
         solverSettings.Add(newSimulationSolverParameter(Constants.Parameters.USE_JACOBIAN, 1));
         solverSettings.Add(newSimulationSolverParameter(Constants.Parameters.H0, 1E-10));
         solverSettings.Add(newSimulationSolverParameter(Constants.Parameters.H_MIN, 0));
         solverSettings.Add(newSimulationSolverParameter(Constants.Parameters.H_MAX, 60));
         solverSettings.Add(newSimulationSolverParameter(Constants.Parameters.MX_STEP, 100000));
         return solverSettings;
      }

      private IParameter newSimulationSolverParameter(string name, double value)
      {
         var parameter = _parameterFactory.CreateParameter(name, value, groupName: Constants.Groups.SOLVER_SETTINGS);
         parameter.BuildingBlockType = PKSimBuildingBlockType.Simulation;
         parameter.CanBeVaried = false;
         parameter.CanBeVariedInPopulation = false;
         parameter.Visible = true;
         parameter.Editable = true;
         parameter.IsDefault = true;
         return parameter;
      }
   }
}
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityParameterFactory
   {
      SensitivityParameter CreateFor(ParameterSelection parameterSelection, SensitivityAnalysis sensitivityAnalysis);
      SensitivityParameter EmptySensitivityParameter();
   }

   public class SensitivityParameterFactory : ISensitivityParameterFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IContainerTask _containerTask;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IFullPathDisplayResolver _fullPathDisplayResolver;

      public SensitivityParameterFactory(IObjectBaseFactory objectBaseFactory, IContainerTask containerTask, IDimensionFactory dimensionFactory, IFullPathDisplayResolver fullPathDisplayResolver)
      {
         _objectBaseFactory = objectBaseFactory;
         _containerTask = containerTask;
         _dimensionFactory = dimensionFactory;
         _fullPathDisplayResolver = fullPathDisplayResolver;
      }

      public SensitivityParameter EmptySensitivityParameter()
      {
         return createSensitivityParameter();
      }

      public SensitivityParameter CreateFor(ParameterSelection parameterSelection, SensitivityAnalysis sensitivityAnalysis)
      {
         if (sensitivityAnalysis.AnalyzesParameter(parameterSelection))
            return null;

         var nameToUse = uniqueNameFor(sensitivityAnalysis, parameterSelection.Parameter);

         return createSensitivityParameter(parameterSelection, nameToUse);
      }

      private SensitivityParameter createSensitivityParameter(ParameterSelection parameterSelection = null, string nameToUse = "")
      {
         var sensitivityParameter = _objectBaseFactory.Create<SensitivityParameter>().WithName(nameToUse);
         sensitivityParameter.ParameterSelection = parameterSelection;
         addParametersTo(sensitivityParameter);
         return sensitivityParameter;
      }

      private void addParametersTo(SensitivityParameter sensitivityParameter)
      {
         sensitivityParameter.Add(createParameter(Constants.Parameters.VARIATION_RANGE, _dimensionFactory.Dimension(Constants.Dimension.FRACTION), defaultValue: 0.1, min: 0, minAllowed: false));
         sensitivityParameter.Add(createParameter(Constants.Parameters.NUMBER_OF_STEPS, Constants.Dimension.NO_DIMENSION, defaultValue: 2, min: 1, max: 10));
      }

      private string uniqueNameFor(SensitivityAnalysis sensitivityAnalysis, IParameter parameter)
      {
         var defaultName = _fullPathDisplayResolver.FullPathFor(parameter);
         return _containerTask.CreateUniqueName(sensitivityAnalysis.AllSensitivityParameters, defaultName, canUseBaseName: true);
      }

      private IParameter createParameter(string name, IDimension dimension, double defaultValue, double? min, double? max = null, bool minAllowed = true)
      {
         var formula = _objectBaseFactory.Create<ConstantFormula>();
         formula.Value = defaultValue;
         formula.Dimension = dimension;

         var parameter = _objectBaseFactory.Create<IParameter>()
            .WithDimension(dimension)
            .WithName(name)
            .WithFormula(formula);

         parameter.Editable = true;
         parameter.CanBeVaried = false;
         parameter.Visible = true;
         parameter.MinIsAllowed = minAllowed;
         parameter.MinValue = min;
         parameter.MaxValue = max;
         return parameter;
      }
   }
}
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IIdentificationParameterTask
   {
      /// <summary>
      ///    Adds the required parameters definining the <paramref name="identificationParameter" /> using the characteristics of
      ///    the <paramref name="parameter" />
      /// </summary>
      void AddParameterRangeBasedOn(IdentificationParameter identificationParameter, IParameter parameter);

      /// <summary>
      ///    Updates the range parameters according to the UseAsFactor value
      /// </summary>
      void UpdateParameterRange(IdentificationParameter identificationParameter);

      /// <summary>
      ///    Updates the start value of the given <paramref name="identificationParameter" /> from the first linked parameter
      /// </summary>
      void UpdateStartValuesFromSimulation(IdentificationParameter identificationParameter);
   }

   public class IdentificationParameterTask : IIdentificationParameterTask
   {
      private readonly IObjectBaseFactory _objectBaseFactory;

      public IdentificationParameterTask(IObjectBaseFactory objectBaseFactory)
      {
         _objectBaseFactory = objectBaseFactory;
      }

      public void AddParameterRangeBasedOn(IdentificationParameter identificationParameter, IParameter parameter)
      {
         identificationParameter.Add(createParameter(Constants.Parameters.START_VALUE));
         identificationParameter.Add(createParameter(Constants.Parameters.MIN_VALUE));
         identificationParameter.Add(createParameter(Constants.Parameters.MAX_VALUE));

         updateParameterRange(identificationParameter, parameter);
      }

      public void UpdateParameterRange(IdentificationParameter identificationParameter)
      {
         var linkedParmaeter = identificationParameter.AllLinkedParameters.FirstOrDefault();
         var parameter = linkedParmaeter?.Parameter;
         updateParameterRange(identificationParameter, parameter);
      }

      public void UpdateStartValuesFromSimulation(IdentificationParameter identificationParameter)
      {
         if (identificationParameter.UseAsFactor)
            return;

         var linkedParameter = identificationParameter.AllLinkedParameters.FirstOrDefault();
         if (linkedParameter == null)
            return;

         if (linkedParameter.Parameter == null)
            throw new OSPSuiteException(Error.CannotFindSimulationParameterForIdentificationParameter(linkedParameter.FullQuantityPath, identificationParameter.Name));

         identificationParameter.StartValueParameter.Value = linkedParameter.Parameter.Value;
      }

      private void updateParameterRange(IdentificationParameter identificationParameter, IParameter parameter)
      {
         if (parameter == null) return;
         var useAsFactor = identificationParameter.UseAsFactor;
         var dimension = useAsFactor ? Constants.Dimension.NO_DIMENSION : parameter.Dimension;
         var displayUnit = useAsFactor ? dimension.DefaultUnit : parameter.DisplayUnit;
         var startValue = useAsFactor ? Constants.DEFAULT_USE_AS_FACTOR : parameter.Value;
         var minValue = useAsFactor ? startValue / Constants.DEFAULT_PARAMETER_RANGE_FACTOR : parameter.MinValue ?? parameter.Value / Constants.DEFAULT_PARAMETER_RANGE_FACTOR;
         var maxValue = useAsFactor ? startValue * Constants.DEFAULT_PARAMETER_RANGE_FACTOR : parameter.MaxValue ?? parameter.Value * Constants.DEFAULT_PARAMETER_RANGE_FACTOR;

         if (minValue > maxValue)
            swap(ref minValue, ref maxValue);

         updateParameter(identificationParameter, Constants.Parameters.START_VALUE, startValue, dimension, displayUnit);
         updateParameter(identificationParameter, Constants.Parameters.MIN_VALUE, minValue, dimension, displayUnit);
         updateParameter(identificationParameter, Constants.Parameters.MAX_VALUE, maxValue, dimension, displayUnit);
      }

      private static void swap(ref double value1, ref double value2)
      {
         var swap = value1;
         value1 = value2;
         value2 = swap;
      }

      private void updateParameter(IdentificationParameter identificationParameter, string parameterName, double value, IDimension dimension, Unit displayUnit)
      {
         var parameter = identificationParameter.Parameter(parameterName);
         parameter.Dimension = dimension;
         parameter.DisplayUnit = displayUnit;
         parameter.Formula.Dimension = dimension;
         parameter.Formula.DowncastTo<ConstantFormula>().Value = value;
      }

      private IParameter createParameter(string name)
      {
         var parameter = _objectBaseFactory.Create<IParameter>()
            .WithName(name)
            .WithFormula(_objectBaseFactory.Create<ConstantFormula>());

         parameter.Editable = true;
         parameter.CanBeVaried = false;
         parameter.Visible = true;

         return parameter;
      }
   }
}
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.SensitivityAnalyses
{
   public class SensitivityParameter : Container
   {
      public virtual ParameterSelection ParameterSelection { get; set; }
      public virtual IParameter Parameter => ParameterSelection.Parameter;
      public virtual double DefaultValue => Parameter.Value;
      public virtual SensitivityAnalysis SensitivityAnalysis { get; set; }

      public SensitivityParameter()
      {
         Rules?.AddRange(AllRules.All);
      }

      public virtual IParameter VariationRangeParameter => this.Parameter(Constants.Parameters.VARIATION_RANGE);
      public virtual double VariationRangeValue => VariationRangeParameter.Value;

      public virtual IParameter NumberOfStepsParameter => this.Parameter(Constants.Parameters.NUMBER_OF_STEPS);
      public virtual int NumberOfStepsValue => NumberOfStepsParameter.Value.ConvertedTo<int>();

      public virtual bool Analyzes(ParameterSelection parameterSelection)
      {
         return Equals(ParameterSelection, parameterSelection);
      }

      public virtual IEnumerable<double> VariationValues()
      {
         if (DefaultValue == 0)
            return Enumerable.Empty<double>();

         return purify(variationValueForDefaultNotZero());
      }

      private IEnumerable<double> variationValueForDefaultNotZero()
      {
         var nmax = NumberOfStepsValue;
         for (int i = 1; i <= nmax; i++)
         {
            var value = 1 + VariationRangeValue * i / nmax;
            yield return DefaultValue / value;
            yield return DefaultValue * value;
         }
      }

      private IEnumerable<double> purify(IEnumerable<double> values)
      {
         return values.Where(valueIsValid);
      }

      private bool valueIsValid(double value)
      {
         return Parameter.Validate(x => x.Value, value).IsEmpty;
      }

      public virtual void UpdateSimulation(ISimulation newSimulation)
      {
         ParameterSelection.UpdateSimulation(newSimulation);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceSensitivityParameter = source as SensitivityParameter;
         if (sourceSensitivityParameter == null) return;
         ParameterSelection = sourceSensitivityParameter.ParameterSelection?.Clone();
      }

      private static class AllRules
      {
         public static IEnumerable<IBusinessRule> All
         {
            get
            {
               yield return nameNotEmpty;
               yield return nameUnique;
               yield return parameterNotNull;
            }
         }

         private static IBusinessRule parameterNotNull
         {
            get
            {
               return CreateRule.For<SensitivityParameter>()
                  .Property(x => x.Parameter)
                  .WithRule((sensitivityParameter, name) => sensitivityParameter.Parameter != null)
                  .WithError((sensitivityParameter, parameter) => Error.SensitivityAnalysis.TheParameterPathCannotBeFoundInTheSimulation(sensitivityParameter.ParameterSelection.FullQuantityPath));
            }
         }

         private static IBusinessRule nameNotEmpty
         {
            get { return GenericRules.NonEmptyRule<SensitivityParameter>(x => x.Name); }
         }

         private static IBusinessRule nameUnique
         {
            get
            {
               return CreateRule.For<SensitivityParameter>()
                  .Property(x => x.Name)
                  .WithRule((sensitivityParameter, name) =>
                  {
                     var sensitivityAnalysis = sensitivityParameter.SensitivityAnalysis;

                     var otherSensitivityParameter = sensitivityAnalysis?.SensitivityParameterByName(name);
                     if (otherSensitivityParameter == null)
                        return true;

                     return otherSensitivityParameter == sensitivityParameter;
                  })
                  .WithError((field, name) => Error.NameAlreadyExistsInContainerType(name, ObjectTypes.SensitivityParameter));
            }
         }
      }
   }
}
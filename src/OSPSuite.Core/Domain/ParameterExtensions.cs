using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public static class ParameterExtensions
   {
      public static IParameter WithMode(this IParameter parameter, ParameterBuildMode mode)
      {
         parameter.BuildMode = mode;
         return parameter;
      }

      public static T WithRHS<T>(this T parameter, IFormula rhsFormula) where T : IParameter
      {
         parameter.RHSFormula = rhsFormula;
         return parameter;
      }

      public static bool UsesFormula(this IParameter parameter, IFormula formula)
      {
         return (parameter.Formula == formula) || (parameter.RHSFormula == formula);
      }

      public static bool IsDistributed(this IParameter parameter)
      {
         return parameter.IsAnImplementationOf<IDistributedParameter>();
      }

      public static T WithGroup<T>(this T parameter, string groupName) where T : IParameter
      {
         parameter.GroupName = groupName;
         return parameter;
      }

      public static bool IsOfType(this IParameter parameter, PKSimBuildingBlockType buildingBlockType)
      {
         return parameter.BuildingBlockType.Is(buildingBlockType);
      }

      /// <summary>
      ///    Returns true if the value of the parameter was set by the user or if the formula is a constant formula or a
      ///    distributed formula (E.g. an actual constant value that does not depend on other parameter or time)
      /// </summary>
      public static bool IsConstantParameter(this IParameter parameter)
      {
         return parameter.IsFixedValue || parameter.Formula.IsConstant() || parameter.Formula.IsDistributed();
      }

      public static IParameter WithUpdatedMetaFrom(this IParameter parameter, ParameterValue parameterValue)
      {
         parameter.Origin.UpdatePropertiesFrom(parameterValue.Origin);
         parameter.Info.UpdatePropertiesFrom(parameterValue.Info);
         parameter.IsDefault = parameterValue.IsDefault;
         return parameter;
      }
   }
}
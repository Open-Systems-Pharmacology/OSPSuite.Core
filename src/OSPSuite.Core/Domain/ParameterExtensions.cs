using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Formulas;

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

      public static T WithGroup<T>(this T parameter, string groupName) where T:IParameter
      {
         parameter.GroupName = groupName;
         return parameter;
      }

      public static bool IsOfType(this IParameter parameter, PKSimBuildingBlockType buildingBlockType)
      {
         return parameter.BuildingBlockType.Is(buildingBlockType);
      }
   }
}
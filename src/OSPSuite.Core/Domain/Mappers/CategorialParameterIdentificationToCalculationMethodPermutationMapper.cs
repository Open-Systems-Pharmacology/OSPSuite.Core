using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface ICategorialParameterIdentificationToCalculationMethodPermutationMapper
   {
      IEnumerable<CalculationMethodCombination> MapFrom(ParameterIdentification parameterIdentification);
   }

   public class CategorialParameterIdentificationToCalculationMethodPermutationMapper : ICategorialParameterIdentificationToCalculationMethodPermutationMapper
   {
      public IEnumerable<CalculationMethodCombination> MapFrom(ParameterIdentification parameterIdentification)
      {
         var treeRoot = new TreeItem<CalculationMethodWithCompoundName>();
         var categorialParameterIdentificationRunMode = parameterIdentification.Configuration.RunMode.DowncastTo<CategorialParameterIdentificationRunMode>();
         parameterIdentification.DistinctCompoundNames().Each(compoundName =>
         {
            buildCalculationMethodCombinationTree(compoundName, categorialParameterIdentificationRunMode.CalculationMethodCacheFor(compoundName), treeRoot);
         });

         return allCombinations(treeRoot).Where(x => !categorialParameterIdentificationRunMode.AllTheSame || x.AllCompoundsUseSameCalculationMethod());
      }

      private static IEnumerable<CalculationMethodCombination> allCombinations(TreeItem<CalculationMethodWithCompoundName> treeRoot)
      {
         return treeRoot.UniquePaths().Select(x => new CalculationMethodCombination(x.ToList()));
      }

      private void buildCalculationMethodCombinationTree(string compoundName, CalculationMethodCache configurationCache, TreeItem<CalculationMethodWithCompoundName> treeRoot)
      {
         var categoryCache = new Cache<string, IEnumerable<CalculationMethodWithCompoundName>>();

         configurationCache.GroupBy(method => method.Category).Each(grouping =>
         {
            categoryCache[grouping.Key] = grouping.Select(method => new CalculationMethodWithCompoundName(method, compoundName)).ToList();
         });

         categoryCache.Keys.Each(key => treeRoot.AddToAllLeaves(categoryCache[key]));
      }
   }
}
using System;
using System.Linq;
using System.Text;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface ICategorialParameterIdentificationDescriptionCreator
   {
      string CreateDescriptionFor(CalculationMethodCombination combination, CategorialParameterIdentificationRunMode runMode, bool isSingleCategory);
   }

   public class CategorialParameterIdentificationDescriptionCreator : ICategorialParameterIdentificationDescriptionCreator
   {
      private readonly IDisplayNameProvider _displayNameProvider;

      public CategorialParameterIdentificationDescriptionCreator(IDisplayNameProvider displayNameProvider)
      {
         _displayNameProvider = displayNameProvider;
      }

      public string CreateDescriptionFor(CalculationMethodCombination combination, CategorialParameterIdentificationRunMode runMode, bool isSingleCategory)
      {
         if (isSingleCategory && compoundNamesNotRequired(runMode, combination))
            return Captions.ParameterIdentification.CategorialDescriptionWithoutCompoundNameOrCategory(combination.CalculationMethods.First().CalculationMethod.DisplayName);

         if (compoundNamesNotRequired(runMode, combination))
            return createCaptionWithoutCompoundName(combination);

         if (isSingleCategory)
            return createCaptionWithoutCategoryName(combination);

         return createCaption(combination);
      }

      private string createCaptionWithoutCategoryName(CalculationMethodCombination combination)
      {
         Func<CalculationMethodWithCompoundName, string> captionCreator = cm => Captions.ParameterIdentification.CategorialDescriptionWithoutCategory(cm.CompoundName, cm.CalculationMethod.DisplayName);

         return combinationCaptionBuilder(combination, captionCreator);
      }

      private static bool compoundNamesNotRequired(CategorialParameterIdentificationRunMode runMode, CalculationMethodCombination combination)
      {
         return runMode.AllTheSame || combination.HasSingleCompound;
      }

      private string createCaption(CalculationMethodCombination combination)
      {
         Func<CalculationMethodWithCompoundName, string> captionCreator = cm => Captions.ParameterIdentification.CategorialDescription(cm.CompoundName, getDisplayNameForCategory(cm), cm.CalculationMethod.DisplayName);

         return combinationCaptionBuilder(combination, captionCreator);
      }

      private string getDisplayNameForCategory(CalculationMethodWithCompoundName cm)
      {
         return _displayNameProvider.DisplayNameFor(new Category<CalculationMethod> { Name = cm.CalculationMethod.Category });
      }

      private string createCaptionWithoutCompoundName(CalculationMethodCombination combination)
      {
         Func<CalculationMethodWithCompoundName, string> captionCreator = cm => Captions.ParameterIdentification.CategorialDescriptionWithoutCompoundName(getDisplayNameForCategory(cm), cm.CalculationMethod.DisplayName);

         return combinationCaptionBuilder(combination, captionCreator);
      }

      private static string combinationCaptionBuilder(CalculationMethodCombination combination, Func<CalculationMethodWithCompoundName, string> captionCreator)
      {
         var sb = new StringBuilder();
         combination.CalculationMethods.Each(cm => sb.AppendLine(captionCreator(cm)));

         return sb.ToString().Trim();
      }
   }
}
using OSPSuite.Assets;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO.Charts
{
   public class FoldValueDTO : IValidatable
   {
      public float FoldValue { get; set; };

      public IBusinessRuleSet Rules => AllRules.Default;

      private static class AllRules
      {
         private static IBusinessRule foldValueGreaterThanOne
         {
            get
            {
               return CreateRule.For<FoldValueDTO>()
                  .Property(item => item.FoldValue)
                  .WithRule((x, value) => value > 1.0)
                  .WithError(Error.FoldValueMustBeGreaterThanOne);
            }
         }

         public static IBusinessRuleSet Default => new BusinessRuleSet(foldValueGreaterThanOne);
      }
   }
}
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO.Charts
{
   public class FoldValueDTO : IValidatable
   {
      public float FoldValue { get; set; }

      public IBusinessRuleSet Rules
      {
         get { return AllRules.Default; }
      }

      private static class AllRules
      {
         public static IBusinessRule FoldValueGreaterThanOne
         {
            get
            {
               return CreateRule.For<FoldValueDTO>()
                  .Property(item => item.FoldValue)
                  .WithRule((x, value) => value > 1.0)
                  .WithError("Fold value must be a number greater than one.");
            }
         }

         public static IBusinessRuleSet Default
         {
            get { return new BusinessRuleSet(FoldValueGreaterThanOne); }
         }
      }
   }
}
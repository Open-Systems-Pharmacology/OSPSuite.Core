namespace OSPSuite.Core.Domain
{
   public static class MoleculeAmountExtension
   {
      public static T WithScaleFactor<T>(this T amount, double scaleFactor) where T : MoleculeAmount
      {
         amount.ScaleDivisor = scaleFactor;
         return amount;
      }
   }
}
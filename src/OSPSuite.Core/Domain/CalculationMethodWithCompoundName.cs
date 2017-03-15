namespace OSPSuite.Core.Domain
{
   public class CalculationMethodWithCompoundName
   {
      public CalculationMethod CalculationMethod { get; }
      public string CompoundName { get; }

      public CalculationMethodWithCompoundName(CalculationMethod method, string compoundName)
      {
         CalculationMethod = method;
         CompoundName = compoundName;
      }

      public override string ToString()
      {
         return $"{CompoundName} - {CalculationMethod}";
      }
   }
}
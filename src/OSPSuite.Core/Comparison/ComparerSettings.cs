using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public enum FormulaComparison
   {
      /// <summary>
      ///    Parameter values will be compared=>all dependent parameters will be taken into consideration
      /// </summary>
      Value,

      /// <summary>
      ///    Only formula will be compared=>dependent parameters having the same formula but different values will not appear in
      ///    the difference
      /// </summary>
      Formula
   }

   /// <summary>
   ///    Settings used to perform the comparison
   /// </summary>
   public class ComparerSettings
   {
      /// <summary>
      ///    Relative tolerance used to compare double value. Default value is 1e-2
      /// </summary>
      public double RelativeTolerance { get; set; }

      /// <summary>
      ///    If set to true, parameter like dimension, units etc will be ignored. Default is true
      /// </summary>
      public bool OnlyComputingRelevant { get; set; }

      /// <summary>
      ///    Set the formula comparison mode. If set to <code>FormulaComparison.Value</code>, all dependent parameter of a
      ///    changed quantity will
      ///    also appear in the difference. If set to <code>FormulaComparison.Formula</code>, a quantity will only appear if the
      ///    compared formulas are indeed different.
      ///    Default is set to <code>FormulaComparison.Formula</code>
      /// </summary>
      public FormulaComparison FormulaComparison { get; set; }

      /// <summary>
      /// Specifies if hidden entities for PKSim (such as hidden parameters or constant formula) should be part of the comparison. This settings is application dependant (default is <c>false</c>)
      /// </summary>
      public bool CompareHiddenEntities { get; set; }

      /// <summary>
      /// Specifies if Value origin should be displayed for compared quantities. Default is <c>true</c>
      /// </summary>
      public bool ShowValueOrigin { get; set; }


      public ComparerSettings()
      {
         OnlyComputingRelevant = true;
         FormulaComparison = FormulaComparison.Formula;
         RelativeTolerance = Constants.DOUBLE_RELATIVE_EPSILON;
         CompareHiddenEntities = false;
         ShowValueOrigin = true;
      }
   }
}
namespace OSPSuite.Core.Domain
{
   public class PKAnalysisFormatter : DoubleFormatter
   {
      public virtual string Format(object valueToFormat)
      {
         if (valueToFormat == null)
            return string.Empty;

         double value;

         if (double.TryParse(valueToFormat.ToString(), out value))
            return base.Format(value);

         return "n.a";
      }
   }

}
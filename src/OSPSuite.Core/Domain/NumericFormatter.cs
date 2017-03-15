using OSPSuite.Utility.Format;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public class UnitFormatter : NumericFormatter<double>
   {
      private readonly string _unit;

      public UnitFormatter() : this(string.Empty)
      {
      }

      public UnitFormatter(Unit unit) : this(unit?.Name ?? string.Empty)
      {
      }

      public UnitFormatter(string unit) : base(NumericFormatterOptions.Instance)
      {
         _unit = unit;
      }

      public override string Format(double valueToFormat)
      {
         return Format(valueToFormat, _unit);
      }

      public virtual string Format(double valueToFormat, Unit unit)
      {
         string unitName = string.Empty;
         if (unit != null)
            unitName = unit.Name;
         return Format(valueToFormat, unitName);
      }

      public virtual string Format(double valueToFormat, string unit)
      {
         if (double.IsNaN(valueToFormat))
            return Constants.NAN;

         var formattedValue = base.Format(valueToFormat);
         if (string.IsNullOrEmpty(unit))
            return formattedValue;

         return $"{formattedValue} {unit}";
      }
   }

   public class DoubleFormatter : UnitFormatter
   {
      public DoubleFormatter()
         : base(string.Empty)
      {
      }
   }

   public class NameValueUnitsFormatter : DoubleFormatter
   {
      public string Format(double valueToFormat, string unit, string name)
      {
         return $"{name}={Format(valueToFormat, unit)}";
      }
   }
}
namespace OSPSuite.Core.Domain
{
   public class OriginDataParameter : IWithName
   {
      /// <summary>
      ///    Name of parameter. Can be null if parameter is used as field
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      ///    Value of parameter, always in base unit
      /// </summary>
      public double Value { get; set; }

      /// <summary>
      ///    Unit used When the parameter was entered. This is the unit selected by the user and it not necessarily the base unit
      /// </summary>
      public string Unit { get; set; }

      public OriginDataParameter Clone() => new OriginDataParameter
      {
         Value = Value,
         Unit = Unit,
         Name = Name
      };

      public void Deconstruct(out double value, out string unit)
      {
         value = Value;
         unit = Unit;
      }

      public void Deconstruct(out double value, out string unit, out string name)
      {
         value = Value;
         unit = Unit;
         name = Name;
      }

      public OriginDataParameter()
      {
      }

      public OriginDataParameter(double value, string unit = "", string name = "")
      {
         Name = name;
         Value = value;
         Unit = unit;
      }
   }
}

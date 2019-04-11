using System;
using System.Globalization;

namespace OSPSuite.Core.Domain.UnitSystem
{
   public class Unit
   {
      internal string FactorFormula { get; set; }

      /// <summary>
      ///    Factor with which a value in this unit will be multiplied to be converted in the base unit of a dimension.
      ///    For the base unit, the factor is 1
      /// </summary>
      public virtual double Factor { get; set; }

      /// <summary>
      ///    Offset that will be added to a value in this unit to be converted in the base unit of a dimension (default is 0)
      /// </summary>
      public virtual double Offset { get; }

      /// <summary>
      ///    Name of unit. Should be unique in the dimension containing this unit
      /// </summary>
      public virtual string Name { get; }

      /// <summary>
      ///    Gets or sets if a unit should be displayed or not (some kernel unit are typically hidden)
      ///    Default is false
      /// </summary>
      public virtual bool Visible { get; set; }

      [Obsolete("For deserialization")]
      public Unit()
      {
         Visible = true;
      }

      public Unit(string name, double factor, double offset)
      {
         Name = name;

         if (factor <= 0)
            throw new ArgumentException("Factor <= 0");

         Factor = factor;
         Offset = offset;
         Visible = true;
         FactorFormula = factor.ToString(NumberFormatInfo.InvariantInfo);
      }

      public virtual double BaseUnitValueToUnitValue(double baseUnitValue)
      {
         return baseUnitValue / Factor - Offset;
      }

      public virtual double UnitValueToBaseUnitValue(double unitValue)
      {
         return (unitValue + Offset) * Factor;
      }

      public override string ToString()
      {
         return Name;
      }
   }
}
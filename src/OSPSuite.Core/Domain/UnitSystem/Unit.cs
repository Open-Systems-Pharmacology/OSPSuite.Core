using System;
using System.Collections.Generic;
using System.Globalization;
using OSPSuite.Utility.Collections;

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

      /// <summary>
      ///    Returns the list of unit synonyms defined for this unit
      /// </summary>
      public virtual IReadOnlyCollection<UnitSynonym> UnitSynonyms => _unitSynonyms;

      private readonly Cache<string, UnitSynonym> _unitSynonyms = new Cache<string, UnitSynonym>(x => x.Name);

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

      public virtual bool HasSynonym(string name) => _unitSynonyms.Contains(name);

      public virtual void AddUnitSynonym(string name) => AddUnitSynonym(new UnitSynonym(name));

      public virtual void AddUnitSynonym(UnitSynonym unitSynonym)
      {
         if (HasSynonym(unitSynonym.Name))
            return;

         _unitSynonyms.Add(unitSynonym);
      }

      public virtual double BaseUnitValueToUnitValue(double baseUnitValue) => baseUnitValue / Factor - Offset;

      public virtual double UnitValueToBaseUnitValue(double unitValue) => (unitValue + Offset) * Factor;

      public override string ToString() => Name;
   }
}
using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class IdentificationParameterHistory
   {
      private readonly List<double> _values = new List<double>();
      public string Name { get; }
      public IReadOnlyList<double> Values => _values;
      public IDimension Dimension { get; }
      public Unit DisplayUnit { get; }
      public string DisplayName { get; }

      public IdentificationParameterHistory(IdentificationParameter identificationParameter)
      {
         Name = identificationParameter.Name;
         Dimension = identificationParameter.Dimension;
         DisplayUnit = identificationParameter.DisplayUnit;
         DisplayName = Constants.NameWithUnitFor(Name, DisplayUnit);
      }


      public double ValueAt(int i)
      {
         return _values[i];
      }

      public void AddValue(double value)
      {
         _values.Add(value);
      }

      public double DisplayValueAt(int i)
      {
         return Dimension.BaseUnitValueToUnitValue(DisplayUnit, ValueAt(i));
      }
   }
}
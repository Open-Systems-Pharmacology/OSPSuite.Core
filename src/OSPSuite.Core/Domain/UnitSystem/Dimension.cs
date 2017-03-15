using System;
using System.Collections.Generic;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.UnitSystem
{
   public interface IDimension : IComparable<IDimension>, IComparable
   {
      BaseDimensionRepresentation BaseRepresentation { get; }

      /// <summary>
      ///    Internal name of dimension. Name will be used for display name if display name is not set
      /// </summary>
      string Name { get; }

      /// <summary>
      ///    Returns the display name of the dimension. If not defined, the name will be returned
      /// </summary>
      string DisplayName { get; set; }

      /// <summary>
      ///    Base unit of dimension
      /// </summary>
      Unit BaseUnit { get; }

      /// <summary>
      ///    Default Unit of dimension, is not necessarily the base unit
      /// </summary>
      Unit DefaultUnit { get; set; }

      /// <summary>
      ///    name of default dimension
      /// </summary>
      string DefaultUnitName { get; }

      /// <summary>
      ///    Returns the units defined in the dimension
      /// </summary>
      IEnumerable<Unit> Units { get; }

      /// <summary>
      ///    Returns the names of all units defined in the dimension
      /// </summary>
      IEnumerable<string> GetUnitNames();

      /// <summary>
      ///    Returns the unit defined with the given name.
      /// </summary>
      /// <param name="name">Name of unit</param>
      /// <exception cref="KeyNotFoundException">is thrown if unit with the given name is not found.</exception>
      Unit Unit(string name);

      /// <summary>
      ///    Returns the unit defined with the given name or the default unit if the unit is not found
      /// </summary>
      /// <param name="name">Name of unit</param>
      Unit UnitOrDefault(string name);

      /// <summary>
      ///    Converts the given value (in base unit) to a representation of the value in the <paramref name="unit" />
      /// </summary>
      /// <param name="unit">Unit into which the value should be converted</param>
      /// <param name="value">Value in base unit to convert</param>
      double BaseUnitValueToUnitValue(Unit unit, double value);

      /// <summary>
      ///    Converts the given value in <paramref name="unit" /> to values in the base unit
      /// </summary>
      /// <param name="unit">Unit of the value given as parameter</param>
      /// <param name="value">Value to be converted in base unit </param>
      double UnitValueToBaseUnitValue(Unit unit, double value);

      /// <summary>
      ///    Adds a unit to the dimension. The unit won't be set as default unit
      /// </summary>
      Unit AddUnit(string unitName, double factor, double offset);

      /// <summary>
      ///    Adds a unit to the dimension and set the unit as default unit if the flag is set to true
      /// </summary>
      Unit AddUnit(string unitName, double factor, double offset, bool isDefault);

      void RemoveUnit(string unitName);

      void AddUnit(Unit unit);

      /// <summary>
      ///    Returns true if the dimension has a unit named <paramref name="unitName" /> otherwise false
      /// </summary>
      bool HasUnit(string unitName);

      /// <summary>
      ///    Returns true if the dimension has a unit with the same name as <paramref name="unit" /> otherwise false
      /// </summary>
      bool HasUnit(Unit unit);

      /// <summary>
      ///    Returns true if values in base unit can be converted to unit <paramref name="unitName" /> otherwise false; can be
      ///    different from HasUnit for MergedDimension with missing Molweight.
      /// </summary>
      bool CanConvertToUnit(string unitName);
   }

   public class Dimension : IDimension
   {
      private readonly BaseDimensionRepresentation _baseRepresentation;
      private readonly ICache<string, Unit> _units;
      private Unit _defaultUnit;
      private Unit _baseUnit;
      public string Name { get; private set; }
      private string _displayName;

      public Dimension() //parameterless constructor for deserialization
      {
         _units = new Cache<string, Unit>(x => x.Name);
         Name = string.Empty;
      }

      public Dimension(BaseDimensionRepresentation baseRepresentation, string dimensionName, string baseUnitName) : this()
      {
         _baseRepresentation = baseRepresentation;
         Name = dimensionName;
         BaseUnit = new Unit(baseUnitName, 1.0, 0.0);
         _units.Add(BaseUnit);
      }

      public BaseDimensionRepresentation BaseRepresentation
      {
         get { return _baseRepresentation; }
      }

      public string DisplayName
      {
         get { return string.IsNullOrEmpty(_displayName) ? Name : _displayName; }
         set { _displayName = value; }
      }

      public Unit BaseUnit
      {
         get { return _baseUnit; }
         internal set
         {
            _baseUnit = value;
            _defaultUnit = value;
         }
      }

      public Unit DefaultUnit
      {
         get { return _defaultUnit; }
         set
         {
            if (!HasUnit(value.Name))
               throw new ArgumentException("value not in Units list");

            _defaultUnit = value;
         }
      }

      public string DefaultUnitName
      {
         get { return DefaultUnit.Name; }
      }

      public IEnumerable<Unit> Units
      {
         get { return _units; }
      }

      public IEnumerable<string> GetUnitNames()
      {
         return _units.Keys;
      }

      public Unit Unit(string name)
      {
         return _units[name];
      }

      public Unit UnitOrDefault(string name)
      {
         return HasUnit(name) ? Unit(name) : DefaultUnit;
      }

      public double BaseUnitValueToUnitValue(Unit unit, double value)
      {
         return value / unit.Factor - unit.Offset;
      }

      public double UnitValueToBaseUnitValue(Unit unit, double unitValue)
      {
         return (unitValue + unit.Offset) * unit.Factor;
      }

      public Unit AddUnit(string unitName, double factor, double offset)
      {
         return AddUnit(unitName, factor, offset, false);
      }

      public Unit AddUnit(string unitName, double factor, double offset, bool isDefault)
      {
         if (_units.Contains(unitName))
            throw new NotUniqueIdException(unitName);

         var unit = new Unit(unitName, factor, offset);
         AddUnit(unit);

         if (isDefault)
            DefaultUnit = unit;

         return unit;
      }

      public void RemoveUnit(string unitName)
      {
         _units.Remove(unitName);
      }

      public void AddUnit(Unit unit)
      {
         if (HasUnit(unit.Name))
            throw new NotUniqueIdException(unit.Name);

         _units.Add(unit.Name, unit);
      }

      public bool HasUnit(string unitName)
      {
         return _units.Contains(unitName);
      }

      public bool HasUnit(Unit unit)
      {
         return unit != null && HasUnit(unit.Name);
      }

      public bool CanConvertToUnit(string unitName)
      {
         return HasUnit(unitName);
      }

      public int CompareTo(IDimension other)
      {
         if (other == null)
            return 1;

         return string.Compare(DisplayName, other.DisplayName, StringComparison.Ordinal);
      }

      public override string ToString()
      {
         return DisplayName;
      }

      public int CompareTo(object obj)
      {
         return CompareTo(obj as IDimension);
      }
   }
}
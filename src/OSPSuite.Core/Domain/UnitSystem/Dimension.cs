using System;
using System.Collections.Generic;
using System.Linq;
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
      ///    Returns the unit defined with the given name (or synonym) or null if not found
      /// </summary>
      /// <param name="unitName">Name of unit</param>
      /// <param name="ignoreCase">Should ignore case when matching units. Default is <c>false</c></param>
      Unit FindUnit(string unitName, bool ignoreCase = false);

      /// <summary>
      ///    Returns the true if a unt with the given name (or synonym) is found otherwise false
      /// </summary>
      /// <param name="unitName">Name of unit</param>
      /// <param name="ignoreCase">Should ignore case when matching units. Default is <c>false</c></param>
      bool SupportsUnit(string unitName, bool ignoreCase = false);

      /// <summary>
      ///    Returns the unit defined with the given name or the default unit if the unit is not found
      /// </summary>
      /// <param name="name">Name of unit</param>
      Unit UnitOrDefault(string name);

      /// <summary>
      ///    Returns the unit defined at the 0-based index <paramref name="index" />.
      /// </summary>
      /// <param name="index">0-based index if the unit in the unit array</param>
      /// ///
      /// <exception cref="ArgumentOutOfRangeException">is thrown if index does not match the units array dimensions</exception>
      /// <returns>The unit at <paramref name="index" /></returns>
      Unit UnitAt(int index);

      /// <summary>
      ///    Converts the given value (in base unit) to a representation of the value in the <paramref name="unit" />
      /// </summary>
      /// <param name="unit">Unit into which the value should be converted</param>
      /// <param name="valueInBaseUnit">Value in base unit to convert</param>
      double BaseUnitValueToUnitValue(Unit unit, double valueInBaseUnit);

      /// <summary>
      ///    Converts the given value in <paramref name="unit" /> to values in the base unit
      /// </summary>
      /// <param name="unit">Unit of the value given as parameter</param>
      /// <param name="valueInUnit">Value to be converted in base unit </param>
      double UnitValueToBaseUnitValue(Unit unit, double valueInUnit);

      /// <summary>
      ///    Adds a unit to the dimension and set the unit as default unit if the flag is set to true
      /// </summary>
      /// <param name="unitName">Name of unit. Needs to be unique</param>
      /// <param name="factor">Value multiplied by this factor will be in base unit</param>
      /// <param name="offset">Add the factor to a value to convert to base unit</param>
      /// <param name="isDefault">True if this unit is the default unit otherwise False. Default is <c>false</c></param>
      Unit AddUnit(string unitName, double factor, double offset, bool isDefault = false);

      void RemoveUnit(string unitName);

      void AddUnit(Unit unit);

      /// <summary>
      ///    Returns true if the dimension has a unit or a unit synonym named <paramref name="unitName" /> otherwise false
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
      private readonly Cache<string, Unit> _units = new Cache<string, Unit>(x => x.Name);

      private Unit _defaultUnit;
      private Unit _baseUnit;
      public string Name { get; } = string.Empty;
      private string _displayName;
      public BaseDimensionRepresentation BaseRepresentation { get; }

      [Obsolete("For serialization")]
      public Dimension()
      {
      }

      public Dimension(BaseDimensionRepresentation baseRepresentation, string dimensionName, string baseUnitName) : this()
      {
         BaseRepresentation = baseRepresentation;
         Name = dimensionName;
         BaseUnit = new Unit(baseUnitName, 1.0, 0.0);
         _units.Add(BaseUnit);
      }

      public string DisplayName
      {
         get => string.IsNullOrEmpty(_displayName) ? Name : _displayName;
         set => _displayName = value;
      }

      public Unit BaseUnit
      {
         get => _baseUnit;
         internal set
         {
            _baseUnit = value;
            _defaultUnit = value;
         }
      }

      public Unit DefaultUnit
      {
         get => _defaultUnit;
         set
         {
            if (!HasUnit(value.Name))
               throw new ArgumentException("value not in Units list");

            _defaultUnit = value;
         }
      }

      public string DefaultUnitName => DefaultUnit.Name;

      public IEnumerable<Unit> Units => _units;

      public IEnumerable<string> GetUnitNames() => _units.Keys;

      public Unit Unit(string name)
      {
         if (_units.Contains(name))
            return _units[name];

         //it might be a unit synonym. We use first because the contract specifies
         return _units.First(x => x.HasSynonym(name));
      }

      public Unit FindUnit(string unitName, bool ignoreCase = false)
      {
         var comparisonStrategy = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
         return Units.FirstOrDefault(x => x.SupportsName(unitName, comparisonStrategy));
      }

      public bool SupportsUnit(string unitName, bool ignoreCase = false)
      {
         var comparisonStrategy = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
         return Units.Any(x => x.SupportsName(unitName, comparisonStrategy));
      }

      public Unit UnitOrDefault(string name) => HasUnit(name) ? Unit(name) : DefaultUnit;

      public Unit UnitAt(int index)
      {
         if (index < 0 || index >= _units.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

         return _units.ElementAt(index);
      }

      public double BaseUnitValueToUnitValue(Unit unit, double valueInBaseUnit)
      {
         return valueInBaseUnit / unit.Factor - unit.Offset;
      }

      public double UnitValueToBaseUnitValue(Unit unit, double valueInUnit)
      {
         return (valueInUnit + unit.Offset) * unit.Factor;
      }

      public Unit AddUnit(string unitName, double factor, double offset) => AddUnit(unitName, factor, offset, false);

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

      private bool hasUnitWithSynonym(string unitName) => _units.Any(x => x.HasSynonym(unitName));

      public bool HasUnit(string unitName) => _units.Contains(unitName) || hasUnitWithSynonym(unitName);

      public bool HasUnit(Unit unit) => unit != null && HasUnit(unit.Name);

      public bool CanConvertToUnit(string unitName) => HasUnit(unitName);

      public int CompareTo(IDimension other)
      {
         if (other == null)
            return 1;

         return string.Compare(DisplayName, other.DisplayName, StringComparison.Ordinal);
      }

      public override string ToString() => DisplayName;

      public int CompareTo(object obj)
      {
         return CompareTo(obj as IDimension);
      }
   }
}
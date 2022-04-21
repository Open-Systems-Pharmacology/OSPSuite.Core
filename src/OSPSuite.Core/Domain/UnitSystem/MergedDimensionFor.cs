using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.UnitSystem
{
   public interface IMergedDimension : IDimension
   {
      IDimension SourceDimension { get; }
      IReadOnlyList<IDimension> TargetDimensions { get; }
   }

   public class MergedDimensionFor<T> : IMergedDimension where T : IWithDimension
   {
      private readonly IReadOnlyList<IDimensionConverter> _converters;

      private readonly ICache<string, Unit> _units = new Cache<string, Unit>(x => x.Name);

      public string DisplayName { get; set; }

      public IDimension SourceDimension { get; }

      public IReadOnlyList<IDimension> TargetDimensions { get; }

      public MergedDimensionFor(IDimension sourceDimension, IReadOnlyList<IDimension> targetDimensions, IReadOnlyList<IDimensionConverter> converters)
      {
         SourceDimension = sourceDimension;
         TargetDimensions = targetDimensions.Where(dim => converters.Any(c => c.CanConvertTo(dim))).ToList();
         _converters = converters;
      }

      private void fillUpUnits()
      {
         _units.AddRange(SourceDimension.Units);
         TargetDimensions.Each(dim => { _units.AddRange(dim.Units); });
      }

      public BaseDimensionRepresentation BaseRepresentation => SourceDimension.BaseRepresentation;

      public string Name => SourceDimension.Name;

      public Unit BaseUnit => SourceDimension.BaseUnit;

      public Unit DefaultUnit
      {
         get => SourceDimension.DefaultUnit;
         set => SourceDimension.DefaultUnit = value;
      }

      public string DefaultUnitName => SourceDimension.DefaultUnitName;

      public IEnumerable<Unit> Units => cachedUnit;

      public IEnumerable<string> GetUnitNames()
      {
         return cachedUnit.Keys;
      }

      private ICache<string, Unit> cachedUnit
      {
         get
         {
            if (!_units.Any())
               fillUpUnits();

            return _units;
         }
      }

      public Unit Unit(string name)
      {
         if (cachedUnit.Contains(name))
            return cachedUnit[name];

         //The contract specifies an exception if not found. That's why we ask for the cachedUnit again. It will throw
         return FindUnit(name) ?? cachedUnit[name];
      }

      public Unit FindUnit(string unitName, bool ignoreCase = false)
      {
         if (unitName == null)
            return null;

         return SourceDimension.FindUnit(unitName, ignoreCase) ??
                TargetDimensions.Select(x => x.FindUnit(unitName, ignoreCase)).FirstOrDefault();
      }

      public bool SupportsUnit(string unitName, bool ignoreCase = false)
      {
         return FindUnit(unitName, ignoreCase) != null;
      }

      public Unit UnitOrDefault(string name) => FindUnit(name) ?? DefaultUnit;

      public Unit UnitAt(int index)
      {
         return cachedUnit.ElementAt(index);
      }

      public double BaseUnitValueToUnitValue(Unit unit, double valueInBaseUnit)
      {
         if (SourceDimension.HasUnit(unit))
            return SourceDimension.BaseUnitValueToUnitValue(unit, valueInBaseUnit);

         var usedDimension = targetDimensionWith(unit);
         var usedConverter = converterFor(usedDimension);

         if (usedConverter.CanResolveParameters())
            return usedDimension.BaseUnitValueToUnitValue(unit, usedConverter.ConvertToTargetBaseUnit(valueInBaseUnit));

         throw new UnableToResolveParametersException(unit, usedConverter.UnableToResolveParametersMessage);
      }

      public float BaseUnitValueToUnitValue(Unit unit, float valueInBaseUnit)
      {
         return Convert.ToSingle(BaseUnitValueToUnitValue(unit, Convert.ToDouble(valueInBaseUnit)));
      }

      private IDimensionConverter converterFor(IDimension usedDimension)
      {
         return _converters.First(converter => converter.CanConvertTo(usedDimension));
      }

      public double UnitValueToBaseUnitValue(Unit unit, double valueInUnit)
      {
         if (SourceDimension.HasUnit(unit))
            return SourceDimension.UnitValueToBaseUnitValue(unit, valueInUnit);

         var usedDimension = targetDimensionWith(unit);
         var usedConverter = converterFor(usedDimension);

         if (usedConverter.CanResolveParameters())
            return usedConverter.ConvertToSourceBaseUnit(usedDimension.UnitValueToBaseUnitValue(unit, valueInUnit));

         throw new UnableToResolveParametersException(unit, usedConverter.UnableToResolveParametersMessage);
      }

      public float UnitValueToBaseUnitValue(Unit unit, float valueInUnit)
      {
         return Convert.ToSingle(UnitValueToBaseUnitValue(unit, Convert.ToDouble(valueInUnit)));

      }

      private IDimension targetDimensionWith(Unit unit)
      {
         return TargetDimensions.FirstOrDefault(dimensions => dimensions.HasUnit(unit));
      }

      public Unit AddUnit(string unitName, double factor, double offset)
      {
         throw new InvalidOperationException("Cannot call AddUnit in MergedDimension");
      }

      public Unit AddUnit(string unitName, double factor, double offset, bool isDefault)
      {
         throw new InvalidOperationException("Cannot call AddUnit in MergedDimension");
      }

      public void RemoveUnit(string unitName)
      {
         throw new InvalidOperationException("Cannot call RemoveUnit in MergedDimension");
      }

      public void AddUnit(Unit unit)
      {
         throw new InvalidOperationException("Cannot call AddUnit in MergedDimension");
      }

      public bool HasUnit(string unitName) => SupportsUnit(unitName);

      public bool HasUnit(Unit unit)
      {
         return unit != null && HasUnit(unit.Name);
      }

      public bool CanConvertToUnit(string unitName)
      {
         if (!HasUnit(unitName))
            return false;

         var unit = Unit(unitName);
         if (SourceDimension.HasUnit(unit))
            return true;

         var usedDimension = targetDimensionWith(unit);
         var usedConverter = converterFor(usedDimension);

         return usedConverter.CanResolveParameters();
      }

      public int CompareTo(IDimension other) => SourceDimension.CompareTo(other);

      public override string ToString() => DisplayName;

      public int CompareTo(object obj) => SourceDimension.CompareTo(obj);
   }

   public class UnableToResolveParametersException : OSPSuiteException
   {
      public Unit Unit { get; }
      public string UnableToResolveParametersMessage { get; }

      public UnableToResolveParametersException(Unit unit, string unableToResolveParametersMessage) : base(
         $"Unable to convert to {unit.Name}.\n{unableToResolveParametersMessage}")
      {
         Unit = unit;
         UnableToResolveParametersMessage = unableToResolveParametersMessage;
      }
   }
}
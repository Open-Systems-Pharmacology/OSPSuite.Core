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
      IEnumerable<IDimension> TargetDimensions { get; }
   }

   public class MergedDimensionFor<T> : IMergedDimension where T : IWithDimension
   {
      private readonly IReadOnlyList<IDimensionConverterFor> _converters;
      private readonly ICache<string, Unit> _units = new Cache<string, Unit>(x => x.Name);
      public string DisplayName { get; set; }

      public MergedDimensionFor(IDimension sourceDimension, IEnumerable<IDimension> targetDimensions, IReadOnlyList<IDimensionConverterFor> converters)
      {
         SourceDimension = sourceDimension;
         TargetDimensions = targetDimensions;
         _converters = converters;
      }

      private void fillUpUnits()
      {
         _units.AddRange(SourceDimension.Units);
         _converters.Each(addUnitsFromConverter);
      }

      private void addUnitsFromConverter(IDimensionConverterFor converter)
      {
         TargetDimensions.Where(converter.CanConvertTo).Each(dim => { _units.AddRange(dim.Units); });
      }

      public IDimension SourceDimension { get; }

      public IEnumerable<IDimension> TargetDimensions { get; }

      public BaseDimensionRepresentation BaseRepresentation => SourceDimension.BaseRepresentation;

      public string Name => SourceDimension.Name;

      public Unit BaseUnit => SourceDimension.BaseUnit;

      public Unit DefaultUnit
      {
         get { return SourceDimension.DefaultUnit; }
         set { SourceDimension.DefaultUnit = value; }
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
         return cachedUnit[name];
      }

      public Unit UnitOrDefault(string name)
      {
         return HasUnit(name) ? Unit(name) : DefaultUnit;
      }

      public double BaseUnitValueToUnitValue(Unit unit, double value)
      {
         if (SourceDimension.Units.Contains(unit))
            return SourceDimension.BaseUnitValueToUnitValue(unit, value);

         var usedDimension = targetDimensionWith(unit);
         var usedConverter = converterFor(usedDimension);

         if (usedConverter.CanResolveParameters())
            return usedDimension.BaseUnitValueToUnitValue(unit, usedConverter.ConvertToTargetBaseUnit(value));

         throw new UnableToResolveParametersException(unit, usedConverter.UnableToResolveParametersMessage);
      }

      private IDimensionConverterFor converterFor(IDimension usedDimension)
      {
         return _converters.First(converter => converter.CanConvertTo(usedDimension));
      }

      public double UnitValueToBaseUnitValue(Unit unit, double value)
      {
         if (SourceDimension.Units.Contains(unit))
            return SourceDimension.UnitValueToBaseUnitValue(unit, value);

         var usedDimension = targetDimensionWith(unit);
         var usedConverter = converterFor(usedDimension);

         if (usedConverter.CanResolveParameters())
            return usedConverter.ConvertToSourceBaseUnit(usedDimension.UnitValueToBaseUnitValue(unit, value));

         throw new UnableToResolveParametersException(unit, usedConverter.UnableToResolveParametersMessage);
      }

      private IDimension targetDimensionWith(Unit unit)
      {
         return TargetDimensions.FirstOrDefault(dimensions => dimensions.Units.Contains(unit));
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

      public bool HasUnit(string unitName)
      {
         return cachedUnit.Contains(unitName);
      }

      public bool HasUnit(Unit unit)
      {
         return unit != null && HasUnit(unit.Name);
      }

      public bool CanConvertToUnit(string unitName)
      {
         if (!HasUnit(unitName)) return false;

         var unit = Unit(unitName);
         if (SourceDimension.Units.Contains(unit)) return true;

         var usedDimension = targetDimensionWith(unit);
         var usedConverter = converterFor(usedDimension);

         return usedConverter.CanResolveParameters();
      }

      public int CompareTo(IDimension other)
      {
         return SourceDimension.CompareTo(other);
      }

      public override string ToString()
      {
         return DisplayName;
      }

      public int CompareTo(object obj)
      {
         return SourceDimension.CompareTo(obj);
      }
   }

   public class UnableToResolveParametersException : OSPSuiteException
   {
      public Unit Unit { get; private set; }
      public string UnableToResolveParametersMessage { get; private set; }

      public UnableToResolveParametersException(Unit unit, string unableToResolveParametersMessage) : base($"Unable to convert to {unit.Name}.\n{unableToResolveParametersMessage}")
      {
         Unit = unit;
         UnableToResolveParametersMessage = unableToResolveParametersMessage;
      }
   }
}
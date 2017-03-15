using System;
using System.Linq;
using OSPSuite.Utility;

namespace OSPSuite.Core.Domain.UnitSystem
{
   public enum BaseDimension
   {
      Length = 0,
      Mass = 1,
      Time = 2,
      ElectricCurrent = 3,
      Temperature = 4,
      Amount = 5,
      LuminousIntensity = 6
   }

   public class BaseDimensionRepresentation
   {
      private const int BASE_DIMENSIONS = 7;
      private readonly double[] _exponents = new double[BASE_DIMENSIONS];
      // Prefix 10, Length [m], Mass [kg], Time [s], 
      // Electric Current [A], Thermodynamic Temperature [K], Amount of Substance [mol], Luminous Intensity [cd] 
      // are the SI-Base Dimensions 

      public BaseDimensionRepresentation(double[] exponents)
      {
         if (exponents.Length != BASE_DIMENSIONS)
            throw new ArgumentException(BASE_DIMENSIONS + " != Length of Array Exponents = " + exponents.Length);
         _exponents = exponents;
      }

      public BaseDimensionRepresentation() : this(new double[BASE_DIMENSIONS])
      {
         for (var i = 0; i < BASE_DIMENSIONS; i++)
         {
            _exponents[i] = 0;
         }
      }

      public BaseDimensionRepresentation(BaseDimensionRepresentation baseDimensionRepresentation)
      {
         LengthExponent = baseDimensionRepresentation.LengthExponent;
         MassExponent = baseDimensionRepresentation.MassExponent;
         TimeExponent = baseDimensionRepresentation.TimeExponent;
         ElectricCurrentExponent = baseDimensionRepresentation.ElectricCurrentExponent;
         TemperatureExponent = baseDimensionRepresentation.TemperatureExponent;
         AmountExponent = baseDimensionRepresentation.AmountExponent;
         LuminousIntensityExponent = baseDimensionRepresentation.LuminousIntensityExponent;
      }

      public double MassExponent
      {
         set { _exponents[(int) BaseDimension.Mass] = value; }
         get { return Exponent(BaseDimension.Mass); }
      }

      public double LuminousIntensityExponent
      {
         set { _exponents[(int) BaseDimension.LuminousIntensity] = value; }
         get { return Exponent(BaseDimension.LuminousIntensity); }
      }

      public double AmountExponent
      {
         set { _exponents[(int) BaseDimension.Amount] = value; }
         get { return Exponent(BaseDimension.Amount); }
      }

      public double ElectricCurrentExponent
      {
         set { _exponents[(int) BaseDimension.ElectricCurrent] = value; }
         get { return Exponent(BaseDimension.ElectricCurrent); }
      }

      public double TemperatureExponent
      {
         set { _exponents[(int) BaseDimension.Temperature] = value; }
         get { return Exponent(BaseDimension.Temperature); }
      }

      public double TimeExponent
      {
         set { _exponents[(int) BaseDimension.Time] = value; }
         get { return Exponent(BaseDimension.Time); }
      }

      public double LengthExponent
      {
         set { _exponents[(int) BaseDimension.Length] = value; }
         get { return Exponent(BaseDimension.Length); }
      }

      public double Exponent(BaseDimension baseDimension)
      {
         return _exponents[(int) baseDimension];
      }

      public override bool Equals(object obj)
      {
         return Equals(obj as BaseDimensionRepresentation);
      }

      protected bool Equals(BaseDimensionRepresentation other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;

         return EnumHelper.AllValuesFor<BaseDimension>().All(x => Exponent(x) == other.Exponent(x));
      }

      public override int GetHashCode()
      {
         int hc = _exponents.Length;
         for (int i = 0; i < _exponents.Length; ++i)
         {
            hc = unchecked(hc * 17 + _exponents[i].GetHashCode());
         }
         return hc;
      }

      public BaseDimensionRepresentation MultipliedBy(BaseDimensionRepresentation other)
      {
         var exponents = new double[BASE_DIMENSIONS];
         foreach (var baseDimension in Enum.GetValues(typeof (BaseDimension)))
         {
            var i = (int) baseDimension;
            exponents[i] = _exponents[i] + other._exponents[i];
         }
         return new BaseDimensionRepresentation(exponents);
      }

      public BaseDimensionRepresentation DividedBy(BaseDimensionRepresentation other)
      {
         var exponents = new double[BASE_DIMENSIONS];
         foreach (var baseDimension in Enum.GetValues(typeof (BaseDimension)))
         {
            var i = (int) baseDimension;
            exponents[i] = _exponents[i] - other._exponents[i];
         }
         return new BaseDimensionRepresentation(exponents);
      }
   }
}
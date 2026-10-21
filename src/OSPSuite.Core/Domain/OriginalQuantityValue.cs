using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    The value a quantity of a simulation had before it was changed in the simulation.
   ///    Used to track the changes made to a simulation since it was created from its building blocks.
   /// </summary>
   public class OriginalQuantityValue : IWithValueOrigin, IWithDisplayUnit
   {
      public enum Types
      {
         Quantity,
         ScaleDivisor
      }

      public string Path { get; set; }
      public double Value { get; set; }
      public ValueOrigin ValueOrigin { get; } = new ValueOrigin();
      public Unit DisplayUnit { get; set; }
      public IDimension Dimension { get; set; }
      public Types Type { get; set; }
      public string Id => new ObjectPath(Path.ToPathArray().Append(Type.ToString()));
      public bool IsQuantityChange => Type == Types.Quantity;
      public bool IsScaleChange => Type == Types.ScaleDivisor;

      public OriginalQuantityValue WithPropertiesFrom(OriginalQuantityValue sourceOriginalQuantityValue)
      {
         Path = sourceOriginalQuantityValue.Path;
         Value = sourceOriginalQuantityValue.Value;
         ValueOrigin.UpdateFrom(sourceOriginalQuantityValue.ValueOrigin);
         DisplayUnit = sourceOriginalQuantityValue.DisplayUnit;
         Dimension = sourceOriginalQuantityValue.Dimension;
         Type = sourceOriginalQuantityValue.Type;
         return this;
      }

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         ValueOrigin.UpdateFrom(sourceValueOrigin);
      }
   }
}

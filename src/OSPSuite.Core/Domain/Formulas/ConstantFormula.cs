using System.Collections.Generic;
using System.Globalization;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Formulas
{
   /// <summary>
   ///   representing a constant value independent from any other Object in the Model
   /// </summary>
   public class ConstantFormula : Formula, IWithValue
   {
      /// <summary>
      ///   Initializes a new instance of the <see cref="ConstantFormula" /> class.
      /// </summary>
      /// <param name="value"> The value. </param>
      public ConstantFormula(double value)
      {
         Value = value;
      }

      public ConstantFormula()
      {
      }

      protected override double CalculateFor(IEnumerable<IObjectReference> usedObjects, IUsingFormula dependentObject)
      {
         return Value;
      }

      private double _value;

      public double Value
      {
         get { return _value; }
         set
         {
            _value = value;
            OnChanged();
         }
      }

      public override string ToString()
      {
         return Value.ToString(NumberFormatInfo.InvariantInfo);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcFormula = source.DowncastTo<ConstantFormula>();
         if (srcFormula == null)
            return;

         Value = srcFormula.Value;
      }
   }
}
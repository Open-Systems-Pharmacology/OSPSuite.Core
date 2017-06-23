using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core
{
   public class AxisXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      
      [Observation]
      public void TestSerialization()
      {
         var dimensionFactory = IoC.Resolve<IDimensionFactory>();
         var x1 = new Axis(AxisTypes.Y) {Dimension = DimensionMolarConcentration};
         //Ensure that we use the actural merged dimension for this axis
         x1.Dimension = dimensionFactory.MergedDimensionFor(x1);
         x1.UnitName = DimensionMassConcentration.Unit("mg/l").Name;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualAxis(x2, x1);
      }
   }
}
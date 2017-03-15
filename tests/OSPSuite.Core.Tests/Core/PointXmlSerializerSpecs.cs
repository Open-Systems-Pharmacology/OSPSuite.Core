using System.Drawing;
using OSPSuite.BDDHelper.Extensions;
using NUnit.Framework;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class PointXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestForPointsSerializer()
      {
         var p = new Point {X = 5, Y = 9};
         var p2 = SerializeAndDeserialize(p);

         p2.X.ShouldBeEqualTo(p.X);
         p2.Y.ShouldBeEqualTo(p.Y);
      }
   }
}
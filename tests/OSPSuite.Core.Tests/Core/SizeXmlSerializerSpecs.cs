using System.Drawing;
using OSPSuite.BDDHelper.Extensions;
using NUnit.Framework;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class SizeXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestForSizeSerializer()
      {
         var size = new Size {Height = 100, Width = 200};
         var size2 = SerializeAndDeserialize(size);

         size2.Height.ShouldBeEqualTo(size.Height);
         size2.Width.ShouldBeEqualTo(size.Width);
      }
   }
}
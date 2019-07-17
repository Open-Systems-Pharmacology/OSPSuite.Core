using System.Drawing;
using NUnit.Framework;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Serializers
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
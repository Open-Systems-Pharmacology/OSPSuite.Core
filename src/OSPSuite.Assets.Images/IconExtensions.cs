using System.Drawing;
using System.IO;

namespace OSPSuite.Assets
{
    public static class IconExtensions
    {
        public static MemoryStream ToMemoryStream(this Icon icon)
        {
            var icoStream = new MemoryStream();
            icon.Save(icoStream);
            icoStream.Position = 0;
            return icoStream;
        }
    }
}
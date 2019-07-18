using System;
using System.IO;

namespace OSPSuite.Assets
{
    public class IconHeader
    {
        public Int16 Reserved;
        public Int16 Type;
        public Int16 Count;

        public IconHeader(MemoryStream iconStream)
        {
            var icoFile = new BinaryReader(iconStream);
            Reserved = icoFile.ReadInt16();
            Type = icoFile.ReadInt16();
            Count = icoFile.ReadInt16();
        }
    }
}
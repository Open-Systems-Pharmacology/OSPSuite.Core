using System;
using System.IO;

namespace OSPSuite.Assets
{
    public class IconEntry
    {
        public byte Width { get; set; }
        public byte Height { get; set; }
        public byte ColorCount { get; set; }
        public byte Reserved { get; set; }
        public Int16 Planes { get; set; }
        public Int16 BitCount { get; set; }
        public Int32 BytesInRes { get; set; }
        public Int32 ImageOffset { get; set; }

        public IconEntry(MemoryStream icoStream)
        {
            var icoFile = new BinaryReader(icoStream);
            Width = icoFile.ReadByte();
            Height = icoFile.ReadByte();
            ColorCount = icoFile.ReadByte();
            Reserved = icoFile.ReadByte();
            Planes = icoFile.ReadInt16();
            BitCount = icoFile.ReadInt16();
            BytesInRes = icoFile.ReadInt32();
            ImageOffset = icoFile.ReadInt32();
        }

        public bool FitIn(IconSize iconSize)
        {
            return Width >= iconSize.Width && Height >= iconSize.Height;
        }
    }
}
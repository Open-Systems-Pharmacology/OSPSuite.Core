using System.Drawing;

namespace OSPSuite.Assets
{
    public class IconSize
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        internal IconSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public string Id
        {
            get { return string.Format("{0}x{1}", Width, Height); }
        }

        public static implicit operator Size(IconSize icon)
        {
            return new Size(icon.Width, icon.Height);
        }

        public override string ToString()
        {
           return Id;
        }
    }
}
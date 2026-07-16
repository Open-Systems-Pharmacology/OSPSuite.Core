namespace OSPSuite.Assets
{
   public class IconSize
   {
      public int Width { get; }
      public int Height { get; }

      internal IconSize(int width, int height)
      {
         Width = width;
         Height = height;
      }

      public string Id => $"{Width}x{Height}";

      public override string ToString()
      {
         return Id;
      }
   }
}
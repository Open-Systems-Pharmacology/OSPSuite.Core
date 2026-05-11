namespace OSPSuite.Assets
{
   public class ApplicationIcon
   {
      public byte[] IconBytes { get; }
      public string IconName { get; set; }
      public int Index { get; set; }

      public ApplicationIcon(byte[] bytes)
      {
         IconBytes = bytes;
         Index = -1;
      }
   }
}

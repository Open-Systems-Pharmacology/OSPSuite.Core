namespace OSPSuite.Core.Qualification
{
   public class InputMapping: IWithSectionReference
   {
      public string Path { get; set; }
      public int? SectionId { get; set; }
      public string SectionReference { get; set; }
   }
}
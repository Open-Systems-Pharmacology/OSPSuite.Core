namespace OSPSuite.Core.Qualification
{
   public class InputMapping : IWithSectionReference
   {
      public int? SectionId { get; set; }

      public string SectionReference { get; set; }
      
      public string Path { get; set; }
   }
}
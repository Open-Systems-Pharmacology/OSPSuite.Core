namespace OSPSuite.Core.Qualification
{
   public interface IWithSectionReference
   {
      int? SectionId { get; set; }
      string SectionReference { get; set; }
   }
}
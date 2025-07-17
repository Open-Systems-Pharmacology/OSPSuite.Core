using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Reporting
{
   public class ReportTemplate : IWithDescription
   {
      public string DisplayName { get; set; }
      public string Description { get; set; }
      public string Path { get; set; }

      public override string ToString()
      {
         return DisplayName;
      }
   }

   public static class ReportPartExtensions
   {
      public static T WithTitle<T>(this T reportPart, string title) where T : ReportPart
      {
         reportPart.Title = title;
         return reportPart;
      }
   }
}
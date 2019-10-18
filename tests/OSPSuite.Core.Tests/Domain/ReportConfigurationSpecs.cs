using NUnit.Framework;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Reporting;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ReportConfiguration : ContextSpecification<ReportConfiguration>
   {
      protected override void Context()
      {
         sut = new ReportConfiguration();
      }
   }

   public class When_passing_an_invalid_path_to_the_report_path : concern_for_ReportConfiguration
   {
      [Observation]
      [TestCase("C:\\toto.txt")]
      [TestCase("C:\\temp\\toto.txt")]
      public void should_be_valid(string path)
      {
         sut.Validate(x => x.ReportFile, path).Message.Contains(Validation.OutputFileNotValid).ShouldBeFalse();
      }
   }
}
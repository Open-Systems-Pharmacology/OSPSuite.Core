using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;

namespace OSPSuite.Core
{
   public abstract class concern_for_FileExtensionToIconMapper : ContextSpecification<IFileExtensionToIconMapper>
   {
      protected override void Context()
      {
         sut = new FileExtensionToIconMapper();
      }
   }

   public class When_mapping_file_extensions_to_icons : concern_for_FileExtensionToIconMapper
   {
      [Observation]
      public void should_return_the_expected_icons_for_known_extensions()
      {
         sut.MapFrom(Constants.Filter.XLSX_EXTENSION).ShouldBeEqualTo(ApplicationIcons.Excel);
         sut.MapFrom(Constants.Filter.XLS_EXTENSION).ShouldBeEqualTo(ApplicationIcons.Excel);
         sut.MapFrom(Constants.Filter.DOCX_EXTENSION).ShouldBeEqualTo(ApplicationIcons.JournalExportToWord);
         sut.MapFrom(Constants.Filter.DOC_EXTENSION).ShouldBeEqualTo(ApplicationIcons.JournalExportToWord);
         sut.MapFrom(Constants.Filter.PDF_EXTENSION).ShouldBeEqualTo(ApplicationIcons.PDF);
         sut.MapFrom(Constants.Filter.MATLAB_EXTENSION).ShouldBeEqualTo(ApplicationIcons.Matlab);
         sut.MapFrom(Constants.Filter.FIG_EXTENSION).ShouldBeEqualTo(ApplicationIcons.Matlab);
         sut.MapFrom(Constants.Filter.MAT_EXTENSION).ShouldBeEqualTo(ApplicationIcons.Matlab);
         sut.MapFrom(Constants.Filter.R_EXTENSION).ShouldBeEqualTo(ApplicationIcons.R);
         sut.MapFrom(Constants.Filter.RD_EXTENSION).ShouldBeEqualTo(ApplicationIcons.R);
         sut.MapFrom(Constants.Filter.PKML_EXTENSION).ShouldBeEqualTo(ApplicationIcons.PKML);
         sut.MapFrom(Constants.Filter.CSV_EXTENSION).ShouldBeEqualTo(ApplicationIcons.ResultsImportFromCSV);
         sut.MapFrom(Constants.Filter.TEXT_EXTENSION).ShouldBeEqualTo(ApplicationIcons.Report);
      }

      [Observation]
      public void should_return_file_icon_for_unknown_and_undefined_extension()
      {
         sut.MapFrom(null).ShouldBeEqualTo(ApplicationIcons.File);
         sut.MapFrom("*.unknown").ShouldBeEqualTo(ApplicationIcons.File);
      }
   }
}	
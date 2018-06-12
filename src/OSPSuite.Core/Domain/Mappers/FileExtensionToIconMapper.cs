using OSPSuite.Assets;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IFileExtensionToIconMapper : IMapper<string, ApplicationIcon>
   {
   }

   public class FileExtensionToIconMapper : IFileExtensionToIconMapper
   {
      public ApplicationIcon MapFrom(string extension)
      {
         //tries to match knowns extensions with some of our predefined icons

         if (string.IsNullOrEmpty(extension))
            return ApplicationIcons.File;

         extension = extension.ToLowerInvariant();

         if (extension.IsOneOf(Constants.Filter.XLSX_EXTENSION, Constants.Filter.XLS_EXTENSION))
            return ApplicationIcons.Excel;

         if (extension.IsOneOf(Constants.Filter.DOCX_EXTENSION, Constants.Filter.DOC_EXTENSION))
            return ApplicationIcons.JournalExportToWord;

         if (extension.IsOneOf(Constants.Filter.PDF_EXTENSION))
            return ApplicationIcons.PDF;

         if (extension.IsOneOf(Constants.Filter.MATLAB_EXTENSION, Constants.Filter.FIG_EXTENSION, Constants.Filter.MAT_EXTENSION))
            return ApplicationIcons.Matlab;

         if (extension.IsOneOf(Constants.Filter.R_EXTENSION, Constants.Filter.RD_EXTENSION))
            return ApplicationIcons.R;

         if (extension.IsOneOf(Constants.Filter.PKML_EXTENSION))
            return ApplicationIcons.PKML;

         if (extension.IsOneOf(Constants.Filter.CSV_EXTENSION))
            return ApplicationIcons.ResultsImportFromCSV;

         if (extension.IsOneOf(Constants.Filter.TEXT_EXTENSION))
            return ApplicationIcons.Report;

         return ApplicationIcons.File;
      }
   }
}
using OSPSuite.Assets;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IFileExtensionToIconMapper : IMapper<string, string>
   {
   }

   public class FileExtensionToIconMapper : IFileExtensionToIconMapper
   {
      public string MapFrom(string extension)
      {
         //tries to match known extensions with some of our predefined icons

         if (string.IsNullOrEmpty(extension))
            return IconNames.FILE;

         extension = extension.ToLowerInvariant();

         if (extension.IsOneOf(Constants.Filter.XLSX_EXTENSION, Constants.Filter.XLS_EXTENSION))
            return IconNames.EXCEL;

         if (extension.IsOneOf(Constants.Filter.DOCX_EXTENSION, Constants.Filter.DOC_EXTENSION))
            return IconNames.JOURNAL_EXPORT_TO_WORD;

         if (extension.IsOneOf(Constants.Filter.PDF_EXTENSION))
            return IconNames.PDF;

         if (extension.IsOneOf(Constants.Filter.MATLAB_EXTENSION, Constants.Filter.FIG_EXTENSION, Constants.Filter.MAT_EXTENSION))
            return IconNames.MATLAB;

         if (extension.IsOneOf(Constants.Filter.R_EXTENSION, Constants.Filter.RD_EXTENSION))
            return IconNames.R;

         if (extension.IsOneOf(Constants.Filter.PKML_EXTENSION))
            return IconNames.PKML;

         if (extension.IsOneOf(Constants.Filter.CSV_EXTENSION))
            return IconNames.RESULTS_IMPORT_FROM_CSV;

         if (extension.IsOneOf(Constants.Filter.TEXT_EXTENSION))
            return IconNames.REPORT;

         return IconNames.FILE;
      }
   }
}
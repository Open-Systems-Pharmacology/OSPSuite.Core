using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public class When_validating_the_constants_defined_in_core : StaticContextSpecification
   {
      [Observation]
      public void the_save_excel_filter_should_save_to_xls_first()
      {
         var posXlsx = Constants.Filter.EXCEL_SAVE_FILE_FILTER.IndexOf($"{Constants.Filter.XLSX_EXTENSION})", StringComparison.Ordinal);
         var posXls = Constants.Filter.EXCEL_SAVE_FILE_FILTER.IndexOf($"{Constants.Filter.XLS_EXTENSION})", StringComparison.Ordinal);
         posXlsx.ShouldBeSmallerThan(posXls);
      }
   }
}
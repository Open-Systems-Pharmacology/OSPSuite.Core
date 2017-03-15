using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core.Serialization.SimModel.Services
{
   public interface ITableFormulaToTableFormulaExportMapper : IMapper<TableFormula, TableFormulaExport>
   {
   }

   class TableFormulaToTableFormulaExportMapper : ITableFormulaToTableFormulaExportMapper
   {
      public TableFormulaExport MapFrom(TableFormula input)
      {
         var export = new TableFormulaExport();
         input.AllPoints().Each(export.AddPoint);

         export.UseDerivedValues = input.UseDerivedValues;

         return export;
      }
   }
}
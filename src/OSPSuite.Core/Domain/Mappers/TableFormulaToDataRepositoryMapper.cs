using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface ITableFormulaToDataRepositoryMapper : IMapper<TableFormula, DataRepository>
   {
   }

   public class TableFormulaToDataRepositoryMapper : ITableFormulaToDataRepositoryMapper
   {
      private readonly IIdGenerator _idGenerator;

      public TableFormulaToDataRepositoryMapper(IIdGenerator idGenerator)
      {
         _idGenerator = idGenerator;
      }

      public DataRepository MapFrom(TableFormula tableFormula)
      {
         var dataRepository = new DataRepository(_idGenerator.NewId()).WithName(tableFormula.Name);
         var baseGrid = new BaseGrid(_idGenerator.NewId(), tableFormula.XDimension.Name, tableFormula.XDimension) {DataInfo = {DisplayUnitName = tableFormula.XDisplayUnit.Name}};
         var value = new DataColumn(_idGenerator.NewId(), tableFormula.Name, tableFormula.Dimension, baseGrid) {DataInfo = {DisplayUnitName = tableFormula.YDisplayUnit.Name}};

         baseGrid.Values = tableFormula.AllPoints().Select(x => x.X).ToFloatArray();
         value.Values = tableFormula.AllPoints().Select(x => x.Y).ToFloatArray();

         dataRepository.Add(value);
         return dataRepository;
      }
   }
}

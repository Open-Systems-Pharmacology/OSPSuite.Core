using System.Data;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Presentation.Mappers.ParameterIdentifications
{
   public interface IWeightedDataRepositoryToDataTableMapper : IMapper<WeightedObservedData, DataTable>
   {
   }

   public class WeightedDataRepositoryToDataTableMapper : IWeightedDataRepositoryToDataTableMapper
   {
      private readonly IDataRepositoryExportTask _dataRepositoryTask;
      private readonly IDimensionFactory _dimensionFactory;

      public WeightedDataRepositoryToDataTableMapper(IDataRepositoryExportTask dataRepositoryTask, IDimensionFactory dimensionFactory)
      {
         _dataRepositoryTask = dataRepositoryTask;
         _dimensionFactory = dimensionFactory;
      }

      public DataTable MapFrom(WeightedObservedData weightedObservedData)
      {
         var columns = weightedObservedData.ObservedData.ToList();
         var dataColumn = createWeightColumn(weightedObservedData.ObservedData.BaseGrid, weightedObservedData);

         columns.Add(dataColumn);
         return _dataRepositoryTask.ToDataTable(columns, new DataColumnExportOptions {ForceColumnTypeAsObject = true}).First();
      }

      private DataColumn createWeightColumn(BaseGrid baseGrid, WeightedObservedData weightedObservedData)
      {
         return new DataColumn(Captions.ParameterIdentification.Weight, _dimensionFactory.NoDimension, baseGrid) {Values = weightedObservedData.Weights};
      }
   }
}
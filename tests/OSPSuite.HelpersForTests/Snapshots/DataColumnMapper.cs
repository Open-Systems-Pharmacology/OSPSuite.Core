using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;

namespace OSPSuite.Helpers.Snapshots
{
   public class DataColumnMapper : DataColumnMapper<TestProject>
   {
      private readonly IDimensionFactory _dimensionFactory;

      public DataColumnMapper(DataInfoMapper<TestProject> dataInfoMapper, QuantityInfoMapper<TestProject> quantityInfoMapper, IDimensionFactory dimensionFactory) : base(dataInfoMapper, quantityInfoMapper)
      {
         _dimensionFactory = dimensionFactory;
      }

      protected override IDimension DimensionFrom(DataColumn snapshot)
      {
         return _dimensionFactory.Dimension(snapshot.Dimension);
      }
   }
}
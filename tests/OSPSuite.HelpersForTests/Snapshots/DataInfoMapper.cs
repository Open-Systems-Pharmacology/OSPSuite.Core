using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Snapshots.Mappers;

namespace OSPSuite.Helpers.Snapshots
{
   public class DataInfoMapper : DataInfoMapper<TestProject>
   {
      public DataInfoMapper(ExtendedPropertyMapper<TestProject> extendedPropertyMapper, IDimension molWeightDimension) : base(extendedPropertyMapper, molWeightDimension)
      {
      }
   }
}
using System.Threading.Tasks;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Helpers;
using ModelDataInfo = OSPSuite.Core.Domain.Data.DataInfo;
using SnapshotDataInfo = OSPSuite.Core.Snapshots.DataInfo;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_DataInfoMapper : ContextSpecificationAsync<DataInfoMapper>
   {
      protected ExtendedPropertyMapper _extendedPropertyMapper;
      protected ExtendedProperty _extendedPropertySnapshot;
      protected ModelDataInfo _dataInfo;
      protected IExtendedProperty _extendedProperty;

      protected override Task Context()
      {
         var molWeightDimension = A.Fake<IDimension>();
         _extendedPropertyMapper = A.Fake<ExtendedPropertyMapper>();
         var dimensionFactory = A.Fake<IDimensionFactory>();

         A.CallTo(() => dimensionFactory.Dimension(Constants.Parameters.MOL_WEIGHT)).Returns(molWeightDimension);

         sut = new DataInfoMapper(_extendedPropertyMapper, dimensionFactory);

         _extendedPropertySnapshot = new ExtendedProperty();

         _dataInfo = new ModelDataInfo(ColumnOrigins.Observation, AuxiliaryType.GeometricStdDev, "unitName", "category", 2.3) { LLOQ = 0.4f };
         _extendedProperty = new ExtendedProperty<string> { Name = "Hello" };
         _dataInfo.ExtendedProperties.Add(_extendedProperty);
         A.CallTo(() => molWeightDimension.BaseUnitValueToUnitValue(molWeightDimension.DefaultUnit, _dataInfo.MolWeight.Value)).Returns(5.0);
         A.CallTo(() => molWeightDimension.UnitValueToBaseUnitValue(molWeightDimension.DefaultUnit, 5.0)).Returns(_dataInfo.MolWeight.Value);

         A.CallTo(() => _extendedPropertyMapper.MapToSnapshot(_extendedProperty)).Returns(_extendedPropertySnapshot);
         A.CallTo(() => _extendedPropertyMapper.MapToModel(_extendedPropertySnapshot, A<SnapshotContext>._)).Returns(_extendedProperty);

         return Task.FromResult(true);
      }
   }

   public class When_mapping_snapshot_to_data_info : concern_for_DataInfoMapper
   {
      private SnapshotDataInfo _snapshot;
      private ModelDataInfo _result;

      protected override async Task Context()
      {
         await base.Context();
         _snapshot = await sut.MapToSnapshot(_dataInfo);
      }

      protected override async Task Because()
      {
         _result = await sut.MapToModel(_snapshot, new SnapshotContext(new TestProject(), SnapshotVersions.Current));
      }

      [Observation]
      public void the_extended_properties_should_match()
      {
         _result.ExtendedProperties.ShouldOnlyContain(_extendedProperty);
      }

      [Observation]
      public void the_data_info_should_have_properties_as_in_original()
      {
         _result.AuxiliaryType.ShouldBeEqualTo(_dataInfo.AuxiliaryType);
         _result.Category.ShouldBeEqualTo(_dataInfo.Category);
         _result.ComparisonThreshold.ShouldBeEqualTo(_dataInfo.ComparisonThreshold);
         _result.LLOQ.ShouldBeEqualTo(_dataInfo.LLOQ);
         _result.MolWeight.ShouldBeEqualTo(_dataInfo.MolWeight);
         _result.Origin.ShouldBeEqualTo(_dataInfo.Origin);
      }
   }

   public class When_mapping_data_info_to_snapshot : concern_for_DataInfoMapper
   {
      private SnapshotDataInfo _snapshot;

      protected override async Task Because()
      {
         _snapshot = await sut.MapToSnapshot(_dataInfo);
      }

      [Observation]
      public void the_snapshot_includes_the_extended_properties_snapshot()
      {
         _snapshot.ExtendedProperties.ShouldContain(_extendedPropertySnapshot);
      }

      [Observation]
      public void the_snapshot_properties_are_set_as_expected()
      {
         _snapshot.AuxiliaryType.ShouldBeEqualTo(_dataInfo.AuxiliaryType);
         _snapshot.Category.ShouldBeEqualTo(_dataInfo.Category);
         _snapshot.ComparisonThreshold.ShouldBeEqualTo(_dataInfo.ComparisonThreshold);
         _snapshot.LLOQ.ShouldBeEqualTo(_dataInfo.LLOQ);
         _snapshot.MolWeight.ShouldBeEqualTo(5.0);
         _snapshot.Origin.ShouldBeEqualTo(_dataInfo.Origin);
      }
   }
}
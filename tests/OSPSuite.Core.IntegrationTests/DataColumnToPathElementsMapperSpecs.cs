using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_DataColumnToPathElementsMapper : ContextForIntegration<DataColumnToPathElementsMapper>
   {
      protected override void Context()
      {
         base.Context();
         sut = new DataColumnToPathElementsMapper(new PathToPathElementsMapper(new EntityPathResolverForSpecs()));
      }
   }

   public class When_resolving_name_for_a_basegrid_column : concern_for_DataColumnToPathElementsMapper
   {
      private DataColumn _dataColumn;
      private PathElements _result;

      protected override void Context()
      {
         base.Context();
         _dataColumn = new BaseGrid("basegrid", new Dimension(new BaseDimensionRepresentation(), "dimensionName", "baseUnitName")) { QuantityInfo = new QuantityInfo(new List<string>(), QuantityType.BaseGrid), DataInfo = new DataInfo(ColumnOrigins.BaseGrid) };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dataColumn, null);
      }

      [Observation]
      public void the_name_mapped_should_be_the_name_of_the_column()
      {
         _result[PathElementId.Name].DisplayName.ShouldBeEqualTo(_dataColumn.Name);
      }
   }

   public class When_resolving_name_for_a_calculation_column : concern_for_DataColumnToPathElementsMapper
   {
      private DataColumn _dataColumn;
      private PathElements _result;
      private IContainer _topContainer;

      protected override void Context()
      {
         base.Context();
         _topContainer = new Container().WithName("topContainerName");
         _topContainer.Add(new Container().WithName("secondContainer"));
         _dataColumn = new DataColumn { QuantityInfo = new QuantityInfo(new List<string> { "topContainerName", "secondContainer" }, QuantityType.Molecule), DataInfo = new DataInfo(ColumnOrigins.Calculation), Name = "dataColumnDisplayName" };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dataColumn, _topContainer);
      }

      [Observation]
      public void the_top_container_mapped_should_be_the_first_element_of_the_quantity_path()
      {
         _result[PathElementId.TopContainer].DisplayName.ShouldBeEqualTo(_dataColumn.QuantityInfo.Path.First());
         _result[PathElementId.Container].DisplayName.ShouldBeEqualTo(_dataColumn.QuantityInfo.Path.ElementAt(1));
         _result[PathElementId.Name].DisplayName.ShouldBeEqualTo(_dataColumn.QuantityInfo.Path.ElementAt(1));
      }
   }

   public class When_resolving_name_for_a_observation_column : concern_for_DataColumnToPathElementsMapper
   {
      private DataColumn _dataColumn;
      private PathElements _result;

      protected override void Context()
      {
         base.Context();
         _dataColumn = new DataColumn { QuantityInfo = new QuantityInfo(new List<string> {"observationColumnName"}, QuantityType.Molecule), DataInfo = new DataInfo(ColumnOrigins.Observation), Name = "dataColumnDisplayName" };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dataColumn, null);
      }

      [Observation]
      public void the_top_container_mapped_should_be_the_first_element_of_the_quantity_path()
      {
         _result[PathElementId.TopContainer].DisplayName.ShouldBeEqualTo(_dataColumn.QuantityInfo.Path.First());
      }
   }

   public class When_resolving_name_for_a_deviation_column : concern_for_DataColumnToPathElementsMapper
   {
      private DataColumn _dataColumn;
      private PathElements _result;

      protected override void Context()
      {
         base.Context();
         _dataColumn = new DataColumn { QuantityInfo = new QuantityInfo(new List<string>(), QuantityType.Molecule), DataInfo = new DataInfo(ColumnOrigins.DeviationLine), Name = "dataColumnDisplayName" };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dataColumn, null);
      }

      [Observation]
      public void the_name_mapped_should_be_the_name_of_the_column()
      {
         _result[PathElementId.Name].DisplayName.ShouldBeEqualTo(_dataColumn.Name);
      }
   }
}

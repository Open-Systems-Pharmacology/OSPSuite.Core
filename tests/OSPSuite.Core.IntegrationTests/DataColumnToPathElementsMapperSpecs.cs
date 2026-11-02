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

   public class When_resolving_name_for_an_observation_column_with_the_normalized_observed_data_path : concern_for_DataColumnToPathElementsMapper
   {
      private DataColumn _dataColumn;
      private PathElements _result;

      protected override void Context()
      {
         base.Context();
         var path = new List<string> {"Obs Data", Constants.ObservedData.CONTAINER_NAME, "VenousBlood", "Plasma", "Drug", "Concentration"};
         _dataColumn = new DataColumn {QuantityInfo = new QuantityInfo(path, QuantityType.Undefined), DataInfo = new DataInfo(ColumnOrigins.Observation), Name = "dataColumnDisplayName"};
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dataColumn, null);
      }

      [Observation]
      public void should_map_the_repository_name_to_simulation_and_top_container()
      {
         _result[PathElementId.Simulation].DisplayName.ShouldBeEqualTo("Obs Data");
         _result[PathElementId.TopContainer].DisplayName.ShouldBeEqualTo("Obs Data");
      }

      [Observation]
      public void should_map_the_organ_to_container()
      {
         _result[PathElementId.Container].DisplayName.ShouldBeEqualTo("VenousBlood");
      }

      [Observation]
      public void should_map_the_compartment_to_bottom_compartment()
      {
         _result[PathElementId.BottomCompartment].DisplayName.ShouldBeEqualTo("Plasma");
      }

      [Observation]
      public void should_map_the_molecule_to_molecule()
      {
         _result[PathElementId.Molecule].DisplayName.ShouldBeEqualTo("Drug");
      }

      [Observation]
      public void should_map_the_column_name_to_name()
      {
         _result[PathElementId.Name].DisplayName.ShouldBeEqualTo("Concentration");
      }
   }

   public class When_resolving_name_for_an_observation_column_whose_path_does_not_have_the_normalized_length : concern_for_DataColumnToPathElementsMapper
   {
      private DataColumn _dataColumn;
      private PathElements _result;

      protected override void Context()
      {
         base.Context();
         var path = new List<string> {"Obs Data", "VenousBlood", "Plasma", "Drug", "Concentration"};
         _dataColumn = new DataColumn {QuantityInfo = new QuantityInfo(path, QuantityType.Undefined), DataInfo = new DataInfo(ColumnOrigins.Observation), Name = "dataColumnDisplayName"};
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dataColumn, null);
      }

      [Observation]
      public void should_fall_back_to_the_first_element_as_top_container_and_the_rest_as_container()
      {
         _result[PathElementId.TopContainer].DisplayName.ShouldBeEqualTo("Obs Data");
         _result[PathElementId.Container].DisplayName.ShouldBeEqualTo("VenousBlood-Plasma-Drug-Concentration");
      }

      [Observation]
      public void should_not_map_a_molecule()
      {
         _result[PathElementId.Molecule].DisplayName.ShouldBeEmpty();
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

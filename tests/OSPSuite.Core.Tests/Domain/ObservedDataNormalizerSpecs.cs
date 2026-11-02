using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ObservedDataNormalizer : ContextSpecification<IObservedDataNormalizer>
   {
      protected DataRepository _observedData;
      protected DataColumn _column;

      protected override void Context()
      {
         sut = new ObservedDataNormalizer();
         _observedData = DomainHelperForSpecs.ObservedData("OBS_DATA", obsDataColumnName: "Concentration");
         _column = _observedData.AllButBaseGrid().Single();
      }

      protected void AddMetaData(string name, string value)
      {
         _observedData.ExtendedProperties.Add(new ExtendedProperty<string> {Name = name, Value = value});
      }

      protected override void Because()
      {
         sut.UpdateQuantityInfo(_observedData);
      }
   }

   public class When_normalizing_observed_data_with_organ_compartment_and_molecule_metadata : concern_for_ObservedDataNormalizer
   {
      protected override void Context()
      {
         base.Context();
         AddMetaData(Constants.ObservedData.ORGAN, "VenousBlood");
         AddMetaData(Constants.ObservedData.COMPARTMENT, "Plasma");
         AddMetaData(Constants.ObservedData.MOLECULE, "Drug");
      }

      [Observation]
      public void should_set_the_path_of_the_observation_columns_to_repository_container_organ_compartment_molecule_and_column_name()
      {
         _column.QuantityInfo.Path.ShouldOnlyContainInOrder("OBS_DATA", Constants.ObservedData.CONTAINER_NAME, "VenousBlood", "Plasma", "Drug", "Concentration");
      }

      [Observation]
      public void should_set_the_quantity_type_of_the_observation_columns_to_undefined()
      {
         _column.QuantityInfo.Type.ShouldBeEqualTo(QuantityType.Undefined);
      }

      [Observation]
      public void should_set_the_path_of_the_base_grid_to_repository_and_base_grid_name()
      {
         _observedData.BaseGrid.QuantityInfo.Path.ShouldOnlyContainInOrder("OBS_DATA", _observedData.BaseGrid.Name);
      }

      [Observation]
      public void should_set_the_quantity_type_of_the_base_grid_to_time()
      {
         _observedData.BaseGrid.QuantityInfo.Type.ShouldBeEqualTo(QuantityType.Time);
      }
   }

   public class When_normalizing_observed_data_without_organ_compartment_and_molecule_metadata : concern_for_ObservedDataNormalizer
   {
      [Observation]
      public void should_use_empty_path_elements_for_the_missing_metadata()
      {
         _column.QuantityInfo.Path.ShouldOnlyContainInOrder("OBS_DATA", Constants.ObservedData.CONTAINER_NAME, string.Empty, string.Empty, string.Empty, "Concentration");
      }

      [Observation]
      public void should_not_add_the_missing_metadata_to_the_observed_data()
      {
         _observedData.ExtendedProperties.Contains(Constants.ObservedData.ORGAN).ShouldBeFalse();
         _observedData.ExtendedProperties.Contains(Constants.ObservedData.COMPARTMENT).ShouldBeFalse();
         _observedData.ExtendedProperties.Contains(Constants.ObservedData.MOLECULE).ShouldBeFalse();
      }
   }

   public class When_normalizing_observed_data_whose_column_names_contain_the_path_delimiter : concern_for_ObservedDataNormalizer
   {
      protected override void Context()
      {
         base.Context();
         _column.Name = $"Organism{ObjectPath.PATH_DELIMITER}Concentration";
         _observedData.BaseGrid.Name = $"Organism{ObjectPath.PATH_DELIMITER}Time";
      }

      [Observation]
      public void should_escape_the_delimiter_in_the_column_name_so_that_it_does_not_add_a_path_level()
      {
         _column.QuantityInfo.Path.Count().ShouldBeEqualTo(6);
         _column.QuantityInfo.Path.Last().ShouldBeEqualTo("Organism\\Concentration");
      }

      [Observation]
      public void should_escape_the_delimiter_in_the_base_grid_name()
      {
         _observedData.BaseGrid.QuantityInfo.Path.Count().ShouldBeEqualTo(2);
         _observedData.BaseGrid.QuantityInfo.Path.Last().ShouldBeEqualTo("Organism\\Time");
      }
   }
}

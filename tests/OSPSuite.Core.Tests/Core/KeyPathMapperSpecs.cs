using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_KeyPathMapper : ContextSpecification<IKeyPathMapper>
   {
      protected DataColumn _column;
      protected DataInfo _dataInfo;
      protected QuantityInfo _quantityInfo;
      protected string _key;
      private IEntityPathResolver _entityPathResolver;
      private IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         _entityPathResolver = A.Fake<IEntityPathResolver>();
         _objectPathFactory = new ObjectPathFactory(new AliasCreator());
         sut = new KeyPathMapper(_entityPathResolver, _objectPathFactory);
         _column = new DataColumn();
         _dataInfo = new DataInfo(ColumnOrigins.Calculation);
         _quantityInfo = new QuantityInfo("Name", new List<string>(), QuantityType.Undefined);
         _column.DataInfo = _dataInfo;
         _column.QuantityInfo = _quantityInfo;
      }

      protected override void Because()
      {
         _key = sut.MapFrom(_column);
      }
   }

   public class When_mapping_a_data_column_representing_a_base_grid : concern_for_KeyPathMapper
   {
      protected override void Context()
      {
         base.Context();
         _dataInfo.Origin = ColumnOrigins.BaseGrid;
         _quantityInfo.Path = new List<string> {"Time"};
      }

      [Observation]
      public void should_return_the_base_grid_quantity_path()
      {
         _key.ShouldBeEqualTo("Time");
      }
   }

   public class When_mapping_a_data_column_representing_an_unknow_time : concern_for_KeyPathMapper
   {
      protected override void Context()
      {
         base.Context();
         _dataInfo.Origin = ColumnOrigins.Undefined;
         _column.Name = "AA";
         _column.DataInfo.Source = "BB";
      }

      [Observation]
      public void should_return_a_path_containg_the_column_name_and_source()
      {
         _key.Contains("AA").ShouldBeTrue();
         _key.Contains("BB").ShouldBeTrue();
      }
   }

   public class When_mapping_a_column_representing_an_observation_column : concern_for_KeyPathMapper
   {
      protected override void Context()
      {
         base.Context();
         _dataInfo.Origin = ColumnOrigins.Observation;
         _dataInfo.Source = "toto";
         _column.Name = "col";
      }

      [Observation]
      public void should_return_a_key_made_from_the_source_category_and_column_name()
      {
         _key.Contains(_dataInfo.Source).ShouldBeTrue();
         _key.Contains(_column.Name).ShouldBeTrue();
      }
   }

   public class When_mapping_a_column_representing_an_observation_auxiliary_column : concern_for_KeyPathMapper
   {
      protected override void Context()
      {
         base.Context();
         _dataInfo.Origin = ColumnOrigins.ObservationAuxiliary;
         _dataInfo.Source = "toto";
         _column.Name = "col";
      }

      [Observation]
      public void should_return_a_key_made_from_the_source_category_and_column_name()
      {
         _key.Contains(_dataInfo.Source).ShouldBeTrue();
         _key.Contains(_column.Name).ShouldBeTrue();
      }
   }

   public class When_mapping_a_column_representing_a_calculated_quantity_column_for_a_drug_observer : concern_for_KeyPathMapper
   {
      protected override void Context()
      {
         base.Context();
         _dataInfo.Origin = ColumnOrigins.Calculation;
         _quantityInfo.Path = new List<string> {"Sim1", "Organism", "Organ", "Asprin", "Obs"};
         _quantityInfo.Type = QuantityType.Drug | QuantityType.Observer;
      }

      [Observation]
      public void should_return_a_key_were_the_first_entry_corresponding_to_the_simulation_name_was_removed_and_the_one_before_last_entry_corresponding_to_the_compound_name_was_removed()
      {
         _key.ShouldBeEqualTo(new List<string> {"Organism", "Organ", "Obs"}.ToPathString());
      }
   }


   public class When_mapping_a_column_representing_a_calculated_quantity_column_for_a_drug_amount : concern_for_KeyPathMapper
   {
      protected override void Context()
      {
         base.Context();
         _dataInfo.Origin = ColumnOrigins.Calculation;
         _quantityInfo.Path = new List<string> { "Sim1", "Organism", "Organ", "Asprin"};
         _quantityInfo.Type = QuantityType.Drug ;
      }

      [Observation]
      public void should_return_a_key_were_the_first_entry_corresponding_to_the_simulation_name_was_removed_and_the_one_before_last_entry_corresponding_to_the_compound_name_was_removed()
      {
         _key.ShouldBeEqualTo(new List<string> { "Organism", "Organ" }.ToPathString());
      }
   }

   public class When_mapping_a_column_representing_a_calculated_quantity_column_for_a_molecule_observer_that_is_not_a_drug : concern_for_KeyPathMapper
   {
      protected override void Context()
      {
         base.Context();
         _dataInfo.Origin = ColumnOrigins.Calculation;
         _quantityInfo.Path = new List<string> {"Sim1", "Organism", "Organ", "Enzyme", "Obs"};
         _quantityInfo.Type = QuantityType.Enzyme | QuantityType.Observer;
      }

      [Observation]
      public void should_return_a_key_were_the_first_entry_corresponding_to_the_simulation_name_was_removed_and_the_one_before_last_entry_corresponding_to_the_compound_name_was_removed()
      {
         _key.ShouldBeEqualTo(new List<string> {"Organism", "Organ",  "Obs"}.ToPathString());
      }
   }

   public class When_mapping_a_column_representing_a_calculated_auxiliary_quantity_column_for_a_drug_amount : concern_for_KeyPathMapper
   {
      protected override void Context()
      {
         base.Context();
         _dataInfo.Origin = ColumnOrigins.CalculationAuxiliary;
         _quantityInfo.Path = new List<string> { "Sim1", "Organism", "Organ", "Asprin" };
         _quantityInfo.Type = QuantityType.Drug;
      }

      [Observation]
      public void should_return_a_key_were_the_first_entry_corresponding_to_the_simulation_name_was_removed_and_the_one_before_last_entry_corresponding_to_the_compound_name_was_removed()
      {
         _key.ShouldBeEqualTo(new List<string> { "Organism", "Organ" }.ToPathString());
      }
   }
}
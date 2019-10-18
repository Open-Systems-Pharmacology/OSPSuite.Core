using System;
using System.Collections.Generic;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_DataRepository : ContextSpecification<DataRepository>
   {
      protected BaseGrid _baseGrid1, _baseGrid2;
      protected IDimension _length;
      protected IDimension _mass;
      protected IObserver _obsB;
      protected IMoleculeAmount _spA;
      protected IDimension _time;

      protected override void Context()
      {
         base.Context();
         _time = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1}, "Time", "s");
         _time.AddUnit("min", 60, 0);
         _time.AddUnit("h", 3600, 0);

         _length = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Length", "m");
         _length.AddUnit("mm", 0.001, 0.0);
         _mass = new Dimension(new BaseDimensionRepresentation {MassExponent = 1}, "Mass", "kg");
         _mass.AddUnit("mg", 0.000001, 0.0);

         _baseGrid1 = new BaseGrid("BaseGrid1","BaseGrid1", _time);
         _baseGrid1.Values = new[] {0, 1, 2.0F};

         _baseGrid2 = new BaseGrid("BaseGrid2","BaseGrid2", _time);
         _baseGrid1.Values = new[] {0, 1, 2.0F, 3.0F};

         _spA = new MoleculeAmount().WithName("A");
         _obsB = new Observer().WithName("B");
         sut = new DataRepository();
      }
   }

   public class When_testing_for_non_observed_values_below_LLOQ : concern_for_DataRepository
   {
      protected override void Context()
      {
         base.Context();
         sut.Add(_baseGrid1);
         var col1 = new DataColumn("col1", "col1", _length, _baseGrid1)
         {
            Values = new[] { 1f, 11f, 111f, 100f },
            DataInfo = { Origin = ColumnOrigins.Calculation }
         };
         sut.Add(col1);
         col1.DataInfo.LLOQ = 100;
      }

      [Observation]
      public void all_values_should_not_be_indicated()
      {
         sut.HasObservationBelowLLOQ(0).ShouldBeFalse();
         sut.HasObservationBelowLLOQ(1).ShouldBeFalse();
         sut.HasObservationBelowLLOQ(2).ShouldBeFalse();
         sut.HasObservationBelowLLOQ(3).ShouldBeFalse();
      }
   }

   public class When_testing_for_observed_values_below_LLOQ : concern_for_DataRepository
   {
      protected override void Context()
      {
         base.Context();
         sut.Add(_baseGrid1);
         var col1 = new DataColumn("col1", "col1", _length, _baseGrid1)
         {
            Values = new[] {1f, 11f, 111f, 100f},
            DataInfo = {Origin = ColumnOrigins.Observation}
         };
         sut.Add(col1);
         col1.DataInfo.LLOQ = 100;
      }

      [Observation]
      public void values_below_lloq_should_be_indicated()
      {
         sut.HasObservationBelowLLOQ(0).ShouldBeTrue();
         sut.HasObservationBelowLLOQ(1).ShouldBeTrue();
      }

      [Observation]
      public void values_above_lloq_should_not_be_indicated()
      {
         sut.HasObservationBelowLLOQ(2).ShouldBeFalse();
         sut.HasObservationBelowLLOQ(3).ShouldBeFalse();
      }
   }

   public class When_retrieving_base_unit_for_display_unit : concern_for_DataRepository
   {
      private float _result;

      protected override void Context()
      {
         base.Context();
         sut.Add(_baseGrid1);
         _baseGrid1.DisplayUnit = _time.Unit("min");
      }

      protected override void Because()
      {
         _result = sut.ConvertBaseValueForColumn(_baseGrid1.Id, 5f);
      }

      [Observation]
      public void value_should_be_converted()
      {
         _result.ShouldBeEqualTo(5 * 60f);
      }
   }

   public class When_creating_a_data_repository_with_id_an_name : concern_for_DataRepository
   {
      private string _id;
      private string _name;

      protected override void Context()
      {
         _id = "Id";
         _name = "Robert";
      }

      protected override void Because()
      {
         sut = new DataRepository(_id).WithName(_name);
      }

      [Observation]
      public void should_be_able_to_retrieve_the_given_id_an_name()
      {
         Assert.AreEqual(_name, sut.Name);
         Assert.AreEqual(_id, sut.Id);
      }
   }

   public class When_adding_a_column_to_the_repository : concern_for_DataRepository
   {
      private DataColumn _dc1;

      protected override void Context()
      {
         base.Context();
         _dc1 = new DataColumn("Lea", _length, _baseGrid1);
      }

      protected override void Because()
      {
         sut.Add(_dc1);
      }

      [Observation]
      public void should_be_able_to_retrieve_the_column()
      {
         Assert.AreSame(_dc1, sut.GetColumn(_dc1.Id));
         Assert.AreSame(_baseGrid1, sut.GetColumn(_baseGrid1.Id));
         Assert.IsTrue(_dc1.IsInRepository());
         Assert.AreSame(_dc1.Repository, sut);
         Assert.IsTrue(_baseGrid1.IsInRepository());
         Assert.AreSame(_baseGrid1.Repository, sut);
      }
   }

   public class When_adding_two_columns_to_the_repository_with_different_basegrids : concern_for_DataRepository
   {
      private DataColumn _dc1;
      private DataColumn _dc2;

      protected override void Context()
      {
         base.Context();
         _dc1 = new DataColumn("col1", _length, _baseGrid1);
         _dc2 = new DataColumn("col1", _length, _baseGrid2);
      }

      protected override void Because()
      {
         sut.Add(_dc1);
      }

      [Observation]
      public void should_thrown_an_exception()
      {
         The.Action(()=>sut.Add(_dc2)).ShouldThrowAn<Exception>();
      }
   }

   public class When_inserting_values_into_a_data_repositories : concern_for_DataRepository
   {
      private DataColumn _col1;
      private DataColumn _col2;
      private DataColumn _col3;
      private Cache<string, float> _valuesToAdd;

      protected override void Context()
      {
         base.Context();
         _baseGrid1.Values = new List<float> {1f, 2f, 3f};
         _col1 = new DataColumn("col1", "col1", _length, _baseGrid1) {Values = new[] {1f, 11f, 111f}};
         _col2 = new DataColumn("col2", "col2", _length, _baseGrid1) {Values = new[] {2f, 22f, 222f}};
         _col3 = new DataColumn("col3", "col3", _length, _baseGrid1) {Values = new[] {3f, 33f, 333f}};
         _col3.DataInfo.AuxiliaryType = AuxiliaryType.ArithmeticStdDev;
         _col1.AddRelatedColumn(_col3);
         _valuesToAdd = new Cache<string, float>();
         sut.Add(_col1);
         sut.Add(_col2);
         sut.Add(_col3);
      }

      [Observation]
      public void should_replace_the_existing_values_if_a_values_was_already_defined_for_the_given_time()
      {
         _valuesToAdd.Add(_col1.Id, 100);
         _valuesToAdd.Add(_col2.Id, 200);
         sut.InsertValues(2f, _valuesToAdd);

         _col1[1].ShouldBeEqualTo(100);
         _col2[1].ShouldBeEqualTo(200);
      }

      [Observation]
      public void should_insert_the_values_at_the_expected_index_for_a_new_time_value()
      {
         _valuesToAdd.Add(_col1.Id, 100);
         _valuesToAdd.Add(_col2.Id, 200);
         sut.InsertValues(2.5f, _valuesToAdd);

         _col1[2].ShouldBeEqualTo(100);
         _col2[2].ShouldBeEqualTo(200);
      }

      [Observation]
      public void should_not_touch_the_existing_values_if_available()
      {
         _valuesToAdd.Add(_col1.Id, 100);
         sut.InsertValues(2f, _valuesToAdd);
         _col2[1].ShouldBeEqualTo(22f);
      }
   }

   public class When_swaping_out_values_in_a_data_repository : concern_for_DataRepository
   {
      private DataColumn _col1;
      private DataColumn _col2;
      private DataColumn _col3;

      protected override void Context()
      {
         base.Context();
         _baseGrid1.Values = new List<float> { 1f, 2f, 3f };
         _col1 = new DataColumn("col1", "col1", _length, _baseGrid1) { Values = new[] { 1f, 11f, 111f } };
         _col2 = new DataColumn("col2", "col2", _length, _baseGrid1) { Values = new[] { 2f, 22f, 222f } };
         _col3 = new DataColumn("col3", "col3", _length, _baseGrid1) { Values = new[] { 3f, 33f, 333f } };
         _col3.DataInfo.AuxiliaryType = AuxiliaryType.ArithmeticStdDev;
         _col1.AddRelatedColumn(_col3);
         sut.Add(_col1);
         sut.Add(_col2);
         sut.Add(_col3);
      }

      protected override void Because()
      {
         sut.SwapValues(2f, 4f);
      }

      [Observation]
      public void should_have_remove_the_values_at_the_old_index_and_added_them_at_the_expected_index()
      {
         _baseGrid1.Values.ShouldOnlyContainInOrder(1f, 3f,4f);
         _col1.Values.ShouldOnlyContainInOrder(1f, 111f, 11f);
         _col2.Values.ShouldOnlyContainInOrder(2f,222f,22f);
         _col3.Values.ShouldOnlyContainInOrder(3f,333f,33f);
      }
   }

   public class When_swaping_out_values_in_a_data_repository_and_the_origin_value_does_not_exist : concern_for_DataRepository
   {
      private DataColumn _col1;

      protected override void Context()
      {
         base.Context();
         _baseGrid1.Values = new List<float> { 1f, 2f, 3f };
         _col1 = new DataColumn("col1", "col1", _length, _baseGrid1) { Values = new[] { 1f, 11f, 111f } };
         sut.Add(_col1);
      }

      protected override void Because()
      {
         sut.SwapValues(1.5f, 4f);
      }

      [Observation]
      public void should_do_nothing()
      {
         _baseGrid1.Values.ShouldOnlyContainInOrder(1f, 2f, 3f);
         _col1.Values.ShouldOnlyContainInOrder(1f, 11f, 111f);
      }
   }

   public class When_removing_the_values_at_a_given_index : concern_for_DataRepository
   {
      private DataColumn _col1;

      protected override void Context()
      {
         base.Context();
         _baseGrid1.Values = new List<float> { 1f, 2f, 3f };
         _col1 = new DataColumn("col1", "col1", _length, _baseGrid1) { Values = new[] { 1f, 11f, 111f } };
         sut.Add(_col1);
      }

      protected override void Because()
      {
         sut.RemoveValuesAt(1);
      }

      [Observation]
      public void should_have_removed_the_values_defined_at_this_idnex()
      {
         _baseGrid1.Values.ShouldOnlyContainInOrder(1f,  3f);
         _col1.Values.ShouldOnlyContainInOrder(1f, 111f);
      }
   }
   public class when_extended_property_value_is_present : concern_for_DataRepository
   {
      protected override void Context()
      {
         base.Context();
         var data = new ExtendedProperty<string>();
         data.ValueAsObject = "thisdata";
         sut.ExtendedProperties.Add("thisname", data);
      }

      [Observation]
      public void get_extended_property_should_return_proper_string()
      {
         sut.ExtendedPropertyValueFor("thisname").ShouldBeEqualTo("thisdata");
      }
   }

   public class when_extended_property_value_isnt_present : concern_for_DataRepository
   {
      [Observation]
      public void get_extended_property_value_should_return_null()
      {
         sut.ExtendedPropertyValueFor("thisdata").ShouldBeNull();
      }
   }

   public class When_the_repository_is_being_cleared : concern_for_DataRepository
   {
      private DataColumn _dc1;

      protected override void Context()
      {
         base.Context();
         _dc1 = new DataColumn("Lea", _length, _baseGrid1);
         sut.Add(_dc1);
      }

      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_have_removed_all_columns_previously_defined_in_the_repository()
      {
         Assert.IsFalse(sut.Contains(_dc1.Id));
         Assert.IsFalse(_dc1.IsInRepository());
         Assert.AreSame(_dc1.Repository, null);
         Assert.IsFalse(_baseGrid1.IsInRepository());
         Assert.AreSame(_baseGrid1.Repository, null);
      }
   }

   public class When_retrieving_a_columng_by_id_that_does_not_exist_in_the_repository : concern_for_DataRepository
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.GetColumn(Guid.NewGuid().ToString())).ShouldThrowAn<KeyNotFoundException>();
      }
   }

   public class When_adding_the_same_column_twice_to_a_repository : concern_for_DataRepository
   {
      private DataColumn _dc1;

      protected override void Context()
      {
         base.Context();
         _dc1 = new DataColumn("Lea", _length, _baseGrid1);
         sut.Add(_dc1);
         sut.Add(_dc1);
      }

      [Observation]
      public void should_not_do_anything()
      {
         Assert.AreSame(_dc1, sut.GetColumn(_dc1.Id));
      }
   }
}
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core
{
   public abstract class concern_for_MoleculeStartValue : ContextSpecification<MoleculeStartValue>
   {
      protected override void Context()
      {
         sut = new MoleculeStartValue {ContainerPath = new ObjectPath("Path1", "Path2"), Name = "Name"};
      }
   }

   public class when_instantiating_new_MoleculeStartValue : concern_for_MoleculeStartValue
   {
      [Observation]
      public void name_should_be_last_element_in_ObjectPath()
      {
         sut.MoleculeName.ShouldBeEqualTo("Name");
      }

      [Observation]
      public void container_path_should_be_equal_to_all_but_last_element()
      {
         sut.ContainerPath.ShouldOnlyContainInOrder("Path1", "Path2");
      }

      [Observation]
      public void parameter_path_should_be_equal_to_container_path_plus_parameter_name()
      {
         sut.Path.ShouldOnlyContainInOrder("Path1", "Path2", "Name");
      }
   }

   public class when_setting_molecule_name : concern_for_MoleculeStartValue
   {
      protected override void Because()
      {
         sut.Name = "Name2";
      }

      [Observation]
      public void parameter_name_should_be_updated()
      {
         sut.MoleculeName.ShouldBeEqualTo("Name2");
      }

      [Observation]
      public void container_path_is_not_affected()
      {
         sut.ContainerPath.ShouldOnlyContainInOrder("Path1", "Path2");
      }

      [Observation]
      public void parameter_path_should_reflect_new_name()
      {
         sut.Path.ShouldOnlyContainInOrder("Path1", "Path2", "Name2");
      }
   }

   public abstract class when_testing_equivalency_in_msv : concern_for_MoleculeStartValue
   {
      protected MoleculeStartValue _comparable;
      protected bool _result;

      protected override void Context()
      {
         sut = new MoleculeStartValue();
         _comparable = new MoleculeStartValue();

         sut.IsPresent = true;
         _comparable.IsPresent = true;

         sut.Path = new ObjectPath("A", "B", "MoleculeName");
         _comparable.Path = new ObjectPath("A", "B", "MoleculeName");

         sut.ScaleDivisor = 1.0;
         _comparable.ScaleDivisor = 1.0;
      }

      protected override void Because()
      {
         _result = sut.IsEquivalentTo(_comparable);
      }
   }

   public abstract class equivalency_should_test_negative : when_testing_equivalency_in_msv
   {
      [Observation]
      public void msv_should_not_be_equivalent()
      {
         _result.ShouldBeFalse();
      }
   }

   public class when_testing_msv_with_different_scale_factor : equivalency_should_test_negative
   {
      protected override void Context()
      {
         base.Context();
         sut.ScaleDivisor = _comparable.ScaleDivisor * 1.1;
      }
   }

   public class when_testing_msv_with_different_moleculename : equivalency_should_test_negative
   {
      protected override void Context()
      {
         base.Context();
         sut.Path = new ObjectPath("A", "B", "NewName");
      }
   }

   public class when_testing_msv_with_different_isPresent : equivalency_should_test_negative
   {
      protected override void Context()
      {
         base.Context();
         sut.IsPresent = !_comparable.IsPresent;
      }


   }

   public class when_testing_equivalent_msv : when_testing_equivalency_in_msv
   {
      [Observation]
      public void empty_start_value_should_be_equivalent()
      {
         _result.ShouldBeTrue();
      }
   }
}
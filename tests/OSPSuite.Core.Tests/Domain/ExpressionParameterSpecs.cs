using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ExpressionParameter : ContextSpecification<ExpressionParameter>
   {
      protected override void Context()
      {
         sut = new ExpressionParameter() { ContainerPath = new ObjectPath("Path1", "Path2"), Name = "Name" };
      }
   }

   public class When_instantiating_new_ExpressionParameter : concern_for_ExpressionParameter
   {
      [Observation]
      public void name_should_be_last_element_in_ObjectPath()
      {
         sut.Name.ShouldBeEqualTo("Name");
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

   public class When_setting_expression_parameter_name : concern_for_MoleculeStartValue
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

   public abstract class When_testing_equivalency_in_expression_parameters : concern_for_MoleculeStartValue
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

   public abstract class expression_parameter_equivalency_should_test_negative : When_testing_equivalency_in_expression_parameters
   {
      [Observation]
      public void should_not_be_equivalent()
      {
         _result.ShouldBeFalse();
      }
   }

   public class When_testing_expression_parameters_with_different_scale_factor : equivalency_should_test_negative
   {
      protected override void Context()
      {
         base.Context();
         sut.ScaleDivisor = _comparable.ScaleDivisor * 1.1;
      }
   }

   public class When_testing_expression_parameters_with_different_moleculename : equivalency_should_test_negative
   {
      protected override void Context()
      {
         base.Context();
         sut.Path = new ObjectPath("A", "B", "NewName");
      }
   }

   public class When_testing_expression_parameters_with_different_isPresent : equivalency_should_test_negative
   {
      protected override void Context()
      {
         base.Context();
         sut.IsPresent = !_comparable.IsPresent;
      }


   }

   public class When_testing_equivalent_expression_parameters : When_testing_equivalency_in_expression_parameters
   {
      [Observation]
      public void empty_expression_parameters_should_be_equivalent()
      {
         _result.ShouldBeTrue();
      }
   }
}

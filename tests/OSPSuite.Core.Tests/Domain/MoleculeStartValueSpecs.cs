using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_InitialCondition : ContextSpecification<InitialCondition>
   {
      protected override void Context()
      {
         sut = new InitialCondition {ContainerPath = new ObjectPath("Path1", "Path2"), Name = "Name"};
      }
   }

   public class when_instantiating_new_initial_condition : concern_for_InitialCondition
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

   public class when_setting_molecule_name : concern_for_InitialCondition
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

   public class When_setting_initial_condition_name : concern_for_InitialCondition
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
}
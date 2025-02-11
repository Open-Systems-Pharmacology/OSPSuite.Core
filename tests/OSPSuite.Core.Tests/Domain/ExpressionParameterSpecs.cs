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

   public class When_setting_expression_parameter_name : concern_for_InitialCondition
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

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterValue : ContextSpecification<ParameterValue>
   {
      protected override void Context()
      {
         sut = new ParameterValue {ContainerPath = new ObjectPath("Path1", "Path2"), Name = "Name"};
      }
   }

   public class when_instantiating_new_ParameterValue : concern_for_ParameterValue
   {
      [Observation]
      public void name_should_be_last_element_in_ObjectPath()
      {
         sut.ParameterName.ShouldBeEqualTo("Name");
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

   public class when_creating_parameter_value_with_empty_path : concern_for_ParameterValue
   {
      protected override void Context()
      {
         sut = new ParameterValue();
      }

      [Observation]
      public void container_path_should_be_empty()
      {
         sut.ContainerPath.ShouldBeEqualTo(ObjectPath.Empty);
      }

      [Observation]
      public void name_should_be_null()
      {
         sut.ParameterName.ShouldBeEqualTo(ObjectPath.Empty.ToString());
      }
   }

   public class when_setting_parameter_name : concern_for_ParameterValue
   {
      protected override void Because()
      {
         sut.Name = "Name2";
      }

      [Observation]
      public void parameter_name_should_be_updated()
      {
         sut.ParameterName.ShouldBeEqualTo("Name2");
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
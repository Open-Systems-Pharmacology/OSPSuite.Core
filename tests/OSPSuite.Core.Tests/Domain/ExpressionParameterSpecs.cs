using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

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

      [Observation]
      public void the_initial_state_is_not_set()
      {
         sut.HasInitialState.ShouldBeFalse();
      }

      [Observation]
      public void an_initial_value_sets_the_initial_state()
      {
         sut.InitialValue = 1;
         sut.HasInitialState.ShouldBeTrue();
      }

      [Observation]
      public void an_initial_formula_sets_the_initial_state()
      {
         sut.InitialFormulaId = "formulaId";
         sut.HasInitialState.ShouldBeTrue();
      }

      [Observation]
      public void an_initial_unit_sets_the_initial_state()
      {
         sut.InitialUnit = new Unit();
         sut.HasInitialState.ShouldBeTrue();
      }
   }
}

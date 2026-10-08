using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_JacobianMatrix : ContextSpecification<JacobianMatrix>
   {
      protected PartialDerivatives _partialDerivatives;

      protected override void Context()
      {
         sut = new JacobianMatrix(new[] { "P1", "P2" });
         _partialDerivatives = new PartialDerivatives("O1", new[] { "P1", "P2" });
         sut.AddPartialDerivatives(_partialDerivatives);
      }
   }

   public class When_renaming_a_parameter_of_the_jacobian_matrix : concern_for_JacobianMatrix
   {
      protected override void Because()
      {
         sut.RenameParameter("P1", "Renamed");
      }

      [Observation]
      public void should_rename_the_parameter_column()
      {
         sut.ParameterNames.ShouldOnlyContainInOrder("Renamed", "P2");
      }

      [Observation]
      public void should_rename_the_parameter_in_the_partial_derivatives()
      {
         _partialDerivatives.ParameterNames.ShouldOnlyContainInOrder("Renamed", "P2");
      }
   }

   public class When_renaming_a_parameter_that_is_not_in_the_jacobian_matrix : concern_for_JacobianMatrix
   {
      protected override void Because()
      {
         sut.RenameParameter("Unknown", "Renamed");
      }

      [Observation]
      public void should_not_change_the_parameter_columns()
      {
         sut.ParameterNames.ShouldOnlyContainInOrder("P1", "P2");
      }

      [Observation]
      public void should_not_change_the_parameter_names_of_the_partial_derivatives()
      {
         _partialDerivatives.ParameterNames.ShouldOnlyContainInOrder("P1", "P2");
      }
   }
}

using OSPSuite.BDDHelper;
using MathNet.Numerics.LinearAlgebra;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_MathNetExtensions : StaticContextSpecification
   {
   }

   public class When_calculating_the_pseudo_inverse_of_a_matrix : concern_for_MathNetExtensions
   {
      private Matrix<double> A;
      private Matrix<double> INV;

      protected override void Context()
      {
         base.Context();
         A = Matrix<double>.Build.DenseOfColumnArrays(new[] {1d, 2d}, new[] {3d, 4d});
      }

      protected override void Because()
      {
         INV = A.PseudoInverse();
      }

      [Observation]
      public void should_verify_the_transity_rules_1()
      {
         var mat = A.Multiply(INV).Multiply(A);
         mat.ShouldBeEqualTo(A);
      }


      [Observation]
      public void should_verify_the_transity_rules_2()
      {
         var mat = INV.Multiply(A).Multiply(INV);
         mat.ShouldBeEqualTo(INV);
      }
   }
}	
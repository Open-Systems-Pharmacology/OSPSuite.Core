using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_PKParameterSensitivity : ContextSpecification<PKParameterSensitivity>
   {
      protected override void Context()
      {
         sut = new PKParameterSensitivity();
      }
   }

   public class When_updating_the_parameter_name_of_a_pk_parameter_sensitivity : concern_for_PKParameterSensitivity
   {
      protected override void Because()
      {
         sut.ParameterName = "toto";
      }

      [Observation]
      public void should_update_the_id()
      {
         sut.Id.Contains("toto").ShouldBeTrue();
      }
   }

   public class When_updating_the_pk_parameter_name_of_a_pk_parameter_sensitivity : concern_for_PKParameterSensitivity
   {
      protected override void Because()
      {
         sut.PKParameterName = "tata";
      }

      [Observation]
      public void should_update_the_id()
      {
         sut.Id.Contains("tata").ShouldBeTrue();
      }
   }

   public class When_updating_the_path_of_a_pk_parameter_sensitivity : concern_for_PKParameterSensitivity
   {
      protected override void Because()
      {
         sut.QuantityPath = "titi";
      }

      [Observation]
      public void should_update_the_id()
      {
         sut.Id.Contains("titi").ShouldBeTrue();
      }
   }
}
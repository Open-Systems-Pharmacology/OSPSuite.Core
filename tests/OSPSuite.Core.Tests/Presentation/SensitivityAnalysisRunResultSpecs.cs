using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_SensitivityAnalysisRunResult : ContextSpecification<SensitivityAnalysisRunResult>
   {
      protected PKParameterSensitivity _pkParameterSensitivity1;
      protected PKParameterSensitivity _pkParameterSensitivity2;

      protected override void Context()
      {
         sut = new SensitivityAnalysisRunResult();
         _pkParameterSensitivity1 = new PKParameterSensitivity
         {
            ParameterName = "P1",
            PKParameterName = "AUC",
            QuantityPath = "Organism|Liver|Volume",
            Value = 0.8
         };

         _pkParameterSensitivity2 = new PKParameterSensitivity
         {
            ParameterName = "P2",
            PKParameterName = "AUC2",
            QuantityPath = "Organism|Liver|Volume",
            Value = 0.8
         };

         sut.AddPKParameterSensitivity(_pkParameterSensitivity1);
         sut.AddPKParameterSensitivity(_pkParameterSensitivity2);
      }
   }

   public class When_adding_a_pk_parameter_sensitivity_to_a_simulation_analysis_result : concern_for_SensitivityAnalysisRunResult
   {
      [Observation]
      public void should_be_able_to_retrieve_the_parmaeter_sensitivity()
      {
         sut.AllPKParameterSensitivities.ShouldContain(_pkParameterSensitivity2);
      }
   }

   public class When_retrieving_all_parameter_sensitivity_for_a_given_quantity_path_and_pk_parameter : concern_for_SensitivityAnalysisRunResult
   {
      [Observation]
      public void should_return_the_expected_pk_parameter_sensitivity()
      {
         sut.AllFor(_pkParameterSensitivity1.PKParameterName, _pkParameterSensitivity1.QuantityPath).ShouldOnlyContain(_pkParameterSensitivity1);
      }
   }

   public class When_updating_the_parameter_name_of_all_pk_parameter_sensitivities : concern_for_SensitivityAnalysisRunResult
   {
      protected override void Because()
      {
         sut.UpdateSensitivityParameterName(_pkParameterSensitivity1.ParameterName, "NEW_NAME");
      }

      [Observation]
      public void should_update_the_name_of_all_pk_parameter_sensitivities_using_the_parameter_by_name()
      {
         _pkParameterSensitivity1.ParameterName.ShouldBeEqualTo("NEW_NAME");
      }
   }
}
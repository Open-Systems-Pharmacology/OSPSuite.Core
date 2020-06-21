using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SensitivityAnalysisRunResult : ContextSpecification<SensitivityAnalysisRunResult>
   {
      protected PKParameterSensitivity _pkParameterSensitivity1;
      protected PKParameterSensitivity _pkParameterSensitivity2;
      protected PKParameterSensitivity _pkParameterSensitivity3;

      protected override void Context()
      {
         sut = new SensitivityAnalysisRunResult();
         _pkParameterSensitivity1 = new PKParameterSensitivity
         {
            ParameterName = "P1",
            PKParameterName = "AUC",
            QuantityPath = "Organism|Liver|Drug|Concentration",
            Value = 0.8
         };

         _pkParameterSensitivity2 = new PKParameterSensitivity
         {
            ParameterName = "P2",
            PKParameterName = "AUC2",
            QuantityPath = "Organism|Liver|Drug|Concentration",
            Value = 0.8
         };

         _pkParameterSensitivity3 = new PKParameterSensitivity
         {
            ParameterName = "P2",
            PKParameterName = "AUC",
            QuantityPath = "Organism|Kidney|Drug|Concentration",
            Value = 0.8
         };

         sut.AddPKParameterSensitivity(_pkParameterSensitivity1);
         sut.AddPKParameterSensitivity(_pkParameterSensitivity2);
         sut.AddPKParameterSensitivity(_pkParameterSensitivity3);
      }
   }

   public class When_retrieving_pk_parameter_sensitivity_object : concern_for_SensitivityAnalysisRunResult
   {
      [Observation]
      public void should_return_the_expected_object_if_found()
      {
         sut.PKParameterSensitivityFor(_pkParameterSensitivity1.PKParameterName, _pkParameterSensitivity1.QuantityPath, _pkParameterSensitivity1.ParameterName).ShouldBeEqualTo(_pkParameterSensitivity1);
      }

      [Observation]
      public void should_return_null_if_the_parameter_is_not_found()
      {
         sut.PKParameterSensitivityFor(_pkParameterSensitivity1.PKParameterName, _pkParameterSensitivity1.QuantityPath, "Unknown").ShouldBeNull();
      }
   }

   public class When_retrieving_pk_parameter_sensitivity_values : concern_for_SensitivityAnalysisRunResult
   {
      [Observation]
      public void should_return_the_expected_value_if_the_result_exist_for_the_given_parameter_combination()
      {
         sut.PKParameterSensitivityValueFor(_pkParameterSensitivity1.PKParameterName, _pkParameterSensitivity1.QuantityPath, _pkParameterSensitivity1.ParameterName).ShouldBeEqualTo(_pkParameterSensitivity1.Value);
      }

      [Observation]
      public void should_return_NaN_if_the_parameter_is_not_found()
      {
         sut.PKParameterSensitivityValueFor(_pkParameterSensitivity1.PKParameterName, _pkParameterSensitivity1.QuantityPath, "Unknown").ShouldBeEqualTo(double.NaN);
      }
   }

   public class When_adding_a_pk_parameter_sensitivity_to_a_simulation_analysis_result : concern_for_SensitivityAnalysisRunResult
   {
      [Observation]
      public void should_be_able_to_retrieve_the_parameter_sensitivity()
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


   public class When_retrieving_the_name_of_all_pk_parameters_defined_in_a_sensitivity_analysis_result : concern_for_SensitivityAnalysisRunResult
   {
      protected override void Because()
      {
         sut.UpdateSensitivityParameterName(_pkParameterSensitivity1.ParameterName, "NEW_NAME");
      }

      [Observation]
      public void should_return_the_name_of_all_pk_parameters()
      {
         sut.AllPKParameterNames.ShouldOnlyContain("AUC", "AUC2");
      }
   }

   public class When_retrieving_the_paths_of_all_pk_parameters_defined_in_a_sensitivity_analysis_result : concern_for_SensitivityAnalysisRunResult
   {

      [Observation]
      public void should_return_the_name_of_all_pk_parameters()
      {
         sut.AllQuantityPaths.ShouldOnlyContain(_pkParameterSensitivity1.QuantityPath,  _pkParameterSensitivity3.QuantityPath);
      }
   }


   public class When_retrieving_the_pk_parameter_sensitivity_analysis_covering_a_given_total_sensitivity : concern_for_SensitivityAnalysisRunResult
   {
      private IReadOnlyList<PKParameterSensitivity> _result;
      private string _pkParameterName;
      private string _outputPath;
      private PKParameterSensitivity _pk1;
      private PKParameterSensitivity _pk2;
      private PKParameterSensitivity _pk3;

      protected override void Context()
      {
         base.Context();
         _pkParameterName = "AUC";
         _outputPath = "Output";
         _pk1 = new PKParameterSensitivity { PKParameterName = _pkParameterName, QuantityPath = _outputPath, Value = 0.4 };
         _pk2 = new PKParameterSensitivity { PKParameterName = _pkParameterName, QuantityPath = _outputPath, Value = 0.1 };
         _pk3 = new PKParameterSensitivity { PKParameterName = _pkParameterName, QuantityPath = _outputPath, Value = -0.6 };
         sut.AddPKParameterSensitivity(_pk1);
         sut.AddPKParameterSensitivity(_pk2);
         sut.AddPKParameterSensitivity(_pk3);
      }

      protected override void Because()
      {
         _result = sut.AllPKParameterSensitivitiesFor(_pkParameterName, _outputPath, 0.7);
      }

      [Observation]
      public void should_return_the_expected_parameters()
      {
         _result.ShouldOnlyContainInOrder(_pk3, _pk1);
      }
   }

   public class When_retrieving_the_pk_parameter_sensitivity_analysis_covering_a_given_100_percent_of_the_sensitivity : concern_for_SensitivityAnalysisRunResult
   {
      private IReadOnlyList<PKParameterSensitivity> _result;
      private string _pkParameterName;
      private string _outputPath;
      private PKParameterSensitivity _pk1;
      private PKParameterSensitivity _pk2;
      private PKParameterSensitivity _pk3;
      private PKParameterSensitivity _pk4;

      protected override void Context()
      {
         base.Context();
         _pkParameterName = "AUC";
         _outputPath = "Output";
         _pk1 = new PKParameterSensitivity { PKParameterName = _pkParameterName, QuantityPath = _outputPath, Value = 0.4 };
         _pk2 = new PKParameterSensitivity { PKParameterName = _pkParameterName, QuantityPath = _outputPath, Value = 0.1 };
         _pk3 = new PKParameterSensitivity { PKParameterName = _pkParameterName, QuantityPath = _outputPath, Value = -0.6 };
         _pk4 = new PKParameterSensitivity { PKParameterName = _pkParameterName, QuantityPath = _outputPath, Value = 0 };
         sut.AddPKParameterSensitivity(_pk1);
         sut.AddPKParameterSensitivity(_pk2);
         sut.AddPKParameterSensitivity(_pk3);
         sut.AddPKParameterSensitivity(_pk4);
      }

      protected override void Because()
      {
         _result = sut.AllPKParameterSensitivitiesFor(_pkParameterName, _outputPath, 1);
      }

      [Observation]
      public void should_return_all_parameter_even_if_their_sensitivity_is_zero()
      {
         _result.ShouldOnlyContainInOrder(_pk3, _pk1, _pk2, _pk4);
      }
   }

}
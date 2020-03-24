using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Converters.v9;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Converters
{
   public abstract class concern_for_Converter730To90 : ContextSpecification<Converter730To90>
   {
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _dimensionFactory = new DimensionFactoryForIntegrationTests();
         sut = new Converter730To90(_dimensionFactory);
      }
   }


   public class When_converting_a_sensitivity_analysis_to_version_90 : concern_for_Converter730To90
   {
      private SensitivityAnalysis _sa;
      private PKParameterSensitivity _parameterSensitivity1;
      private PKParameterSensitivity _parameterSensitivity2;
      private int _version;
      private bool _converted;

      protected override void Context()
      {
         base.Context();
         _parameterSensitivity1 = new PKParameterSensitivity {PKParameterName = "AUC"};
         _parameterSensitivity2 = new PKParameterSensitivity {PKParameterName = "C_max"};
         _sa = new SensitivityAnalysis {Results = new SensitivityAnalysisRunResult()};
         _sa.Results.AddPKParameterSensitivity(_parameterSensitivity1);
         _sa.Results.AddPKParameterSensitivity(_parameterSensitivity2);
      }

      protected override void Because()
      {
         (_version, _converted ) = sut.Convert(_sa);
      }

      [Observation]
      public void should_have_converted_the_pk_parameter_name_that_were_updated()
      {
         _version.ShouldBeEqualTo(PKMLVersion.V9_0);
         _converted.ShouldBeTrue();
         _parameterSensitivity1.PKParameterName.ShouldBeEqualTo("AUC_tEnd");
      }


      [Observation]
      public void should_not_update_pk_parameter_names_that_were_not_changed()
      {
         _parameterSensitivity2.PKParameterName.ShouldBeEqualTo("C_max");

      }
   }
}
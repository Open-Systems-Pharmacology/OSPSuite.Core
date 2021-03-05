using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Converters.v10;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Serialization;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Converters
{
   public abstract class concern_for_Converter90To100 : ContextSpecification<Converter90To100>
   {
      protected override void Context()
      {
         sut = new Converter90To100();
      }
   }

   public class When_converting_a_parameter_identification_to_version_10 : concern_for_Converter90To100
   {
      private ParameterIdentification _parameterIdentification;
      private int _version;
      private bool _converted;
      private OptimizedParameterValue _optimizedParameterValue;
      private IdentificationParameter _identificationParameter;

      protected override void Context()
      {
         base.Context();
         _optimizedParameterValue = new OptimizedParameterValue("PARAM", 10, 20, 0, 0, Scalings.Linear);

         _parameterIdentification = new ParameterIdentification();
         _identificationParameter = DomainHelperForSpecs.IdentificationParameter("PARAM", 0.1, 100, 20);
         _identificationParameter.Scaling = Scalings.Log;


         _parameterIdentification.AddIdentificationParameter(_identificationParameter);
         var runResult = new ParameterIdentificationRunResult();
         runResult.BestResult.AddValue(_optimizedParameterValue);
         _parameterIdentification.AddResult(runResult);
      }

      protected override void Because()
      {
         (_version, _converted) = sut.Convert(_parameterIdentification);
      }

      [Observation]
      public void should_have_updated_the_min_and_max_of_all_parameter_identification_results_based_on_the_corresponding_identification_parameters()
      {
         _converted.ShouldBeTrue();
         _version.ShouldBeEqualTo(PKMLVersion.V10_0);
         _optimizedParameterValue.Min.ShouldBeEqualTo(_identificationParameter.MinValue);
         _optimizedParameterValue.Max.ShouldBeEqualTo(_identificationParameter.MaxValue);
         _optimizedParameterValue.Scaling.ShouldBeEqualTo(_identificationParameter.Scaling);
      }
   }
}
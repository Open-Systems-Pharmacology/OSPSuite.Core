using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Converter.v6_3;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Helpers;

namespace OSPSuite.Converter.v6_3
{
   public abstract class concern_for_Converter62To63 : ContextSpecification<Converter62To63>
   {
      protected override void Context()
      {
         base.Context();
         sut = new Converter62To63();
      }
   }

   public class When_converting_a_parameter_identification_with_results : concern_for_Converter62To63
   {
      private ParameterIdentification _parameterIdentification;
      private DataRepository _simulationResult;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         _simulationResult = DomainHelperForSpecs.SimulationDataRepositoryFor("SIM");
         var runResult = new ParameterIdentificationRunResult();
         runResult.BestResult.AddResult(_simulationResult);
         _parameterIdentification.AddResult(runResult);
      }

      protected override void Because()
      {
         sut.Convert(_parameterIdentification);
      }

      [Observation]
      public void should_ensure_that_the_origin_of_all_calculation_columns_is_set_to_column_auxiliary()
      {
         _simulationResult.AllButBaseGrid().Each(x => x.DataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.CalculationAuxiliary));
      }
   }
}
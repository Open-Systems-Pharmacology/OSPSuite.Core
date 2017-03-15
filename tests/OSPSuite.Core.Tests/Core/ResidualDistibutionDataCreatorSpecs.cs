using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_ResidualDistibutionDataCreator : ContextSpecification<IResidualDistibutionDataCreator>
   {
      protected IBinIntervalsCreator _binIntervalCreator;
      protected DataRepository _observedData;

      protected override void Context()
      {
         _binIntervalCreator = A.Fake<IBinIntervalsCreator>();
         _observedData = DomainHelperForSpecs.ObservedData();
         sut = new ResidualDistibutionDataCreator(_binIntervalCreator);
      }
   }

   public class When_creating_the_residual_distribution_data_for_a_given_optimization_run_result : concern_for_ResidualDistibutionDataCreator
   {
      private OptimizationRunResult _optimizationRunResult;
      private OutputResiduals _outputResidual1;
      private OutputResiduals _outputResidual2;
      private ContinuousDistributionData _data;
      private BinInterval _binInterval1;
      private BinInterval _binInterval2;
      private ResidualsResult _residualResults;

      protected override void Context()
      {
         base.Context();
         _outputResidual1 = new OutputResiduals("O1", _observedData, new[] { new Residual(1f, 5f, 1), new Residual(2f, 10f, 1) });
         _outputResidual2 = new OutputResiduals("O2", _observedData, new[] { new Residual(3f, 15f, 1), new Residual(4f, 19f, 1), new Residual(4f, 20f, 1) });

         _residualResults = new ResidualsResult();
         _optimizationRunResult = new OptimizationRunResult { ResidualsResult = _residualResults };
         _residualResults.AddOutputResiduals(_outputResidual1);
         _residualResults.AddOutputResiduals(_outputResidual2);

         _binInterval1 = new BinInterval(0, 17);
         _binInterval2 = new BinInterval(18, 25);
         A.CallTo(_binIntervalCreator).WithReturnType<IReadOnlyList<BinInterval>>().Returns(new[] { _binInterval1, _binInterval2 });
      }

      protected override void Because()
      {
         _data = sut.CreateFor(_optimizationRunResult);
      }

      [Observation]
      public void should_return_a_residual_distribution_data_containing_the_expected_count()
      {
         _data.DataTable.Rows.Count.ShouldBeEqualTo(2);
         _data.DataTable.Rows[0][_data.YAxisName].ShouldBeEqualTo(3);
         _data.DataTable.Rows[1][_data.YAxisName].ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_return_a_residual_distribution_data_containing_the_expected_data_min_value()
      {
         _data.XMinData.ShouldBeEqualTo(5f);
      }

      [Observation]
      public void should_return_a_residual_distribution_data_containing_the_expected_data_max_value()
      {
         _data.XMaxData.ShouldBeEqualTo(20f);
      }
   }
}
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;

namespace OSPSuite.Core
{
   public abstract class concern_for_SensitivityAnalysisPKAnalysesTask : ContextSpecification<ISensitivityAnalysisPKAnalysesTask>
   {
      protected ILazyLoadTask _lazyLoadTask;
      protected IPKValuesCalculator _pkValuesCalculator;
      protected IPKParameterRepository _pkParameterRepository;
      protected IPKCalculationOptionsFactory _pkCalculationOptionsFactory;

      protected override void Context()
      {
         _lazyLoadTask = A.Fake<ILazyLoadTask>();
         _pkValuesCalculator = A.Fake<IPKValuesCalculator>();
         _pkParameterRepository = A.Fake<IPKParameterRepository>();
         _pkCalculationOptionsFactory = A.Fake<IPKCalculationOptionsFactory>();
         sut = new SensitivityAnalysisPKAnalysesTask(_lazyLoadTask, _pkValuesCalculator, _pkParameterRepository, _pkCalculationOptionsFactory);
      }
   }

   public class When_calculating_the_pk_analyses_for_a_sensitivity_analysis : concern_for_SensitivityAnalysisPKAnalysesTask
   {
      private ISimulation _simulation;
      private SimulationResults _runResults;
      private PKParameter _p1;
      private PKParameter _p2;
      private PopulationSimulationPKAnalyses _popAnalaysis;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>();
         _runResults = A.Fake<SimulationResults>();
         var outputSelections = new OutputSelections();
         outputSelections.AddOutput(new QuantitySelection("Liver|Cell|Drug|Concentration", QuantityType.Drug));

         A.CallTo(() => _simulation.OutputSelections).Returns(outputSelections);
         var pKCalculationOptions = new PKCalculationOptions();
         A.CallTo(_pkCalculationOptionsFactory).WithReturnType<PKCalculationOptions>().Returns(pKCalculationOptions);

         _p1 = new PKParameter().WithName("AUC");
         _p1.Mode = PKParameterMode.Single;
         _p2 = new PKParameter().WithName("AUC" + Constants.PKParameters.NormSuffix);
         _p2.Mode = PKParameterMode.Single;

         A.CallTo(() => _pkParameterRepository.All()).Returns(new[] {_p1, _p2});
         var individualResults = A.Fake<IndividualResults>();
         A.CallTo(() => _runResults.AllIndividualResults).Returns(new HashSet<IndividualResults>(new [] {individualResults}));
         var pKValues = new PKValues();
         pKValues.AddValue(_p1.Name, 10f);
         pKValues.AddValue(_p2.Name, 20f);
         A.CallTo(_pkValuesCalculator).WithReturnType<PKValues>().Returns(pKValues);
      }

      protected override void Because()
      {
         _popAnalaysis = sut.CalculateFor(_simulation, 1, _runResults);
      }

      [Observation]
      public void should_exclude_the_norm_parameters()
      {
         _popAnalaysis.All().Count().ShouldBeEqualTo(1);
         _popAnalaysis.PKParameterFor("Liver|Cell|Drug|Concentration", _p1.Name).Values[0].ShouldBeEqualTo(10f);
         _popAnalaysis.HasPKParameterFor("Liver|Cell|Drug|Concentration", _p2.Name).ShouldBeFalse();
      }
   }
}
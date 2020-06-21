using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.R.Domain;

namespace OSPSuite.R.Mapper
{
   public abstract class concern_for_SensitivityAnalysisToCoreSensitivityAnalysisMapper : ContextSpecification<SensitivityAnalysisToCoreSensitivityAnalysisMapper>
   {
      protected ISensitivityAnalysisTask _sensitivityAnalysisTask;
      protected ISimulationAnalyzer _simulationAnalyzer;
      protected IParameterAnalysableParameterSelector _parameterSelector;
      protected IContainerTask _containerTask;
      protected SensitivityAnalysis _sensitivityAnalysis;
      protected ISimulation _simulation;
      protected Core.Domain.SensitivityAnalyses.SensitivityAnalysis _result;
      protected Core.Domain.SensitivityAnalyses.SensitivityAnalysis _coreSensitivityAnalysis;
      protected PathCache<IParameter> _constantParameterCache;
      protected string _parameterPath1 = "Param1";
      protected string _parameterPath2 = "Param2";
      protected string _parameterPath3 = "Param3";
      protected string _parameterPath4 = "Param4";
      protected List<ParameterSelection> _allSensitivityParametersCreated;

      protected override void Context()
      {
         _sensitivityAnalysisTask = A.Fake<ISensitivityAnalysisTask>();
         _simulationAnalyzer = A.Fake<ISimulationAnalyzer>();
         _parameterSelector = A.Fake<IParameterAnalysableParameterSelector>();
         _containerTask = A.Fake<IContainerTask>();

         _simulation = A.Fake<ISimulation>();
         _sensitivityAnalysis = new SensitivityAnalysis(_simulation);
         _coreSensitivityAnalysis = new Core.Domain.SensitivityAnalyses.SensitivityAnalysis();

         sut = new SensitivityAnalysisToCoreSensitivityAnalysisMapper(_sensitivityAnalysisTask, _simulationAnalyzer, _parameterSelector, _containerTask);

         _constantParameterCache = new PathCacheForSpecs<IParameter>
         {
            {_parameterPath1, DomainHelperForSpecs.ConstantParameterWithValue(10).WithName("P1")},
            {_parameterPath2, DomainHelperForSpecs.ConstantParameterWithValue(10).WithName("P2")},
            {_parameterPath3, DomainHelperForSpecs.ConstantParameterWithValue(10).WithName("P3")}
         };

         A.CallTo(() => _sensitivityAnalysisTask.CreateSensitivityAnalysisFor(_simulation)).Returns(_coreSensitivityAnalysis);
         A.CallTo(_containerTask).WithReturnType<PathCache<IParameter>>().Returns(_constantParameterCache);

         _allSensitivityParametersCreated = new List<ParameterSelection>();
         A.CallTo(() => _sensitivityAnalysisTask.AddParametersTo(_coreSensitivityAnalysis, A<IReadOnlyList<ParameterSelection>>._))
            .Invokes(x => _allSensitivityParametersCreated.AddRange(x.GetArgument<IReadOnlyList<ParameterSelection>>(1)));
      }
   }

   public class When_mapping_a_r_sensitivity_analysis_without_predefined_variable_parameters_to_a_core_sensitivity_analysis_ : concern_for_SensitivityAnalysisToCoreSensitivityAnalysisMapper
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _simulationAnalyzer.AllPathOfParametersUsedInSimulation(_simulation)).Returns(new[]
         {
            _parameterPath1,
            _parameterPath2,
            _parameterPath3,
            _parameterPath4,
         });
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_sensitivityAnalysis);
      }

      [Observation]
      public void should_create_a_new_core_sensitivity_analysis()
      {
         _result.ShouldBeEqualTo(_coreSensitivityAnalysis);
      }

      [Observation]
      public void should_return_a_sensitivity_analysis_with_all_potentially_variables_parameters()
      {
         _allSensitivityParametersCreated.Count.ShouldBeEqualTo(3);
         _allSensitivityParametersCreated.Find(x => x.Path == _parameterPath1).ShouldNotBeNull();
         _allSensitivityParametersCreated.Find(x => x.Path == _parameterPath2).ShouldNotBeNull();
         _allSensitivityParametersCreated.Find(x => x.Path == _parameterPath3).ShouldNotBeNull();
      }

      [Observation]
      public void should_not_add_an_entry_for_parameters_that_could_not_be_varied()
      {
         _allSensitivityParametersCreated.Find(x => x.Path == _parameterPath4).ShouldBeNull();
      }
   }

   public class When_mapping_a_r_sensitivity_analysis_with_predefined_variable_parameters_to_a_core_sensitivity_analysis_ : concern_for_SensitivityAnalysisToCoreSensitivityAnalysisMapper
   {
      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis.AddParameterPath(_parameterPath1);
         _sensitivityAnalysis.AddParameterPath(_parameterPath2);
         _sensitivityAnalysis.AddParameterPath(_parameterPath4);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_sensitivityAnalysis);
      }

      [Observation]
      public void should_create_a_new_core_sensitivity_analysis()
      {
         _result.ShouldBeEqualTo(_coreSensitivityAnalysis);
      }

      [Observation]
      public void should_return_a_sensitivity_analysis_containing_only_predefined_parameters()
      {
         _allSensitivityParametersCreated.Count.ShouldBeEqualTo(2);
         _allSensitivityParametersCreated.Find(x => x.Path == _parameterPath1).ShouldNotBeNull();
         _allSensitivityParametersCreated.Find(x => x.Path == _parameterPath2).ShouldNotBeNull();
      }

      [Observation]
      public void should_exclude_parameters_that_could_not_be_found()
      {
         _allSensitivityParametersCreated.Find(x => x.Path == _parameterPath4).ShouldBeNull();
      }
   }

   public class When_filtering_the_parameters_of_a_simulation_that_can_be_used_for_sensitivity_analysis : concern_for_SensitivityAnalysisToCoreSensitivityAnalysisMapper
   {
      [Observation]
      public void should_exclude_parameters_that_cannot_be_varied_in_general()
      {
         var parameter = new Parameter();
         A.CallTo(() => _parameterSelector.CanUseParameter(parameter)).Returns(false);
         sut.ParameterCanBeUsedForSensitivity(parameter).ShouldBeFalse();
      }

      [Observation]
      public void should_exclude_parameters_that_are_not_constant()
      {
         var parameter = new Parameter {Formula = new ExplicitFormula()};
         A.CallTo(() => _parameterSelector.CanUseParameter(parameter)).Returns(true);
         sut.ParameterCanBeUsedForSensitivity(parameter).ShouldBeFalse();
      }

      [Observation]
      public void should_include_parameters_that_have_a_formula_but_are_fixed()
      {
         var parameter = new Parameter {Formula = new ExplicitFormula("1+2"), Value = 5};
         A.CallTo(() => _parameterSelector.CanUseParameter(parameter)).Returns(true);
         sut.ParameterCanBeUsedForSensitivity(parameter).ShouldBeTrue();
      }

      [Observation]
      public void should_include_parameters_that_have_a_distributed_formula()
      {
         var parameter = DomainHelperForSpecs.NormalDistributedParameter(defaultMean: 10);
         A.CallTo(() => _parameterSelector.CanUseParameter(parameter)).Returns(true);
         sut.ParameterCanBeUsedForSensitivity(parameter).ShouldBeTrue();
      }

      [Observation]
      public void should_exclude_parameters_that_are_constant_with_a_value_of_zero()
      {
         var parameter = DomainHelperForSpecs.ConstantParameterWithValue(0);
         A.CallTo(() => _parameterSelector.CanUseParameter(parameter)).Returns(true);
         sut.ParameterCanBeUsedForSensitivity(parameter).ShouldBeFalse();
      }
   }
}
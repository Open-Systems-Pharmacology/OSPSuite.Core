using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Mappers
{
   internal abstract class concern_for_ObserverBuilderToObserverMapper : ContextSpecification<IObserverBuilderToObserverMapper>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      protected IFormulaBuilderToFormulaMapper _formulaMapper;
      protected IEntityTracker _entityTracker;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _formulaMapper = A.Fake<IFormulaBuilderToFormulaMapper>();
         _entityTracker = A.Fake<IEntityTracker>();
         sut = new ObserverBuilderToObserverMapper(_objectBaseFactory, _formulaMapper, _entityTracker);
      }
   }

   internal class When_mapping_an_observer_from_an_observer_builder : concern_for_ObserverBuilderToObserverMapper
   {
      private ObserverBuilder _observerBuilder;
      private IFormula _mappedFormula;
      private Observer _observer;
      private SimulationConfiguration _simulationConfiguration;
      private SimulationBuilder _simulationBuilder;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _simulationBuilder = new SimulationBuilderForSpecs(_simulationConfiguration);
         _observerBuilder = new ObserverBuilder().WithName("toto").WithDimension(A.Fake<IDimension>());
         _observerBuilder.Formula = A.Fake<IFormula>();
         _mappedFormula = A.Fake<IFormula>();
         A.CallTo(() => _objectBaseFactory.Create<Observer>()).Returns(A.Fake<Observer>());
         A.CallTo(() => _formulaMapper.MapFrom(_observerBuilder.Formula, _simulationBuilder)).Returns(_mappedFormula);
      }

      protected override void Because()
      {
         _observer = sut.MapFrom(_observerBuilder, _simulationBuilder);
      }

      [Observation]
      public void should_return_an_observer_whose_name_was_set_to_the_name_of_the_observer()
      {
         _observer.Name.ShouldBeEqualTo(_observerBuilder.Name);
      }

      [Observation]
      public void should_return_an_observer_whose_formula_was_set_to_the_mapped_formula_of_the_builder()
      {
         _observer.Formula.ShouldBeEqualTo(_mappedFormula);
      }

      [Observation]
      public void should_return_an_observer_whose_dimension_was_set_to_the_dimension_of_the_builder()
      {
         _observer.Dimension.ShouldBeEqualTo(_observerBuilder.Dimension);
      }

      [Observation]
      public void should_have_added_a_reference_to_the_observer_builder_for_the_created_observer()
      {
         A.CallTo(() => _entityTracker.Track(_observer, _observerBuilder, _simulationBuilder)).MustHaveHappened();
      }
   }
}
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_ObserverBuilderToObserverMapper : ContextSpecification<IObserverBuilderToObserverMapper>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      protected IFormulaBuilderToFormulaMapper _formulaMapper;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _formulaMapper = A.Fake<IFormulaBuilderToFormulaMapper>();
         sut = new ObserverBuilderToObserverMapper(_objectBaseFactory,_formulaMapper);
      }
   }

   
   public class When_mapping_an_observer_from_an_observer_builder : concern_for_ObserverBuilderToObserverMapper
  {
      private IObserverBuilder _observerBuilder;
      private IFormula _mappedFormula;
      private IObserver _observer;
      private IBuildConfiguration _buildConfiguration;

      protected override void Context()
      {
         base.Context();
         _buildConfiguration = A.Fake<IBuildConfiguration>();
         _observerBuilder = A.Fake<IObserverBuilder>().WithName("toto").WithDimension(A.Fake<IDimension>());
         _observerBuilder.Formula = A.Fake<IFormula>();
         _mappedFormula = A.Fake<IFormula>();
         A.CallTo(()=>_objectBaseFactory.Create<IObserver>()).Returns(A.Fake<IObserver>());
         A.CallTo(() => _formulaMapper.MapFrom(_observerBuilder.Formula, _buildConfiguration)).Returns(_mappedFormula);
      }
      protected override void Because()
      {
         _observer = sut.MapFrom(_observerBuilder,_buildConfiguration);
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
         A.CallTo(() => _buildConfiguration.AddBuilderReference(_observer, _observerBuilder)).MustHaveHappened();
      }
   }

}	
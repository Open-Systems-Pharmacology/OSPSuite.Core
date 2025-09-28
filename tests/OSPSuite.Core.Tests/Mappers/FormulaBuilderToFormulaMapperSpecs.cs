using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Mappers
{
   internal abstract class concern_for_FormulaBuilderToFormulaMapper : ContextSpecification<IFormulaBuilderToFormulaMapper>
   {
      protected ICloneManagerForModel _cloneManagerForModel;

      protected override void Context()
      {
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         sut = new FormulaBuilderToFormulaMapper(_cloneManagerForModel);
      }
   }

   internal class When_mapping_a_formula_from_a_formula_builder : concern_for_FormulaBuilderToFormulaMapper
   {
      protected IFormula _mappedFormula;
      protected IFormula _formula;
      private IFormula _clonedFormula;
      private SimulationConfiguration _simulationConfiguration;
      private SimulationBuilder _simulationBuilder;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _simulationBuilder = new SimulationBuilderForSpecs(_simulationConfiguration);
         _formula = A.Fake<IFormula>();
         _clonedFormula = A.Fake<IFormula>();
         A.CallTo(() => _cloneManagerForModel.Clone(_formula)).Returns(_clonedFormula);
         A.CallTo(() => _cloneManagerForModel.Clone((IFormula) null)).Returns(null);
      }

      protected override void Because()
      {
         _mappedFormula = sut.MapFrom(_formula, _simulationBuilder);
      }

      [Observation]
      public void should_return_a_clone_of_the_given_formula()
      {
         _mappedFormula.ShouldBeEqualTo(_clonedFormula);
      }

      [Observation]
      public void should_return_null_if_the_given_formula_was_null()
      {
         sut.MapFrom(null, _simulationBuilder).ShouldBeNull();
      }
   }
}
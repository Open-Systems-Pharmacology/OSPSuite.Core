using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   internal abstract class concern_for_ProcessRateParameterCreatorSpecs : ContextSpecification<IProcessRateParameterCreator>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      protected IFormulaBuilderToFormulaMapper _formulaMapper;
      protected IEntityTracker _entityTracker;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _formulaMapper = A.Fake<IFormulaBuilderToFormulaMapper>();
         _entityTracker = A.Fake<IEntityTracker>();
         sut = new ProcessRateParameterCreator(_objectBaseFactory, _formulaMapper, _entityTracker);
      }
   }

   internal class When_creating_a_parameter_rate_for_a_process_builder : concern_for_ProcessRateParameterCreatorSpecs
   {
      private IFormula _kinetic;
      private IParameter _processRateParameter;
      private ProcessBuilder _processBuilder;
      private SimulationConfiguration _simulationConfiguration;
      private FormulaUsablePath _formulaUsablePathB;
      private FormulaUsablePath _formulaUsablePathA;
      private FormulaUsablePath _formulaUsablePathFU;
      private FormulaUsablePath _formulaUsablePathBW;
      private SimulationBuilder _simulationBuilder;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
         _processBuilder = new ReactionBuilder();
         _processBuilder.CreateProcessRateParameter = true;
         _kinetic = new ExplicitFormula("(A+B)*fu/BW");
         _formulaUsablePathA = new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, "A").WithAlias("A");
         _kinetic.AddObjectPath(_formulaUsablePathA);
         _formulaUsablePathB = new FormulaUsablePath("B").WithAlias("B");
         _kinetic.AddObjectPath(_formulaUsablePathB);
         _formulaUsablePathFU = new FormulaUsablePath(ObjectPathKeywords.MOLECULE, "fu").WithAlias("fu");
         _kinetic.AddObjectPath(_formulaUsablePathFU);
         _formulaUsablePathBW = new FormulaUsablePath("Organism", "BW").WithAlias("BW");
         _kinetic.AddObjectPath(_formulaUsablePathBW);
         _processBuilder.CreateProcessRateParameter = true;
         _processBuilder.ProcessRateParameterPersistable = true;
         A.CallTo(() => _formulaMapper.MapFrom(_kinetic, _simulationBuilder)).Returns(_kinetic);
         _processBuilder.Name = "Reaction";
         _processBuilder.Formula = _kinetic;
         _processRateParameter = new Parameter();
         A.CallTo(() => _objectBaseFactory.Create<IParameter>()).Returns(_processRateParameter);
      }

      protected override void Because()
      {
         _processRateParameter = sut.CreateProcessRateParameterFor(_processBuilder, _simulationBuilder);
      }

      [Observation]
      public void should_have_created_the_parameter()
      {
         _processRateParameter.ShouldNotBeNull();
      }

      [Observation]
      public void created_parameter_should_be_persistable()
      {
         _processRateParameter.Persistable.ShouldBeTrue();
      }

      [Observation]
      public void created_parameter_should_be_default()
      {
         _processRateParameter.IsDefault.ShouldBeTrue();
      }

      [Observation]
      public void should_update_relative_paths()
      {
         _formulaUsablePathA.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER, "A");
         _formulaUsablePathB.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER, "B");
      }

      [Observation]
      public void should_leave_absolute_paths_unchanged()
      {
         _formulaUsablePathBW.ShouldOnlyContainInOrder("Organism", "BW");
         _formulaUsablePathFU.ShouldOnlyContainInOrder(ObjectPathKeywords.MOLECULE, "fu");
      }
   }
}
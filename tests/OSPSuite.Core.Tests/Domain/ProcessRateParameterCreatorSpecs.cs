using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ProcessRateParameterCreatorSpecs : ContextSpecification<IProcessRateParameterCreator>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      protected IFormulaBuilderToFormulaMapper _formulaMapper;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _formulaMapper = A.Fake<IFormulaBuilderToFormulaMapper>();
         sut = new ProcessRateParameterCreator(_objectBaseFactory,_formulaMapper);
      }
   }
   public class When_creating_a_parameter_rate_for_a_process_builder : concern_for_ProcessRateParameterCreatorSpecs
   {
      private IFormula _kinetic;
      private IParameter _processRateParameter;
      private IProcessBuilder _processBuilder;
      private IBuildConfiguration _buildConfiguration;
      private IFormulaUsablePath _formulaUsablePathB;
      private IFormulaUsablePath _formulaUsablePathA;
      private IFormulaUsablePath _formulaUsablePathFU;
      private FormulaUsablePath _formulaUsablePathBW;

      protected override void Context()
      {
         base.Context();
         _buildConfiguration = A.Fake<IBuildConfiguration>();
         _processBuilder = new ReactionBuilder();
         _processBuilder.CreateProcessRateParameter = true;
         _kinetic = new ExplicitFormula("(A+B)*fu/BW");
         _formulaUsablePathA = new FormulaUsablePath(new[] {ObjectPath.PARENT_CONTAINER, "A"}).WithAlias("A");
         _kinetic.AddObjectPath(_formulaUsablePathA);
         _formulaUsablePathB = new FormulaUsablePath(new[] { "B" }).WithAlias("B");
         _kinetic.AddObjectPath(_formulaUsablePathB);
         _formulaUsablePathFU = new FormulaUsablePath(new[] {ObjectPathKeywords.MOLECULE, "fu"}).WithAlias("fu");
         _kinetic.AddObjectPath(_formulaUsablePathFU);
         _formulaUsablePathBW = new FormulaUsablePath(new[] { "Organism", "BW" }).WithAlias("BW");
         _kinetic.AddObjectPath(_formulaUsablePathBW);
         _processBuilder.CreateProcessRateParameter = true;
         _processBuilder.ProcessRateParameterPersistable = true;
         A.CallTo(() => _formulaMapper.MapFrom(_kinetic, _buildConfiguration)).Returns(_kinetic);
         _processBuilder.Name = "Reaction";
         _processBuilder.Formula = _kinetic;
         _processRateParameter = new Parameter();
         A.CallTo(() => _objectBaseFactory.Create<IParameter>()).Returns(_processRateParameter);

      }

      protected override void Because()
      {
         _processRateParameter = sut.CreateProcessRateParameterFor(_processBuilder, _buildConfiguration);
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
      public void should_update_reative_paths()
      {
         _formulaUsablePathA.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER,ObjectPath.PARENT_CONTAINER,"A");
         _formulaUsablePathB.ShouldOnlyContainInOrder(ObjectPath.PARENT_CONTAINER,  "B");
      }

      [Observation]
      public void should_leave_absolutPaths_unchanged()
      {
         _formulaUsablePathBW.ShouldOnlyContainInOrder("Organism", "BW");
         _formulaUsablePathFU.ShouldOnlyContainInOrder(ObjectPathKeywords.MOLECULE, "fu");
      }
   }
   
}	
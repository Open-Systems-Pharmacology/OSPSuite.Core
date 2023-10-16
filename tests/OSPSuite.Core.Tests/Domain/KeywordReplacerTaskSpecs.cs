using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using static OSPSuite.Core.Domain.ObjectPathKeywords;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_KeywordReplacerTask : ContextSpecification<IKeywordReplacerTask>
   {
      protected string _modelName;
      protected FormulaUsablePath _objPathFirstNeighbor;
      protected FormulaUsablePath _objPathMolecule;
      protected FormulaUsablePath _objPathOrganism;
      protected IModel _model;
      protected ReplacementContext _replacementContext;

      protected override void Context()
      {
         _modelName = "model";
         _model = A.Fake<IModel>().WithName(_modelName);
         _model.Root = A.Fake<IContainer>().WithName(_modelName);
         A.CallTo(() => _model.Root.GetChildren<IContainer>())
            .Returns(new[] {new Container().WithName(Constants.ORGANISM), new Container().WithName("B")});
         _objPathFirstNeighbor = new FormulaUsablePath(new[] {FIRST_NEIGHBOR, "A"});
         _objPathMolecule = new FormulaUsablePath(new[] {"B"});
         _objPathOrganism = new FormulaUsablePath(new[] {Constants.ORGANISM, "C"});
         sut = new KeywordReplacerTask(new ObjectPathFactory(new AliasCreator()));
         _replacementContext = new ReplacementContext(_model);
      }
   }

   public class When_replacing_the_keyword_in_a_reaction : concern_for_KeywordReplacerTask
   {
      private Reaction _reaction;

      protected override void Context()
      {
         base.Context();
         _reaction = new Reaction().WithFormula(A.Fake<IFormula>());
         A.CallTo(() => _reaction.Formula.ObjectPaths).Returns(new[] {_objPathFirstNeighbor, _objPathMolecule, _objPathOrganism});
      }

      protected override void Because()
      {
         sut.ReplaceInReactionContainer(_reaction, _replacementContext);
      }

      [Observation]
      public void should_have_replace_the_molecule_properties_keyword_with_the_global_path_to_the_molecule_properties_container_in_the_model()
      {
         _objPathMolecule.ElementAt(0).ShouldBeEqualTo(_modelName);
         _objPathMolecule.ElementAt(1).ShouldBeEqualTo("B");
      }

      [Observation]
      public void should_have_replace_the_organism_keyword_with_the_global_path_to_the_organism_container_in_the_model()
      {
         _objPathOrganism.ElementAt(0).ShouldBeEqualTo(_modelName);
         _objPathOrganism.ElementAt(1).ShouldBeEqualTo(Constants.ORGANISM);
         _objPathOrganism.ElementAt(2).ShouldBeEqualTo("C");
      }
   }

   public class When_replacing_the_keyword_in_a_molecule_properties_container : concern_for_KeywordReplacerTask
   {
      private IContainer _moleculeContainer;
      private string _moleculeName;

      protected override void Context()
      {
         base.Context();
         _objPathMolecule = new FormulaUsablePath(new[] {MOLECULE});
         _moleculeName = "REPLACED";
         _moleculeContainer = new Container().WithName(_modelName);
         var moleculeParameter = new Parameter().WithFormula(A.Fake<IFormula>());
         _moleculeContainer.Add(moleculeParameter);
         A.CallTo(() => moleculeParameter.Formula.ObjectPaths).Returns(new[] {_objPathFirstNeighbor, _objPathMolecule, _objPathOrganism});
      }

      protected override void Because()
      {
         sut.ReplaceIn(_moleculeContainer, _moleculeName, _replacementContext);
      }

      [Observation]
      public void should_have_replace_the_molecule_properties_keyword_with_the_global_path_to_the_molecule_properties_container_in_the_model()
      {
         _objPathMolecule.ElementAt(0).ShouldBeEqualTo(_modelName);
         _objPathMolecule.ElementAt(1).ShouldBeEqualTo(_moleculeName);
      }

      [Observation]
      public void should_have_replace_the_organism_keyword_with_the_global_path_to_the_organism_container_in_the_model()
      {
         _objPathOrganism.ElementAt(0).ShouldBeEqualTo(_modelName);
         _objPathOrganism.ElementAt(1).ShouldBeEqualTo(Constants.ORGANISM);
         _objPathOrganism.ElementAt(2).ShouldBeEqualTo("C");
      }
   }

   public class When_replacing_keywords_in_a_parameter_defined_in_a_global_molecule_container : concern_for_KeywordReplacerTask
   {
      private IParameter _parameter;
      private IContainer _rootContainer;
      private IContainer _moleculeContainer;
      private FormulaUsablePath _objectPath;

      protected override void Context()
      {
         base.Context();
         _rootContainer = new Container().WithName("ROOT");
         _moleculeContainer = new Container().WithName("CYP").WithContainerType(ContainerType.Molecule);
         _parameter = DomainHelperForSpecs.ConstantParameterWithValue(4).WithName("P").WithParentContainer(_moleculeContainer);
         _parameter.Formula = new ExplicitFormula();
         _objectPath = new FormulaUsablePath("SIM", MOLECULE, "test");
         _parameter.Formula.AddObjectPath(_objectPath);
         _replacementContext = new ReplacementContext(_rootContainer);
      }

      protected override void Because()
      {
         sut.ReplaceIn(_parameter, _replacementContext);
      }

      [Observation]
      public void should_have_replaced_the_molecule_keywords()
      {
         _objectPath.ElementAt(1).ShouldBeEqualTo(_moleculeContainer.Name);
      }
   }
}
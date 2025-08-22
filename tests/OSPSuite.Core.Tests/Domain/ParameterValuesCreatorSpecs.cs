using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   internal abstract class concern_for_ParameterValuesCreator : ContextSpecification<ParameterValuesCreator>
   {
      protected IProjectRetriever _projectRetriever;
      protected IProject _currentProject;

      protected override void Context()
      {
         _currentProject = A.Fake<IProject>();
         _projectRetriever = A.Fake<IProjectRetriever>();
         A.CallTo(() => _projectRetriever.CurrentProject).Returns(_currentProject);
         sut = new ParameterValuesCreator(
            new IdGenerator(),
            new EntityPathResolverForSpecs(),
            _projectRetriever,
            new CloneManagerForBuildingBlock(new ObjectBaseFactoryForSpecs(DimensionFactoryForSpecs.Factory), new DataRepositoryTask()));
      }
   }

   internal class When_creating_a_parameter_value_for_a_parameter_with_formula_and_object_path : concern_for_ParameterValuesCreator
   {
      private ObjectPath _objectPath;
      private IParameter _parameter;
      private ParameterValue _psv;

      protected override void Context()
      {
         base.Context();
         _objectPath = new ObjectPath("A", "B", "C");
         _parameter = new Parameter().WithDimension(DomainHelperForSpecs.FractionDimensionForSpecs());
         _parameter.Formula = new ExplicitFormula("t");
      }

      protected override void Because()
      {
         _psv = sut.CreateParameterValue(_objectPath, _parameter);
      }

      [Observation]
      public void the_value_should_be_null()
      {
         _psv.Value.ShouldBeNull();
      }

      [Observation]
      public void the_formula_should_be_set()
      {
         _psv.Formula.IsExplicit().ShouldBeTrue();
      }
   }

   internal class When_creating_a_parameter_value_for_a_parameter_and_object_path : concern_for_ParameterValuesCreator
   {
      private ObjectPath _objectPath;
      private IParameter _parameter;
      private ParameterValue _psv;

      protected override void Context()
      {
         base.Context();
         _objectPath = new ObjectPath("A", "B", "C");
         _parameter = DomainHelperForSpecs.ConstantParameterWithValue(5).WithDimension(DomainHelperForSpecs.FractionDimensionForSpecs());
         _parameter.Formula = new ExplicitFormula("5");
      }

      protected override void Because()
      {
         _psv = sut.CreateParameterValue(_objectPath, _parameter);
      }

      [Observation]
      public void the_formula_should_be_null()
      {
         _psv.Formula.ShouldBeNull();
      }

      [Observation]
      public void should_return_a_parameter_value_using_the_provided_path_as_well_as_the_dimension_and_the_value_of_the_parameter()
      {
         _psv.Path.ToString().ShouldBeEqualTo(_objectPath.ToString());
         _psv.Dimension.ShouldBeEqualTo(_parameter.Dimension);
         _psv.Value.ShouldBeEqualTo(_parameter.Value);
      }
   }

   internal class When_creating_parameter_values_for_spatial_structure_and_molecules : concern_for_ParameterValuesCreator
   {
      private SpatialStructure _spatialStructure;
      private List<MoleculeBuilder> _molecules;
      private IReadOnlyList<ParameterValue> _parameterValues;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = new SpatialStructure();
         var topContainer = new Container().WithName("Top").WithMode(ContainerMode.Physical);
         _spatialStructure.AddTopContainer(topContainer);
         var physicalContainer = new Container().WithName("physicalContainer").WithMode(ContainerMode.Physical);
         topContainer.Add(physicalContainer);
         topContainer.Add(new Container().WithName("logicalContainer").WithMode(ContainerMode.Logical));

         _molecules = new List<MoleculeBuilder>();

         var constantFormulaParameter = DomainHelperForSpecs.ConstantParameterWithValue(5).WithDimension(DomainHelperForSpecs.FractionDimensionForSpecs()).WithName("constantFormulaParameterName");
         constantFormulaParameter.ContainerCriteria = new DescriptorCriteria { new MatchTagCondition("physicalContainer") };
         var normalDistributionParameter = DomainHelperForSpecs.NormalDistributedParameter().WithDimension(DomainHelperForSpecs.FractionDimensionForSpecs()).WithName("normalDistributionParameterName");
         var molecule = new MoleculeBuilder().WithName("Molecule1");
         molecule.AddParameter(constantFormulaParameter);
         molecule.AddParameter(normalDistributionParameter);

         _molecules.Add(molecule);
      }

      protected override void Because()
      {
         _parameterValues = sut.CreateFrom(_spatialStructure, _molecules);
      }

      [Observation]
      public void parameter_values_should_only_be_created_for_each_constant_formula_parameter_in_each_physical_container_matching_tags()
      {
         _parameterValues.Select(x => x.Path.ToString()).ShouldOnlyContain("Top|physicalContainer|Molecule1|constantFormulaParameterName");
      }
   }

   internal class When_creating_parameters_without_criteria_from_proteins_and_organ : concern_for_ParameterValuesCreator
   {
      private IContainer _organContainer;
      private IReadOnlyList<MoleculeBuilder> _molecules;
      private MoleculeBuilder _protein;
      private IReadOnlyList<ParameterValue> _parameterValues;
      private Container _compartmentContainer;
      private ExpressionProfileBuildingBlock _expressionProfile;

      protected override void Context()
      {
         base.Context();

         _expressionProfile = new ExpressionProfileBuildingBlock
         {
            new ExpressionParameter().WithName("thalf")
         }.WithName("protein|something|something");


         var expressionParameter = new ExpressionParameter().WithName(Constants.Parameters.REL_EXP);
         expressionParameter.ContainerPath = new ObjectPath("organ", "Top", "compartment", "protein");
         expressionParameter.Value = 5.0;
         
         _expressionProfile.Add(expressionParameter);

         expressionParameter = new ExpressionParameter().WithName(Constants.Parameters.REL_EXP);
         expressionParameter.ContainerPath = new ObjectPath("organ", "org", "compartment", "protein");
         expressionParameter.Value = 4.0;

         _expressionProfile.Add(expressionParameter);

         expressionParameter = new ExpressionParameter().WithName(Constants.Parameters.REL_EXP);
         expressionParameter.ContainerPath = new ObjectPath("organ", "org2", "compartment", "protein");
         expressionParameter.Value = 4.0;

         _expressionProfile.Add(expressionParameter);

         A.CallTo(() => _currentProject.All<ExpressionProfileBuildingBlock>()).Returns(new List<ExpressionProfileBuildingBlock> { _expressionProfile });
         _protein = new MoleculeBuilder().WithName("protein");
         _protein.QuantityType = QuantityType.Transporter;
         var parameter = new Parameter().WithName(Constants.Parameters.REL_EXP).WithFormula(new ExplicitFormula("99"));
         _protein.AddParameter(parameter);
         var globalParameter = new Parameter().WithName("thalf");
         globalParameter.BuildMode = ParameterBuildMode.Global;
         _protein.AddParameter(globalParameter);
         var anotherParameter = new Parameter().WithName("some other parameter");

         _protein.AddParameter(anotherParameter);

         _molecules = new[] { _protein };
         _organContainer = new Container().WithName("organ").WithMode(ContainerMode.Physical);
         _organContainer.ParentPath = new ObjectPath("Top");
         _compartmentContainer = new Container().WithName("compartment").WithMode(ContainerMode.Physical);
         _compartmentContainer.ContainerType = ContainerType.Compartment;
         _organContainer.Add(_compartmentContainer);
      }

      protected override void Because()
      {
         _parameterValues = sut.CreateExpressionFrom(_organContainer, _molecules);
      }

      [Observation]
      public void the_parameter_values_with_compartment_match_should_use_the_best_match_expression_value()
      {
         _parameterValues.Single(x => x.Path.ToString().Equals($"Top|organ|compartment|protein|{Constants.Parameters.REL_EXP}")).Value.ShouldBeEqualTo(4.0);
      }

      [Observation]
      public void the_parameter_values_without_compartment_match_should_have_value_from_molecule_parameter()
      {
         var formula = _parameterValues.Single(x => x.Path.ToString().Equals($"Top|organ|protein|{Constants.Parameters.REL_EXP}")).Formula as ExplicitFormula;
         formula.ToString().ShouldBeEqualTo("99");
      }

      [Observation]
      public void the_parameter_values_should_include_local_expression_parameters_for_all_containers()
      {
         _parameterValues.Select(x => x.Path.ToString()).ShouldOnlyContain($"Top|organ|compartment|protein|{Constants.Parameters.REL_EXP}", $"Top|organ|protein|{Constants.Parameters.REL_EXP}");
      }
   }

   internal class When_creating_expression_parameters_from_proteins_and_organ : concern_for_ParameterValuesCreator
   {
      private IContainer _organContainer;
      private IReadOnlyList<MoleculeBuilder> _molecules;
      private MoleculeBuilder _protein;
      private ParameterValue[] _parameterValues;
      private Container _compartmentContainer;
      private ExpressionProfileBuildingBlock _expressionProfile;

      protected override void Context()
      {
         base.Context();
         var expressionParameter = new ExpressionParameter
         {
            Path = new ObjectPath("Organism", "VenousBlood", "Plasma", "protein", Constants.Parameters.REL_EXP)
         };

         var expressionParameter2 = new ExpressionParameter
         {
            Path = new ObjectPath("Organism", "VenousBlood", "Plasma", "protein", "somename")
         };
         _expressionProfile = new ExpressionProfileBuildingBlock
         {
            expressionParameter,
            expressionParameter2
         }.WithName("protein|something|something");
         A.CallTo(() => _currentProject.All<ExpressionProfileBuildingBlock>()).Returns(new List<ExpressionProfileBuildingBlock> { _expressionProfile });
         _protein = new MoleculeBuilder().WithName("protein");
         _protein.QuantityType = QuantityType.Transporter;
         var parameter = new Parameter().WithName(Constants.Parameters.REL_EXP);
         var parameter2 = new Parameter().WithName("somename");
         expressionParameter.Formula = new ExplicitFormula("5");
         expressionParameter2.Value = 5.0;
         expressionParameter2.Formula = null;
         parameter.ContainerCriteria = new DescriptorCriteria { new MatchTagCondition("Plasma") };
         parameter2.ContainerCriteria = new DescriptorCriteria { new MatchTagCondition("Plasma") };
         _protein.AddParameter(parameter);
         _protein.AddParameter(parameter2);
         var anotherParameter = new Parameter().WithName("some other parameter");

         _protein.AddParameter(anotherParameter);

         _molecules = new[] { _protein };
         _organContainer = new Container().WithName("VenousBlood").WithMode(ContainerMode.Physical);
         _organContainer.ParentPath = new ObjectPath("Organism");
         _compartmentContainer = new Container().WithName("Plasma").WithMode(ContainerMode.Physical);
         _organContainer.Add(_compartmentContainer);
      }

      protected override void Because()
      {
         _parameterValues = sut.CreateExpressionFrom(_organContainer, _molecules).ToArray();
      }

      [Observation]
      public void the_parameter_value_should_have_matching_formula()
      {
         _parameterValues[0].Formula.IsExplicit().ShouldBeTrue();
         _parameterValues[1].Formula.ShouldBeNull();
      }

      [Observation]
      public void the_parameter_value_should_have_matching_value()
      {
         _parameterValues[0].Value.ShouldBeNull();
         _parameterValues[1].Value.ShouldBeEqualTo(5.0);
      }

      [Observation]
      public void the_parameter_values_should_include_expression_parameters_for_the_compartment()
      {
         _parameterValues.Select(x => x.Path.ToString()).ShouldOnlyContain($"Organism|VenousBlood|Plasma|protein|{Constants.Parameters.REL_EXP}", "Organism|VenousBlood|Plasma|protein|somename");
      }
   }
}
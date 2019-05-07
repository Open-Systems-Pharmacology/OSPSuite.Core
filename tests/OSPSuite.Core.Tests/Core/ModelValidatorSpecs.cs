using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   internal abstract class concern_for_ModelValidator : ContextSpecification<IModelValidator>
   {
      protected IContainer _rootContainer;
      protected IContainer _validContainer;
      protected IContainer _invalidContainer;
      protected IFormula _validFormula;
      protected IFormula _invalidFormula;
      protected IObjectPathFactory _objectPathFactory;
      protected IBuildConfiguration _buildConfiguration;
      protected IObjectTypeResolver _objectTypeResolver;

      protected override void Context()
      {
         _objectPathFactory = new ObjectPathFactory(new AliasCreator());
         _buildConfiguration = A.Fake<IBuildConfiguration>();
         _validFormula = new ExplicitFormula("5*PAR1");
         _validFormula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("ROOT", "VALID", "PARA1").WithAlias("PAR1"));
         _invalidFormula = new ExplicitFormula("toto");
         _invalidFormula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("ROOT", "INVALID", "PARA5").WithAlias("PAR1"));
         _rootContainer = new Container().WithName("ROOT");
         _validContainer = new Container().WithName("VALID");
         _validContainer.Add(new Parameter().WithName("PARA1").WithFormula(new ConstantFormula(1)));
         _validContainer.Add(new Parameter().WithName("PARA2").WithFormula(_validFormula));
         _validContainer.Add(new Reaction().WithName("REACTION").WithFormula(_validFormula));
         _validContainer.Add(new Transport().WithName("TRANSPORT").WithFormula(_validFormula));
         _invalidContainer = new Container().WithName("INVALID");

         _invalidContainer.Add(new Parameter().WithName("PARA1").WithFormula(new ConstantFormula(1)));
         _invalidContainer.Add(new Parameter().WithName("PARA2").WithFormula(_invalidFormula));
         _invalidContainer.Add(new Reaction().WithName("REACTION").WithFormula(_invalidFormula));
         _invalidContainer.Add(new Transport().WithName("TRANSPORT").WithFormula(_invalidFormula));

         _rootContainer.Add(_validContainer);
         _rootContainer.Add(_invalidContainer);
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
      }
   }

   internal class When_validating_the_referencs_used_in_parmaters_inside_a_container_where_all_the_references_used_in_its_formula_were_found : concern_for_ModelValidator
   {
      private ValidationResult _results;

      protected override void Context()
      {
         base.Context();
         sut = new ValidatorForQuantities(_objectTypeResolver, _objectPathFactory);
      }

      protected override void Because()
      {
         _results = sut.Validate(_validContainer, _buildConfiguration);
      }

      [Observation]
      public void should_return_a_successful_results()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }

   internal class When_validating_the_references_used_in_parameters_inside_a_container_where_some_references_were_not_found : concern_for_ModelValidator
   {
      private ValidationResult _results;

      protected override void Context()
      {
         base.Context();
         sut = new ValidatorForQuantities(_objectTypeResolver, _objectPathFactory); ;
      }

      protected override void Because()
      {
         _results = sut.Validate(_rootContainer, _buildConfiguration);
      }

      [Observation]
      public void should_return_a_validation_result_with_one_validation_message_for_each_reference_that_was_not_found_in_the_container()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   internal class When_validating_the_referencs_used_in_a_reaction_or_transport_inside_a_container_where_all_the_references_used_in_its_formula_were_found : concern_for_ModelValidator
   {
      private ValidationResult _results;

      protected override void Context()
      {
         base.Context();
         sut = new ValidatorForReactionsAndTransports(_objectTypeResolver, _objectPathFactory); ;
      }

      protected override void Because()
      {
         _results = sut.Validate(_validContainer, _buildConfiguration);
      }

      [Observation]
      public void should_return_a_successful_results()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }

   internal class When_validating_the_referencs_used_in_a_reaction_or_transport_inside_a_container_where_some_references_were_not_found : concern_for_ModelValidator
   {
      private ValidationResult _results;

      protected override void Context()
      {
         base.Context();
         var reactions = new ReactionBuildingBlock {new ReactionBuilder().WithName("REACTION")};
         _buildConfiguration.Reactions = reactions;

         var passiveTransports = new PassiveTransportBuildingBlock {new TransportBuilder().WithName("TRANSPORT")};
         _buildConfiguration.PassiveTransports = passiveTransports;
         sut = new ValidatorForReactionsAndTransports(_objectTypeResolver, _objectPathFactory); ;
      }

      protected override void Because()
      {
         _results = sut.Validate(_rootContainer, _buildConfiguration);
      }

      [Observation]
      public void should_return_a_validation_result_with_one_validation_message_for_each_reference_that_was_not_found_in_the_container()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   internal class When_validating_the_references_used_in_a_quantity_inside_a_container_where_all_the_references_used_in_its_formula_were_found : concern_for_ModelValidator
   {
      private ValidationResult _results;

      protected override void Context()
      {
         base.Context();
         _validContainer.Add(new MoleculeAmount().WithName("Valid").WithFormula(_validFormula));
         sut = new ValidatorForQuantities(_objectTypeResolver, _objectPathFactory); ;
      }

      protected override void Because()
      {
         _results = sut.Validate(_validContainer, _buildConfiguration);
      }

      [Observation]
      public void should_return_a_successful_results()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }

   internal class When_validating_the_references_used_in_a_quantity_inside_a_container_where_one_references_point_to_a_container_instead_of_a_usable_entity : concern_for_ModelValidator
   {
      private ValidationResult _results;

      protected override void Context()
      {
         base.Context();

         var invalidFormula = new ExplicitFormula("toto");
         invalidFormula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("ROOT", "VALID", "PARA5").WithAlias("PAR1"));
         _validContainer.Add(new Parameter().WithName("PARA3").WithFormula(invalidFormula));

         //a referenced entity that exists but not usable in formula
         _validContainer.Add(new Container().WithName("PARA5"));
         sut = new ValidatorForQuantities(_objectTypeResolver, _objectPathFactory); ;
      }

      protected override void Because()
      {
         _results = sut.Validate(_validContainer, _buildConfiguration);
      }

      [Observation]
      public void should_return_the_expected_results()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }


   internal class When_validating_the_references_used_in_a_quantity_inside_a_container_where_some_references_were_not_found : concern_for_ModelValidator
   {
      private ValidationResult _results;

      protected override void Context()
      {
         base.Context();
         _validContainer.Add(new MoleculeAmount().WithName("Valid").WithFormula(_validFormula));
         _invalidContainer.Add(new MoleculeAmount().WithName("Invalid").WithFormula(_invalidFormula));
         sut = new ValidatorForQuantities(_objectTypeResolver, _objectPathFactory);
      }

      protected override void Because()
      {
         _results = sut.Validate(_rootContainer, _buildConfiguration);
      }

      [Observation]
      public void should_return_a_validation_result_with_one_validation_message_for_each_reference_that_was_not_found_in_the_container()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }


   internal class When_validating_the_references_used_in_a_parameter_with_sum_formula_: concern_for_ModelValidator
   {
      private ValidationResult _results;

      protected override void Context()
      {
         base.Context();
         var sumFormula = new SumFormula();
         sumFormula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("..", "..", "f_vas").WithAlias("f_vas_#i"));
         _validContainer.Add(new Parameter().WithName("DynamicParam").WithFormula(sumFormula));

         sut = new ValidatorForQuantities(_objectTypeResolver, _objectPathFactory); ;
      }

      protected override void Because()
      {
         _results = sut.Validate(_validContainer, _buildConfiguration);
      }

      [Observation]
      public void should_return_a_valid_state()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }
}
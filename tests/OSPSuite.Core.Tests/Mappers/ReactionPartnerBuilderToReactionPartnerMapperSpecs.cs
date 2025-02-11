using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;

namespace OSPSuite.Core.Mappers
{
   internal abstract class concern_for_ReactionPartnerBuilderToReactionPartnerMapper : ContextSpecification<IReactionPartnerBuilderToReactionPartnerMapper>
   {
      protected override void Context()
      {
         sut = new ReactionPartnerBuilderToReactionPartnerMapper();
      }
   }

   internal class When_mapping_a_reaction_partner_builder_to_a_reaction_partner : concern_for_ReactionPartnerBuilderToReactionPartnerMapper
   {
      private ReactionPartnerBuilder _reactionPartnerBuilder;
      private ReactionPartner _reactionPartner;
      private IContainer _container;
      private MoleculeAmount _moleculeAmount;
      private SimulationConfiguration _simulationConfiguration;
      private SimulationBuilder _simulationBuilder;

      protected override void Context()
      {
         base.Context();
         _reactionPartnerBuilder = new ReactionPartnerBuilder();
         _simulationConfiguration = new SimulationConfiguration();
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
         _container = new Container();
         _reactionPartnerBuilder.MoleculeName = "Drug";
         _moleculeAmount = new MoleculeAmount().WithName("Drug");
         _container.Add(_moleculeAmount);
         _reactionPartnerBuilder.StoichiometricCoefficient = 1.1;
      }

      protected override void Because()
      {
         _reactionPartner = sut.MapFromLocal(_reactionPartnerBuilder, _container, _simulationBuilder);
      }

      [Observation]
      public void should_have_copied_the_stoichiometric_coefficient()
      {
         _reactionPartner.StoichiometricCoefficient.ShouldBeEqualTo(_reactionPartnerBuilder.StoichiometricCoefficient);
      }

      [Observation]
      public void should_have_set_the_partner_property_to_molecule_amount()
      {
         _reactionPartner.Partner.ShouldBeEqualTo(_moleculeAmount);
      }
   }

   internal class When_mapping_a_reaction_partner_builder_to_a_reaction_partner_missing_a_amount : concern_for_ReactionPartnerBuilderToReactionPartnerMapper
   {
      private ReactionPartnerBuilder _reactionPartnerBuilder;
      private IContainer _container;
      private SimulationConfiguration _simulationConfiguration;
      private SimulationBuilder _simulationBuilder;

      protected override void Context()
      {
         base.Context();
         _reactionPartnerBuilder = new ReactionPartnerBuilder();
         _container = new Container();
         _simulationConfiguration = new SimulationConfiguration();
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration);
         _reactionPartnerBuilder.MoleculeName = "Drug";
         _reactionPartnerBuilder.StoichiometricCoefficient = 1.1;
      }

      [Observation]
      public void should_throw_a_MissingMoleculeAmountException()
      {
         The.Action(() => sut.MapFromLocal(_reactionPartnerBuilder, _container, _simulationBuilder)).ShouldThrowAn<MissingMoleculeAmountException>();
      }
   }
}
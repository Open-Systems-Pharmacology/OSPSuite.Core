using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_reaction_partner_builder_to_reaction_partner_mapper : ContextSpecification<IReactionPartnerBuilderToReactionPartnerMapper>
   {
      protected override void Context()
      {
         sut = new ReactionPartnerBuilderToReactionPartnerMapper();
      }
   }

   
   public class When_mapping_a_reaction_partner_builder_to_a_reaction_partner : concern_for_reaction_partner_builder_to_reaction_partner_mapper
   {
      private IReactionPartnerBuilder _reactionPartnerBuilder;
      private IReactionPartner _reactionPartner;
      private IContainer _container;
      private IMoleculeAmount _moleculeAmount;
      private IBuildConfiguration _buildConfiguration;

      protected override void Context()
      {
         base.Context();
         _reactionPartnerBuilder = A.Fake<IReactionPartnerBuilder>();
         _buildConfiguration = A.Fake<IBuildConfiguration>();
         _container = new Container();
         _reactionPartnerBuilder.MoleculeName="Drug";
         _moleculeAmount = A.Fake<IMoleculeAmount>().WithName("Drug");
         _container.Add(_moleculeAmount);
         _reactionPartnerBuilder.StoichiometricCoefficient = 1.1;
      }
      protected override void Because()
      {
         _reactionPartner = sut.MapFromLocal(_reactionPartnerBuilder,_container,_buildConfiguration);
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

   
   public class When_mapping_a_reaction_partner_builder_to_a_reaction_partner_missin_a_amount : concern_for_reaction_partner_builder_to_reaction_partner_mapper
   {
      private IReactionPartnerBuilder _reactionPartnerBuilder;
      private IContainer _container;
      private IBuildConfiguration _buildConfiguration;

      protected override void Context()
      {
         base.Context();
         _reactionPartnerBuilder = A.Fake<IReactionPartnerBuilder>();
         _container = new Container();
         _buildConfiguration = A.Fake<IBuildConfiguration>();
         _reactionPartnerBuilder.MoleculeName = "Drug";
         _reactionPartnerBuilder.StoichiometricCoefficient = 1.1;
      }
   

      [Observation]
      public void should_throw_a_MissingMoleculeAmountException()
      {
         The.Action(() => sut.MapFromLocal(_reactionPartnerBuilder, _container, _buildConfiguration)).ShouldThrowAn<MissingMoleculeAmountException>();
      }
     
   }
}	
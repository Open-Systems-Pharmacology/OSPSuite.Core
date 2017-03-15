using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   internal abstract class concern_for_ReactionCreator : ContextSpecification<IReactionCreator>
   {
      protected IReactionBuilderToReactionMapper _reactionMapper;
      protected IKeywordReplacerTask _keywordReplacerTask;
      protected IContainerTask _containerTask;
      protected IParameterBuilderCollectionToParameterCollectionMapper _parameterMapper;
      protected IContainer _rootContainer;
      protected IModel _model;
      protected IReactionBuilder _reactionBuilder;
      protected IBuildConfiguration _buildConfiguration;
      protected IReactionPartnerBuilder _educt1;
      protected IReactionPartnerBuilder _educt2;
      protected IReactionPartnerBuilder _product1;
      protected IContainer _globalContainer;
      protected IReaction _reaction;
      protected bool _result;

      protected override void Context()
      {
         _reactionMapper = A.Fake<IReactionBuilderToReactionMapper>();
         _keywordReplacerTask = A.Fake<IKeywordReplacerTask>();
         _containerTask = A.Fake<IContainerTask>();
         _parameterMapper = A.Fake<IParameterBuilderCollectionToParameterCollectionMapper>();

         sut = new ReactionCreator(_reactionMapper, _keywordReplacerTask, _containerTask, _parameterMapper);

         _model = A.Fake<IModel>();
         _reactionBuilder = A.Fake<IReactionBuilder>();
         _reactionBuilder.ContainerCriteria = new DescriptorCriteria();
         _reactionBuilder.Description = "A great description";
         _reactionBuilder.Name = "Reaction";
         _educt1 = A.Fake<IReactionPartnerBuilder>();
         _educt1.MoleculeName = "sp1";
         _educt2 = A.Fake<IReactionPartnerBuilder>();
         _educt2.MoleculeName = "sp2";
         _product1 = A.Fake<IReactionPartnerBuilder>();
         _product1.MoleculeName = "sp3";
         A.CallTo(() => _reactionBuilder.Educts).Returns(new[] {_educt1, _educt2});
         A.CallTo(() => _reactionBuilder.Products).Returns(new[] {_product1});
         A.CallTo(() => _reactionBuilder.ModifierNames).Returns(new[] {"modifier"});

         _buildConfiguration = A.Fake<IBuildConfiguration>();
         _rootContainer = new Container().WithMode(ContainerMode.Physical);
         _model.Root = _rootContainer;
         _globalContainer = new Container();

         _reaction = A.Fake<IReaction>().WithName(_reactionBuilder.Name);
         A.CallTo(() => _reactionMapper.MapFromLocal(A<IReactionBuilder>._, A<IContainer>._, _buildConfiguration)).Returns(_reaction);
         A.CallTo(() => _containerTask.CreateOrRetrieveSubContainerByName(_rootContainer, _reactionBuilder.Name)).Returns(_globalContainer);
      }

      protected override void Because()
      {
         _result = sut.CreateReaction(_reactionBuilder, _model, _buildConfiguration);
      }
   }

   internal class When_creating_the_reaction_based_on_a_given_builder_in_a_container_hieararchy : concern_for_ReactionCreator
   {
      private IContainer _liver;
      private IContainer _kidney;
      private IContainer _subLiver;
      private IContainer _helpContainer;
      private IContainer _subKidney;

      protected override void Context()
      {
         base.Context();
         //root container has all species
         _rootContainer.Add(A.Fake<IMoleculeAmount>().WithName("sp1"));
         _rootContainer.Add(A.Fake<IMoleculeAmount>().WithName("sp2"));
         _rootContainer.Add(A.Fake<IMoleculeAmount>().WithName("sp3"));
         _rootContainer.Add(A.Fake<IMoleculeAmount>().WithName("modifier"));

         //Liver has only two species
         _liver = new Container().WithName("Liver").WithMode(ContainerMode.Physical);
         _liver.Add(A.Fake<IMoleculeAmount>().WithName("sp1"));
         _liver.Add(A.Fake<IMoleculeAmount>().WithName("sp3"));
         _rootContainer.Add(_liver);

         //SubLiver has all species
         _subLiver = new Container().WithName("SubLiver").WithMode(ContainerMode.Physical);
         _subLiver.Add(A.Fake<IMoleculeAmount>().WithName("sp1"));
         _subLiver.Add(A.Fake<IMoleculeAmount>().WithName("sp2"));
         _subLiver.Add(A.Fake<IMoleculeAmount>().WithName("sp3"));
         _subLiver.Add(A.Fake<IMoleculeAmount>().WithName("modifier"));
         _liver.Add(_subLiver);

         //kidney has only one species
         _kidney = new Container().WithName("Kidney").WithMode(ContainerMode.Physical);
         _kidney.Add(A.Fake<IMoleculeAmount>().WithName("sp1"));
         _rootContainer.Add(_kidney);

         //SubKidney has only modifier
         _subKidney = new Container().WithName("SubKidney").WithMode(ContainerMode.Physical);
         _subKidney.Add(A.Fake<IMoleculeAmount>().WithName("sp1"));
         _subKidney.Add(A.Fake<IMoleculeAmount>().WithName("sp2"));
         _subKidney.Add(A.Fake<IMoleculeAmount>().WithName("sp3"));
         _rootContainer.Add(_subKidney);

         //help container has all species but is logical
         _helpContainer = new Container().WithName("Helper").WithMode(ContainerMode.Logical);
         _helpContainer.Add(A.Fake<IMoleculeAmount>().WithName("sp1"));
         _helpContainer.Add(A.Fake<IMoleculeAmount>().WithName("sp2"));
         _helpContainer.Add(A.Fake<IMoleculeAmount>().WithName("sp3"));
         _helpContainer.Add(A.Fake<IMoleculeAmount>().WithName("modifier"));
         _rootContainer.Add(_helpContainer);
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }

      [Observation]
      public void should_create_the_reaction_in_all_physical_containers_containing_all_the_educts_and_products_and_modifier_necessary_for_the_reaction_to_take_place()
      {
         _rootContainer.ContainsName(_reactionBuilder.Name).ShouldBeTrue();
         _subLiver.ContainsName(_reactionBuilder.Name).ShouldBeTrue();
      }

      [Observation]
      public void should_have_updated_the_description_of_the_reaction_in_the_global_container()
      {
         _globalContainer.Description.ShouldBeEqualTo(_reactionBuilder.Description);
      }

      [Observation]
      public void should_not_create_the_reaction_in_any_container_for_which_at_least_one_educt_or_one_product()
      {
         _liver.ContainsName(_reactionBuilder.Name).ShouldBeFalse();
         _kidney.ContainsName(_reactionBuilder.Name).ShouldBeFalse();
      }

      [Observation]
      public void should_not_create_the_reaction_in_any_container_for_which_only_modifier_is_missing()
      {
         _subKidney.ContainsName(_reactionBuilder.Name).ShouldBeFalse();
      }

      [Observation]
      public void should_not_create_the_reaction_in_any_logical_container()
      {
         _helpContainer.ContainsName(_reactionBuilder.Name).ShouldBeFalse();
      }
   }

   internal class When_creating_the_reaction_based_on_a_given_builder_and_no_instance_is_created : concern_for_ReactionCreator
   {
      private IContainer _helpContainer;

      protected override void Context()
      {
         base.Context();

         //help container has all species but is logical
         _helpContainer = new Container().WithName("Helper").WithMode(ContainerMode.Logical);
         _helpContainer.Add(A.Fake<IMoleculeAmount>().WithName("sp1"));
         _helpContainer.Add(A.Fake<IMoleculeAmount>().WithName("sp2"));
         _helpContainer.Add(A.Fake<IMoleculeAmount>().WithName("sp3"));
         _helpContainer.Add(A.Fake<IMoleculeAmount>().WithName("modifier"));
         _rootContainer.Add(_helpContainer);
      }

      [Observation]
      public void should_return_false()
      {
         _result.ShouldBeFalse();
      }

      [Observation]
      public void should_not_create_the_reaction_in_any_logical_container()
      {
         _helpContainer.ContainsName(_reactionBuilder.Name).ShouldBeFalse();
      }

      [Observation]
      public void should_not_create_the_reaction_in_any_container_for_which_at_least_one_educt_or_one_product_or_modifier()
      {
         _rootContainer.ContainsName(_reactionBuilder.Name).ShouldBeFalse();
      }
   }

   internal class When_creating_the_reaction_based_on_a_reaction_builder_with_specific_container_criteria_defined : concern_for_ReactionCreator
   {
      private Container _liver;

      protected override void Context()
      {
         base.Context();
         _reactionBuilder.ContainerCriteria = Create.Criteria(x => x.With("Liver"));

         //root container has all species but does not match the criteria
         _rootContainer.Add(A.Fake<IMoleculeAmount>().WithName("sp1"));
         _rootContainer.Add(A.Fake<IMoleculeAmount>().WithName("sp2"));
         _rootContainer.Add(A.Fake<IMoleculeAmount>().WithName("sp3"));
         _rootContainer.Add(A.Fake<IMoleculeAmount>().WithName("modifier"));

         //Liver has all species and matches the criteria
         _liver = new Container().WithName("Liver").WithMode(ContainerMode.Physical);
         _liver.Add(A.Fake<IMoleculeAmount>().WithName("sp1"));
         _liver.Add(A.Fake<IMoleculeAmount>().WithName("sp2"));
         _liver.Add(A.Fake<IMoleculeAmount>().WithName("sp3"));
         _liver.Add(A.Fake<IMoleculeAmount>().WithName("modifier"));
         _rootContainer.Add(_liver);
      }

      [Observation]
      public void should_create_the_reaction_in_all_container_satisyfing_the_criteria()
      {
         _liver.ContainsName(_reactionBuilder.Name).ShouldBeTrue();
      }

      [Observation]
      public void should_not_create_the_reaction_in_container_not_satisfying_the_rection_criteria()
      {
         _rootContainer.ContainsName(_reactionBuilder.Name).ShouldBeFalse();
      }
   }
}
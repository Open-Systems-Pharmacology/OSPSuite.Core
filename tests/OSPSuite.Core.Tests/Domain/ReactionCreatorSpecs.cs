using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   internal abstract class concern_for_ReactionCreator : ContextSpecification<IReactionCreator>
   {
      protected IReactionBuilderToReactionMapper _reactionMapper;
      protected IKeywordReplacerTask _keywordReplacerTask;
      protected IContainerTask _containerTask;
      protected IParameterBuilderCollectionToParameterCollectionMapper _parameterMapper;
      protected IContainer _rootContainer;
      protected IModel _model;
      protected ReactionBuilder _reactionBuilder;
      protected SimulationConfiguration _simulationConfiguration;
      protected ReactionPartnerBuilder _educt1;
      protected ReactionPartnerBuilder _educt2;
      protected ReactionPartnerBuilder _product1;
      protected IContainer _globalContainer;
      protected Reaction _reaction;
      protected bool _result;
      protected SimulationBuilder _simulationBuilder;
      private ModelConfiguration _modelConfiguration;
      private IEntityTracker _entityTracker;

      protected override void Context()
      {
         _reactionMapper = A.Fake<IReactionBuilderToReactionMapper>();
         _keywordReplacerTask = A.Fake<IKeywordReplacerTask>();
         _containerTask = A.Fake<IContainerTask>();
         _parameterMapper = A.Fake<IParameterBuilderCollectionToParameterCollectionMapper>();
         _entityTracker = A.Fake<IEntityTracker>();
         sut = new ReactionCreator(_reactionMapper, _keywordReplacerTask, _containerTask, _parameterMapper, _entityTracker);

         _model = A.Fake<IModel>();
         _simulationConfiguration = new SimulationConfiguration();
         _simulationBuilder =A.Fake<SimulationBuilder>();
         _reactionBuilder = new ReactionBuilder
         {
            ContainerCriteria = new DescriptorCriteria(),
            Description = "A great description",
            Name = "Reaction"
         };
         _educt1 = new ReactionPartnerBuilder
         {
            MoleculeName = "sp1"
         };
         _educt2 = new ReactionPartnerBuilder
         {
            MoleculeName = "sp2"
         };
         _product1 = new ReactionPartnerBuilder
         {
            MoleculeName = "sp3"
         };
         _reactionBuilder.AddEduct(_educt1);
         _reactionBuilder.AddEduct(_educt2);
         _reactionBuilder.AddProduct(_product1);
         _reactionBuilder.AddModifier("modifier");

         _rootContainer = new Container().WithMode(ContainerMode.Physical);
         _model.Root = _rootContainer;
         _globalContainer = new Container();

         _reaction = new Reaction().WithName(_reactionBuilder.Name);
         A.CallTo(() => _reactionMapper.MapFromLocal(A<ReactionBuilder>._, A<IContainer>._, _simulationBuilder)).Returns(_reaction);
         A.CallTo(() => _containerTask.CreateOrRetrieveSubContainerByName(_rootContainer, _reactionBuilder.Name)).Returns(_globalContainer);

         _modelConfiguration = new ModelConfiguration(_model, _simulationConfiguration, _simulationBuilder);
      }

      protected override void Because()
      {
         _result = sut.CreateReaction(_reactionBuilder, _modelConfiguration);
      }
   }

   internal class When_creating_the_reaction_based_on_a_given_builder_in_a_container_hierarchy_without_global_parameters_in_the_reaction : When_creating_the_reaction_based_on_a_given_builder_in_a_container_hierarchy
   {
      [Observation]
      public void the_global_container_is_created()
      {
         A.CallTo(() => _containerTask.CreateOrRetrieveSubContainerByName(_rootContainer, _reactionBuilder.Name)).MustHaveHappened();
      }
   }

   internal class When_creating_the_reaction_based_on_a_given_builder_in_a_container_hierarchy_with_global_parameters_in_the_reaction : When_creating_the_reaction_based_on_a_given_builder_in_a_container_hierarchy
   {
      private Parameter _globalParameter;

      protected override void Context()
      {
         base.Context();
         _globalParameter = new Parameter
         {
            BuildMode = ParameterBuildMode.Global
         };
         _reactionBuilder.Add(_globalParameter);
      }

      [Observation]
      public void the_global_container_is_not_created()
      {
         A.CallTo(() => _containerTask.CreateOrRetrieveSubContainerByName(_rootContainer, _reactionBuilder.Name)).MustHaveHappened();
      }

      [Observation]
      public void should_have_updated_the_description_of_the_reaction_in_the_global_container()
      {
         _globalContainer.Description.ShouldBeEqualTo(_reactionBuilder.Description);
      }
   }

   internal abstract class When_creating_the_reaction_based_on_a_given_builder_in_a_container_hierarchy : concern_for_ReactionCreator
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
         _rootContainer.Add(new MoleculeAmount().WithName("sp1"));
         _rootContainer.Add(new MoleculeAmount().WithName("sp2"));
         _rootContainer.Add(new MoleculeAmount().WithName("sp3"));
         _rootContainer.Add(new MoleculeAmount().WithName("modifier"));

         //Liver has only two species
         _liver = new Container().WithName("Liver").WithMode(ContainerMode.Physical);
         _liver.Add(new MoleculeAmount().WithName("sp1"));
         _liver.Add(new MoleculeAmount().WithName("sp3"));
         _rootContainer.Add(_liver);

         //SubLiver has all species
         _subLiver = new Container().WithName("SubLiver").WithMode(ContainerMode.Physical);
         _subLiver.Add(new MoleculeAmount().WithName("sp1"));
         _subLiver.Add(new MoleculeAmount().WithName("sp2"));
         _subLiver.Add(new MoleculeAmount().WithName("sp3"));
         _subLiver.Add(new MoleculeAmount().WithName("modifier"));
         _liver.Add(_subLiver);

         //kidney has only one species
         _kidney = new Container().WithName("Kidney").WithMode(ContainerMode.Physical);
         _kidney.Add(new MoleculeAmount().WithName("sp1"));
         _rootContainer.Add(_kidney);

         //SubKidney has only modifier
         _subKidney = new Container().WithName("SubKidney").WithMode(ContainerMode.Physical);
         _subKidney.Add(new MoleculeAmount().WithName("sp1"));
         _subKidney.Add(new MoleculeAmount().WithName("sp2"));
         _subKidney.Add(new MoleculeAmount().WithName("sp3"));
         _rootContainer.Add(_subKidney);

         //help container has all species but is logical
         _helpContainer = new Container().WithName("Helper").WithMode(ContainerMode.Logical);
         _helpContainer.Add(new MoleculeAmount().WithName("sp1"));
         _helpContainer.Add(new MoleculeAmount().WithName("sp2"));
         _helpContainer.Add(new MoleculeAmount().WithName("sp3"));
         _helpContainer.Add(new MoleculeAmount().WithName("modifier"));
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
         _helpContainer.Add(new MoleculeAmount().WithName("sp1"));
         _helpContainer.Add(new MoleculeAmount().WithName("sp2"));
         _helpContainer.Add(new MoleculeAmount().WithName("sp3"));
         _helpContainer.Add(new MoleculeAmount().WithName("modifier"));
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
         _rootContainer.Add(new MoleculeAmount().WithName("sp1"));
         _rootContainer.Add(new MoleculeAmount().WithName("sp2"));
         _rootContainer.Add(new MoleculeAmount().WithName("sp3"));
         _rootContainer.Add(new MoleculeAmount().WithName("modifier"));

         //Liver has all species and matches the criteria
         _liver = new Container().WithName("Liver").WithMode(ContainerMode.Physical);
         _liver.Add(new MoleculeAmount().WithName("sp1"));
         _liver.Add(new MoleculeAmount().WithName("sp2"));
         _liver.Add(new MoleculeAmount().WithName("sp3"));
         _liver.Add(new MoleculeAmount().WithName("modifier"));
         _rootContainer.Add(_liver);
      }

      [Observation]
      public void should_create_the_reaction_in_all_container_satisfying_the_criteria()
      {
         _liver.ContainsName(_reactionBuilder.Name).ShouldBeTrue();
      }

      [Observation]
      public void should_not_create_the_reaction_in_container_not_satisfying_the_reaction_criteria()
      {
         _rootContainer.ContainsName(_reactionBuilder.Name).ShouldBeFalse();
      }
   }
}
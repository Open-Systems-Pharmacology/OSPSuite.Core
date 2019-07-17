using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   internal abstract class concern_for_MoleculePropertiesContainerTask : ContextSpecification<IMoleculePropertiesContainerTask>
   {
      protected IParameterBuilderCollectionToParameterCollectionMapper _parameterCollectionMapper;
      protected IContainerTask _containerTask;
      protected IContainer _rootContainer;
      protected IKeywordReplacerTask _keywordReplacer;
      protected IBuildConfiguration _buildConfiguration;

      protected override void Context()
      {
         _containerTask = A.Fake<IContainerTask>();
         _buildConfiguration = A.Fake<IBuildConfiguration>();
         _rootContainer = new Container();
         _parameterCollectionMapper = A.Fake<IParameterBuilderCollectionToParameterCollectionMapper>();
         _keywordReplacer = A.Fake<IKeywordReplacerTask>();
         sut = new MoleculePropertiesContainerTask(_containerTask, _parameterCollectionMapper, _keywordReplacer);
      }
   }

   internal class When_retrieving_the_neighborhood_container_for_a_molecule_that_was_not_already_created : concern_for_MoleculePropertiesContainerTask
   {
      private Neighborhood _neighborhood;

      protected override void Context()
      {
         base.Context();
         _neighborhood = new Neighborhood().WithName("Neighborhood");
      }

      [Observation]
      public void should_create_a_molecule_container_into_the_neighborhood_and_return_the_newly_created_container()
      {
         The.Action(() => sut.NeighborhoodMoleculeContainerFor(_neighborhood, "a non existing molecule")).ShouldThrowAn<MissingMoleculeContainerException>();
      }
   }

   internal class When_creating_the_global_molecule_container_for_a_given_molecule_builder : concern_for_MoleculePropertiesContainerTask
   {
      private IContainer _result;
      private IMoleculeBuilder _moleculeBuilder;
      private IContainer _moleculeContainer;
      private IParameter _para1;
      private IParameter _para2;
      private IContainer _globalMoleculeDependentProperties;
      private IParameter _globalMoleculeDepParam1, _globalMoleculeDepParam2;

      protected override void Context()
      {
         base.Context();
         _para1 = new Parameter().WithName("Para1");
         _para2 = new Parameter().WithName("Para2");
         _moleculeBuilder = new MoleculeBuilder().WithName("tralal");
         _moleculeBuilder.IsFloating = true;
         _moleculeBuilder.IsXenobiotic = true;
         _moleculeBuilder.Add(_para1);
         _moleculeBuilder.Add(_para2);
         _moleculeContainer = A.Fake<IContainer>();
         A.CallTo(() => _containerTask.CreateOrRetrieveSubContainerByName(_rootContainer, _moleculeBuilder.Name)).Returns(_moleculeContainer);

         _globalMoleculeDependentProperties = new Container();

         A.CallTo(() => _parameterCollectionMapper.MapGlobalOrPropertyFrom(_moleculeBuilder, _buildConfiguration))
            .Returns(new[] {_para1, _para2});


      
         _globalMoleculeDepParam1 = new Parameter().WithName("GMDP1");
         _globalMoleculeDepParam2 = new Parameter().WithName("GMDP2");

         _globalMoleculeDependentProperties.Add(_globalMoleculeDepParam1);
         _globalMoleculeDependentProperties.Add(_globalMoleculeDepParam2);

         A.CallTo(() => _parameterCollectionMapper.MapAllFrom(_globalMoleculeDependentProperties, _buildConfiguration))
            .Returns(new[] {_globalMoleculeDepParam1, _globalMoleculeDepParam2});

         _buildConfiguration.SpatialStructure = A.Fake<ISpatialStructure>();
         A.CallTo(() => _buildConfiguration.SpatialStructure.GlobalMoleculeDependentProperties).Returns(_globalMoleculeDependentProperties);
      }

      protected override void Because()
      {
         _result = sut.CreateGlobalMoleculeContainerFor(_rootContainer, _moleculeBuilder, _buildConfiguration);
      }

      [Observation]
      public void should_create_a_molecule_container_into_the_global_molecule_container_and_return_the_newly_created_container()
      {
         _result.ShouldBeEqualTo(_moleculeContainer);
      }

      [Observation]
      public void should_add_all_global_and_property_parameters_into_the_molecule_container_but_not_the_local_parameters()
      {
         A.CallTo(() => _moleculeContainer.Add(_para1)).MustHaveHappened();
         A.CallTo(() => _moleculeContainer.Add(_para2)).MustHaveHappened();
      }

      [Observation]
      public void should_add_all_global_molecule_dependent_parameters_to_molecule_container()
      {
         A.CallTo(() => _moleculeContainer.Add(_globalMoleculeDepParam1)).MustHaveHappened();
         A.CallTo(() => _moleculeContainer.Add(_globalMoleculeDepParam2)).MustHaveHappened();
      }
   }

   internal class When_creating_the_global_molecule_container_for_a_given_molecule_builder_with_interaction_containers : concern_for_MoleculePropertiesContainerTask
   {
      private IContainer _result;
      private IMoleculeBuilder _moleculeBuilder;
      private IParameter _para;

      protected override void Context()
      {
         base.Context();
         _para = new Parameter().WithName("Para1");
         _moleculeBuilder = new MoleculeBuilder().WithName("Molecule");
         var interactionContainer = new InteractionContainer().WithName("Interaction");
         _moleculeBuilder.AddInteractionContainer(interactionContainer);

         A.CallTo(() => _containerTask.CreateOrRetrieveSubContainerByName(A<IContainer>._, A<string>._))
            .ReturnsLazily(x =>
            {
               var parent = x.GetArgument<IContainer>(0);
               var container = new Container().WithName(x.GetArgument<string>(1));
               parent.Add(container);
               return container;
            });

         A.CallTo(_parameterCollectionMapper).WithReturnType<IEnumerable<IParameter>>().Returns(new[] {_para});
      }

      protected override void Because()
      {
         _result = sut.CreateGlobalMoleculeContainerFor(_rootContainer, _moleculeBuilder, _buildConfiguration);
      }

      [Observation]
      public void should_create_a_container_for_the_interaction_container()
      {
         _result.EntityAt<IContainer>("Interaction").ShouldNotBeNull();
      }

      [Observation]
      public void should_add_the_interaction_parameters()
      {
         _result.EntityAt<IParameter>("Interaction", _para.Name).ShouldNotBeNull();
      }
   }

   internal class When_creating_the_global_molecule_container_for_a_given_xenobiotic_molecule_builder : concern_for_MoleculePropertiesContainerTask
   {
      private IContainer _result;
      private IMoleculeBuilder _moleculeBuilder;
      private IContainer _moleculeContainer;
      private IParameter _para1;
      private IContainer _globalMoleculeDependentProperties;
      private IParameter _globalMoleculeDepParam1;

      protected override void Context()
      {
         base.Context();
         _moleculeBuilder = A.Fake<IMoleculeBuilder>().WithName("tralal");
         _moleculeBuilder.IsFloating = true;
         _moleculeBuilder.IsXenobiotic = false;
         _moleculeContainer = A.Fake<IContainer>();
         _para1 = new Parameter().WithName("Para1");
         A.CallTo(() => _containerTask.CreateOrRetrieveSubContainerByName(_rootContainer, _moleculeBuilder.Name)).Returns(_moleculeContainer);

         A.CallTo(() => _parameterCollectionMapper.MapGlobalOrPropertyFrom(_moleculeBuilder, _buildConfiguration))
            .Returns(new[] {_para1});

         _globalMoleculeDependentProperties = new Container();

         _globalMoleculeDepParam1 = new Parameter().WithName("GMDP1");

         _globalMoleculeDependentProperties.Add(_globalMoleculeDepParam1);
         A.CallTo(() => _parameterCollectionMapper.MapGlobalOrPropertyFrom(_globalMoleculeDependentProperties, _buildConfiguration))
            .Returns(new[] {_globalMoleculeDepParam1});

         _buildConfiguration.SpatialStructure = A.Fake<ISpatialStructure>();
         A.CallTo(() => _buildConfiguration.SpatialStructure.GlobalMoleculeDependentProperties).Returns(_globalMoleculeDependentProperties);
      }

      protected override void Because()
      {
         _result = sut.CreateGlobalMoleculeContainerFor(_rootContainer, _moleculeBuilder, _buildConfiguration);
      }

      [Observation]
      public void should_create_a_molecule_container_into_the_global_molecule_container_and_return_the_newly_created_container()
      {
         _result.ShouldBeEqualTo(_moleculeContainer);
      }

      [Observation]
      public void should_add_all_global_and_property_parameters_into_the_molecule_container_but_not_the_local_parameters()
      {
         A.CallTo(() => _moleculeContainer.Add(_para1)).MustHaveHappened();
      }

      [Observation]
      public void should_not_add_all_global_molecule_dependent_parameters_to_molecule_container()
      {
         A.CallTo(() => _moleculeContainer.Add(_globalMoleculeDepParam1)).MustNotHaveHappened();
      }
   }
}
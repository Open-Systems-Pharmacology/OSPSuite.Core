using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   internal abstract class concern_for_ObserverBuilderTask : ContextSpecification<IObserverBuilderTask>
   {
      protected IObserverBuilderToObserverMapper _observerMapper;
      protected SimulationConfiguration _simulationConfiguration;
      private IModel _model;
      protected ObserverBuildingBlock _observerBuildingBlock;
      protected IContainer _rootContainer;
      protected IContainer _rootNeighborhood;
      protected IContainer _organism;
      protected string _molecule1Name = "molecule1";
      protected string _molecule2Name = "molecule2";
      protected IKeywordReplacerTask _keywordReplacerTask;
      protected IContainerTask _containerTask;
      private MoleculeBuilder _molecule1;
      private MoleculeBuilder _molecule2;
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private InitialConditionsBuildingBlock _initialConditions;
      protected SimulationBuilder _simulationBuilder;
      private IEntityTracker _entityTracker;
      private IReactionMerger _reactionMerger;

      protected override void Context()
      {
         _reactionMerger = A.Fake<IReactionMerger>();
         _observerMapper = A.Fake<IObserverBuilderToObserverMapper>();
         _entityTracker= A.Fake<IEntityTracker>();
         _observerBuildingBlock = new ObserverBuildingBlock();
         _molecule1 = new MoleculeBuilder().WithName(_molecule1Name);
         _molecule2 = new MoleculeBuilder().WithName(_molecule2Name);
         _moleculeBuildingBlock = new MoleculeBuildingBlock
         {
            _molecule1, _molecule2
         };
         _initialConditions = new InitialConditionsBuildingBlock
         {
            new InitialCondition {Path = new ObjectPath("Liver", _molecule1Name), IsPresent = true},
            new InitialCondition {Path = new ObjectPath("Liver", _molecule2Name), IsPresent = true},
         };

         var module = new Module
         {
            _observerBuildingBlock,
            _moleculeBuildingBlock,
            _initialConditions
         };
         _simulationConfiguration = new SimulationConfiguration();
         _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module));


         _model = A.Fake<IModel>();
         _rootContainer = new Container();

         _organism = new Container().WithName("Organism");
         _organism.AddTag(new Tag("Organism"));
         _rootContainer.Add(_organism);

         _rootNeighborhood = new Container();
         _model.Root = _rootContainer;
         _model.Neighborhoods = _rootNeighborhood;
         _keywordReplacerTask = A.Fake<IKeywordReplacerTask>();
         _containerTask = A.Fake<IContainerTask>();
         sut = new ObserverBuilderTask(_observerMapper, _containerTask, _keywordReplacerTask,_entityTracker);
      }

      protected override void Because()
      {
         _simulationBuilder = new SimulationBuilder(_simulationConfiguration, _reactionMerger);
         sut.CreateObservers(new ModelConfiguration(_model, _simulationConfiguration, _simulationBuilder));
      }
   }

   internal class When_creating_the_container_observers_in_a_model_based_on_the_build_configuration : concern_for_ObserverBuilderTask
   {
      private ObserverBuilder _obs1;
      private ObserverBuilder _obs2;
      private IContainer _molecule2Amount;
      private IContainer _molecule1Amount;
      private DescriptorCriteria _observerCriteria;

      protected override void Context()
      {
         base.Context();

         _observerCriteria = Create.Criteria(x => x.With("Organism"));

         _obs1 = new ContainerObserverBuilder().WithName("OBS1");
         _obs1.MoleculeList.ForAll = true;
         _obs1.ContainerCriteria = _observerCriteria;
         _obs2 = new ContainerObserverBuilder().WithName("OBS2");
         _obs2.MoleculeList.ForAll = false;
         _obs2.AddMoleculeName(_molecule2Name);
         _obs2.ContainerCriteria = _observerCriteria;

         _molecule1Amount = new MoleculeAmount().WithName(_molecule1Name);
         _molecule2Amount = new MoleculeAmount().WithName(_molecule2Name);
         _organism.Add(_molecule2Amount);
         _organism.Add(_molecule1Amount);
         _observerBuildingBlock.Add(_obs1);
         _observerBuildingBlock.Add(_obs2);
         A.CallTo(() => _observerMapper.MapFrom(_obs1, A<SimulationBuilder>._)).Returns(new Observer().WithName(_obs1.Name));
         A.CallTo(() => _observerMapper.MapFrom(_obs2, A<SimulationBuilder>._)).Returns(new Observer().WithName(_obs2.Name));
         A.CallTo(() => _containerTask.CreateOrRetrieveSubContainerByName(_organism, _molecule1Name)).Returns(_molecule1Amount);
         A.CallTo(() => _containerTask.CreateOrRetrieveSubContainerByName(_organism, _molecule2Name)).Returns(_molecule2Amount);
      }

      [Observation]
      public void should_create_one_observer_for_each_molecule_present_in_the_model_if_the_observer_is_for_all_molecules()
      {
         //one for each observer
         _molecule2Amount.Children.Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_create_one_observer_only_for_the_molecule_for_which_the_observer_was_defined_if_the_observer_is_not_for_all()
      {
         //only for first observer "_obs1"
         _molecule1Amount.Children.Count().ShouldBeEqualTo(1);
      }
   }

   internal class When_creating_the_amount_observer_in_a_model_based_on_the_build_configuration : concern_for_ObserverBuilderTask
   {
      private AmountObserverBuilder _obs1;
      private AmountObserverBuilder _obs2;
      private MoleculeAmount _molecule1Container1;
      private MoleculeAmount _molecule2Container1;
      private MoleculeAmount _molecule1Container2;
      private MoleculeAmount _molecule2Container2;

      protected override void Context()
      {
         base.Context();
         _obs1 = new AmountObserverBuilder().WithName("OBS1");
         _obs1.MoleculeList.ForAll = true;
         _obs2 = new AmountObserverBuilder().WithName("OBS2");
         _obs2.MoleculeList.ForAll = false;
         _obs2.AddMoleculeName(_molecule2Name);
         _observerBuildingBlock.Add(_obs1);
         _observerBuildingBlock.Add(_obs2);
         A.CallTo(() => _observerMapper.MapFrom(_obs1, A<SimulationBuilder>._)).ReturnsLazily(x => new Observer().WithName(_obs1.Name));
         A.CallTo(() => _observerMapper.MapFrom(_obs2, A<SimulationBuilder>._)).ReturnsLazily(x => new Observer().WithName(_obs2.Name));
         _molecule1Container1 = new MoleculeAmount().WithName(_molecule1Name).WithQuantityType(QuantityType.Drug);
         _molecule2Container1 = new MoleculeAmount().WithName(_molecule2Name).WithQuantityType(QuantityType.Enzyme);
         _molecule1Container2 = new MoleculeAmount().WithName(_molecule1Name).WithQuantityType(QuantityType.Drug);
         _molecule2Container2 = new MoleculeAmount().WithName(_molecule2Name).WithQuantityType(QuantityType.Enzyme);
         var container1 = new Container().WithName("Container1");
         container1.AddTag("OBS");
         container1.AddTag("OBS1");
         var container2 = new Container().WithName("Container2");
         container2.AddTag("OBS");
         container1.Add(_molecule1Container1);
         container1.Add(_molecule2Container1);
         container2.Add(_molecule1Container2);
         container2.Add(_molecule2Container2);
         _rootContainer.Add(container1);
         _rootContainer.Add(container2);
         _obs1.ContainerCriteria = Create.Criteria(x => x.With("OBS"));
         _obs2.ContainerCriteria = Create.Criteria(x => x.With("OBS1"));
      }

      [Observation]
      public void should_create_one_observer_under_each_local_molecule_container_if_the_physical_container_satisfies_the_observer_criteria_and_if_the_observer_is_for_all()
      {
         //only obs 1
         _molecule1Container1.Children.Count().ShouldBeEqualTo(1);
         _molecule1Container2.Children.Count().ShouldBeEqualTo(1);
         _molecule2Container2.Children.Count().ShouldBeEqualTo(1);

         _molecule1Container1.Children.First().DowncastTo<Observer>().QuantityType.Is(QuantityType.Drug).ShouldBeTrue();
         _molecule1Container2.Children.First().DowncastTo<Observer>().QuantityType.Is(QuantityType.Drug).ShouldBeTrue();
         _molecule2Container2.Children.First().DowncastTo<Observer>().QuantityType.Is(QuantityType.Enzyme).ShouldBeTrue();
      }

      [Observation]
      public void should_create_one_observer_only_for_the_molecule_for_which_the_observer_was_defined_if_the_physical_container_satisfies_the_observer_criteria_and_if_the_observer_is_not_for_all()
      {
         //one for observer 1 and and observer 2 since container 1 is the only one satisfying observer 2 conditions
         _molecule2Container1.Children.Count().ShouldBeEqualTo(2);
      }
   }
}
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Creates observers in model using observers-building block configuration
   /// </summary>
   internal interface IObserverBuilderTask
   {
      /// <summary>
      ///    Adds observers defined by simulationConfiguration to the given model
      /// </summary>
      void CreateObservers(ModelConfiguration modelConfiguration);
   }

   internal class ObserverBuilderTask : IObserverBuilderTask
   {
      private readonly IObserverBuilderToObserverMapper _observerMapper;
      private readonly IContainerTask _containerTask;
      private readonly IKeywordReplacerTask _keywordReplacerTask;

      //cache only used to speed up task
      private EntityDescriptorMapList<IContainer> _allContainerDescriptors;
      private SimulationBuilder _simulationBuilder;
      private readonly IEntityTracker _entityTracker;

      public ObserverBuilderTask(
         IObserverBuilderToObserverMapper observerMapper,
         IContainerTask containerTask,
         IKeywordReplacerTask keywordReplacerTask, IEntityTracker entityTracker)
      {
         _observerMapper = observerMapper;
         _containerTask = containerTask;
         _keywordReplacerTask = keywordReplacerTask;
         _entityTracker = entityTracker;
      }

      public void CreateObservers(ModelConfiguration modelConfiguration)
      {
         var (model, simulationBuilder, replacementContext) = modelConfiguration;
         _allContainerDescriptors = model.Root.GetAllChildren<IContainer>().ToEntityDescriptorMapList();
         _simulationBuilder = simulationBuilder;
         var observers = simulationBuilder.Observers;
         var presentMolecules = simulationBuilder.AllPresentMolecules().ToList();
         try
         {
            foreach (var observerBuilder in observers.OfType<AmountObserverBuilder>())
               createAmountObserver(observerBuilder, presentMolecules, replacementContext, simulationBuilder);


            foreach (var observerBuilder in observers.OfType<ContainerObserverBuilder>())
               createContainerObserver(observerBuilder, presentMolecules, replacementContext, simulationBuilder);
         }
         finally
         {
            _allContainerDescriptors = null;
            _simulationBuilder = null;
         }
      }

      /// <summary>
      ///    Retrieves molecules for which the given observer can be created
      /// </summary>
      private IEnumerable<MoleculeBuilder> moleculeBuildersValidFor(MoleculeList moleculeList, IEnumerable<MoleculeBuilder> allMolecules)
      {
         if (moleculeList.ForAll)
            return allMolecules.Where(molecule => !moleculeList.MoleculeNamesToExclude.Contains(molecule.Name));

         return allMolecules.Where(molecule => moleculeList.MoleculeNames.Contains(molecule.Name));
      }

      /// <summary>
      ///    Creates amount specific observers-stored under molecules amount
      ///    in the spatial structure of the model.
      ///    Typical example: "Concentration"-Observer (M/V)
      /// </summary>
      private void createAmountObserver(AmountObserverBuilder observerBuilder, IEnumerable<MoleculeBuilder> presentMolecules, ReplacementContext replacementContext, SimulationBuilder simulationBuilder)
      {
         var moleculeNamesForObserver = moleculeBuildersValidFor(simulationBuilder.MoleculeListFor(observerBuilder), presentMolecules)
            .Select(x => x.Name).ToList();

         foreach (var container in _allContainerDescriptors.AllSatisfiedBy(observerBuilder.ContainerCriteria))
         {
            var amountsForObserver = container.GetChildren<MoleculeAmount>(ma => moleculeNamesForObserver.Contains(ma.Name));

            foreach (var amount in amountsForObserver)
            {
               var observer = addObserverInContainer(observerBuilder, amount, amount.QuantityType);
               _keywordReplacerTask.ReplaceIn(observer, amount.Name, replacementContext);
            }
         }
      }

      /// <summary>
      ///    Creates (molecule-specific) observers, stored under
      ///    molecule container of the corresponding molecule in the spatial structure
      ///    of the model.
      ///    Typical example is average drug concentration in an organ
      /// </summary>
      private void createContainerObserver(ContainerObserverBuilder observerBuilder, IEnumerable<MoleculeBuilder> presentMolecules, ReplacementContext replacementContext, SimulationBuilder simulationBuilder)
      {
         var moleculeBuildersForObserver = moleculeBuildersValidFor(simulationBuilder.MoleculeListFor(observerBuilder), presentMolecules).ToList();
         //retrieve a list here to avoid endless loop if observers criteria is not well defined
         foreach (var container in _allContainerDescriptors.AllSatisfiedBy(observerBuilder.ContainerCriteria))
         {
            foreach (var moleculeBuilder in moleculeBuildersForObserver)
            {
               //get the molecule container defined for the molecule name
               var moleculeContainer = container.GetSingleChildByName<IContainer>(moleculeBuilder.Name);
               if (moleculeContainer == null)
               {
                  //should only happen for a logical container.
                  moleculeContainer = _containerTask.CreateOrRetrieveSubContainerByName(container, moleculeBuilder.Name).WithContainerType(ContainerType.Molecule);
                  _entityTracker.Track(moleculeContainer, observerBuilder, _simulationBuilder);
               }

               var observer = addObserverInContainer(observerBuilder, moleculeContainer, moleculeBuilder.QuantityType);
               _keywordReplacerTask.ReplaceIn(observer, moleculeBuilder.Name, replacementContext);
            }
         }
      }

      private Observer addObserverInContainer(ObserverBuilder observerBuilder, IContainer observerContainer, QuantityType moleculeType)
      {
         var observer = _observerMapper.MapFrom(observerBuilder, _simulationBuilder);
         observer.QuantityType = QuantityType.Observer | moleculeType;
         observerContainer.Add(observer);
         return observer;
      }
   }
}
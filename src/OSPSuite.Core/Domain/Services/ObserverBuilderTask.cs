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
      ///    Adds observers defined by buildConfiguration to the given model
      /// </summary>
      /// <param name="buildConfiguration">the build configuration</param>
      /// <param name="model">the model where the observers should be defined</param>
      void CreateObservers(IBuildConfiguration buildConfiguration, IModel model);
   }

   internal class ObserverBuilderTask : IObserverBuilderTask
   {
      private readonly IObserverBuilderToObserverMapper _observerMapper;
      private readonly IContainerTask _containerTask;
      private readonly IKeywordReplacerTask _keywordReplacerTask;

      //cache only used to speed up task
      private EntityDescriptorMapList<IContainer> _allContainerDescriptors;
      private IBuildConfiguration _buildConfiguration;

      public ObserverBuilderTask(
         IObserverBuilderToObserverMapper observerMapper, 
         IContainerTask containerTask, 
         IKeywordReplacerTask keywordReplacerTask)
      {
         _observerMapper = observerMapper;
         _containerTask = containerTask;
         _keywordReplacerTask = keywordReplacerTask;
      }

      public void CreateObservers(IBuildConfiguration buildConfiguration, IModel model)
      {
         _allContainerDescriptors = model.Root.GetAllChildren<IContainer>().ToEntityDescriptorMapList();
         _buildConfiguration = buildConfiguration;
         var observers = buildConfiguration.Observers;
         var presentMolecules = buildConfiguration.AllPresentMolecules().ToList();
         try
         {
            foreach (var observerBuilder in observers.AmountObserverBuilders)
               createAmountObserver(observerBuilder, model, presentMolecules);


            foreach (var observerBuilder in observers.ContainerObserverBuilders)
               createContainerObserver(observerBuilder, model, presentMolecules);
         }
         finally
         {
              _allContainerDescriptors = null;
            _buildConfiguration = null;
         }
      }

      /// <summary>
      ///    Retrieves molecules for which the given observer can be created
      /// </summary>
      private IEnumerable<IMoleculeBuilder> moleculeBuildersValidFor(MoleculeList moleculeList, IEnumerable<IMoleculeBuilder> allMolecules)
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
      private void createAmountObserver(IAmountObserverBuilder observerBuilder, IModel model, IEnumerable<IMoleculeBuilder> presentMolecules)
      {
         var moleculeNamesForObserver = moleculeBuildersValidFor(observerBuilder.MoleculeList, presentMolecules)
            .Select(x => x.Name).ToList();

         foreach (var container in _allContainerDescriptors.AllSatisfiedBy(observerBuilder.ContainerCriteria))
         {
            var amountsForObserver = container.GetChildren<IMoleculeAmount>(ma => moleculeNamesForObserver.Contains(ma.Name));

            foreach (var amount in amountsForObserver)
            {
               var observer = addObserverInContainer(observerBuilder, amount, amount.QuantityType);
               _keywordReplacerTask.ReplaceIn(observer, model.Root, amount.Name);
            }
         }
      }

      /// <summary>
      ///    Creates (molecule-specific) observers, stored under
      ///    molecule container of the corresponding molecule in the spatial structure
      ///    of the model.
      ///    Typical example is average drug concentration in an organ
      /// </summary>
      private void createContainerObserver(IContainerObserverBuilder observerBuilder, IModel model, IEnumerable<IMoleculeBuilder> presentMolecules)
      {
         var moleculeBuildersForObserver = moleculeBuildersValidFor(observerBuilder.MoleculeList, presentMolecules).ToList();
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
                  _buildConfiguration.AddBuilderReference(moleculeContainer, observerBuilder);
               }
               var observer = addObserverInContainer(observerBuilder, moleculeContainer, moleculeBuilder.QuantityType);
               _keywordReplacerTask.ReplaceIn(observer, model.Root, moleculeBuilder.Name);
            }
         }
      }

      private IObserver addObserverInContainer(IObserverBuilder observerBuilder, IContainer observerContainer, QuantityType moleculeType)
      {
         var observer = _observerMapper.MapFrom(observerBuilder, _buildConfiguration);
         observer.QuantityType = QuantityType.Observer | moleculeType;
         observerContainer.Add(observer);
         return observer;
      }
   }
}
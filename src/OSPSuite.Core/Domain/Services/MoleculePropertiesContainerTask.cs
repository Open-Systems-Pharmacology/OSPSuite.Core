using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;

namespace OSPSuite.Core.Domain.Services
{
   internal interface IMoleculePropertiesContainerTask
   {
      /// <summary>
      ///    Retrieves the sub container in the neighborhood whose name is equal to the name of the given molecule.
      /// </summary>
      /// <exception cref="MissingMoleculeContainerException">
      ///    is thrown if the container does not exist in the root for the molecule
      /// </exception>
      IContainer NeighborhoodMoleculeContainerFor(Neighborhood neighborhood, string moleculeName);

      /// <summary>
      ///    Creates the global molecule container for the molecule and add parameters with the build modes other than "Local"
      ///    into the created molecule container
      /// </summary>
      IContainer CreateGlobalMoleculeContainerFor(MoleculeBuilder moleculeBuilder, ModelConfiguration modelConfiguration);

      /// <summary>
      ///    Returns (and creates if not already there) the sub container for the transport process named
      ///    <paramref name="transportName" />
      ///    in the <paramref name="neighborhood" /> for the transporter <paramref name="transporterMolecule" />
      /// </summary>
      IContainer NeighborhoodMoleculeTransportContainerFor(Neighborhood neighborhood, string transportedMoleculeName, TransporterMoleculeContainer transporterMolecule, string transportName, SimulationBuilder simulationBuilder);
   }

   internal class MoleculePropertiesContainerTask : IMoleculePropertiesContainerTask
   {
      private readonly IContainerTask _containerTask;
      private readonly IParameterBuilderCollectionToParameterCollectionMapper _parameterCollectionMapper;
      private readonly IKeywordReplacerTask _keywordReplacer;
      private readonly IObjectTracker _objectTracker;

      public MoleculePropertiesContainerTask(
         IContainerTask containerTask,
         IParameterBuilderCollectionToParameterCollectionMapper parameterCollectionMapper,
         IKeywordReplacerTask keywordReplacer, IObjectTracker objectTracker)
      {
         _containerTask = containerTask;
         _parameterCollectionMapper = parameterCollectionMapper;
         _keywordReplacer = keywordReplacer;
         _objectTracker = objectTracker;
      }

      public IContainer NeighborhoodMoleculeContainerFor(Neighborhood neighborhood, string moleculeName)
      {
         var moleculeContainer = neighborhood.GetSingleChildByName<IContainer>(moleculeName);
         if (moleculeContainer == null)
            throw new MissingMoleculeContainerException(moleculeName);

         return moleculeContainer;
      }

      public IContainer CreateGlobalMoleculeContainerFor(MoleculeBuilder moleculeBuilder, ModelConfiguration modelConfiguration)
      {
         var (model, simulationBuilder, replacementContext) = modelConfiguration;
         var globalMoleculeContainer = addContainerUnder(model.Root, moleculeBuilder, moleculeBuilder.Name, simulationBuilder)
            .WithContainerType(ContainerType.Molecule);

         //this is the container that will be used to create all the local molecule containers
         var spatialStructureGlobalMoleculeContainer = model.Root.Container(Constants.MOLECULE_PROPERTIES);
                  
         //Add global molecule dependent parameters
         if (moleculeBuilder.IsFloatingXenobiotic && spatialStructureGlobalMoleculeContainer != null)
            globalMoleculeContainer.AddChildren(addAllParametersFrom(spatialStructureGlobalMoleculeContainer, simulationBuilder));

         //Only non local parameters from the molecule are added to the global container 
         //Local parameters will be filled in elsewhere (by the MoleculeAmount-Mapper)
         globalMoleculeContainer.AddChildren(allGlobalOrPropertyParametersFrom(moleculeBuilder, simulationBuilder));

         foreach (var transporterMoleculeContainer in moleculeBuilder.TransporterMoleculeContainerCollection)
         {
            addContainerWithParametersUnder(globalMoleculeContainer, transporterMoleculeContainer, transporterMoleculeContainer.TransportName, simulationBuilder);
         }

         foreach (var interactionContainer in moleculeBuilder.InteractionContainerCollection)
         {
            addContainerWithParametersUnder(globalMoleculeContainer, interactionContainer, interactionContainer.Name, simulationBuilder);
         }

         _keywordReplacer.ReplaceIn(globalMoleculeContainer, moleculeBuilder.Name, replacementContext);

         return globalMoleculeContainer;
      }

      private IContainer addContainerUnder(IContainer parentContainer, IContainer templateContainer, string newContainerName, SimulationBuilder simulationBuilder)
      {
         var instanceContainer = _containerTask.CreateOrRetrieveSubContainerByName(parentContainer, newContainerName);
         simulationBuilder.AddBuilderReference(instanceContainer, templateContainer);
         _objectTracker.TrackObject(instanceContainer, templateContainer, simulationBuilder);
         instanceContainer.Icon = templateContainer.Icon;
         instanceContainer.Description = templateContainer.Description;
         return instanceContainer;
      }

      private IContainer addContainerWithParametersUnder(IContainer parentContainer, IContainer templateContainer, string newContainerName, SimulationBuilder simulationBuilder)
      {
         return addContainerUnder(parentContainer, templateContainer, newContainerName, simulationBuilder)
            .WithChildren(allGlobalOrPropertyParametersFrom(templateContainer, simulationBuilder));
      }

      private IEnumerable<IParameter> allGlobalOrPropertyParametersFrom(IContainer container, SimulationBuilder simulationBuilder)
      {
         return _parameterCollectionMapper.MapGlobalOrPropertyFrom(container, simulationBuilder);
      }

      private IEnumerable<IParameter> addAllParametersFrom(IContainer container, SimulationBuilder simulationBuilder)
      {
         return _parameterCollectionMapper.MapAllFrom(container, simulationBuilder);
      }

      private IEnumerable<IParameter> allLocalParametersFrom(IContainer container, SimulationBuilder simulationBuilder)
      {
         return _parameterCollectionMapper.MapLocalFrom(container, simulationBuilder);
      }

      public IContainer NeighborhoodMoleculeTransportContainerFor(Neighborhood neighborhood, string transportedMoleculeName, TransporterMoleculeContainer transporterMolecule, string transportName, SimulationBuilder simulationBuilder)
      {
         var moleculeContainer = NeighborhoodMoleculeContainerFor(neighborhood, transportedMoleculeName);
         var transportContainer = moleculeContainer.EntityAt<IContainer>(transporterMolecule.TransportName);
         if (transportContainer != null)
            return transportContainer;

         return _containerTask.CreateOrRetrieveSubContainerByName(moleculeContainer, transporterMolecule.TransportName)
            .WithChildren(allLocalParametersFrom(transporterMolecule, simulationBuilder));
      }
   }
}
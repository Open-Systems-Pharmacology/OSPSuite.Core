using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;

namespace OSPSuite.Core.Domain.Services
{
   public interface IMoleculePropertiesContainerTask
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
      IContainer CreateGlobalMoleculeContainerFor(IContainer rootContainer, IMoleculeBuilder moleculeBuilder, SimulationConfiguration simulationConfiguration);

      /// <summary>
      /// Returns (and creates if not already there) the sub container for the transport process named <paramref name="transportName"/> 
      /// in the <paramref name="neighborhood"/> for the transporter <paramref name="transporterMolecule"/>
      /// </summary>
      IContainer NeighborhoodMoleculeTransportContainerFor(Neighborhood neighborhood, string transportedMoleculeName, TransporterMoleculeContainer transporterMolecule, string transportName, SimulationConfiguration simulationConfiguration);
   }

   internal class MoleculePropertiesContainerTask : IMoleculePropertiesContainerTask
   {
      private readonly IContainerTask _containerTask;
      private readonly IParameterBuilderCollectionToParameterCollectionMapper _parameterCollectionMapper;
      private readonly IKeywordReplacerTask _keywordReplacer;

      public MoleculePropertiesContainerTask(IContainerTask containerTask, IParameterBuilderCollectionToParameterCollectionMapper parameterCollectionMapper,
         IKeywordReplacerTask keywordReplacer)
      {
         _containerTask = containerTask;
         _parameterCollectionMapper = parameterCollectionMapper;
         _keywordReplacer = keywordReplacer;
      }

      public IContainer NeighborhoodMoleculeContainerFor(Neighborhood neighborhood, string moleculeName)
      {
         var moleculeContainer = neighborhood.GetSingleChildByName<IContainer>(moleculeName);
         if (moleculeContainer == null)
            throw new MissingMoleculeContainerException(moleculeName);

         return moleculeContainer;
      }

      public IContainer CreateGlobalMoleculeContainerFor(IContainer rootContainer, IMoleculeBuilder moleculeBuilder, SimulationConfiguration simulationConfiguration)
      {
         var globalMoleculeContainer = addContainerUnder(rootContainer, moleculeBuilder, moleculeBuilder.Name, simulationConfiguration)
            .WithContainerType(ContainerType.Molecule);

         //Add global molecule dependent parameters
         if (moleculeBuilder.IsFloatingXenobiotic)
         {
            var globalMoleculeDependentProperties = simulationConfiguration.SpatialStructure.GlobalMoleculeDependentProperties;
            globalMoleculeContainer.AddChildren(addAllParametersFrom(globalMoleculeDependentProperties, simulationConfiguration));
         }

         //Only non local parameters from the molecule are added to the global container 
         //Local parameters will be filled in elsewhere (by the MoleculeAmount-Mapper)
         globalMoleculeContainer.AddChildren(allGlobalOrPropertyParametersFrom(moleculeBuilder, simulationConfiguration));

         foreach (var transporterMoleculeContainer in moleculeBuilder.TransporterMoleculeContainerCollection)
         {
            addContainerWithParametersUnder(globalMoleculeContainer, transporterMoleculeContainer, transporterMoleculeContainer.TransportName, simulationConfiguration);
         }

         foreach (var interactionContainer in moleculeBuilder.InteractionContainerCollection)
         {
            addContainerWithParametersUnder(globalMoleculeContainer, interactionContainer, interactionContainer.Name, simulationConfiguration);
         }

         _keywordReplacer.ReplaceIn(globalMoleculeContainer, rootContainer, moleculeBuilder.Name);

         return globalMoleculeContainer;
      }


      private IContainer addContainerUnder(IContainer parentContainer, IContainer templateContainer, string newContainerName, SimulationConfiguration simulationConfiguration)
      {
         var instanceContainer = _containerTask.CreateOrRetrieveSubContainerByName(parentContainer, newContainerName);
         simulationConfiguration.AddBuilderReference(instanceContainer, templateContainer);
         instanceContainer.Icon = templateContainer.Icon;
         instanceContainer.Description = templateContainer.Description;
         return instanceContainer;
      }

      private IContainer addContainerWithParametersUnder(IContainer parentContainer, IContainer templateContainer, string newContainerName, SimulationConfiguration simulationConfiguration)
      {
         return addContainerUnder(parentContainer, templateContainer, newContainerName, simulationConfiguration)
            .WithChildren(allGlobalOrPropertyParametersFrom(templateContainer, simulationConfiguration));
       }

      private IEnumerable<IParameter> allGlobalOrPropertyParametersFrom(IContainer container, SimulationConfiguration simulationConfiguration)
      {
         return _parameterCollectionMapper.MapGlobalOrPropertyFrom(container, simulationConfiguration);
      }
      private IEnumerable<IParameter> addAllParametersFrom(IContainer container, SimulationConfiguration simulationConfiguration)
      {
         return _parameterCollectionMapper.MapAllFrom(container, simulationConfiguration);
      }

      private IEnumerable<IParameter> allLocalParametersFrom(IContainer container, SimulationConfiguration simulationConfiguration)
      {
         return _parameterCollectionMapper.MapLocalFrom(container, simulationConfiguration);
      }

      public IContainer NeighborhoodMoleculeTransportContainerFor(Neighborhood neighborhood, string transportedMoleculeName,TransporterMoleculeContainer transporterMolecule, string transportName, SimulationConfiguration simulationConfiguration)
      {
         var moleculeContainer = NeighborhoodMoleculeContainerFor(neighborhood, transportedMoleculeName);
         var transportContainer = moleculeContainer.EntityAt<IContainer>(transporterMolecule.TransportName);
         if (transportContainer != null)
            return transportContainer;

         return _containerTask.CreateOrRetrieveSubContainerByName(moleculeContainer, transporterMolecule.TransportName)
            .WithChildren(allLocalParametersFrom(transporterMolecule, simulationConfiguration));
      }
   }
}
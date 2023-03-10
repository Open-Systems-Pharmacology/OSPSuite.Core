using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps neighborhood builder object into a neighborhood
   /// </summary>
   public interface INeighborhoodBuilderToNeighborhoodMapper
   {
      /// <summary>
      ///    Maps neighborhood builder object into a neighborhood
      /// </summary>
      /// <param name="neighborhoodBuilder">Neighborhood builder to be mapped</param>
      /// <param name="model">Model, required to link neighbor containers in model</param>
      /// <param name="buildConfiguration">Build configuration</param>
      /// <param name="moleculeNames">
      ///    All molecules present in both neighbors, required to create molecule properties
      ///    subcontainers
      /// </param>
      /// <param name="moleculeNamesWithCopyPropertiesRequired">Molecules having CopyMoleculeDependentProperties=true</param>
      /// <returns></returns>
      Neighborhood MapFrom(NeighborhoodBuilder neighborhoodBuilder, IModel model,
         IBuildConfiguration buildConfiguration, IEnumerable<string> moleculeNames,
         IEnumerable<string> moleculeNamesWithCopyPropertiesRequired);
   }

   public class NeighborhoodBuilderToNeighborhoodMapper : INeighborhoodBuilderToNeighborhoodMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IContainerBuilderToContainerMapper _containerMapper;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;

      public NeighborhoodBuilderToNeighborhoodMapper(IObjectBaseFactory objectBaseFactory,
         IContainerBuilderToContainerMapper containerMapper,
         IKeywordReplacerTask keywordReplacerTask,
         ICloneManagerForModel cloneManagerForModel, IParameterBuilderToParameterMapper parameterMapper)
      {
         _objectBaseFactory = objectBaseFactory;
         _containerMapper = containerMapper;
         _keywordReplacerTask = keywordReplacerTask;
         _cloneManagerForModel = cloneManagerForModel;
         _parameterMapper = parameterMapper;
      }

      public Neighborhood MapFrom(NeighborhoodBuilder neighborhoodBuilder, IModel model,
         IBuildConfiguration buildConfiguration,
         IEnumerable<string> moleculeNames,
         IEnumerable<string> moleculeNamesWithCopyPropertiesRequired)
      {
         var neighborhood = _objectBaseFactory.Create<Neighborhood>();
         neighborhood.UpdatePropertiesFrom(neighborhoodBuilder, _cloneManagerForModel);
         buildConfiguration.AddBuilderReference(neighborhood, neighborhoodBuilder);
         neighborhood.FirstNeighbor = resolveReference(model, neighborhoodBuilder.FirstNeighborPath);
         neighborhood.SecondNeighbor = resolveReference(model, neighborhoodBuilder.SecondNeighborPath);

         if (neighborhoodBuilder.MoleculeProperties != null)
         {
            moleculeNames.Each(moleculeName => neighborhood.Add(
               createMoleculePropertiesFor(neighborhoodBuilder, moleculeName, model.Root, buildConfiguration, moleculeNamesWithCopyPropertiesRequired)));
         }

         //Add neighborhood parameter to the neighborhood (clone the existing parameter)
         neighborhoodBuilder.Parameters.Each(param => neighborhood.Add(_parameterMapper.MapFrom(param, buildConfiguration)));
         return neighborhood;
      }

      private IContainer resolveReference(IModel model, ObjectPath objectPath)
      {
         var objectPathInModel = _keywordReplacerTask.CreateModelPathFor(objectPath, model.Root);
         return objectPathInModel.Resolve<IContainer>(model.Root);
      }

      private IContainer createMoleculePropertiesFor(NeighborhoodBuilder neighborhoodBuilder,
         string moleculeName, IContainer rootContainer,
         IBuildConfiguration buildConfiguration,
         IEnumerable<string> moleculeNamesWithCopyPropertiesRequired)
      {
         //Create a new model container from the neighborhood container 
         var moleculePropertiesContainer = _containerMapper.MapFrom(neighborhoodBuilder.MoleculeProperties, buildConfiguration);
         moleculePropertiesContainer.ContainerType = ContainerType.Molecule;

         //Assignment molecule properties subcontainer name from <MOLECULE_PROPERTIES>
         //to concrete molecule name
         moleculePropertiesContainer.Name = moleculeName;

         _keywordReplacerTask.ReplaceIn(moleculePropertiesContainer, rootContainer, moleculeName);

         //remove children if molecule properties should not be copied
         if (!moleculeNamesWithCopyPropertiesRequired.Contains(moleculeName))
            moleculePropertiesContainer.RemoveChildren();

         return moleculePropertiesContainer;
      }
   }
}
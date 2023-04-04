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
      /// <param name="modelConfiguration">Model, required to link neighbor containers in model</param>
      /// <param name="moleculeNames">
      ///    All molecules present in both neighbors, required to create molecule properties
      ///    subcontainers
      /// </param>
      /// <param name="moleculeNamesWithCopyPropertiesRequired">Molecules having CopyMoleculeDependentProperties=true</param>
      /// <returns></returns>
      Neighborhood MapFrom(NeighborhoodBuilder neighborhoodBuilder, IEnumerable<string> moleculeNames,
         IEnumerable<string> moleculeNamesWithCopyPropertiesRequired, ModelConfiguration modelConfiguration);
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

      public Neighborhood MapFrom(NeighborhoodBuilder neighborhoodBuilder, IEnumerable<string> moleculeNames,
         IEnumerable<string> moleculeNamesWithCopyPropertiesRequired, ModelConfiguration modelConfiguration)
      {
         var (model, simulationConfiguration) = modelConfiguration;
         var neighborhood = _objectBaseFactory.Create<Neighborhood>();
         neighborhood.UpdatePropertiesFrom(neighborhoodBuilder, _cloneManagerForModel);
         simulationConfiguration.AddBuilderReference(neighborhood, neighborhoodBuilder);
         neighborhood.FirstNeighbor = resolveReference(model, neighborhoodBuilder.FirstNeighborPath);
         neighborhood.SecondNeighbor = resolveReference(model, neighborhoodBuilder.SecondNeighborPath);

         if (neighborhoodBuilder.MoleculeProperties != null)
         {
            moleculeNames.Each(moleculeName => neighborhood.Add(
               createMoleculePropertiesFor(neighborhoodBuilder, moleculeName, moleculeNamesWithCopyPropertiesRequired, modelConfiguration)));
         }

         //Add neighborhood parameter to the neighborhood (clone the existing parameter)
         neighborhoodBuilder.Parameters.Each(param => neighborhood.Add(_parameterMapper.MapFrom(param, simulationConfiguration)));
         return neighborhood;
      }

      private IContainer resolveReference(IModel model, ObjectPath objectPath)
      {
         var objectPathInModel = _keywordReplacerTask.CreateModelPathFor(objectPath, model.Root);
         return objectPathInModel.Resolve<IContainer>(model.Root);
      }

      private IContainer createMoleculePropertiesFor(NeighborhoodBuilder neighborhoodBuilder,
         string moleculeName, IEnumerable<string> moleculeNamesWithCopyPropertiesRequired, ModelConfiguration modelConfiguration)
      {
         //Create a new model container from the neighborhood container 
         var (model, simulationConfiguration) = modelConfiguration;
         var moleculePropertiesContainer = _containerMapper.MapFrom(neighborhoodBuilder.MoleculeProperties, simulationConfiguration);
         moleculePropertiesContainer.ContainerType = ContainerType.Molecule;

         //Assignment molecule properties subcontainer name from <MOLECULE_PROPERTIES>
         //to concrete molecule name
         moleculePropertiesContainer.Name = moleculeName;

         _keywordReplacerTask.ReplaceIn(moleculePropertiesContainer, model.Root, moleculeName);

         //remove children if molecule properties should not be copied
         if (!moleculeNamesWithCopyPropertiesRequired.Contains(moleculeName))
            moleculePropertiesContainer.RemoveChildren();

         return moleculePropertiesContainer;
      }
   }
}
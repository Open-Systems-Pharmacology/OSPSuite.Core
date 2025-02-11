using System;
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
   internal interface INeighborhoodBuilderToNeighborhoodMapper
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
      Neighborhood MapFrom(NeighborhoodBuilder neighborhoodBuilder, IReadOnlyList<string> moleculeNames,
         IEnumerable<string> moleculeNamesWithCopyPropertiesRequired, ModelConfiguration modelConfiguration);
   }

   internal class NeighborhoodBuilderToNeighborhoodMapper : INeighborhoodBuilderToNeighborhoodMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IContainerBuilderToContainerMapper _containerMapper;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;

      public NeighborhoodBuilderToNeighborhoodMapper(
         IObjectBaseFactory objectBaseFactory,
         IContainerBuilderToContainerMapper containerMapper,
         IKeywordReplacerTask keywordReplacerTask,
         ICloneManagerForModel cloneManagerForModel, 
         IParameterBuilderToParameterMapper parameterMapper)
      {
         _objectBaseFactory = objectBaseFactory;
         _containerMapper = containerMapper;
         _keywordReplacerTask = keywordReplacerTask;
         _cloneManagerForModel = cloneManagerForModel;
         _parameterMapper = parameterMapper;
      }

      public Neighborhood MapFrom(NeighborhoodBuilder neighborhoodBuilder, IReadOnlyList<string> moleculeNames,
         IEnumerable<string> moleculeNamesWithCopyPropertiesRequired, ModelConfiguration modelConfiguration)
      {
         var (model, simulationBuilder, replacementContext) = modelConfiguration;

         var neighborhood = _objectBaseFactory.Create<Neighborhood>();
         neighborhood.UpdatePropertiesFrom(neighborhoodBuilder, _cloneManagerForModel);
         simulationBuilder.AddBuilderReference(neighborhood, neighborhoodBuilder);
         neighborhood.FirstNeighbor = resolveReference(model, neighborhoodBuilder.FirstNeighborPath, replacementContext);
         neighborhood.SecondNeighbor = resolveReference(model, neighborhoodBuilder.SecondNeighborPath, replacementContext);

         //At least one neighbor cannot be found. We are ignoring this neighborhood
         if (!neighborhood.IsDefined)
            return null;

         if (neighborhoodBuilder.MoleculeProperties != null)
         {
            moleculeNames.Each(moleculeName => neighborhood.Add(
               createMoleculePropertiesFor(neighborhoodBuilder, moleculeName, moleculeNamesWithCopyPropertiesRequired, modelConfiguration)));
         }

         //Add neighborhood parameter to the neighborhood (clone the existing parameter)
         neighborhoodBuilder.Parameters.Each(param => neighborhood.Add(_parameterMapper.MapFrom(param, simulationBuilder)));
         return neighborhood;
      }

      private IContainer resolveReference(IModel model, ObjectPath objectPath, ReplacementContext replacementContext)
      {
         var objectPathInModel = _keywordReplacerTask.CreateModelPathFor(objectPath, replacementContext);
         return objectPathInModel.Resolve<IContainer>(model.Root);
      }

      private IContainer createMoleculePropertiesFor(NeighborhoodBuilder neighborhoodBuilder,
         string moleculeName, IEnumerable<string> moleculeNamesWithCopyPropertiesRequired, ModelConfiguration modelConfiguration)
      {
         var (_, simulationBuilder, replacementContext) = modelConfiguration;
         //Create a new model container from the neighborhood container 
         var moleculePropertiesContainer = _containerMapper.MapFrom(neighborhoodBuilder.MoleculeProperties, simulationBuilder);
         moleculePropertiesContainer.ContainerType = ContainerType.Molecule;

         //Assignment molecule properties subcontainer name from <MOLECULE_PROPERTIES>
         //to concrete molecule name
         moleculePropertiesContainer.Name = moleculeName;

         _keywordReplacerTask.ReplaceIn(moleculePropertiesContainer, moleculeName, replacementContext);

         //remove children if molecule properties should not be copied
         if (!moleculeNamesWithCopyPropertiesRequired.Contains(moleculeName))
            moleculePropertiesContainer.RemoveChildren();

         return moleculePropertiesContainer;
      }
   }
}
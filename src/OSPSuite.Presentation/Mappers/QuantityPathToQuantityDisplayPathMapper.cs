using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Mappers
{
   public class QuantityPathToQuantityDisplayPathMapper : IQuantityPathToQuantityDisplayPathMapper
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IPathToPathElementsMapper _pathToPathElementsMapper;
      private readonly IDataColumnToPathElementsMapper _dataColumnToPathElementsMapper;

      public QuantityPathToQuantityDisplayPathMapper(IObjectPathFactory objectPathFactory, IPathToPathElementsMapper pathToPathElementsMapper,
         IDataColumnToPathElementsMapper dataColumnToPathElementsMapper)
      {
         _objectPathFactory = objectPathFactory;
         _pathToPathElementsMapper = pathToPathElementsMapper;
         _dataColumnToPathElementsMapper = dataColumnToPathElementsMapper;
      }

      public virtual string DisplayPathAsStringFor(ISimulation simulation, DataColumn column, bool addSimulationName = false)
      {
         addSimulationName = addSimulationName || column.IsObservedData();
         return displayPathAsStringFor(simulation, column, addSimulationName);
      }

      public string DisplayPathAsStringFor(ISimulation simulation, DataColumn column, string simulationName)
      {
         if (string.IsNullOrEmpty(simulationName))
            return DisplayPathAsStringFor(simulation, column, addSimulationName: false);

         return displayPathAsStringFor(simulation, column, true, x =>
         {
            x[PathElement.Simulation] = new PathElementDTO {DisplayName = simulationName};
         });
      }

      private string displayPathAsStringFor(ISimulation simulation, DataColumn column, bool addSimulationName, Action<PathElements> pathElementAdjustments = null)
      {
         var pathElements = DisplayPathFor(simulation, column);
         pathElementAdjustments?.Invoke(pathElements);
         return DisplayPathAsStringFrom(pathElements, DefaultPathElementsToUse(addSimulationName, pathElements));
      }

      public virtual string DisplayPathAsStringFor(ISimulation simulation, DataColumn column, IEnumerable<PathElement> pathElementsToUse)
      {
         return DisplayPathAsStringFrom(DisplayPathFor(simulation, column), pathElementsToUse);
      }

      protected virtual PathElements DisplayPathFor(ISimulation simulation, DataColumn column)
      {
         return displayPathForColumn(column, simulation);
      }

      public virtual string DisplayPathAsStringFor(IQuantity quantity, bool addSimulationName = false)
      {
         var pathElements = displayPathFor(quantity);
         return DisplayPathAsStringFrom(pathElements, DefaultPathElementsToUse(addSimulationName, pathElements));
      }

      public virtual string DisplayPathAsStringFor(IQuantity quantity, IEnumerable<PathElement> pathElementsToUse)
      {
         return DisplayPathAsStringFrom(displayPathFor(quantity), pathElementsToUse);
      }

      private PathElements displayPathFor(IQuantity quantity)
      {
         return displayPathForQuantity(quantity.RootContainer, _objectPathFactory.CreateAbsoluteObjectPath(quantity));
      }

      private PathElements displayPathForColumn(DataColumn column, ISimulation simulation)
      {
         return _dataColumnToPathElementsMapper.MapFrom(column, simulation?.Model.Root);
      }

      private PathElements displayPathForQuantity(IContainer rootContainer, IObjectPath objectPath)
      {
         return _pathToPathElementsMapper.MapFrom(rootContainer, objectPath.ToList());
      }

      protected virtual IEnumerable<PathElement> DefaultPathElementsToUse(bool addSimulationName, PathElements pathElements)
      {
         if (addSimulationName)
            yield return PathElement.Simulation;

         yield return PathElement.Molecule;
         yield return PathElement.TopContainer;
         yield return PathElement.Container;
         yield return PathElement.BottomCompartment;
         yield return PathElement.Name;
      }

      protected virtual string DisplayPathAsStringFrom(PathElements pathElements, IEnumerable<PathElement> pathElementsToUse)
      {
         var displayNames = new List<string>();
         pathElementsToUse.Each(x => AddToList(displayNames, pathElements, x));
         return displayNames.ToString(Constants.DISPLAY_PATH_SEPARATOR);
      }

      protected void AddToList(List<string> displayList, PathElements pathElements, PathElement pathElement)
      {
         var displayName = pathElements[pathElement].DisplayName;
         if (string.IsNullOrEmpty(displayName))
            return;

         displayList.Add(displayName);
      }
   }
}
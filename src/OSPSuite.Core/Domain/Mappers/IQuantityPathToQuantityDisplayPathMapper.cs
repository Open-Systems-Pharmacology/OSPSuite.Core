using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IQuantityPathToQuantityDisplayPathMapper
   {
      string DisplayPathAsStringFor(ISimulation simulation, DataColumn column, bool addSimulationName = false);
      string DisplayPathAsStringFor(ISimulation simulation, DataColumn column, string simulationName);
      string DisplayPathAsStringFor(ISimulation simulation, DataColumn column, IEnumerable<PathElementId> pathElementsToUse);
      string DisplayPathAsStringFor(IQuantity quantity, bool addSimulationName = false);
      string DisplayPathAsStringFor(IQuantity quantity, IEnumerable<PathElementId> pathElementsToUse);
   }

   public class QuantityPathToQuantityDisplayPathMapper : IQuantityPathToQuantityDisplayPathMapper
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IPathToPathElementsMapper _pathToPathElementsMapper;
      private readonly IDataColumnToPathElementsMapper _dataColumnToPathElementsMapper;

      public QuantityPathToQuantityDisplayPathMapper(
         IObjectPathFactory objectPathFactory, 
         IPathToPathElementsMapper pathToPathElementsMapper,
         IDataColumnToPathElementsMapper dataColumnToPathElementsMapper)
      {
         _objectPathFactory = objectPathFactory;
         _pathToPathElementsMapper = pathToPathElementsMapper;
         _dataColumnToPathElementsMapper = dataColumnToPathElementsMapper;
      }

      public virtual string DisplayPathAsStringFor(ISimulation simulation, DataColumn column, bool addSimulationName = false)
      {
         addSimulationName = addSimulationName || column.IsObservation();
         return displayPathAsStringFor(simulation, column, addSimulationName);
      }

      public string DisplayPathAsStringFor(ISimulation simulation, DataColumn column, string simulationName)
      {
         if (string.IsNullOrEmpty(simulationName))
            return DisplayPathAsStringFor(simulation, column, addSimulationName: false);

         return displayPathAsStringFor(simulation, column, true, x => { x[PathElementId.Simulation] = new PathElement {DisplayName = simulationName}; });
      }

      private string displayPathAsStringFor(ISimulation simulation, DataColumn column, bool addSimulationName, Action<PathElements> pathElementAdjustments = null)
      {
         var pathElements = DisplayPathFor(simulation, column);
         pathElementAdjustments?.Invoke(pathElements);
         return DisplayPathAsStringFrom(pathElements, DefaultPathElementsToUse(addSimulationName, pathElements));
      }

      public virtual string DisplayPathAsStringFor(ISimulation simulation, DataColumn column, IEnumerable<PathElementId> pathElementsToUse)
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

      public virtual string DisplayPathAsStringFor(IQuantity quantity, IEnumerable<PathElementId> pathElementsToUse)
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

      private PathElements displayPathForQuantity(IContainer rootContainer, ObjectPath objectPath)
      {
         return _pathToPathElementsMapper.MapFrom(rootContainer, objectPath.ToList());
      }

      protected virtual IEnumerable<PathElementId> DefaultPathElementsToUse(bool addSimulationName, PathElements pathElements)
      {
         if (addSimulationName)
            yield return PathElementId.Simulation;

         yield return PathElementId.Molecule;
         yield return PathElementId.TopContainer;
         yield return PathElementId.Container;
         yield return PathElementId.BottomCompartment;
         yield return PathElementId.Name;
      }

      protected virtual string DisplayPathAsStringFrom(PathElements pathElements, IEnumerable<PathElementId> pathElementsToUse)
      {
         var displayNames = new List<string>();
         pathElementsToUse.Each(x => AddToList(displayNames, pathElements, x));
         return displayNames.ToString(Constants.DISPLAY_PATH_SEPARATOR);
      }

      protected void AddToList(List<string> displayList, PathElements pathElements, PathElementId pathElementId)
      {
         var displayName = pathElements[pathElementId].DisplayName;
         if (string.IsNullOrEmpty(displayName))
            return;

         displayList.Add(displayName);
      }
   }
}
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IDataColumnToPathElementsMapper
   {
      /// <summary>
      ///    returns values for the fixed PathColumns to display the path of the dataColumn starting from root container
      /// </summary>
      /// <param name="dataColumn"></param>
      /// <param name="rootContainer"> root container (for quantityPath of dataColumn) </param>
      /// <returns>values for the fixed PathColumns</returns>
      PathElements MapFrom(DataColumn dataColumn, IContainer rootContainer);
   }

   public class DataColumnToPathElementsMapper : IDataColumnToPathElementsMapper
   {
      //layout of the quantity path of observed data columns: [repository, ObservedData, organ, compartment, molecule, column]
      private const int OBSERVED_DATA_PATH_LENGTH = 6;
      private const int OBSERVED_DATA_REPOSITORY_INDEX = 0;
      private const int OBSERVED_DATA_ORGAN_INDEX = 2;
      private const int OBSERVED_DATA_COMPARTMENT_INDEX = 3;
      private const int OBSERVED_DATA_MOLECULE_INDEX = 4;
      private const int OBSERVED_DATA_COLUMN_INDEX = 5;

      private readonly IPathToPathElementsMapper _pathToPathElementsMapper;

      public DataColumnToPathElementsMapper(IPathToPathElementsMapper pathToPathElementsMapper)
      {
         _pathToPathElementsMapper = pathToPathElementsMapper;
      }

      public PathElements MapFrom(DataColumn dataColumn, IContainer rootContainer)
      {
         var quantityPath = dataColumn.QuantityInfo.Path.ToList();

         if (isCalculationColumn(dataColumn, rootContainer, quantityPath))
            return _pathToPathElementsMapper.MapFrom(rootContainer, quantityPath);

         if (dataColumn.IsObservation())
            return ObservedDataPathElementsFor(dataColumn, quantityPath);

         if (dataColumn.IsBaseGrid() || dataColumn.IsDeviation())
            return pathElementsForNameOnlyColumn(dataColumn);

         return pathElementsForUnknownColumns(dataColumn, quantityPath);
      }

      private static PathElements pathElementsForNameOnlyColumn(DataColumn dataColumn)
      {
         return new PathElements
         {
            [PathElementId.Name] = new PathElement {DisplayName = dataColumn.Name}
         };
      }

      /// <summary>
      ///    Maps the quantity path of an observed data column created by <c>IObservedDataNormalizer</c>
      ///    ([repository, ObservedData, organ, compartment, molecule, column]) to path elements so that observed data is
      ///    grouped and labelled like simulation outputs. Any other path falls back to the generic mapping.
      /// </summary>
      protected virtual PathElements ObservedDataPathElementsFor(DataColumn dataColumn, List<string> quantityPath)
      {
         if (quantityPath.Count != OBSERVED_DATA_PATH_LENGTH)
            return pathElementsForUnknownColumns(dataColumn, quantityPath);

         return new PathElements
         {
            [PathElementId.Simulation] = new PathElement {DisplayName = quantityPath[OBSERVED_DATA_REPOSITORY_INDEX]},
            [PathElementId.TopContainer] = new PathElement {DisplayName = quantityPath[OBSERVED_DATA_REPOSITORY_INDEX]},
            [PathElementId.Container] = new PathElement {DisplayName = quantityPath[OBSERVED_DATA_ORGAN_INDEX]},
            [PathElementId.BottomCompartment] = new PathElement {DisplayName = quantityPath[OBSERVED_DATA_COMPARTMENT_INDEX]},
            [PathElementId.Molecule] = new PathElement {DisplayName = quantityPath[OBSERVED_DATA_MOLECULE_INDEX]},
            [PathElementId.Name] = new PathElement {DisplayName = quantityPath[OBSERVED_DATA_COLUMN_INDEX]}
         };
      }

      private PathElements pathElementsForUnknownColumns(DataColumn dataColumn, List<string> quantityPath)
      {
         var pathColumnValues = new PathElements();

         // extract first pathElementId to TopContainer, remaining pathElements to Container
         if (quantityPath.Count > 0)
         {
            pathColumnValues[PathElementId.TopContainer] = new PathElement {DisplayName = quantityPath[0]};
            quantityPath.RemoveAt(0);
         }

         pathColumnValues[PathElementId.Container] = new PathElement {DisplayName = quantityPath.ToString(Constants.DISPLAY_PATH_SEPARATOR)};

         return pathColumnValues;
      }

      private static bool isCalculationColumn(DataColumn dataColumn, IContainer rootContainer, List<string> quantityPath)
      {
         return dataColumn.DataInfo.Origin.IsOneOf(ColumnOrigins.Calculation, ColumnOrigins.CalculationAuxiliary) && quantityPath.Count > 1 && rootContainer != null;
      }
   }
}
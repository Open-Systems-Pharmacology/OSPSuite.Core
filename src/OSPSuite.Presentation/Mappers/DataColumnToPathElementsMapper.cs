using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Mappers
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

         if (dataColumn.IsObservedData())
            return ObservedDataPathElementsFor(dataColumn, quantityPath);

         if (dataColumn.IsBaseGrid())
            return pathElementsForBaseGrid(dataColumn);

         return pathElementsForUnknownColumns(dataColumn, quantityPath);
      }

      private static PathElements pathElementsForBaseGrid(DataColumn dataColumn)
      {
         var pathColumnValues = new PathElements();
         pathColumnValues[PathElement.Name] = new PathElementDTO {DisplayName = dataColumn.Name};
         return pathColumnValues;
      }

      protected virtual PathElements ObservedDataPathElementsFor(DataColumn dataColumn, List<string> quantityPath)
      {
         return pathElementsForUnknownColumns(dataColumn, quantityPath);
      }

      private PathElements pathElementsForUnknownColumns(DataColumn dataColumn, List<string> quantityPath)
      {
         var pathColumnValues = new PathElements();

         // extract first pathElement to TopContainer, remaining pathElements to Container
         if (quantityPath.Count > 0)
         {
            pathColumnValues[PathElement.TopContainer] = new PathElementDTO {DisplayName = quantityPath[0]};
            quantityPath.RemoveAt(0);
         }

         pathColumnValues[PathElement.Container] = new PathElementDTO {DisplayName = quantityPath.ToString(Constants.DISPLAY_PATH_SEPARATOR)};

         return pathColumnValues;
      }

      private static bool isCalculationColumn(DataColumn dataColumn, IContainer rootContainer, List<string> quantityPath)
      {
         return dataColumn.DataInfo.Origin.IsOneOf(ColumnOrigins.Calculation, ColumnOrigins.CalculationAuxiliary) && quantityPath.Count > 1 && rootContainer != null;
      }
   }
}
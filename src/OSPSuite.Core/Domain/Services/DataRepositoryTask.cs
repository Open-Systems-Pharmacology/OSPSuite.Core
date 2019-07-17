using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IDataRepositoryTask
   {
      DataRepository Clone(DataRepository dataRepositoryToClone);

      BaseGrid CloneBaseGrid(DataColumn baseGridToClone);

      DataColumn CloneColumn(DataColumn sourceColumn, BaseGrid clonedBaseGrid);

      /// <summary>
      ///    Update the molweight for the given datacolumn representing the quantity given as parameter.
      ///    Molweight is at most available for observers and molecule amount. If the molecular weight is not found,
      ///    the default value will be set as is (null)
      /// </summary>
      /// <param name="column">Column whose molweight should be updated</param>
      /// <param name="quantity">Quantity whose values are saved in the column</param>
      /// <param name="model">Model where the quantity belongs</param>
      void UpdateMolWeight(DataColumn column, IQuantity quantity, IModel model);
   }

   public class DataRepositoryTask : IDataRepositoryTask
   {
      private readonly ICache<string, string> _idMap = new Cache<string, string>();

      public DataRepository Clone(DataRepository dataRepositoryToClone)
      {
         _idMap.Clear();
         var cloneRepository = new DataRepository {Name = dataRepositoryToClone.Name, Description = dataRepositoryToClone.Description};

         var baseGridColumns = dataRepositoryToClone.Where(col => col.IsBaseGrid());
         baseGridColumns.Each(col => addBaseGridToRepository(cloneRepository, col));

         var nonBaseGridColumns = dataRepositoryToClone.Where(col => !col.IsBaseGrid());
         nonBaseGridColumns.Each(col => addColumnToRepository(cloneRepository, col));

         cloneRepository.ExtendedProperties.UpdateFrom(dataRepositoryToClone.ExtendedProperties);

         return cloneRepository;
      }

      public void UpdateMolWeight(DataColumn column, IQuantity quantity, IModel model)
      {
         var molWeight = model.MolWeightFor(quantity);
         if (molWeight != null)
            column.DataInfo.MolWeight = molWeight;
      }

      private void addBaseGridToRepository(DataRepository dataRepository, DataColumn baseGrid)
      {
         if (isAlreadyCloned(baseGrid)) return;
         dataRepository.Add(cloneBaseGrid(baseGrid));
      }

      private DataColumn addColumnToRepository(DataRepository dataRepository, DataColumn sourceColumn)
      {
         if (isAlreadyCloned(sourceColumn))
            return dataRepository[idOfCloneFor(sourceColumn)];

         var baseGridColumn = dataRepository[idOfCloneFor(sourceColumn.BaseGrid)].DowncastTo<BaseGrid>();

         var newColumn = CloneColumn(sourceColumn, baseGridColumn);
         dataRepository.Add(newColumn);

         foreach (var relatedColumn in sourceColumn.RelatedColumns)
         {
            newColumn.AddRelatedColumn(addColumnToRepository(dataRepository, relatedColumn));
         }

         return newColumn;
      }

      private void updateColumnProperties(DataColumn sourceColumn, DataColumn targetColumn)
      {
         targetColumn.QuantityInfo = sourceColumn.QuantityInfo?.Clone();
         targetColumn.DataInfo = sourceColumn.DataInfo?.Clone();
         targetColumn.Values = sourceColumn.Values;
         targetColumn.IsInternal = sourceColumn.IsInternal;
      }

      public BaseGrid CloneBaseGrid(DataColumn baseGridToClone)
      {
         _idMap.Clear();
         return cloneBaseGrid(baseGridToClone);
      }

      private BaseGrid cloneBaseGrid(DataColumn baseGridToClone)
      {
         var newBaseGrid = new BaseGrid(baseGridToClone.Name, baseGridToClone.Dimension);
         updateColumnProperties(baseGridToClone, newBaseGrid);
         _idMap.Add(baseGridToClone.Id, newBaseGrid.Id);
         return newBaseGrid;
      }

      public DataColumn CloneColumn(DataColumn sourceColumn, BaseGrid clonedBaseGrid)
      {
         var newColumn = new DataColumn(sourceColumn.Name, sourceColumn.Dimension, clonedBaseGrid);
         updateColumnProperties(sourceColumn, newColumn);
         _idMap.Add(sourceColumn.Id, newColumn.Id);
         return newColumn;
      }

      private bool isAlreadyCloned(DataColumn column) => _idMap.Contains(column.Id);

      private string idOfCloneFor(DataColumn column) => _idMap[column.Id];
   }
}
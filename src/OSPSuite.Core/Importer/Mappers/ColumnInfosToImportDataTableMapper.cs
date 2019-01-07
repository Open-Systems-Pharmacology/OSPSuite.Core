using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Importer.Mappers
{
   public interface IColumnInfosToImportDataTableMapper
   {
      /// <summary>
      ///    This method converts a list of meta data categories and a list of column info to an import data table.
      /// </summary>
      /// <param name="metaDataCategories">List of meta data categories.</param>
      /// <param name="columnInfos">List of column infos.</param>
      /// <returns>An import data table.</returns>
      ImportDataTable ConvertToImportDataTable(IReadOnlyList<MetaDataCategory> metaDataCategories, IEnumerable<ColumnInfo> columnInfos);
   }

   public class ColumnInfosToImportDataTableMapper : IColumnInfosToImportDataTableMapper
   {


      /// <summary>
      ///    This method converts a list of meta data categories to a meta data table.
      /// </summary>
      /// <param name="metaDataCategories">List of meta data categories.</param>
      /// <returns>A meta data table.</returns>
      private static MetaDataTable convertToMetaDataTable(IEnumerable<MetaDataCategory> metaDataCategories)
      {
         var retVal = new MetaDataTable();

         foreach (var metaDataCategory in metaDataCategories)
         {
            var column = new MetaDataColumn
            {
               ColumnName = metaDataCategory.Name,
               DisplayName = metaDataCategory.DisplayName,
               Description = metaDataCategory.Description,
               ListOfValues = metaDataCategory.ListOfValues,
               ListOfImages = metaDataCategory.ListOfImages,
               IsListOfValuesFixed = metaDataCategory.IsListOfValuesFixed,
               MinValue = metaDataCategory.MinValue,
               MinValueAllowed = metaDataCategory.MinValueAllowed,
               MaxValue = metaDataCategory.MaxValue,
               MaxValueAllowed = metaDataCategory.MaxValueAllowed,
               Required = metaDataCategory.IsMandatory,
               DataType = metaDataCategory.MetaDataType,
               DefaultValue = metaDataCategory.DefaultValue
            };

            if (metaDataCategory.MetaDataType == typeof(string))
               column.MaxLength = metaDataCategory.MaxLength;

            retVal.Columns.Add(column);
         }

         return retVal;
      }

      public ImportDataTable ConvertToImportDataTable(IReadOnlyList<MetaDataCategory> metaDataCategories, IEnumerable<ColumnInfo> columnInfos)
      {
         var retVal = new ImportDataTable();

         if (metaDataCategories != null && metaDataCategories.Count > 0)
            retVal.MetaData = convertToMetaDataTable(metaDataCategories);

         foreach (var columnInfo in columnInfos)
         {
            var column = new ImportDataColumn
            {
               ColumnName = columnInfo.Name,
               DisplayName = columnInfo.DisplayName,
               Description = columnInfo.Description,
               DataType = columnInfo.DataType,
               Required = (columnInfo.IsMandatory || columnInfo.NullValuesHandling == NullValuesHandlingType.NotAllowed),
               SkipNullValueRows = (columnInfo.NullValuesHandling == NullValuesHandlingType.DeleteRow),
            };
            if (columnInfo.MetaDataCategories != null && columnInfo.MetaDataCategories.Count > 0)
               column.MetaData = convertToMetaDataTable(columnInfo.MetaDataCategories);
            if (columnInfo.DimensionInfos != null && columnInfo.DimensionInfos.Count > 0)
            {
               column.Dimensions = columnInfo.DimensionInfos.Select(dimensioninfo => dimensioninfo.ConvertToDimensions()).ToList();
               column.ActiveDimension = DimensionHelper.FindDimension(column.Dimensions,
                  columnInfo.DefaultDimension.Name);
            }

            if (!string.IsNullOrEmpty(columnInfo.RelatedColumnOf)) //column is error column, so we need auxiliary type as metadata
            {
               column.ColumnNameOfRelatedColumn = columnInfo.RelatedColumnOf;
               //Add AuxiliaryType meta data category
               if (column.MetaData == null) column.MetaData = new MetaDataTable();
                  
               var listOfValues = new Dictionary<string, string>
               {
                  {Constants.STD_DEV_ARITHMETIC, Constants.STD_DEV_ARITHMETIC},
                  {Constants.STD_DEV_GEOMETRIC, Constants.STD_DEV_GEOMETRIC}
               };
               //if there is only the dimensionless dimension defined only geometric error make sense
               if (column.Dimensions.Count == 1)
                  if (column.Dimensions[0].IsDimensionless())
                     listOfValues.Remove(Constants.STD_DEV_ARITHMETIC);

               var auxiliaryTypeColumn = new MetaDataColumn
               {
                  ColumnName = Constants.AUXILIARY_TYPE,
                  DisplayName = "Error Type",
                  DataType = typeof(string),
                  Description = "What is the type of error?",
                  ListOfValues = new Dictionary<string, string>(listOfValues),
                  IsListOfValuesFixed = true,
                  Required = true
               };
               column.MetaData.Columns.Add(auxiliaryTypeColumn);

               // add special condition to the dimensions
               // for geometric error the unit must be dimensionless
               // for arithmetic the unit must be a concrete dimension
               foreach (var dim in column.Dimensions)
               {
                  if (dim.MetaDataConditions == null)
                     dim.MetaDataConditions = new Dictionary<string, string>();
                  
                  if (dim.IsDimensionless()) 
                  {
                     dim.MetaDataConditions.Add(Constants.AUXILIARY_TYPE, Constants.STD_DEV_GEOMETRIC);
                     if (dim.IsDefault)
                     {
                        auxiliaryTypeColumn.DefaultValue = Constants.STD_DEV_GEOMETRIC;
                     }
                  }
                  else
                  {
                     dim.MetaDataConditions.Add(Constants.AUXILIARY_TYPE, Constants.STD_DEV_ARITHMETIC);
                     if (dim.IsDefault)
                     {
                        auxiliaryTypeColumn.DefaultValue = Constants.STD_DEV_ARITHMETIC;
                     }
                  }
               }
            }

            retVal.Columns.Add(column);
         }

         return retVal;
      }
   }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;
using IoC = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Infrastructure.Import.Services
{
   public interface IImporter
   {
      IDataSourceFile LoadFile(ColumnInfoCache columnInfos, string fileName, IReadOnlyList<MetaDataCategory> metaDataCategories);
      void AddFromFile(IDataFormat format, DataSheetCollection dataSheets, ColumnInfoCache columnInfos, IDataSource alreadyExisting);
      IEnumerable<IDataFormat> AvailableFormats(DataSheet dataSheet, ColumnInfoCache columnInfos, IReadOnlyList<MetaDataCategory> metaDataCategories);

      IEnumerable<string> NamesFromConvention
      (
         string namingConvention,
         string fileName,
         Cache<string, IDataSet> dataSets,
         IReadOnlyList<MetaDataMappingConverter> mappings
      );

      int GetImageIndex(DataFormatParameter parameter);
      MappingProblem CheckWhetherAllDataColumnsAreMapped(ColumnInfoCache dataColumns, IEnumerable<DataFormatParameter> mappings);

      IReadOnlyList<DataSetToDataRepositoryMappingResult> DataSourceToDataSets(IDataSource dataSource,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         DataImporterSettings dataImporterSettings, string id);

      (IReadOnlyList<DataSetToDataRepositoryMappingResult> DataRepositories, List<string> MissingSheets) ImportFromConfiguration
      (
         ImporterConfiguration configuration,
         ColumnInfoCache columnInfos,
         string fileName,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         DataImporterSettings dataImporterSettings
      );

      IEnumerable<IDataFormat> CalculateFormat(IDataSourceFile dataSource, ColumnInfoCache columnInfos,
         IReadOnlyList<MetaDataCategory> metaDataCategories, string sheetName);
   }

   public class Importer : IImporter
   {
      private readonly IoC _container;
      private readonly IDataSourceFileParser _parser;
      private readonly IDataSetToDataRepositoryMapper _dataRepositoryMapper;
      private readonly IDimension _molWeightDimension;

      public Importer(
         IoC container,
         IDataSourceFileParser parser,
         IDataSetToDataRepositoryMapper dataRepositoryMapper,
         IDimensionFactory dimensionFactory)
      {
         _container = container;
         _parser = parser;
         _dataRepositoryMapper = dataRepositoryMapper;
         _molWeightDimension = dimensionFactory.Dimension(Constants.Dimension.MOLECULAR_WEIGHT);
      }

      public IEnumerable<IDataFormat> AvailableFormats(DataSheet dataSheet, ColumnInfoCache columnInfos,
         IReadOnlyList<MetaDataCategory> metaDataCategories)
      {
         return _container.ResolveAll<IDataFormat>()
            .Select(x => (x, x.SetParameters(dataSheet, columnInfos, metaDataCategories)))
            .Where(p => p.Item2 > 0)
            .OrderByDescending(p => p.Item2)
            .Select(p => p.x);
      }

      public void AddFromFile(IDataFormat format, DataSheetCollection dataSheets, ColumnInfoCache columnInfos, IDataSource alreadyExisting)
      {
         var dataSets = dataSheets.GetDataSets(format, columnInfos); //ToDo: to be made into a new class DataSetCollection instead of a Cache

         foreach (var key in dataSets.Keys)
         {
            IDataSet current;
            if (alreadyExisting.DataSets.Contains(key))
               current = alreadyExisting.DataSets[key];
            else
            {
               current = new DataSet();
               alreadyExisting.DataSets.Add(key, current);
            }

            current.AddData(dataSets[key].Data);
         }
      }

      public IDataSourceFile LoadFile(ColumnInfoCache columnInfos, string fileName, IReadOnlyList<MetaDataCategory> metaDataCategories)
      {
         var dataSource = _parser.For(fileName);

         //if no DataSheets were loaded, meaning the CsvSeparator Dialog was cancelled, abort the import process without further messages.
         if (dataSource.DataSheets == null || !dataSource.DataSheets.Any())
            return null;


         foreach (var sheetName in dataSource.DataSheets.GetDataSheetNames())
         {
            dataSource.AvailableFormats = CalculateFormat(dataSource, columnInfos, metaDataCategories, sheetName).ToList();
            if (dataSource.AvailableFormats.Any())
            {
               dataSource.FormatCalculatedFrom = sheetName;
               return dataSource;
            }
         }

         throw new UnsupportedFormatException(dataSource.Path);
      }

      public IEnumerable<IDataFormat> CalculateFormat(IDataSourceFile dataSource, ColumnInfoCache columnInfos,
         IReadOnlyList<MetaDataCategory> metaDataCategories, string sheetName)
      {
         if (sheetName == null)
            throw new UnsupportedFormatException(dataSource.Path);

         return AvailableFormats(dataSource.DataSheets.GetDataSheetByName(sheetName), columnInfos, metaDataCategories);
      }

      public IEnumerable<string> NamesFromConvention
      (
         string namingConvention,
         string fileName,
         Cache<string, IDataSet> dataSets,
         IReadOnlyList<MetaDataMappingConverter> mappings
      )
      {
         fileName = Path.GetFileNameWithoutExtension(fileName);
         var counters = new Dictionary<string, int>();
         // Iterate over the list of datasets to generate a unique name for each one based on the naming convention
         return dataSets.KeyValues.SelectMany(ds => ds.Value.Data.Select(s =>
         {
            // Obtain the key first before checking if it already is taken by any other dataset
            var key = s.NameFromConvention(mappings, namingConvention, fileName, ds.Key);
            int counter;
            if (counters.ContainsKey(key))
            {
               counter = counters[key];
            }
            else
            {
               counters.Add(key, 0);
               counter = 0;
            }

            counters[key]++;
            // Only add a number (for making it unique) to the name if the key already existed in the counters
            return key + (counter > 0 ? $"_{counter}" : "");
         })).ToList();
      }

      public int GetImageIndex(DataFormatParameter parameter)
      {
         switch (parameter)
         {
            case MetaDataFormatParameter mp:
               return ApplicationIcons.IconIndex(ApplicationIcons.MetaData);
            case MappingDataFormatParameter mp:
               return ApplicationIcons.IconIndex(ApplicationIcons.UnitInformation);
            case GroupByDataFormatParameter gp:
               return ApplicationIcons.IconIndex(ApplicationIcons.GroupBy);
            default:
               throw new Exception($"{parameter.GetType()} is not currently been handled");
         }
      }

      public MappingProblem CheckWhetherAllDataColumnsAreMapped(ColumnInfoCache dataColumns, IEnumerable<DataFormatParameter> mappings)
      {
         var subset = mappings.OfType<MappingDataFormatParameter>().ToList();

         return new MappingProblem()
         {
            //all the mandatory mappings that have not been mapped to a column
            MissingMapping = dataColumns
               .Where(col => col.IsMandatory && subset.All(cm =>
                  cm.MappedColumn.Name != col.Name)).Select(col => col.Name)
               .ToList(),
            //all the mappings where the unit is missing
            MissingUnit = subset
               .Where(
                  cm => cm.MappedColumn.MissingUnitMapping())
               .Select(cm => cm.MappedColumn.Name)
               .ToList()
         };
      }

      public IReadOnlyList<DataSetToDataRepositoryMappingResult> DataSourceToDataSets(IDataSource dataSource,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         DataImporterSettings dataImporterSettings, string id)
      {
         var dataRepositories = new List<DataSetToDataRepositoryMappingResult>();

         for (var i = 0; i < dataSource.DataSets.SelectMany(ds => ds.Data).Count(); i++)
         {
            var dataRepoMapping = _dataRepositoryMapper.ConvertImportDataSet(dataSource.ImportedDataSetAt(i));
            var dataRepo = dataRepoMapping.DataRepository;
            dataRepo.ConfigurationId = id;
            determineMolecularWeight(metaDataCategories, dataImporterSettings, dataRepo);
            dataRepositories.Add(dataRepoMapping);
         }

         return dataRepositories;
      }

      private void determineMolecularWeight(IReadOnlyList<MetaDataCategory> metaDataCategories, DataImporterSettings dataImporterSettings,
         DataRepository dataRepo)
      {
         var molecularWeightFromMoleculeAsString = extractMolecularWeight(metaDataCategories, dataImporterSettings, dataRepo);
         var molecularWeightValueAsString = dataRepo.ExtendedPropertyValueFor(dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation);

         //when the MW does not come from the column but from a the value of of the MW of a specific molecule
         if (dataImporterSettings.CheckMolWeightAgainstMolecule &&
             !molecularWeightFromMoleculeAsString.IsNullOrEmpty() &&
             !molecularWeightValueAsString.IsNullOrEmpty())
         {
            {
               double.TryParse(molecularWeightFromMoleculeAsString, out var moleculeMolWeight);
               double.TryParse(molecularWeightValueAsString, out var molWeight);

               if (!ValueComparer.AreValuesEqual(moleculeMolWeight, molWeight))
               {
                  throw new InconsistentMoleculeAndMolWeightException();
               }
            }
         }

         //assign the MolWeight coming from the excel column or the assigned Molecule
         if (!molecularWeightValueAsString.IsNullOrEmpty())
         {
            assignMolWeightToDataRepo(molecularWeightValueAsString, dataRepo);
         }
         else if (!molecularWeightFromMoleculeAsString.IsNullOrEmpty())
         {
            assignMolWeightToDataRepo(molecularWeightFromMoleculeAsString, dataRepo);
         }

         //We remove the extended property of MolWeight to avoid the duplication, since the MolWeight exists also in the DataRepository properties
         dataRepo.ExtendedProperties.Remove(dataImporterSettings.NameOfMetaDataHoldingMolecularWeightInformation);
      }

      private void assignMolWeightToDataRepo(string molecularWeightValueAsString, DataRepository dataRepo)
      {
         if (double.TryParse(molecularWeightValueAsString, out var molWeight))
         {
            //we are assuming that the MW coming from the column in excel is always in g/mol, that's why we need this conversion here
            dataRepo.AllButBaseGrid().Each(x => x.DataInfo.MolWeight = molWeightValueInCoreUnit(molWeight));
         }
      }

      private double molWeightValueInCoreUnit(double valueInDisplayUnit)
      {
         return _molWeightDimension.UnitValueToBaseUnitValue(_molWeightDimension.DefaultUnit, valueInDisplayUnit);
      }

      private static bool isMolWeightUnique(IReadOnlyList<MetaDataCategory> moleculeDescriptions, string moleculeName)
      {
         //if there is no moleculeCategory, or no specified molecules
         if (!moleculeDescriptions.Any() || !moleculeDescriptions.FirstOrDefault().ListOfValues.Any()) return false;

         var moleculeWeightOfFirstMolecule = moleculeDescriptions.FirstOrDefault().ListOfValues.FirstOrDefault(v =>
            v.Key == moleculeName).Value;

         return moleculeDescriptions.FirstOrDefault().ListOfValues
            .Where(x => x.Key == moleculeName)
            .All(v => v.Value == moleculeWeightOfFirstMolecule);
      }

      private static string extractMolecularWeight(IReadOnlyList<MetaDataCategory> metaDataCategories, DataImporterSettings dataImporterSettings,
         DataRepository dataRepo)
      {
         var metaDataCategoryForMoleculeDescriptions =
            metaDataCategories?.Where(md => md.Name == dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation).ToList();

         var moleculeName = dataRepo.ExtendedPropertyValueFor(dataImporterSettings.NameOfMetaDataHoldingMoleculeInformation);
         //if we find no molecules, or more than one molecules with different molWeights, we do not need to check
         if (metaDataCategoryForMoleculeDescriptions == null ||
             !isMolWeightUnique(metaDataCategoryForMoleculeDescriptions, moleculeName))
            return null;

         var molecularWeight = metaDataCategoryForMoleculeDescriptions.FirstOrDefault().ListOfValues.FirstOrDefault(x =>
            x.Key == moleculeName).Value;

         return molecularWeight;
      }

      public (IReadOnlyList<DataSetToDataRepositoryMappingResult> DataRepositories, List<string> MissingSheets) ImportFromConfiguration
      (
         ImporterConfiguration configuration,
         ColumnInfoCache columnInfos,
         string fileName,
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         DataImporterSettings dataImporterSettings
      )
      {
         var dataSource = new DataSource(this);
         IDataSourceFile dataSourceFile = null;

         dataSourceFile = LoadFile(columnInfos, fileName, metaDataCategories);

         if (dataSourceFile == null)
         {
            return (Enumerable.Empty<DataSetToDataRepositoryMappingResult>().ToList(), Enumerable.Empty<string>().ToList());
         }

         dataSourceFile.Format.CopyParametersFromConfiguration(configuration);
         var mappings = dataSourceFile.Format.GetParameters<MetaDataFormatParameter>().Select(md => new MetaDataMappingConverter()
         {
            Id = md.MetaDataId,
            Index = sheetName => md.IsColumn ? dataSourceFile.DataSheets.GetDataSheetByName(sheetName).GetColumnDescription(md.ColumnName).Index : -1
         }).Union
         (
            dataSourceFile.Format.GetParameters<GroupByDataFormatParameter>().Select(md => new MetaDataMappingConverter()
            {
               Id = md.ColumnName,
               Index = sheetName => dataSourceFile.DataSheets.GetDataSheetByName(sheetName).GetColumnDescription(md.ColumnName).Index
            })
         ).ToList();
         dataSource.SetMappings(dataSourceFile.Path, mappings);
         dataSource.NanSettings = configuration.NanSettings;
         dataSource.SetDataFormat(dataSourceFile.Format);
         dataSource.SetNamingConvention(configuration.NamingConventions);
         var sheets = new DataSheetCollection();
         var missingSheets = new List<string>();
         var sheetList = dataImporterSettings.IgnoreSheetNamesAtImport ? dataSourceFile.DataSheets.GetDataSheetNames() : configuration.LoadedSheets;

         foreach (var key in sheetList)
         {
            if (!dataSourceFile.DataSheets.Contains(key))
            {
               missingSheets.Add(key);
               continue;
            }

            sheets.AddSheet(dataSourceFile.DataSheets.GetDataSheetByName(key));
         }

         var errors = dataSource.AddSheets(sheets, columnInfos, configuration.FilterString);
         if (errors.Any())
            throw new ImporterParsingException(errors);
         return (DataSourceToDataSets(dataSource, metaDataCategories, dataImporterSettings, configuration.Id), missingSheets);
      }
   }

   public class MappingProblem
   {
      public IReadOnlyList<string> MissingMapping { get; set; }
      public IReadOnlyList<string> MissingUnit { get; set; }
   }
}
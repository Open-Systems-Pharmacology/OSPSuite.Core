using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;


namespace OSPSuite.Starter.Tasks
{
   public interface IImporterConfigurationDataGenerator
   {
      void AddMoleculeValuesToMetaDataList(IList<MetaDataCategory> metaDataCategories);
      void AddOrganValuesToMetaDataList(IList<MetaDataCategory> metaDataCategories);
      IReadOnlyList<ColumnInfo> DefaultPKSimConcentrationImportConfiguration();
      IReadOnlyList<ColumnInfo> DefaultTestConcentrationImportConfiguration();
      IReadOnlyList<MetaDataCategory> DefaultTestMetaDataCategories();
      IReadOnlyList<ColumnInfo> DefaultGroupByConcentrationImportConfiguration();
      IReadOnlyList<MetaDataCategory> DefaultGroupByTestMetaDataCategories();
      IReadOnlyList<ColumnInfo> GetOntogenyColumnInfo();
      IReadOnlyList<MetaDataCategory> DefaultMoBiMetaDataCategories();
      IReadOnlyList<ColumnInfo> DefaultMoBiConcentrationImportConfiguration();
   }

   public class ImporterConfigurationDataGenerator : IImporterConfigurationDataGenerator
   {
      private static readonly IDimensionFactory _dimensionFactory = IoC.Resolve<IDimensionFactory>();
      private readonly IDimension _molarConcentrationDimension;
      private readonly IDimension _massConcentrationDimension;
      private readonly IDimension _ageInYearsDimension;

      public ImporterConfigurationDataGenerator()
      {
         _molarConcentrationDimension = _dimensionFactory.Dimension("Concentration (molar)");
         _massConcentrationDimension = _dimensionFactory.Dimension("Concentration (mass)");
         _ageInYearsDimension = _dimensionFactory.Dimension("Age in years");
      }

      public IReadOnlyList<ColumnInfo> DefaultMoBiConcentrationImportConfiguration()
      {
         var columns = new List<ColumnInfo>();

         var timeDimension = _dimensionFactory.Dimension("Time");
         var timeColumn = new ColumnInfo
         {
            DefaultDimension = timeDimension,
            Name = "Time",
            DisplayName = "Time",
            IsMandatory = true,
         };

         timeColumn.SupportedDimensions.Add(timeDimension);
         columns.Add(timeColumn);

         var mainDimension = _dimensionFactory.Dimension("Concentration (molar)");
         var noDimension = _dimensionFactory.Dimension("Dimensionless");
         var measurementInfo = new ColumnInfo
         {
            DefaultDimension = mainDimension,
            Name = "Measurement",
            DisplayName = "Measurement",
            IsMandatory = true,
            BaseGridName = timeColumn.Name
         };
         foreach (var dimension in _dimensionFactory.Dimensions)
         {
            if (dimension.Equals(timeDimension)) continue;
            if (dimension.Equals(noDimension)) continue;
            measurementInfo.SupportedDimensions.Add(dimension);
         }
         columns.Add(measurementInfo);

         var errorInfo = new ColumnInfo
         {
            DefaultDimension = mainDimension,
            Name = "Error",
            DisplayName = "Error",
            IsMandatory = false,
            BaseGridName = timeColumn.Name,
            RelatedColumnOf = measurementInfo.Name
         };
         foreach (var dimension in _dimensionFactory.Dimensions)
         {
            if (dimension.Equals(timeDimension)) continue;
            errorInfo.SupportedDimensions.Add(dimension);
         }
         columns.Add(errorInfo);

         return columns;
      }

      public IReadOnlyList<ColumnInfo> DefaultPKSimConcentrationImportConfiguration()
      {
         var columns = new List<ColumnInfo>();
         var timeColumn = createTimeColumn();

         columns.Add(timeColumn);

         var concentrationInfo = createConcentrationColumn(timeColumn);

         columns.Add(concentrationInfo);

         var errorInfo = createErrorColumn(timeColumn, concentrationInfo);

         columns.Add(errorInfo);

         return columns;
      }

      public IReadOnlyList<MetaDataCategory> DefaultMoBiMetaDataCategories()
      {
         var categories = new List<MetaDataCategory>();
         var molWeightCategory = new MetaDataCategory
         {
            Name = Constants.ObservedData.MOLECULARWEIGHT,
            DisplayName = Constants.ObservedData.MOLECULARWEIGHT,
            Description = Constants.ObservedData.MOLECULARWEIGHT,
            MetaDataType = typeof(double),
            IsMandatory = false,
            MinValue = 0,
            MinValueAllowed = false
         };
         categories.Add(molWeightCategory);


         var organCategory = createMetaDataCategory<string>(Constants.ObservedData.ORGAN, isMandatory: false, isListOfValuesFixed: true,
            fixedValuesRetriever: addUndefinedValueTo);
         categories.Add(organCategory);

         var compartmentCategory = createMetaDataCategory<string>(Constants.ObservedData.COMPARTMENT, isMandatory: false, isListOfValuesFixed: true,
            fixedValuesRetriever: addUndefinedValueTo);
         categories.Add(compartmentCategory);

         var moleculeCategory = createMetaDataCategory<string>(Constants.ObservedData.MOLECULE, isMandatory: false, isListOfValuesFixed: true,
            fixedValuesRetriever: addUndefinedValueTo);
         categories.Add(moleculeCategory);

         return categories;
      }

      private void addUndefinedValueTo(MetaDataCategory metaDataCategory)
      {
         metaDataCategory.ListOfValues.Add("Undefined", "Undefined");
      }

      public void AddMoleculeValuesToMetaDataList(IList<MetaDataCategory> metaDataCategories)
      {
         var metaDataCategory = metaDataCategories.FindByName(Constants.ObservedData.MOLECULE);
         metaDataCategory.IsListOfValuesFixed = true;
         metaDataCategory.DefaultValue = "JustOne";
         metaDataCategory.ListOfValues.Add("JustOne", "22");
         metaDataCategory.ListOfValues.Add("Skin", "20");
         metaDataCategory.ShouldListOfValuesBeIncluded = true;
         metaDataCategory.SelectDefaultValue = true;
      }

      public void AddOrganValuesToMetaDataList(IList<MetaDataCategory> metaDataCategories)
      {
         var metaDataCategory = metaDataCategories.FindByName(Constants.ObservedData.ORGAN);
         metaDataCategory.IsListOfValuesFixed = true;
         metaDataCategory.DefaultValue = "VenousBlood";
         metaDataCategory.ListOfValues.Add("VenousBlood", "Venous Blood");
         metaDataCategory.ListOfValues.Add("ArterialBlood", "Arterial Blood");
         metaDataCategory.ListOfValues.Add("PeripherialVenousBlood", "Peripherial Venous Blood");
         metaDataCategory.ListOfValues.Add("Skin", "Skin");
         metaDataCategory.ShouldListOfValuesBeIncluded = true;
         metaDataCategory.SelectDefaultValue = true;
      }

      private static MetaDataCategory getOrganCategory()
      {
         var organCategory = createMetaDataCategory<string>("Organ", isMandatory: true, isListOfValuesFixed: true, fixedValuesRetriever: category =>
         {
            category.ListOfValues.Add("VenousBloodPlasma", "Venous Blood Plasma");
            category.ListOfValues.Add("ArterialBloodPlasma", "Arterial Blood Plasma");
            category.ListOfValues.Add("PeripherialVenousBloodPlasma", "Peripherial Venous Blood Plasma");
            category.ListOfValues.Add("Urine", "Urine");
         });

         organCategory.Description = "Organ";
         organCategory.ShouldListOfValuesBeIncluded = true;
         return organCategory;
      }

      public IReadOnlyList<MetaDataCategory> DefaultGroupByTestMetaDataCategories()
      {
         var categories = new List<MetaDataCategory>
         {
            createMetaDataCategory<string>("Category", isMandatory: true, isListOfValuesFixed: true, fixedValuesRetriever: category =>
            {
               category.ListOfValues.Add("A", "A");
               category.ListOfValues.Add("B", "B");
               category.ListOfValues.Add("C", "C");
            }),

            createMetaDataCategory<bool>("Tested?", isMandatory: true)
         };

         return categories;
      }

      public IReadOnlyList<ColumnInfo> DefaultGroupByConcentrationImportConfiguration()
      {
         var columns = new List<ColumnInfo>();
         var timeColumn = createTimeColumn();

         createMetaData().Each(category => timeColumn.MetaDataCategories.Add(category));
         columns.Add(timeColumn);

         var concentrationColumn = createConcentrationColumn(timeColumn);
         createMetaData().Each(category => concentrationColumn.MetaDataCategories.Add(category));

         columns.Add(concentrationColumn);

         return columns;
      }

      private static MetaDataCategory createMetaDataCategory<T>(string descriptiveName, bool isMandatory = false, bool isListOfValuesFixed = false, Action<MetaDataCategory> fixedValuesRetriever = null)
      {
         var category = new MetaDataCategory
         {
            Name = descriptiveName,
            DisplayName = descriptiveName,
            Description = descriptiveName,
            MetaDataType = typeof(T),
            IsMandatory = isMandatory,
            IsListOfValuesFixed = isListOfValuesFixed
         };

         fixedValuesRetriever?.Invoke(category);

         return category;
      }

      private static ColumnInfo createTimeColumn()
      {
         var timeColumn = new ColumnInfo
         {
            DefaultDimension = _dimensionFactory.Dimension("Time"),
            Name = "Time",
            DisplayName = "Time",
            IsMandatory = true,
         };

         timeColumn.SupportedDimensions.Add(_dimensionFactory.Dimension("Time"));
         return timeColumn;
      }

      public IReadOnlyList<MetaDataCategory> DefaultTestMetaDataCategories()
      {
         var categories = new List<MetaDataCategory>();

         var gender = createMetaDataCategory<string>(descriptiveName: "Gender", isMandatory: false, isListOfValuesFixed: true, fixedValuesRetriever: category =>
         {
            category.ListOfValues.Add("Male", "Male");
            category.ListOfValues.Add("Female", "Female");
            category.ListOfValues.Add("Mixed", "Mixed");
            category.ListOfValues.Add("Unspecified", "Unspecified");
         });
         categories.Add(gender);

         var molecule = createMetaDataCategory<string>(descriptiveName: "MoleculeName", isMandatory: true);
         categories.Add(molecule);

         categories.Add(createMetaDataCategory<double>(descriptiveName: "MolWeight", isMandatory: true));

         categories.Add(speciesMetaDataCategory());

         var tissue = createMetaDataCategory<string>(descriptiveName: "Tissue", isMandatory: false);
         categories.Add(tissue);

         categories.Add(getOrganCategory());

         categories.Add(getRemarkCategory());

         return categories;
      }

      private static IEnumerable<MetaDataCategory> createMetaData()
      {
         var images = new Dictionary<string, string>();
         var path = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location);

         var genderCategory = createMetaDataCategory<string>(descriptiveName: "Gender", isMandatory: false, isListOfValuesFixed: true, fixedValuesRetriever: category =>
         {
            category.ListOfValues.Add("Male", "Male");
            category.ListOfValues.Add("Female", "Female");
         });
         images.Each(image => genderCategory.ListOfImages.Add(image.Key, image.Value));
         yield return genderCategory;

         yield return speciesMetaDataCategory();

         yield return createMetaDataCategory<DateTime>(descriptiveName: "Date of Measurement", isMandatory: true);

         yield return createMetaDataCategory<bool>(descriptiveName: "GMP relavent", isMandatory: true);

         var refValue = createMetaDataCategory<double>(descriptiveName: "Reference Value");
         refValue.MinValue = 0;
         refValue.MinValueAllowed = false;
         refValue.MaxValue = 1;
         refValue.MaxValueAllowed = true;
         yield return refValue;

         var measurements = createMetaDataCategory<int>(descriptiveName: "Number of Measurements", isMandatory: false);
         measurements.MinValue = 0;
         measurements.MinValueAllowed = true;
         measurements.MaxValue = 6;
         yield return measurements;

         var remark = getRemarkCategory();
         yield return remark;
      }

      private static MetaDataCategory getRemarkCategory()
      {
         var remark = createMetaDataCategory<string>(descriptiveName: "Remark", isMandatory: false);
         remark.MaxLength = 255;
         return remark;
      }

      private static MetaDataCategory speciesMetaDataCategory()
      {
         var speciesCategory = createMetaDataCategory<string>(descriptiveName: "Species", isMandatory: true, isListOfValuesFixed: false);
         new Dictionary<string, string>
         {
            {"Human", "Human"},
            {"Dog", "Dog"},
            {"Mouse", "Mouse"},
            {"Rat", "Rat"},
            {"MiniPig", "Mini Pig"}
         }.Each(item => speciesCategory.ListOfValues.Add(item.Key, item.Value));
         return speciesCategory;
      }

      private ColumnInfo createErrorColumn(ColumnInfo timeColumn, ColumnInfo concentrationInfo)
      {
         var errorInfo = new ColumnInfo
         {
            DefaultDimension = _molarConcentrationDimension,
            Name = "Error",
            DisplayName = "Error",
            IsMandatory = false,
            BaseGridName = timeColumn.Name,
            RelatedColumnOf = concentrationInfo.Name
         };

         errorInfo.SupportedDimensions.Add(_molarConcentrationDimension);
         errorInfo.SupportedDimensions.Add(_massConcentrationDimension);
         errorInfo.SupportedDimensions.Add(_dimensionFactory.NoDimension);
         return errorInfo;
      }

      private ColumnInfo createConcentrationColumn(ColumnInfo timeColumn)
      {
         var concentrationInfo = new ColumnInfo
         {
            DefaultDimension = _molarConcentrationDimension,
            Name = "Concentration",
            DisplayName = "Concentration",
            IsMandatory = true,
            BaseGridName = timeColumn.Name
         };

         concentrationInfo.SupportedDimensions.Add(_molarConcentrationDimension);
         concentrationInfo.SupportedDimensions.Add(_massConcentrationDimension);
         return concentrationInfo;
      }

      public IReadOnlyList<ColumnInfo> DefaultTestConcentrationImportConfiguration()
      {
         var columnInfos = new List<ColumnInfo>();
         var timeColumn = createTimeColumn();
         createMetaData().Each(category => timeColumn.MetaDataCategories.Add(category));
         columnInfos.Add(timeColumn);

         columnInfos.Add(createConcentrationColumn(timeColumn));

         var categoryColum = new ColumnInfo
         {
            IsMandatory = false,
            BaseGridName = timeColumn.Name,
            DefaultDimension = _dimensionFactory.Dimension("Dimensionless"),
            Name = "Category",
            DisplayName = "Categorie of Experiment",
         };
         categoryColum.SupportedDimensions.Add(_dimensionFactory.Dimension("Dimensionless"));
         columnInfos.Add(categoryColum);

         var dateCategory = new ColumnInfo
         {
            BaseGridName = timeColumn.Name,
            IsMandatory = false,
            DefaultDimension = _dimensionFactory.Dimension("Dimensionless"),
            Name = "DateofMeasurement",
            DisplayName = "Date of Measurement",
         };
         dateCategory.SupportedDimensions.Add(_dimensionFactory.Dimension("Dimensionless"));
         columnInfos.Add(dateCategory);


         var releasedColumn = new ColumnInfo
         {
            BaseGridName = timeColumn.Name,
            IsMandatory = false,
            DefaultDimension = _dimensionFactory.Dimension("Dimensionless"),
            Name = "Released?",
            DisplayName = "Approved?",
         };
         releasedColumn.SupportedDimensions.Add(_dimensionFactory.Dimension("Dimensionless"));
         columnInfos.Add(releasedColumn);
         return columnInfos;
      }

      public IReadOnlyList<ColumnInfo> GetOntogenyColumnInfo()
      {
         var columns = new List<ColumnInfo>();

         var ageColumn = new ColumnInfo
         {
            DefaultDimension = _ageInYearsDimension,
            Name = "Post Menstrual Age",
            DisplayName = "Post Menstrual Age",
            IsMandatory = true,
         };


         ageColumn.SupportedDimensions.Add(_ageInYearsDimension);
         columns.Add(ageColumn);

         var ontogenyFactor = new ColumnInfo
         {
            DefaultDimension = _dimensionFactory.NoDimension,
            Name = "Ontogeny Factor",
            DisplayName = "Post Menstrual Age",
            IsMandatory = true,
            BaseGridName = ageColumn.Name,
         };
         ontogenyFactor.SupportedDimensions.Add(_dimensionFactory.NoDimension);
         columns.Add(ontogenyFactor);

         var geoMean = new ColumnInfo
         {
            DefaultDimension = _dimensionFactory.NoDimension,
            Name = "Standard Deviation",
            DisplayName = "Standard Deviation",
            IsMandatory = false,
            BaseGridName = ageColumn.Name,
            RelatedColumnOf = ontogenyFactor.Name
         };

         geoMean.SupportedDimensions.Add(_dimensionFactory.NoDimension);
         columns.Add(geoMean);

         return columns;
      }
   }
}
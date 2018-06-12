using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Importer;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Starter.Tasks
{
   public interface IImporterConfigurationDataGenerator
   {
      IReadOnlyList<ColumnInfo> DefaultPKSimConcentrationImportConfiguration();
      IReadOnlyList<MetaDataCategory> DefaultPKSimMetaDataCategories();
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
            Description = "Time",
            DisplayName = "Time",
            IsMandatory = true,
            NullValuesHandling = NullValuesHandlingType.DeleteRow,
         };

         timeColumn.DimensionInfos.Add(new DimensionInfo {Dimension = timeDimension, IsMainDimension = true});
         columns.Add(timeColumn);

         var mainDimension = _dimensionFactory.Dimension("Concentration (molar)");
         var noDimension = _dimensionFactory.Dimension("Dimensionless");
         var measurementInfo = new ColumnInfo
         {
            DefaultDimension = mainDimension,
            Name = "Measurement",
            Description = "Measurement",
            DisplayName = "Measurement",
            IsMandatory = true,
            NullValuesHandling = NullValuesHandlingType.DeleteRow,
            BaseGridName = timeColumn.Name
         };
         foreach (var dimension in _dimensionFactory.Dimensions)
         {
            if (dimension.Equals(timeDimension)) continue;
            if (dimension.Equals(noDimension)) continue;
            measurementInfo.DimensionInfos.Add(new DimensionInfo
            {
               Dimension = dimension,
               IsMainDimension = dimension.Equals(mainDimension)
            });
         }
         columns.Add(measurementInfo);

         var errorInfo = new ColumnInfo
         {
            DefaultDimension = mainDimension,
            Name = "Error",
            Description = "Error",
            DisplayName = "Error",
            IsMandatory = false,
            NullValuesHandling = NullValuesHandlingType.Allowed,
            BaseGridName = timeColumn.Name,
            RelatedColumnOf = measurementInfo.Name
         };
         foreach (var dimension in _dimensionFactory.Dimensions)
         {
            if (dimension.Equals(timeDimension)) continue;
            errorInfo.DimensionInfos.Add(new DimensionInfo
            {
               Dimension = dimension,
               IsMainDimension = dimension.Equals(mainDimension)
            });
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
            Name = "MolWeight",
            DisplayName = "Molecular weight [Molecular weight]",
            Description = "Molecular weight",
            MetaDataType = typeof(double),
            IsMandatory = false,
            MinValue = 0,
            MinValueAllowed = false
         };
         categories.Add(molWeightCategory);


         var organCategory = createMetaDataCategory<string>(ObservedData.ORGAN, isMandatory: false, isListOfValuesFixed: true,
            fixedValuesRetriever: addUndefinedValueTo);
         categories.Add(organCategory);

         var compartmentCategory = createMetaDataCategory<string>(ObservedData.COMPARTMENT, isMandatory: false, isListOfValuesFixed: true,
            fixedValuesRetriever: addUndefinedValueTo);
         categories.Add(compartmentCategory);

         var moleculeCategory = createMetaDataCategory<string>(ObservedData.MOLECULE, isMandatory: false, isListOfValuesFixed: true,
            fixedValuesRetriever: addUndefinedValueTo);
         categories.Add(moleculeCategory);

         return categories;
      }

      private void addUndefinedValueTo(MetaDataCategory metaDataCategory)
      {
         metaDataCategory.ListOfValues.Add("Undefined", "Undefined");
      }

      public IReadOnlyList<MetaDataCategory> DefaultPKSimMetaDataCategories()
      {
         var categories = new List<MetaDataCategory>();

         var speciesCategory = speciesMetaDataCategory();
         categories.Add(speciesCategory);

         var organCategory = getOrganCategory();
         categories.Add(organCategory);

         var compCategory = getCompartmentCategory();
         categories.Add(compCategory);

         var concentrationCategory = getConcentrationCategory();
         categories.Add(concentrationCategory);

         categories.Add(createMetaDataCategory<string>("Study Id"));
         categories.Add(createMetaDataCategory<string>("Gender"));
         categories.Add(createMetaDataCategory<string>("Dose"));
         categories.Add(createMetaDataCategory<string>("Route"));
         categories.Add(createMetaDataCategory<string>("Patient Id"));

         return categories;
      }

      private static MetaDataCategory getConcentrationCategory()
      {
         var metaDataCategory = createMetaDataCategory<string>("Molecule", isMandatory: true);
         metaDataCategory.IsListOfValuesFixed = true;
         metaDataCategory.DefaultValue = "JustOne";
         metaDataCategory.ListOfValues.Add("JustOne", "JustOne");
         return metaDataCategory;
      }

      private static MetaDataCategory getCompartmentCategory()
      {
         var compartmentCategory = createMetaDataCategory<string>("Compartment", isMandatory: true, isListOfValuesFixed: true, fixedValuesRetriever: category =>
         {
            category.ListOfValues.Add("Tissue", "Tissue");
            category.ListOfValues.Add("Interstitial", "Interstitial");
            category.ListOfValues.Add("Intracellular", "Intracellular");
            category.ListOfValues.Add("Urine", "Urine");
         });
         compartmentCategory.Description = "Compartment";
         return compartmentCategory;
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
            Description = "Time",
            DisplayName = "Time",
            IsMandatory = true,
            NullValuesHandling = NullValuesHandlingType.DeleteRow,
         };

         timeColumn.DimensionInfos.Add(new DimensionInfo {Dimension = _dimensionFactory.Dimension("Time"), IsMainDimension = true});
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
         var images = new Dictionary<string, ApplicationIcon>();
         var path = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location);

         if (path != null)
         {
            var resourcePath = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\Dev\OSPSuite.Resources\Icons\"));

            var maleIcon =
               new Icon(Path.Combine(resourcePath, "MetaData.ico"));
            images.Add("Male", new ApplicationIcon(maleIcon));
            var femaleIcon =
               new Icon(Path.Combine(resourcePath, "UnitInformation.ico"));
            images.Add("Female", new ApplicationIcon(femaleIcon));
         }

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
            Description = "Error",
            DisplayName = "Error",
            IsMandatory = false,
            NullValuesHandling = NullValuesHandlingType.Allowed,
            BaseGridName = timeColumn.Name,
            RelatedColumnOf = concentrationInfo.Name
         };

         errorInfo.DimensionInfos.Add(new DimensionInfo {Dimension = _molarConcentrationDimension, IsMainDimension = true});
         errorInfo.DimensionInfos.Add(new DimensionInfo {Dimension = _massConcentrationDimension, IsMainDimension = false});
         errorInfo.DimensionInfos.Add(new DimensionInfo {Dimension = _dimensionFactory.NoDimension, IsMainDimension = false});
         return errorInfo;
      }

      private ColumnInfo createConcentrationColumn(ColumnInfo timeColumn)
      {
         var concentrationInfo = new ColumnInfo
         {
            DefaultDimension = _molarConcentrationDimension,
            Name = "Concentration",
            Description = "Concentration",
            DisplayName = "Concentration",
            IsMandatory = true,
            NullValuesHandling = NullValuesHandlingType.DeleteRow,
            BaseGridName = timeColumn.Name
         };

         concentrationInfo.DimensionInfos.Add(new DimensionInfo {Dimension = _molarConcentrationDimension, IsMainDimension = true});
         concentrationInfo.DimensionInfos.Add(new DimensionInfo {Dimension = _massConcentrationDimension, IsMainDimension = false});
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
            Description = "The category of the experiment.",
            DisplayName = "Categorie of Experiment",
            NullValuesHandling = NullValuesHandlingType.Allowed,
            DataType = typeof(string)
         };
         categoryColum.DimensionInfos.Add(new DimensionInfo {Dimension = _dimensionFactory.Dimension("Dimensionless"), IsMainDimension = true});
         columnInfos.Add(categoryColum);

         var dateCategory = new ColumnInfo
         {
            BaseGridName = timeColumn.Name,
            IsMandatory = false,
            DefaultDimension = _dimensionFactory.Dimension("Dimensionless"),
            Name = "DateofMeasurement",
            DisplayName = "Date of Measurement",
            Description = "The date when the measurement was made",
            DataType = typeof(DateTime)
         };
         dateCategory.DimensionInfos.Add(new DimensionInfo {Dimension = _dimensionFactory.Dimension("Dimensionless"), IsMainDimension = true});
         columnInfos.Add(dateCategory);


         var releasedColumn = new ColumnInfo
         {
            BaseGridName = timeColumn.Name,
            IsMandatory = false,
            DefaultDimension = _dimensionFactory.Dimension("Dimensionless"),
            Name = "Released?",
            DisplayName = "Approved?",
            Description = "Has the measurement been approved?",
            DataType = typeof(bool)
         };
         releasedColumn.DimensionInfos.Add(new DimensionInfo {Dimension = _dimensionFactory.Dimension("Dimensionless"), IsMainDimension = true});
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
            Description = "Post Menstrual Age",
            DisplayName = "Post Menstrual Age",
            IsMandatory = true,
            NullValuesHandling = NullValuesHandlingType.DeleteRow,
         };


         ageColumn.DimensionInfos.Add(new DimensionInfo {Dimension = _ageInYearsDimension, IsMainDimension = true});
         columns.Add(ageColumn);

         var ontogenyFactor = new ColumnInfo
         {
            DefaultDimension = _dimensionFactory.NoDimension,
            Name = "Ontogeny Factor",
            Description = "Post Menstrual Age",
            DisplayName = "Post Menstrual Age",
            IsMandatory = true,
            NullValuesHandling = NullValuesHandlingType.DeleteRow,
            BaseGridName = ageColumn.Name,
         };
         ontogenyFactor.DimensionInfos.Add(new DimensionInfo {Dimension = _dimensionFactory.NoDimension, IsMainDimension = true});
         columns.Add(ontogenyFactor);

         var geoMean = new ColumnInfo
         {
            DefaultDimension = _dimensionFactory.NoDimension,
            Name = "Standard Deviation",
            Description = "Standard Deviation",
            DisplayName = "Standard Deviation",
            IsMandatory = false,
            NullValuesHandling = NullValuesHandlingType.Allowed,
            BaseGridName = ageColumn.Name,
            RelatedColumnOf = ontogenyFactor.Name
         };

         geoMean.DimensionInfos.Add(new DimensionInfo {Dimension = _dimensionFactory.NoDimension, IsMainDimension = true});
         columns.Add(geoMean);

         return columns;
      }
   }
}
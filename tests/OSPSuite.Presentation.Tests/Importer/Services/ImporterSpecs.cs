using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Import;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Presentation.Importer.Services 
{
   class ParsedDataSetTest : ParsedDataSet
   {
      public ParsedDataSetTest() : base
         (
            new List<string>(), 
            A.Fake<IUnformattedData>(), 
            new List<UnformattedRow>(), 
            new Dictionary<ExtendedColumn, IList<SimulationPoint>>())
      {
      }

      public void SetDataAndDescription(Dictionary<ExtendedColumn, IList<SimulationPoint>> data, List<InstantiatedMetaData> description)
      {
         Data = data;
         Description = description;
      }
   }
   public abstract class ConcernForImporter : ContextSpecification<OSPSuite.Infrastructure.Import.Services.Importer>
   {
      protected IUnformattedData _basicFormat;
      protected IContainer _container;
      protected IDataSourceFileParser _parser;
      protected IDataSetToDataRepositoryMapper _dataRepositoryMapper;
      protected IDialogCreator _dialogCreator;
      protected ColumnInfoCache _columnInfos;
      protected IDimension _fakedTimeDimension;
      protected IDimension _fakedConcentrationDimension;
      protected IDimension _fakedErrorDimension;
      protected IDimensionFactory _fakedDimensionFactory;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _fakedTimeDimension = A.Fake<IDimension>();
         _fakedConcentrationDimension = A.Fake<IDimension>();
         _fakedErrorDimension = A.Fake<IDimension>();
         _fakedDimensionFactory = A.Fake<IDimensionFactory>();
         _basicFormat = A.Fake<UnformattedData>();
         _container = A.Fake<IContainer>();
         _dialogCreator = A.Fake<IDialogCreator>();
         var dataFormat = A.Fake<IDataFormat>();
         _columnInfos = new ColumnInfoCache
         {
            new ColumnInfo() { DisplayName = "Time" },
            new ColumnInfo() { DisplayName = "Concentration" },
            new ColumnInfo() { DisplayName = "Error" }
         };

         _columnInfos["Time"].SupportedDimensions.Add(_fakedTimeDimension);
         _columnInfos["Concentration"].SupportedDimensions.Add(_fakedConcentrationDimension);
         _columnInfos["Error"].SupportedDimensions.Add(_fakedErrorDimension);

         A.CallTo(() => dataFormat.SetParameters(_basicFormat, _columnInfos, null)).Returns(1);
         A.CallTo(() => _container.ResolveAll<IDataFormat>()).Returns(new List<IDataFormat>() {dataFormat});
         _parser = A.Fake<IDataSourceFileParser>();
         _dataRepositoryMapper = A.Fake<IDataSetToDataRepositoryMapper>();
         A.CallTo(() => _container.Resolve<IDataSourceFileParser>()).Returns(_parser);
         A.CallTo(() => _fakedTimeDimension.HasUnit("min")).Returns(true);
         A.CallTo(() => _fakedConcentrationDimension.HasUnit("pmol/l")).Returns(true);
         A.CallTo(() => _fakedErrorDimension.HasUnit("pmol/l")).Returns(true);
         sut = new OSPSuite.Infrastructure.Import.Services.Importer(_container, _parser, _dataRepositoryMapper, _fakedDimensionFactory);
      }
   }

   public class When_checking_data_format : ConcernForImporter
   {
      [TestCase]
      public void identify_basic_format()
      {
         var formats = sut.AvailableFormats(_basicFormat, _columnInfos, null);
         formats.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_getting_name_from_convention : ConcernForImporter
   {
      private string _fileName;
      private string _fileExtension;
      private Cache<string, IDataSet> _dataSets;
      private IEnumerable<MetaDataMappingConverter> _mappings;
      private string _prefix;
      private string _postfix;
      
      protected override void Because()
      {
         base.Because();
         _fileName = "file";
         _fileExtension = "xls";
         var parsedDataSet1 = new ParsedDataSetTest();
         parsedDataSet1.SetDataAndDescription
         (
            A.Fake<Dictionary<ExtendedColumn, IList<SimulationPoint>>>(),
            new List<InstantiatedMetaData>()
            {
               new InstantiatedMetaData()
               {
                  Id = 0,
                  Value = "Value1"
               },
               new InstantiatedMetaData()
               {
                  Id = 1,
                  Value = "Value2"
               }
            }
         );
         var parsedDataSet2 = new ParsedDataSetTest();
         parsedDataSet2.SetDataAndDescription
         (
            A.Fake<Dictionary<ExtendedColumn, IList<SimulationPoint>>>(),
            new List<InstantiatedMetaData>()
            {
               new InstantiatedMetaData()
               {
                  Id = 0,
                  Value = "Value1"
               },
               new InstantiatedMetaData()
               {
                  Id = 1,
                  Value = "Value2"
               }
            }
         );
         _dataSets = new Cache<string, IDataSet>()
         {
            {
               "key1", 
               new DataSet()
            },
            {
               "key2",
               new DataSet()
            }
         };
         _dataSets["key1"].AddData(new List<ParsedDataSet>() { parsedDataSet1 } );
         _dataSets["key2"].AddData(new List<ParsedDataSet>() { parsedDataSet2 });
         _mappings = new List<MetaDataMappingConverter>()
         {
            new MetaDataMappingConverter()
            {
               Id = "Id1",
               Index = (dataSheet) => 0
            },
            new MetaDataMappingConverter()
            {
               Id = "Id2",
               Index = (dataSheet) => 1
            }
         };
         _prefix = "prefix";
         _postfix = "postfix";
      }

      [TestCase]
      public void replaces_filename()
      {
         var names = sut.NamesFromConvention(_prefix + "{Source}" + _postfix, _fileName, _dataSets, _mappings);
         names.Count().ShouldBeEqualTo(2);
         names.ElementAt(0).ShouldBeEqualTo(_prefix + _fileName + _postfix);
      }

      [TestCase]
      public void replaces_sheetname()
      {
         var names = sut.NamesFromConvention(_prefix + "{Sheet}" + _postfix, _fileName, _dataSets, _mappings);
         names.Count().ShouldBeEqualTo(2);
         names.ElementAt(0).ShouldBeEqualTo(_prefix + "key1" + _postfix);
      }

      [TestCase]
      public void replaces_metadata()
      {
         for (var i = 1; i < 3; i++)
         {
            var names = sut.NamesFromConvention(_prefix + $"{{Id{i}}}" + _postfix, _fileName, _dataSets, _mappings);
            names.Count().ShouldBeEqualTo(2);
            names.ElementAt(0).ShouldBeEqualTo(_prefix + $"Value{i}" + _postfix);
         }
      }

      [TestCase]
      public void replaces_filename_without_extension()
      {
         var names = sut.NamesFromConvention(_prefix + "{Source}" + _postfix, $"{_fileName}.{_fileExtension}", _dataSets, _mappings);
         names.Count().ShouldBeEqualTo(2);
         names.ElementAt(0).ShouldBeEqualTo(_prefix + _fileName + _postfix);
      }

      [TestCase]
      public void names_should_be_different()
      {
         var names = sut.NamesFromConvention(_prefix + "{Source}.{Id1}-{Id2}" + _postfix, _fileName, _dataSets, _mappings);
         names.Count().ShouldBeEqualTo(2);
         names.ElementAt(0).ShouldNotBeEqualTo(names.ElementAt(1));
      }
   }

   public abstract class ConcernForImporter2 : ContextSpecification<OSPSuite.Infrastructure.Import.Services.Importer>
   {
      protected IUnformattedData _basicFormat;
      protected IContainer _container;
      protected IDataSourceFileParser _parser;
      protected IDataSetToDataRepositoryMapper _dataRepositoryMapper;
      protected IDialogCreator _dialogCreator;
      protected ColumnInfoCache _columnInfos;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected IDimension _fakedTimeDimension;
      protected IDimension _fakedConcentrationDimension;
      protected IDimension _fakedErrorDimension;
      protected IDimensionFactory _fakedDimensionFactory;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _fakedTimeDimension = A.Fake<IDimension>();
         _fakedConcentrationDimension = A.Fake<IDimension>();
         _fakedErrorDimension = A.Fake<IDimension>();
         _container = A.Fake<IContainer>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _fakedDimensionFactory = A.Fake<IDimensionFactory>();
         var dataFormat = A.Fake<IDataFormat>();
         _columnInfos = new ColumnInfoCache()
         {
            new ColumnInfo() { DisplayName = "Time" },
            new ColumnInfo() { DisplayName = "Concentration" },
            new ColumnInfo() { DisplayName = "Error" }
         };
         _metaDataCategories = new List<MetaDataCategory>()
         {
            new MetaDataCategory() {Name = "Organ"},
            new MetaDataCategory() {Name = "Compartment"},
            new MetaDataCategory() {Name = "Species"},
            new MetaDataCategory() {Name = "Dose"},
            new MetaDataCategory() {Name = "Molecule"},
            new MetaDataCategory() {Name = "Route"}
         };

         _columnInfos["Time"].SupportedDimensions.Add(_fakedTimeDimension);
         _columnInfos["Concentration"].SupportedDimensions.Add(_fakedConcentrationDimension);
         _columnInfos["Error"].SupportedDimensions.Add(_fakedErrorDimension);

         A.CallTo(() => _fakedTimeDimension.HasUnit("min")).Returns(true);
         A.CallTo(() => _fakedConcentrationDimension.HasUnit("pmol/l")).Returns(true);
         A.CallTo(() => _fakedErrorDimension.HasUnit("pmol/l")).Returns(true);
         A.CallTo(() => _container.ResolveAll<IDataFormat>()).Returns(new List<IDataFormat>()
            {new DataFormatHeadersWithUnits(), new DataFormatNonmem(), new MixColumnsDataFormat()});
         _parser = A.Fake<IDataSourceFileParser>();
         _dataRepositoryMapper = A.Fake<IDataSetToDataRepositoryMapper>();
         A.CallTo(() => _container.Resolve<IDataSourceFileParser>()).Returns(_parser);
         sut = new OSPSuite.Infrastructure.Import.Services.Importer(_container, _parser, _dataRepositoryMapper, _fakedDimensionFactory);
      }
   }

   public class When_listing_available_formats_on_simple_format : ConcernForImporter2
   {
      protected override void Because()
      {
         _basicFormat = new TestUnformattedData
         (
            new Cache<string, ColumnDescription>()
            {
               {
                  "Organ",
                  new ColumnDescription(0, new List<string>() { "PeripheralVenousBlood" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Time [min]",
                  new ColumnDescription(5, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Concentration [pmol/l]",
                  new ColumnDescription(6, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Error [pmol/l]",
                  new ColumnDescription(7, null, ColumnDescription.MeasurementLevel.Numeric)
               }
            }
         );
      }

      [TestCase]
      public void rank_columns_format_highest_for_its_format()
      {
         var formats = sut.AvailableFormats(_basicFormat, _columnInfos, _metaDataCategories);
         formats.First().ShouldBeAnInstanceOf(typeof(DataFormatHeadersWithUnits));
      }
   }

   public class When_listing_available_formats_on_nonmem_format : ConcernForImporter2
   {
      protected override void Because()
      {
         _basicFormat = new TestUnformattedData
         (
            new Cache<string, ColumnDescription>()
            {
               {
                  "Organ",
                  new ColumnDescription(0, new List<string>() { "PeripheralVenousBlood" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Time",
                  new ColumnDescription(5, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Time_unit",
                  new ColumnDescription(5, new List<string>() { "min" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "lloq",
                  new ColumnDescription(5, null, ColumnDescription.MeasurementLevel.Numeric)
                  
               },
               {
                  "Concentration",
                  new ColumnDescription(6, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Concentration_unit",
                  new ColumnDescription(5, new List<string>() { "pmol/l" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Error",
                  new ColumnDescription(7, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Error_unit",
                  new ColumnDescription(5, new List<string>() { "pmol/l" }, ColumnDescription.MeasurementLevel.Discrete)
               }
            }
         );
      }

      [TestCase]
      public void rank_nonmem_highest_for_its_format()
      {
         var formats = sut.AvailableFormats(_basicFormat, _columnInfos, _metaDataCategories);
         formats.First().ShouldBeAnInstanceOf(typeof(DataFormatNonmem));
      }
   }

   public class When_listing_available_formats_on_mixin_format : ConcernForImporter2
   {
      protected override void Because()
      {
         _basicFormat = new TestUnformattedData
         (
            new Cache<string, ColumnDescription>()
            {
               {
                  "Organ",
                  new ColumnDescription(0, new List<string>() { "PeripheralVenousBlood" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Time [min]",
                  new ColumnDescription(5, null,ColumnDescription.MeasurementLevel.Numeric )
               },
               {
                  "lloq",
                  new ColumnDescription(5, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Concentration",
                  new ColumnDescription(6, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Concentration_unit",
                  new ColumnDescription(5, new List<string>() { "pmol/l" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Error",
                  new ColumnDescription(7, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Error_unit",
                  new ColumnDescription(5, new List<string>() { "pmol/l" }, ColumnDescription.MeasurementLevel.Discrete)
                  
               }
            }
         );
      }

      [TestCase]
      public void rank_mixin_highest_for_its_format()
      {
         var formats = sut.AvailableFormats(_basicFormat, _columnInfos, _metaDataCategories);
         formats.First().ShouldBeAnInstanceOf(typeof(MixColumnsDataFormat));
      }
   }

   public class When_listing_available_formats_on_mixin_format_with_invalid_unit : ConcernForImporter2
   {
      protected override void Because()
      {
         _basicFormat = new TestUnformattedData
         (
            new Cache<string, ColumnDescription>()
            {
               {
                  "Time [invalidUnit]",
                  new ColumnDescription(5, null,ColumnDescription.MeasurementLevel.Numeric )
               },
               {
                  "Concentration",
                  new ColumnDescription(6, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Concentration_unit",
                  new ColumnDescription(5, new List<string>() { "pmol/l" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Error",
                  new ColumnDescription(7, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Error_unit",
                  new ColumnDescription(5, new List<string>() { "pmol/l" }, ColumnDescription.MeasurementLevel.Discrete)

               }
            }
         );
      }

      [TestCase]
      public void the_invalid_unit_is_detected()
      {
         var formats = sut.AvailableFormats(_basicFormat, _columnInfos, _metaDataCategories);
         (formats.First().Parameters.First(parameter => parameter.ColumnName == "Time [invalidUnit]") as MappingDataFormatParameter).MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo(UnitDescription.InvalidUnit);
      }
   }
}

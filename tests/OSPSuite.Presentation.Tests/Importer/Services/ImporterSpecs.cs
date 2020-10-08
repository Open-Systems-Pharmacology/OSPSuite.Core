using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Importer;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraCharts.Designer.Native;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer.Services 
{
   class ParsedDataSetTest : ParsedDataSet
   {
      public ParsedDataSetTest() : base
         (
            new List<(string ColumnName, IList<string> ExistingValues)>(), 
            A.Fake<IUnformattedData>(), 
            new List<IEnumerable<string>>(), 
            new Dictionary<Column, IList<ValueAndLloq>>())
      {
      }

      public void SetDataAndDescription(Dictionary<Column, IList<ValueAndLloq>> data, List<InstantiatedMetaData> description)
      {
         Data = data;
         Description = description;
      }
   }
   public abstract class ConcernForImporter : ContextSpecification<Importer>
   {
      protected IUnformattedData _basicFormat;
      protected IContainer _container;
      protected IDataSourceFileParser _parser;
      protected IDialogCreator _dialogCreator;
      protected IReadOnlyList<ColumnInfo> _columnInfos;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _basicFormat = A.Fake<UnformattedData>();
         _container = A.Fake<IContainer>();
         _dialogCreator = A.Fake<IDialogCreator>();
         var dataFormat = A.Fake<IDataFormat>();
         _columnInfos = new List<ColumnInfo>()
         {
            new ColumnInfo() { DisplayName = "Time" },
            new ColumnInfo() { DisplayName = "Concentration" },
            new ColumnInfo() { DisplayName = "Error" }
         };

         A.CallTo(() => dataFormat.SetParameters(_basicFormat, _columnInfos)).Returns(1);
         A.CallTo(() => _container.ResolveAll<IDataFormat>()).Returns(new List<IDataFormat>() {dataFormat});
         _parser = A.Fake<IDataSourceFileParser>();
         A.CallTo(() => _container.Resolve<IDataSourceFileParser>()).Returns(_parser);
         sut = new Importer(_container, _parser);
      }
   }

   public class When_checking_data_format : ConcernForImporter
   {
      [TestCase]
      public void identify_basic_format()
      {
         var formats = sut.AvailableFormats(_basicFormat, _columnInfos);
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
            A.Fake<Dictionary<Column, IList<ValueAndLloq>>>(),
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
         parsedDataSet1.SetDataAndDescription
         (
            A.Fake<Dictionary<Column, IList<ValueAndLloq>>>(),
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
               {
                  Data = new List<ParsedDataSet>()
                  {
                     parsedDataSet1
                  }
               }
            },
            {
               "key2",
               new DataSet()
               {
                  Data = new List<ParsedDataSet>()
                  {
                     parsedDataSet2
                  }
               }
            }
         };
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
         var names = sut.NamesFromConvention(_prefix + "{File}" + _postfix, _fileName, _dataSets, _mappings);
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
         var names = sut.NamesFromConvention(_prefix + "{File}" + _postfix, $"{_fileName}.{_fileExtension}", _dataSets, _mappings);
         names.Count().ShouldBeEqualTo(2);
         names.ElementAt(0).ShouldBeEqualTo(_prefix + _fileName + _postfix);
      }

      [TestCase]
      public void names_should_be_different()
      {
         var names = sut.NamesFromConvention(_prefix + "{File}.{Id1}-{Id2}" + _postfix, _fileName, _dataSets, _mappings);
         names.Count().ShouldBeEqualTo(2);
         names.ElementAt(0).ShouldNotBeEqualTo(names.ElementAt(1));
      }
   }

   public abstract class ConcernForImporter2 : ContextSpecification<Importer>
   {
      protected IUnformattedData _basicFormat;
      protected IContainer _container;
      protected IDataSourceFileParser _parser;
      protected IDialogCreator _dialogCreator;
      protected IReadOnlyList<ColumnInfo> _columnInfos;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _container = A.Fake<IContainer>();
         _dialogCreator = A.Fake<IDialogCreator>();
         var dataFormat = A.Fake<IDataFormat>();
         _columnInfos = new List<ColumnInfo>()
         {
            new ColumnInfo() { DisplayName = "Time" },
            new ColumnInfo() { DisplayName = "Concentration" },
            new ColumnInfo() { DisplayName = "Error" }
         };

         A.CallTo(() => _container.ResolveAll<IDataFormat>()).Returns(new List<IDataFormat>() { new DataFormat_TMetaData_C(), new DataFormat_Nonmem() });
         _parser = A.Fake<IDataSourceFileParser>();
         A.CallTo(() => _container.Resolve<IDataSourceFileParser>()).Returns(_parser);
         sut = new Importer(_container, _parser);
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
                  new ColumnDescription(0)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "PeripheralVenousBlood" }
                  }
               },
               {
                  "Time [min]",
                  new ColumnDescription(5)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric,
                     ExistingValues = null
                  }
               },
               {
                  "Concentration [pmol/l]",
                  new ColumnDescription(6)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric,
                     ExistingValues = null
                  }
               },
               {
                  "Error [pmol/l]",
                  new ColumnDescription(7)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric,
                     ExistingValues = null
                  }
               }
            }
         );
      }

      [TestCase]
      public void rank_columns_format_highest_for_its_format()
      {
         var formats = sut.AvailableFormats(_basicFormat, _columnInfos);
         formats.First().ShouldBeAnInstanceOf(typeof(DataFormat_TMetaData_C));
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
                  new ColumnDescription(0)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "PeripheralVenousBlood" }
                  }
               },
               {
                  "Time",
                  new ColumnDescription(5)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric,
                     ExistingValues = null
                  }
               },
               {
                  "Time_unit",
                  new ColumnDescription(5)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "min" }
                  }
               },
               {
                  "lloq",
                  new ColumnDescription(5)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric,
                     ExistingValues = null
                  }
               },
               {
                  "Concentration",
                  new ColumnDescription(6)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric,
                     ExistingValues = null
                  }
               },
               {
                  "Concentration_unit",
                  new ColumnDescription(5)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "pmol/l" }
                  }
               },
               {
                  "Error",
                  new ColumnDescription(7)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric,
                     ExistingValues = null
                  }
               },
               {
                  "Error_unit",
                  new ColumnDescription(5)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "pmol/l" }
                  }
               }
            }
         );
      }

      [TestCase]
      public void rank_nonmem_highest_for_its_format()
      {
         var formats = sut.AvailableFormats(_basicFormat, _columnInfos);
         formats.First().ShouldBeAnInstanceOf(typeof(DataFormat_Nonmem));
      }
   }
}

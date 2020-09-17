using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Importer;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.Importer.Services 
{
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

         A.CallTo(() => dataFormat.SetParameters(_basicFormat, _columnInfos)).Returns(true);
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
      private IReadOnlyDictionary<string, IDataSet> _dataSets;
      private IEnumerable<MetaDataMappingConverter> _mappings;
      private string _prefix;
      private string _postfix;

      protected override void Because()
      {
         base.Because();
         _fileName = "file";
         _fileExtension = "xls";
         _dataSets = new Dictionary<string, IDataSet>()
         {
            {
               "key1", 
               new DataSet()
               {
                  Data = new Dictionary<IEnumerable<InstanstiatedMetaData>, Dictionary<Column, IList<ValueAndLloq>>>()
                  {
                     { 
                        new List<InstanstiatedMetaData>()
                        {
                           new InstanstiatedMetaData()
                           {
                              Id = 0,
                              Value = "Value1"
                           },
                           new InstanstiatedMetaData()
                           {
                              Id = 1,
                              Value = "Value2"
                           }
                        },
                        A.Fake<Dictionary<Column, IList<ValueAndLloq>>>()
                     }
                  }
               }
            },
            {
               "key2",
               new DataSet()
               {
                  Data = new Dictionary<IEnumerable<InstanstiatedMetaData>, Dictionary<Column, IList<ValueAndLloq>>>()
                  {
                     {
                        new List<InstanstiatedMetaData>()
                        {
                           new InstanstiatedMetaData()
                           {
                              Id = 0,
                              Value = "Value1"
                           },
                           new InstanstiatedMetaData()
                           {
                              Id = 1,
                              Value = "Value2"
                           }
                        },
                        A.Fake<Dictionary<Column, IList<ValueAndLloq>>>()
                     }
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
}

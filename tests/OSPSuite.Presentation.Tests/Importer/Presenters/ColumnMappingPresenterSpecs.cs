using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Services;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Importer.Presenters 
{
   public abstract class ConcernForColumnMappingPresenter : ContextSpecification<ColumnMappingPresenter>
   {
      protected IDataFormat _basicFormat;
      protected IColumnMappingControl _view;
      protected IImporter _importer;
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _basicFormat = A.Fake<IDataFormat>();
         A.CallTo(() => _basicFormat.Parameters).Returns(new List<DataFormatParameter>() { 
               new MappingDataFormatParameter("Time", new Column() { Name = "Time", Unit = new UnitDescription("min") }),
               new MappingDataFormatParameter("Observation", new Column() { Name = "Concentration", Unit = new UnitDescription("mol/l") }),
               //new MetaDataFormatParameter("Sp", "Species"),
               new GroupByDataFormatParameter("Study id")
            });
         _view = A.Fake<IColumnMappingControl>();
         _importer = A.Fake<IImporter>();
         A.CallTo(() => _importer.CheckWhetherAllDataColumnsAreMapped(A<IReadOnlyList<ColumnInfo>>.Ignored,
            A<IEnumerable<DataFormatParameter>>.Ignored)).Returns(new MappingProblem()
            {MissingMapping = new List<string>(), MissingUnit = new List<string>()});
      }

      protected override void Context()
      {
         base.Context();
         _columnInfos = new List<ColumnInfo>()
         {
            new ColumnInfo() { Name = "Time", IsMandatory = true },
            new ColumnInfo() { Name = "Concentration", IsMandatory = true },
            new ColumnInfo() { Name = "Error", IsMandatory = false }
         };
         _metaDataCategories = new List<MetaDataCategory>()
         {
            new MetaDataCategory()
            {
               Name = "Time",
               IsMandatory = true
            },
            new MetaDataCategory()
            {
               Name = "Concentration",
               IsMandatory = true
            },
            new MetaDataCategory()
            {
               DisplayName = "Error",
               IsMandatory = false
            }
         };
         sut = new ColumnMappingPresenter(_view, _importer, A.Fake<IMappingParameterEditorPresenter>());
      }
   }

   public class When_setting_data_format : ConcernForColumnMappingPresenter
   {
      protected override void Because()
      {
         base.Because();
         sut.SetSettings(_metaDataCategories, _columnInfos);
         sut.SetDataFormat(_basicFormat);
      }

      [TestCase]
      public void identify_basic_format()
      {
         A.CallTo(
            () => _view.SetMappingSource(
               A<IList<ColumnMappingDTO>>.That.Matches(l => 
                  l.Count(m => m.CurrentColumnType == ColumnMappingDTO.ColumnType.Mapping && m.Source is MappingDataFormatParameter && (m.Source as MappingDataFormatParameter).MappedColumn.Name == "Time") == 1 &&
                  l.Count(m => m.CurrentColumnType == ColumnMappingDTO.ColumnType.Mapping && m.Source is MappingDataFormatParameter && (m.Source as MappingDataFormatParameter).MappedColumn.Name == "Concentration") == 1 &&
                  l.Count(m => m.CurrentColumnType == ColumnMappingDTO.ColumnType.GroupBy && m.Source is GroupByDataFormatParameter && (m.Source as GroupByDataFormatParameter).ColumnName == "Study id") == 1
            ))).MustHaveHappened();
      }
   }

   public class When_getting_available_options : ConcernForColumnMappingPresenter
   {
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected DataImporterSettings _dataImporterSettings;

      protected override void Context()
      {
         base.Context();
         _metaDataCategories = A.Fake<IReadOnlyList<MetaDataCategory>>();
         _columnInfos = new List<ColumnInfo>() 
         {
            new ColumnInfo() { DisplayName = "Time" },
            new ColumnInfo() { DisplayName = "Concentration" }
         };
         _dataImporterSettings = A.Fake<DataImporterSettings>();
      }

      protected override void Because()
      {
         base.Because();
         sut.SetSettings(_metaDataCategories, _columnInfos);
         sut.SetDataFormat(_basicFormat);
      }

      /*[TestCase]
      public void fills_correct_options()
      {
         var options = sut.GetAvailableOptionsFor(
      (ColumnMappingDTO.ColumnType.Mapping, "Time", "", A.Fake<DataFormatParameter>(), 0));
         var columnMappingOptions = options.ToList();
         columnMappingOptions.Count().ShouldBeEqualTo(3);
         columnMappingOptions.ElementAt(0).Label.ShouldBeEqualTo("Time(min)");
         columnMappingOptions.ElementAt(1).Label.ShouldBeEqualTo("<None>");
         columnMappingOptions.ElementAt(2).Label.ShouldBeEqualTo("Group by");
      }*/
   }
}

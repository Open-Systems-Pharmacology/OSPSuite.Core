using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Importer.Views;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Presenters 
{
   public abstract class ConcernForColumnMappingPresenter : ContextSpecification<ColumnMappingPresenter>
   {
      protected IDataFormat _basicFormat;
      protected IColumnMappingControl _view;
      protected IImporterTask _importerTask;
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _basicFormat = A.Fake<IDataFormat>();
         A.CallTo(() => _basicFormat.Parameters).Returns(new List<DataFormatParameter>() { 
               new MappingDataFormatParameter("Time", new Column() { Name = "Time", SelectedUnit = "min" }),
               new MappingDataFormatParameter("Observation", new Column() { Name = "Concentration", SelectedUnit = "mol/l" }),
               //new MetaDataFormatParameter("Sp", "Species"),
               new GroupByDataFormatParameter("Study id")
            });
         _view = A.Fake<IColumnMappingControl>();
         _importerTask = A.Fake<IImporterTask>();
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
         sut = new ColumnMappingPresenter(_view, _importerTask, A.Fake<IApplicationController>());
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
               A<IList<ColumnMappingViewModel>>.That.Matches(l => 
                  l.Count(m => m.CurrentColumnType == ColumnMappingViewModel.ColumnType.Mapping && m.Source is MappingDataFormatParameter && (m.Source as MappingDataFormatParameter).MappedColumn.Name == "Time") == 1 &&
                  l.Count(m => m.CurrentColumnType == ColumnMappingViewModel.ColumnType.Mapping && m.Source is MappingDataFormatParameter && (m.Source as MappingDataFormatParameter).MappedColumn.Name == "Concentration") == 1 &&
                  l.Count(m => m.CurrentColumnType == ColumnMappingViewModel.ColumnType.GroupBy && m.Source is GroupByDataFormatParameter && (m.Source as GroupByDataFormatParameter).ColumnName == "Study id") == 1
            ))).MustHaveHappened();
      }
   }

   static public class ColumnMappingViewModelExtensions
   {
      static public bool IsEquivalentTo(this ColumnMappingViewModel self, ColumnMappingViewModel other)
      {
         if (self == null)
            return other == null;
         if (self.MappingName != other.MappingName || self.Description != other.Description)
            return false;
         switch (self.Source)
         {
            case MappingDataFormatParameter mp:
               if (!(other.Source is MappingDataFormatParameter))
                  return false;
               var mmp = other.Source as MappingDataFormatParameter;
               return mp.ColumnName == mmp.ColumnName && ((mp.MappedColumn == null && mmp.MappedColumn == null) || (mp.MappedColumn.Name == mmp.MappedColumn.Name && mp.MappedColumn.SelectedUnit == mmp.MappedColumn.SelectedUnit));
            case MetaDataFormatParameter mp:
               if (!(other.Source is MetaDataFormatParameter))
                  return false;
               var mdmp = other.Source as MetaDataFormatParameter;
               return mp.ColumnName == mdmp.ColumnName && mp.MetaDataId == mdmp.MetaDataId;
            case GroupByDataFormatParameter mp:
               if (!(other.Source is GroupByDataFormatParameter))
                  return false;
               return mp.ColumnName == other.Source.ColumnName;
         }
         return false;
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
         var options = sut.GetAvailableOptionsFor(new ColumnMappingViewModel(ColumnMappingViewModel.ColumnType.Mapping, "Time", "", A.Fake<DataFormatParameter>(), 0));
         var columnMappingOptions = options.ToList();
         columnMappingOptions.Count().ShouldBeEqualTo(3);
         columnMappingOptions.ElementAt(0).Label.ShouldBeEqualTo("Time(min)");
         columnMappingOptions.ElementAt(1).Label.ShouldBeEqualTo("<None>");
         columnMappingOptions.ElementAt(2).Label.ShouldBeEqualTo("Group by");
      }*/
   }
}

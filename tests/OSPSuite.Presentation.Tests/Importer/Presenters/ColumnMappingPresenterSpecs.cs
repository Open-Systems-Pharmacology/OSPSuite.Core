using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Importer.Views.Dialog;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Presenters 
{
   public abstract class ConcernForColumnMappingPresenter : ContextSpecification<ColumnMappingPresenter>
   {
      protected IDataFormat _basicFormat;
      protected IColumnMappingControl _view;
      protected IImporterTask _importerTask;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _basicFormat = A.Fake<IDataFormat>();
         A.CallTo(() => _basicFormat.Parameters).Returns(new List<DataFormatParameter>() { 
               new MappingDataFormatParameter("Time", new Column() { Name = Column.ColumnNames.Time, Unit = "min" }),
               new MappingDataFormatParameter("Observation", new Column() { Name = Column.ColumnNames.Concentration, Unit = "mol/l" }),
               new MetaDataFormatParameter("Sp", "Species"),
               new GroupByDataFormatParameter("Study id")
            });
         _view = A.Fake<IColumnMappingControl>();
         _importerTask = A.Fake<IImporterTask>();
      }

      protected override void Context()
      {
         base.Context();
         sut = new ColumnMappingPresenter(_view, _importerTask, A.Fake<IEmptyDialog>());
      }
   }

   public class When_setting_data_format : ConcernForColumnMappingPresenter
   {
      protected override void Because()
      {
         base.Because();
         sut.SetDataFormat(_basicFormat, new List<IDataFormat>() { _basicFormat }, "");
      }

      [TestCase]
      public void identify_basic_format()
      {
         A.CallTo(
            () => _view.SetMappingSource(
               A<IList<ColumnMappingViewModel>>.That.Matches(l => 
                  l.Count() == 4 &&
                  l.ElementAt(0).Equals(new ColumnMappingViewModel("Time", "Mapping,Time,min", new MappingDataFormatParameter("Time", new Column() { Name = Column.ColumnNames.Time, Unit = "min" }))) &&
                  l.ElementAt(1).Equals(new ColumnMappingViewModel("Observation", "Mapping,Concentration,mol/l", new MappingDataFormatParameter("Observation", new Column() { Name = Column.ColumnNames.Concentration, Unit = "mol/l" }))) &&
                  l.ElementAt(2).Equals(new ColumnMappingViewModel("Sp", "MetaData,Species", new MetaDataFormatParameter("Sp", "Species"))) &&
                  l.ElementAt(3).Equals(new ColumnMappingViewModel("Study id", "GroupBy", new GroupByDataFormatParameter("Study id"))))
            )).MustHaveHappened();
      }
   }

   public class When_getting_available_options : ConcernForColumnMappingPresenter
   {
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected DataImporterSettings dataImporterSettings;

      protected override void Context()
      {
         base.Context();
         _metaDataCategories = A.Fake<IReadOnlyList<MetaDataCategory>>();
         _columnInfos = new List<ColumnInfo>() 
         {
            new ColumnInfo() { DisplayName = "Time" },
            new ColumnInfo() { DisplayName = "Concentration" }
         };
         dataImporterSettings = A.Fake<DataImporterSettings>();
      }

      protected override void Because()
      {
         base.Because();
         sut.SetSettings(_metaDataCategories, _columnInfos, dataImporterSettings);
         sut.SetDataFormat(_basicFormat, new List<IDataFormat>() { _basicFormat }, "");
      }

      [TestCase]
      public void fills_correct_options()
      {
         var options = sut.GetAvailableOptionsFor(new ColumnMappingViewModel("Time", "", A.Fake<DataFormatParameter>()));
         options.Count().ShouldBeEqualTo(3);
         options.ElementAt(0).Label.ShouldBeEqualTo("Time(min)");
         options.ElementAt(1).Label.ShouldBeEqualTo("<None>");
         options.ElementAt(2).Label.ShouldBeEqualTo("Group by");
      }
   }
}

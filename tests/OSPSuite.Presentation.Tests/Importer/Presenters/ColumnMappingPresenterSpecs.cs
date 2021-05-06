using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using System.Linq;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Core.Import;
using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.Importer.Presenters 
{
   public abstract class ConcernForColumnMappingPresenter : ContextSpecification<ColumnMappingPresenter>
   {
      protected IDataFormat _basicFormat;
      protected IColumnMappingView _view;
      protected IImporter _importer;
      protected IMappingParameterEditorPresenter _mappingParameterEditorPresenter;
      protected IMetaDataParameterEditorPresenter _metaDataParameterEditorPresenter;
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected List<DataFormatParameter> _parameters = new List<DataFormatParameter>() 
      {
         new MappingDataFormatParameter("Time", new Column() { Name = "Time", Unit = new UnitDescription("min") }),
         new MappingDataFormatParameter("Observation", new Column() { Name = "Concentration", Unit = new UnitDescription("mol/l") }),
         new MappingDataFormatParameter("Error", new Column() { Name = "Error", Unit = new UnitDescription("?", "") }),
         new GroupByDataFormatParameter("Study id")
      };

      public override void GlobalContext()
      {
         base.GlobalContext();
         _basicFormat = A.Fake<IDataFormat>();
         A.CallTo(() => _basicFormat.Parameters).Returns(_parameters);
         _view = A.Fake<IColumnMappingView>();
         _importer = A.Fake<IImporter>();
         A.CallTo(() => _importer.CheckWhetherAllDataColumnsAreMapped(A<IReadOnlyList<ColumnInfo>>.Ignored,
            A<IEnumerable<DataFormatParameter>>.Ignored)).Returns(new MappingProblem()
            {MissingMapping = new List<string>(), MissingUnit = new List<string>()});
      }

      protected override void Because()
      {
         base.Because();
         sut.SetSettings(_metaDataCategories, _columnInfos);
         sut.SetDataFormat(_basicFormat);
      }

      protected override void Context()
      {
         base.Context();
         _columnInfos = new List<ColumnInfo>()
         {
            new ColumnInfo() { Name = "Time", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo() { Name = "Concentration", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo() { Name = "Error", IsMandatory = false, RelatedColumnOf = "Concentration", BaseGridName = "Time" }
         };
         _metaDataCategories = new List<MetaDataCategory>()
         {
            new MetaDataCategory()
            {
               Name = "Time",
               IsMandatory = true,
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
         _mappingParameterEditorPresenter = A.Fake<IMappingParameterEditorPresenter>();
         _metaDataParameterEditorPresenter = A.Fake<IMetaDataParameterEditorPresenter>();
         sut = new ColumnMappingPresenter(_view, _importer, _mappingParameterEditorPresenter, _metaDataParameterEditorPresenter);
      }
   }

   public class When_setting_data_format : ConcernForColumnMappingPresenter
   {
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

   public class When_initializing_error_unit : ConcernForColumnMappingPresenter
   {
      protected override void Because()
      {
         base.Because();
         sut.InitializeErrorUnit();
      }

      [TestCase]
      public void the_unit_is_properly_set()
      {
         Assert.AreEqual(_basicFormat.Parameters.OfType<MappingDataFormatParameter>().First(p => p.ColumnName == "Observation").MappedColumn.Unit, _basicFormat.Parameters.OfType<MappingDataFormatParameter>().First(p => p.ColumnName == "Error").MappedColumn.Unit);
      }
   }

   public class When_initializing_error_unit_on_initialized_error : ConcernForColumnMappingPresenter
   {
      protected override void Because()
      {
         A.CallTo(() => _basicFormat.Parameters).Returns(new List<DataFormatParameter>() {
               new MappingDataFormatParameter("Time", new Column() { Name = "Time", Unit = new UnitDescription("min") }),
               new MappingDataFormatParameter("Observation", new Column() { Name = "Concentration", Unit = new UnitDescription("mol/l") }),
               new MappingDataFormatParameter("Error", new Column() { Name = "Error", Unit = new UnitDescription("g/l"), ErrorStdDev = Constants.STD_DEV_GEOMETRIC }),
               new GroupByDataFormatParameter("Study id")
            });
         base.Because();
         sut.InitializeErrorUnit();
      }

      [TestCase]
      public void the_unit_is_properly_set()
      {
         Assert.AreEqual("", _basicFormat.Parameters.OfType<MappingDataFormatParameter>().First(p => p.ColumnName == "Error").MappedColumn.Unit.SelectedUnit);
      }
   }

   public class When_updating_description_for_model_with_first_error_type : ConcernForColumnMappingPresenter
   {
      protected override void Because()
      {
         base.Because();
         A.CallTo(() => _mappingParameterEditorPresenter.Unit).Returns(new UnitDescription(""));
         A.CallTo(() => _mappingParameterEditorPresenter.SelectedErrorType).Returns(0);
         sut.SetSubEditorSettingsForMapping(new ColumnMappingDTO
         (
            ColumnMappingDTO.ColumnType.Mapping, 
            "Error", 
            _parameters[2],
            0,
            _columnInfos[2]
         ));
         sut.UpdateDescriptrionForModel();
      }

      [TestCase]
      public void the_ErrorStdDev_is_properly_set()
      {
         Assert.AreEqual(Constants.STD_DEV_ARITHMETIC, (_basicFormat.Parameters[2] as MappingDataFormatParameter).MappedColumn.ErrorStdDev);
      }
   }

   public class When_updating_description_for_model_with_second_error_type : ConcernForColumnMappingPresenter
   {
      protected override void Because()
      {
         base.Because();
         A.CallTo(() => _mappingParameterEditorPresenter.Unit).Returns(new UnitDescription(""));
         A.CallTo(() => _mappingParameterEditorPresenter.SelectedErrorType).Returns(1);
         sut.SetSubEditorSettingsForMapping(new ColumnMappingDTO
         (
            ColumnMappingDTO.ColumnType.Mapping,
            "Error",
            _parameters[2],
            0,
            _columnInfos[2]
         ));
         sut.UpdateDescriptrionForModel();
      }

      [TestCase]
      public void the_ErrorStdDev_is_properly_set()
      {
         Assert.AreEqual(Constants.STD_DEV_GEOMETRIC, (_basicFormat.Parameters[2] as MappingDataFormatParameter).MappedColumn.ErrorStdDev);
      }
   }
}

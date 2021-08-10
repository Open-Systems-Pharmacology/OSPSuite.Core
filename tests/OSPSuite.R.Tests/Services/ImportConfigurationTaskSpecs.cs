using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_ImportConfigurationTask : ContextForIntegration<IImportConfigurationTask>
   {
      protected ImporterConfiguration _configuration;
      protected ColumnInfo[] _columnInfos = new ColumnInfo[] 
      {
         new ColumnInfo() { DisplayName = "Time", IsMandatory = true },
         new ColumnInfo() { DisplayName = "Measurement", IsMandatory = true, BaseGridName = "Time" },
         new ColumnInfo() { DisplayName = "Error", IsMandatory = false, BaseGridName = "Time", RelatedColumnOf = "Measurement" }
      };
      protected override void Context()
      {
         base.Context();
         sut = Api.GetImportConfigurationTask();
         _configuration = Api.GetDataImporterTask().CreateConfiguration();
      }
   }

   public class When_editing_import_configuration : concern_for_ImportConfigurationTask
   {
      [Observation]
      public void should_get_and_set_time()
      {
         sut.GetTime(_configuration, _columnInfos).ShouldBeNull();
         var time = new MappingDataFormatParameter();
         sut.SetTime(_configuration, time, _columnInfos);
         sut.GetTime(_configuration, _columnInfos).ShouldBeEqualTo(time);
      }

      [Observation]
      public void should_get_and_set_concentration()
      {
         sut.GetMeasurement(_configuration, _columnInfos).ShouldBeNull();
         var concentration = new MappingDataFormatParameter();
         sut.SetMeasurement(_configuration, concentration, _columnInfos);
         sut.GetMeasurement(_configuration, _columnInfos).ShouldBeEqualTo(concentration);
      }

      [Observation]
      public void should_get_and_set_error()
      {
         sut.GetError(_configuration, _columnInfos).ShouldBeNull();
         var error = new MappingDataFormatParameter();
         sut.SetError(_configuration, error, _columnInfos);
         sut.GetError(_configuration, _columnInfos).ShouldBeEqualTo(error);
      }

      [Observation]
      public void should_get_add_and_remove_all_grouping_columns()
      {
         sut.GetAllGroupingColumns(_configuration).ShouldBeEmpty();
         var grouping = new GroupByDataFormatParameter("column1");
         sut.AddGroupingColumn(_configuration, grouping);
         sut.GetAllGroupingColumns(_configuration).ShouldContain(grouping);
         sut.RemoveGroupingColumn(_configuration, grouping);
         sut.GetAllGroupingColumns(_configuration).ShouldBeEmpty();
      }

      [Observation]
      public void should_get_add_and_remove_all_loaded_sheets()
      {
         sut.GetAllLoadedSheets(_configuration).ShouldBeEmpty();
         var sheet = "sheet1";
         sut.AddLoadedSheet(_configuration, sheet);
         sut.GetAllLoadedSheets(_configuration).ShouldContain(sheet);
         sut.RemoveLoadedSheet(_configuration, sheet);
         sut.GetAllLoadedSheets(_configuration).ShouldBeEmpty();
      }
   }
}

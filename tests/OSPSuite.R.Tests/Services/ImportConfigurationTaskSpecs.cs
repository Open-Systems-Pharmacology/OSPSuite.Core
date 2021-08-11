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
         sut.GetTime(_configuration).ShouldBeNull();
         var time = new MappingDataFormatParameter();
         sut.SetTime(_configuration, time);
         sut.GetTime(_configuration).ShouldBeEqualTo(time);
      }

      [Observation]
      public void should_get_and_set_concentration()
      {
         sut.GetMeasurement(_configuration).ShouldBeNull();
         var concentration = new MappingDataFormatParameter();
         sut.SetMeasurement(_configuration, concentration);
         sut.GetMeasurement(_configuration).ShouldBeEqualTo(concentration);
      }

      [Observation]
      public void should_get_and_set_error()
      {
         sut.GetError(_configuration).ShouldBeNull();
         var error = new MappingDataFormatParameter();
         sut.SetError(_configuration, error);
         sut.GetError(_configuration).ShouldBeEqualTo(error);
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

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using System;
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
         var time = sut.GetTime(_configuration);
         var columnName = new Guid().ToString();
         time.ColumnName = columnName;
         sut.GetTime(_configuration).ColumnName.ShouldBeEqualTo(columnName);
      }

      [Observation]
      public void should_get_and_set_concentration()
      {
         var concentration = sut.GetMeasurement(_configuration);
         var columnName = new Guid().ToString();
         concentration.ColumnName = columnName;
         sut.GetMeasurement(_configuration).ColumnName.ShouldBeEqualTo(columnName);
      }

      [Observation]
      public void should_get_and_set_error()
      {
         var error = sut.GetError(_configuration);
         var columnName = new Guid().ToString();
         error.ColumnName = columnName;
         sut.GetError(_configuration).ColumnName.ShouldBeEqualTo(columnName);
      }

      [Observation]
      public void should_get_add_and_remove_all_grouping_columns()
      {
         sut.GetAllGroupingColumns(_configuration).ShouldBeEmpty();
         var groupingColumn = "column1";
         sut.AddGroupingColumn(_configuration, groupingColumn);
         sut.GetAllGroupingColumns(_configuration).ShouldContain(groupingColumn);
         sut.RemoveGroupingColumn(_configuration, groupingColumn);
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

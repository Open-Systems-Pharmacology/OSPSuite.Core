using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using System;
using System.IO;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_DataImporter : ContextForIntegration<IDataImporterTask>
   {
      protected override void Context()
      {
         base.Context();
         sut = Api.GetDataImporterTask();
      }

      protected string getFileFullName(string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName);
   }

   public class When_importing_data_from_r : concern_for_DataImporter
   {
      [Observation]
      public void should_import_simple_data()
      {
         sut.ImportXslxFromConfiguration(getFileFullName("importerConfiguration1.xml"), getFileFullName("sample1.xlsx")).Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_import_simple_data_from_csv()
      {
         sut.ImportCsvFromConfiguration(getFileFullName("importerConfiguration1.csv.xml"), getFileFullName("sample1.csv"), ';').Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_return_empty_on_invalid_file_name()
      {
         sut.ImportXslxFromConfiguration(getFileFullName("importerConfiguration1.xml"), "").Count.ShouldBeEqualTo(0);
      }
   }
}

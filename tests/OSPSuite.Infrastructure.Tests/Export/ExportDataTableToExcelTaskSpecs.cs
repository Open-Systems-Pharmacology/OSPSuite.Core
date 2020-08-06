using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Format;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Infrastructure.Export
{
   public abstract class concern_for_ExportDataTableToExcelTask : ContextSpecification<IExportDataTableToExcelTask>
   {
      protected string _exportExcelFilePath;
      protected string _exportExcelFile = "export.xlsx";

      protected override void Context()
      {
         sut = new ExportDataTableToExcelTask();
      }
   }

   public class When_exporting_a_dataTable : concern_for_ExportDataTableToExcelTask
   {
      protected DataTable _dataTable = new DataTable();

      protected override void Context()
      {
         base.Context();

         _exportExcelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _exportExcelFile);

         _dataTable.Columns.Add("Column1");
         _dataTable.Columns.Add("Column2");

         var row1 = _dataTable.NewRow();
         row1["Column1"] = "str1";
         row1["Column2"] = "str2";

         var row2 = _dataTable.NewRow();
         row2["Column1"] = "str3";
         row2["Column2"] = "str4";

         _dataTable.Rows.Add(row1);
         _dataTable.Rows.Add(row2);

         if (File.Exists(_exportExcelFilePath))
         {
            File.Delete(_exportExcelFilePath); //just in case something went wrong and we did not clean up
         }
      }

      [Test]
      public void should_create_export_file()
      {
         _dataTable.TableName = "TestSheet";
         sut.ExportDataTableToExcel( _dataTable, _exportExcelFilePath, false);

         File.Exists(_exportExcelFilePath).ShouldBeTrue();
         File.Delete(_exportExcelFilePath);

      }
   }
}
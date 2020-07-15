using DevExpress.Utils.StructuredStorage.Internal.Reader;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   internal class TestUnformattedData : UnformattedData
   {
      public TestUnformattedData(Dictionary<string, ColumnDescription> headers)
      {
         Headers = headers;
      }
   }
   public abstract class concern_for_ColumnsDataFormat : ContextSpecification<ColumnsDataFormat>
   {
      protected IUnformattedData basicFormat;

      protected override void Context()
      {
         sut = new ColumnsDataFormat();
         basicFormat = new TestUnformattedData
         (
            new Dictionary<string, ColumnDescription>()
            {
               {
                  "Organ",
                  new ColumnDescription(0)
                  {
                     Level = ColumnDescription.MeasurmentLevel.DISCRETE,
                     ExistingValues = new List<string>() { "PeripheralVenousBlood" }
                  }
               },
               {
                  "Compartment",
                  new ColumnDescription(1)
                  {
                     Level = ColumnDescription.MeasurmentLevel.DISCRETE,
                     ExistingValues = new List<string>() { "Arterialized" }
                  }
               },
               {
                  "Species",
                  new ColumnDescription(2)
                  {
                     Level = ColumnDescription.MeasurmentLevel.DISCRETE,
                     ExistingValues = new List<string>() { "Human" }
                  }
               },
               {
                  "Dose",
                  new ColumnDescription(3)
                  {
                     Level = ColumnDescription.MeasurmentLevel.DISCRETE,
                     ExistingValues = new List<string>() { "75 [g] glucose" }
                  }
               },
               {
                  "Molecule",
                  new ColumnDescription(4)
                  {
                     Level = ColumnDescription.MeasurmentLevel.DISCRETE,
                     ExistingValues = new List<string>() { "GLP-1_7-36 total", "Glucose", "Insuline", "GIP_total", "Glucagon" }
                  }
               },
               {
                  "Time [min]",
                  new ColumnDescription(5)
                  {
                     Level = ColumnDescription.MeasurmentLevel.NUMERIC,
                     ExistingValues = null
                  }
               },
               {
                  "Concentration (molar) [pmol/l]",
                  new ColumnDescription(6)
                  {
                     Level = ColumnDescription.MeasurmentLevel.NUMERIC,
                     ExistingValues = null
                  }
               },
               {
                  "Error [pmol/l]",
                  new ColumnDescription(7)
                  {
                     Level = ColumnDescription.MeasurmentLevel.NUMERIC,
                     ExistingValues = null
                  }
               },
               {
                  "Route",
                  new ColumnDescription(8)
                  {
                     Level = ColumnDescription.MeasurmentLevel.DISCRETE,
                     ExistingValues = new List<string>() { "po" }
                  }
               },
               {
                  "Groupe Id",
                  new ColumnDescription(9)
                  {
                     Level = ColumnDescription.MeasurmentLevel.DISCRETE,
                     ExistingValues = new List<string>() { "H", "T2DM" }
                  }
               }
            }
         );
      }
   }

   public class when_checking_format : concern_for_ColumnsDataFormat
   {
      [TestCase]
      public void identify_basic_format()
      {
         sut.CheckFile(basicFormat).ShouldBeTrue();
      }

      [TestCase]
      public void reject_single_column_data()
      {
         var singleColumn = new TestUnformattedData
         (
            new Dictionary<string, ColumnDescription>()
            {
               {
                  "Organ",
                  basicFormat.Headers["Organ"]
               }
            }
         );
         sut.CheckFile(singleColumn).ShouldBeFalse();
      }

      [TestCase]
      public void reject_multicolumn_with_less_than_two_numeric_columns()
      {
         var singleColumn = new TestUnformattedData
         (
            new Dictionary<string, ColumnDescription>()
            {
               {
                  "Organ",
                  basicFormat.Headers["Organ"]
               },
               {
                  "Time [min]",
                  basicFormat.Headers["Time [min]"]
               }
            }
         );
         sut.CheckFile(singleColumn).ShouldBeFalse();
      }
   }

   public class when_listing_parameters : concern_for_ColumnsDataFormat
   {
      protected override void Because()
      {
         sut.CheckFile(basicFormat);
      }
      
      [TestCase]
      public void identify_time_column()
      {
         var timeParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Time [min]");
         timeParameter.Type.ShouldBeEqualTo(DataFormatParameterType.MAPPING);
         (timeParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = timeParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Time");
         mapping.MappedColumn.Unit.ShouldBeEqualTo("min");
      }

      [TestCase]
      public void identify_error_column()
      {
         var errorParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Error [pmol/l]");
         errorParameter.Type.ShouldBeEqualTo(DataFormatParameterType.MAPPING);
         (errorParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = errorParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Error");
         mapping.MappedColumn.Unit.ShouldBeEqualTo("pmol/l");
      }

      [TestCase]
      public void identify_measurement_column_without_the_name_on_the_headings()
      {
         var measurementParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Concentration (molar) [pmol/l]");
         measurementParameter.Type.ShouldBeEqualTo(DataFormatParameterType.MAPPING);
         (measurementParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = measurementParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Measurement");
         mapping.MappedColumn.Unit.ShouldBeEqualTo("pmol/l");
      }
   }

   public class when_parsing_format : concern_for_ColumnsDataFormat
   {
      [TestCase]
      public void parse_basic_format()
      {
         //var data = sut.Parse(basicFormat);
      }
   }
}

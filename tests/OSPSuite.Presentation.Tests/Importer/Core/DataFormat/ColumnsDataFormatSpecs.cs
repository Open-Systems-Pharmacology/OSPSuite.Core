using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public abstract class concern_for_ColumnsDataFormat : ContextSpecification<ColumnsDataFormat>
   {
      protected Dictionary<string, IList<string>> basicFormat;

      protected override void Context()
      {
         sut = new ColumnsDataFormat();
         basicFormat = new Dictionary<string, IList<string>>()
         {
            {
               "Organ",
               new List<string>()
               {
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood",
                  "PeripheralVenousBlood"
               }
            },
            {
               "Compartment",
               new List<string>()
               {
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized",
                  "Arterialized"
               }
            },
            {
               "Species",
               new List<string>()
               {
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human",
                  "Human"
               }
            },
            {
               "Gender",
               new List<string>()
               {
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  "",
                  ""
               }
            },
            {
               "Dose [unit]",
               new List<string>()
               {
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose",
                  "75 [g] glucose"
               }
            },
            {
               "Molecule",
               new List<string>()
               {
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total",
                  "GLP-1_7-36 total"
               }
            },
            {
               "Time [min]",
               new List<string>()
               {
                  "99.47368421",
                  "116.3157895",
                  "129.4736842",
                  "160",
                  "189.4736842",
                  "219.4736842",
                  "99.47368421",
                  "116.3157895",
                  "129.4736842",
                  "160",
                  "99.47368421",
                  "116.3157895",
                  "129.4736842",
                  "160",
                  "189.4736842",
                  "219.4736842",
                  "99.47368421",
                  "116.3157895",
                  "129.4736842",
                  "160",
                  "99.47368421",
                  "116.3157895",
                  "129.4736842",
                  "160",
                  "189.4736842",
                  "219.4736842",
                  "99.47368421",
                  "116.3157895",
                  "129.4736842",
                  "160"
               }
            },
            {
               "Concentration (molar) [pmol/l]",
               new List<string>()
               {
                  "11",
                  "19",
                  "23",
                  "19",
                  "17",
                  "14",
                  "11",
                  "19",
                  "23",
                  "19",
                  "11",
                  "19",
                  "23",
                  "19",
                  "17",
                  "14",
                  "11",
                  "19",
                  "23",
                  "19",
                  "11",
                  "19",
                  "23",
                  "19",
                  "17",
                  "14",
                  "11",
                  "19",
                  "23",
                  "19"
               }
            },
            {
               "Error [pmol/l]",
               new List<string>()
               {
                  "",
                  "",
                  "",
                  "2.34",
                  "3.89",
                  "6.90",
                  "",
                  "2.34",
                  "3.89",
                  "6.90",
                  "",
                  "",
                  "",
                  "2.34",
                  "3.89",
                  "6.90",
                  "",
                  "2.34",
                  "3.89",
                  "6.90",
                  "",
                  "",
                  "",
                  "2.34",
                  "3.89",
                  "6.90",
                  "",
                  "2.34",
                  "3.89",
                  "6.90"
               }
            },
            {
               "Route",
               new List<string>()
               {
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po",
                  "po"
               }
            }
            ,
            {
               "Groupe Id",
               new List<string>()
               {
                  "H",
                  "H",
                  "H",
                  "H",
                  "H",
                  "T2DM",
                  "T2DM",
                  "T2DM",
                  "T2DM",
                  "T2DM",
                  "H",
                  "H",
                  "H",
                  "H",
                  "H",
                  "T2DM",
                  "T2DM",
                  "T2DM",
                  "T2DM",
                  "T2DM",
                  "H",
                  "H",
                  "H",
                  "H",
                  "H",
                  "T2DM",
                  "T2DM",
                  "T2DM",
                  "T2DM",
                  "T2DM"
               }
            }
         };
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
         var singleColumn = new Dictionary<string, IList<string>>() 
         {
            { 
               "Organ",
               basicFormat["Organ"]
            }
         };
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
   }

   public class when_parsing_format : concern_for_ColumnsDataFormat
   {
      [TestCase]
      public void parse_basic_format()
      {
         var data = sut.Parse(basicFormat);
      }
   }
}

using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public abstract class concern_for_ColumnsDataFormat : ContextSpecification<ColumnsDataFormat>
   {
      protected Dictionary<string, IList<string>> basicFormat = new Dictionary<string, IList<string>>()
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

      protected override void Context()
      {
         sut = new ColumnsDataFormat();
      }
   }

   public class when_checking_format : concern_for_ColumnsDataFormat
   {
      

      [TestCase]
      public void identify_basic_format()
      {
         sut.CheckFile(basicFormat).ShouldBeTrue();
      }
   }
}

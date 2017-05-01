using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper.Extensions;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class DataColumnXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationWithoutRelatedColumn()
      {
         var path = new List<string>(new string[] {"aa", "bb"});
         var baseGrid = new BaseGrid("Bastian", DimensionTime) {Values = new float[] {0F, 3600F, 7200F}};
         var x1 = new DataColumn("Columbus", DimensionLength, baseGrid)
         {
            IsInternal = true,
            QuantityInfo = new QuantityInfo("Quain", path, QuantityType.Parameter),
            DataInfo = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.Undefined, "cm", new DateTime(2010, 10, 22), "Study1", "Dog", 2.4),
            Values = new[] {1.0F, 2.1F, -3.4F}
         };
         x1.DataInfo.LLOQ = 1.0F;
         x1.DataInfo.ComparisonThreshold = 1e-2F;

         var dr1 = new DataRepository("id") {x1};
         var dr2 = SerializeAndDeserialize(dr1);
         var x2 = dr2.AllButBaseGrid().First();
         AssertForSpecs.AreEqualMcDataColumn(x1, x2);
      }

      [Test]
      public void TestSerializationWithRelatedColumnExplicitelyBefore()
      {
         var baseGrid = new BaseGrid("Bastian", DimensionTime) {Values = new float[] {0F, 3600F, 7200F}};
         var x1 = new DataColumn("Columbus", DimensionLength, baseGrid) {IsInternal = false};
         var relCol = new DataColumn("Renate", DimensionLess, baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary, AuxiliaryType.GeometricStdDev, " ", new DateTime(2010, 10, 22), "Study1", "Dog", 2.4)
         };
         x1.AddRelatedColumn(relCol);

         var dr1 = new DataRepository("id") {x1, relCol};
         var dr2 = SerializeAndDeserialize(dr1);

         var x2 = dr2.AllButBaseGrid().First(x => x.IsNamed("Columbus"));
         AssertForSpecs.AreEqualMcDataColumn(x1, x2);
         x2.RelatedColumns.First().Dimension.ShouldNotBeNull();
      }

      [Test]
      public void TestSerializationWithRelatedColumnExplicitelyAfter()
      {
         var baseGrid = new BaseGrid("Bastian", DimensionTime) {Values = new float[] {0F, 3600F, 7200F}};
         var x1 = new DataColumn("Columbus", DimensionLength, baseGrid);
         var relColG = new DataColumn("RenateG", DimensionLess, baseGrid);
         relColG.DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary, AuxiliaryType.GeometricStdDev, " ", new DateTime(2010, 10, 22), "Study1", "Dog", 2.4);
         x1.AddRelatedColumn(relColG);
         var relColA = new DataColumn("RenateA", DimensionLength, baseGrid);
         relColA.DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary, AuxiliaryType.ArithmeticStdDev, "cm", new DateTime(2010, 10, 22), "Study1", "Dog", 2.4);
         x1.AddRelatedColumn(relColA);

         var dr1 = new DataRepository("id") {x1, relColG, relColA};
         var dr2 = SerializeAndDeserialize(dr1);
         var x2 = dr2.AllButBaseGrid().First(x => x.IsNamed("Columbus"));

         AssertForSpecs.AreEqualMcDataColumn(x1, x2);
      }
   }
}
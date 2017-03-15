using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core
{
   public class DataColumnSpecs : ContextSpecification<DataColumn>
   {
      private BaseGrid _baseGrid;
      private IDimension _length;
      private IDimension _mass;
      private IDimension _time;
      private IDimension _dimensionless;

      protected override void Context()
      {
         base.Context();
         _time = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1},"Time", "s");
         _time.AddUnit("min", 60, 0);
         _time.AddUnit("h", 3600, 0);

         _length = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1},"Length", "m");
         _length.AddUnit("mm", 0.001, 0);

         _mass = new Dimension(new BaseDimensionRepresentation {MassExponent = 1},"Mass", "kg");
         _mass.AddUnit("mg", 0.000001, 0.0);

         _dimensionless = new Dimension(new BaseDimensionRepresentation(),"Dimensionless", " ");
         
         _baseGrid = new BaseGrid("BaseGrid", _time);
         _baseGrid.Values = new[] {-1.0F, 0.0F, 2.0F};
         sut = new DataColumn("Colin", _length, _baseGrid);
      }

      [Observation]
      public void TestConstructor()
      {
         var name = "Colin";
         DataColumn sut = new DataColumn(name, _length, _baseGrid);

         Assert.AreEqual(name, sut.Name);
         Assert.AreSame(_length, sut.Dimension);
         Assert.AreSame(_baseGrid, sut.BaseGrid);
      }

      [Observation]
      public void TestAccessByIndex()
      {
         sut[1] = 5.0F;
         sut[0] = 8.0F;
         float v3;

         Assert.AreEqual(8F, sut[0]);
         Assert.AreEqual(5F, sut[1]);
         Assert.AreEqual(0F, sut[2]);
         The.Action(() => v3 = sut[3]).ShouldThrowAn<ArgumentOutOfRangeException>();
      }

      [Observation]
      public void TestGetValues()
      {
         sut[1] = 5.0F;
         sut[0] = 8.0F;
         var values = sut.Values;

         Assert.AreEqual(3, values.Count);
         Assert.AreEqual(8F, values[0]);
         Assert.AreEqual(5F, values[1]);
         Assert.AreEqual(0F, values[2]);
      }

      [Observation]
      public void TestSetValues()
      {
         sut.Values = new[] {8.0F, 0.0F, -5.0F};

         Assert.AreEqual(8F, sut[0]);
         Assert.AreEqual(0F, sut[1]);
         Assert.AreEqual(-5F, sut[2]);
      }

      [Observation]
      public void TestSetValuesWithWrongLength()
      {
         The.Action(() => sut.Values = new[] {8.0F, 0.0F}).ShouldThrowAn<ArgumentException>();
      }

      [Observation]
      public void TestGetValue()
      {
         sut.Values = new[] {4.0F, 2.0F, 3.0F};

         Assert.AreEqual(2F, sut.GetValue(0F));
         Assert.AreEqual(3F, sut.GetValue(-0.5F));
         Assert.AreEqual(2.5F, sut.GetValue(1F));
         Assert.AreEqual(4F, sut.GetValue(-1.0F));
         Assert.AreEqual(3F, sut.GetValue(2.0F));
         float.IsNaN(sut.GetValue(-1.1F)).ShouldBeTrue();
         float.IsNaN(sut.GetValue(2.1F)).ShouldBeTrue();
      }

      [Observation]
      public void TestAddRelatedColumn()
      {
         sut.Values = new[] {8.0F, 0.0F, -5.0F};
         var relatedColumn = new DataColumn("Regina", _dimensionless, _baseGrid);
         relatedColumn.DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary, AuxiliaryType.GeometricStdDev, " ", DateTime.MinValue, "", "", 0);
         relatedColumn.Values = new[] {0.1F, 0.2F, 0.1F};

         sut.AddRelatedColumn(relatedColumn);
         Assert.AreEqual(relatedColumn, sut.GetRelatedColumn(AuxiliaryType.GeometricStdDev));
      }

      [Observation]
      public void TestAddRelatedColumnWithWrongBaseGrid()
      {
         BaseGrid baseGrid2 = new BaseGrid("BaseGrid2", _time);
         baseGrid2.Values = new[] {-1.0F, 0.0F, 3.0F};

         sut.Values = new[] {8.0F, 0.0F, -5.0F};
         var relatedColumn = new DataColumn("Regina", _dimensionless, baseGrid2);
         relatedColumn.DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary, AuxiliaryType.GeometricStdDev, " ", DateTime.MinValue, "", "", 0);
         relatedColumn.Values = new[] {0.1F, 0.2F, 0.1F};

         The.Action(() => sut.AddRelatedColumn(relatedColumn)).ShouldThrowAn<InvalidArgumentException>();
      }

    
      [Observation]
      public void TestAddRelatedColumnArithmeticStdDevWithWrongDimension()
      {
         sut.Values = new[] {8.0F, 0.0F, -5.0F};
         var relatedColumn = new DataColumn("Regina", _dimensionless, _baseGrid);
         relatedColumn.DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary, AuxiliaryType.ArithmeticStdDev, " ", DateTime.MinValue, "", "", 0);
         relatedColumn.Values = new[] {0.1F, 0.2F, 0.1F};

         The.Action(() => sut.AddRelatedColumn(relatedColumn)).ShouldThrowAn<InvalidArgumentException>();
      }

      [Observation]
      public void TestAddRelatedColumnGeometricStdDevWithWrongDimension()
      {
         sut.Values = new[] {8.0F, 0.0F, -5.0F};
         var relatedColumn = new DataColumn("Regina", _length, _baseGrid);
         relatedColumn.DataInfo = new DataInfo(ColumnOrigins.ObservationAuxiliary, AuxiliaryType.GeometricStdDev, " ", DateTime.MinValue, "", "", 0);
         relatedColumn.Values = new[] {0.1F, 0.2F, 0.1F};

         The.Action(() => sut.AddRelatedColumn(relatedColumn)).ShouldThrowAn<InvalidArgumentException>();
      }

      [Observation]
      public void TestWithConstantArray()
      {
         const float constValue = 123.456F;

         sut.Values = new[] {constValue};

         var values = sut.Values;

         values.Count.ShouldBeEqualTo(sut.BaseGrid.Count);

         for (int i = 0; i < values.Count; i++)
            values[i].ShouldBeEqualTo(constValue);
      }

      [Observation]
      public void TestSetAndGetValues()
      {
         const float constValue = 123.456F;

         sut.Values = new[] { constValue };

         var values = sut.Values;

         values.Count.ShouldBeEqualTo(sut.BaseGrid.Count);

         for (int i = 0; i < values.Count; i++)
            values[i].ShouldBeEqualTo(constValue);

         sut.Values = new []{1.0F};
         values = sut.Values;
         for (int i = 0; i < values.Count; i++)
            values[i].ShouldBeEqualTo(1.0F);

      }

      [Observation]
      public void Retrieving_the_display_value_of_a_data_column()
      {
         sut.DisplayUnit = _length.Unit("mm");
         sut.DisplayUnit.ShouldBeEqualTo(_length.Unit("mm"));
      }

   }
}
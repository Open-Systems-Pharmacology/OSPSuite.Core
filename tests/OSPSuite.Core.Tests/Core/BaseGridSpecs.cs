using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core
{
   public class BaseGridSpecs : ContextSpecification<BaseGrid>
   {
      private IDimension time;

      protected override void Context()
      {
         base.Context();
         time = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1}, "Time", "s");
         time.AddUnit("min", 60, 0);
         time.AddUnit("h", 3600, 0);
         sut = new BaseGrid("BaseGrid", time);
      }

      [Observation]
      public void TestConstructor()
      {
         var name = "BaseGrid";
         BaseGrid sut = new BaseGrid(name, time);

         Assert.AreEqual(name, sut.Name);
         Assert.AreSame(time, sut.Dimension);
         Assert.AreSame(sut, sut.BaseGrid);
      }

      [Observation]
      public void TestInsert3DifferentValues()
      {
         sut.Insert(2.0F);
         sut.Insert(3.0F);
         sut.Insert(1.0F);

         Assert.AreEqual(3, sut.Count);
         Assert.AreEqual(1.0F, sut[0]);
         Assert.AreEqual(2.0F, sut[1]);
         Assert.AreEqual(3.0F, sut[2]);
      }

      [Observation]
      public void TestInsert2SameValues()
      {
         sut.Insert(2.0F);
         sut.Insert(2.0F);

         Assert.AreEqual(1, sut.Count);
         Assert.AreEqual(2.0F, sut[0]);
      }

      [Observation]
      public void TestSetNonStrictlyMonotonicValues()
      {
         The.Action(() => sut.Values = new[] {0.5F, -2.0F, 1.5F}).ShouldThrowAn<InvalidArgumentException>();
      }

      [Observation]
      public void TestSetValues()
      {
         sut.Values = new[] {-2.0F, 0.5F, 1.5F};

         Assert.AreEqual(3, sut.Count);
         Assert.AreEqual(-2.0F, sut[0]);
         Assert.AreEqual(0.5F, sut[1]);
         Assert.AreEqual(1.5F, sut[2]);
      }

      [Observation]
      public void TestGetValues()
      {
         sut.Insert(2.0F);
         sut.Insert(3.0F);
         sut.Insert(1.0F);

         var values = sut.Values;

         Assert.AreEqual(3, values.Count);
         Assert.AreEqual(1.0F, values[0]);
         Assert.AreEqual(2.0F, values[1]);
         Assert.AreEqual(3.0F, values[2]);
      }

      [Observation]
      public void test_insert_values()
      {
         bool replaced;
         sut.Insert(2.0F);
         sut.Insert(3.0F);
         sut.Insert(1.0F);

         int index = sut.InsertOrReplace(2.0F, out replaced);
         index.ShouldBeEqualTo(1);
         replaced.ShouldBeTrue();

         index = sut.InsertOrReplace(3.0F, out replaced);
         index.ShouldBeEqualTo(2);
         replaced.ShouldBeTrue();

         index = sut.InsertOrReplace(1.0F, out replaced);
         index.ShouldBeEqualTo(0);
         replaced.ShouldBeTrue();

         index = sut.InsertOrReplace(1.5F, out replaced);
         index.ShouldBeEqualTo(1);
         replaced.ShouldBeFalse();
      }

      [Observation]
      public void TestInsertInterval()
      {
         sut.InsertInterval(-2.0F, 2.0F, 5);

         Assert.AreEqual(5, sut.Count);
         Assert.AreEqual(-2.0F, sut[0]);
         Assert.AreEqual(-1.0F, sut[1]);
         Assert.AreEqual(0.0F, sut[2]);
         Assert.AreEqual(1.0F, sut[3]);
         Assert.AreEqual(2.0F, sut[4]);
      }

      [Test]
      public void TestMixedInsert()
      {
         sut.Values = new[] {-2.0F, 0.5F, 1.0F, 1.5F};
         sut.InsertInterval(-2.0F, 2.0F, 5);
         sut.Insert(2.0F);
         sut.Insert(3.0F);
         sut.Insert(1.0F);

         Assert.AreEqual(8, sut.Count);
         Assert.AreEqual(-2.0F, sut[0]);
         Assert.AreEqual(-1.0F, sut[1]);
         Assert.AreEqual(0.0F, sut[2]);
         Assert.AreEqual(0.5F, sut[3]);
         Assert.AreEqual(1.0F, sut[4]);
         Assert.AreEqual(1.5F, sut[5]);
         Assert.AreEqual(2.0F, sut[6]);
         Assert.AreEqual(3.0F, sut[7]);
      }

      [Observation]
      public void TestGetValue()
      {
         sut.Values = new[] {-2.0F, 0.5F, 1.5F};

         Assert.AreEqual(0.6F, sut.GetValue(0.6F));
         Assert.AreEqual(1.5F, sut.GetValue(1.5F));
         Assert.AreEqual(-2.0F, sut.GetValue(-2.0F));
         Assert.AreEqual(0.5F, sut.GetValue(0.5F));
         float.IsNaN(sut.GetValue(-2.1F)).ShouldBeTrue();
         float.IsNaN(sut.GetValue(1.6F)).ShouldBeTrue();
      }

      [Observation]
      public void TestRemove()
      {
         sut.Values = new[] {-2.0F, 0.5F, 1.0F, 1.5F};
         var removed05 = sut.Remove(0.5F);
         var removed06 = sut.Remove(0.6F);

         Assert.IsTrue(removed05);
         Assert.IsFalse(removed06);
         Assert.AreEqual(3, sut.Count);
         Assert.AreEqual(-2.0F, sut[0]);
         Assert.AreEqual(1.0F, sut[1]);
         Assert.AreEqual(1.5F, sut[2]);
      }

      [Observation]
      public void TestIndexOf()
      {
         sut.Values = new[] {-2.0F, 0.5F, 1.0F, 1.5F};

         Assert.AreEqual(1, sut.IndexOf(0.5F));
         Assert.AreEqual(-1, sut.IndexOf(0.6F));
      }

      [Observation]
      public void TestRightIndexOf()
      {
         sut.Values = new[] {-2.0F, 0.5F, 1.0F, 1.5F};

         Assert.AreEqual(1, sut.RightIndexOf(0.5F));
         Assert.AreEqual(2, sut.RightIndexOf(0.6F));
         Assert.AreEqual(4, sut.RightIndexOf(1.6F));
         Assert.AreEqual(0, sut.RightIndexOf(-3F));
      }

      [Observation]
      public void TestLeftIndexOf()
      {
         sut.Values = new[] {-2.0F, 0.5F, 1.0F, 1.5F};

         Assert.AreEqual(1, sut.LeftIndexOf(0.5F));
         Assert.AreEqual(1, sut.LeftIndexOf(0.6F));
         Assert.AreEqual(3, sut.LeftIndexOf(1.6F));
         Assert.AreEqual(-1, sut.LeftIndexOf(-3F));
      }
   }
}
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public class BaseGridSpecs : ContextSpecification<BaseGrid>
   {
      private IDimension time;

      protected override void Context()
      {
         base.Context();
         time = new Dimension(new BaseDimensionRepresentation { TimeExponent = 1 }, "Time", "s");
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
      public void TestSetValues()
      {
         sut.Values = new[] { -2.0F, 0.5F, 1.5F };

         Assert.AreEqual(3, sut.Count);
         Assert.AreEqual(-2.0F, sut[0]);
         Assert.AreEqual(0.5F, sut[1]);
         Assert.AreEqual(1.5F, sut[2]);
      }

      [Observation]
      public void TestGetValue()
      {
         sut.Values = new[] { -2.0F, 0.5F, 1.5F };

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
         sut.Values = new[] { -2.0F, 0.5F, 1.0F, 1.5F };
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
         sut.Values = new[] { -2.0F, 0.5F, 1.0F, 1.5F };

         Assert.AreEqual(1, sut.IndexOf(0.5F));
         Assert.AreEqual(-1, sut.IndexOf(0.6F));
      }

      [Observation]
      public void TestRightIndexOf()
      {
         sut.Values = new[] { -2.0F, 0.5F, 1.0F, 1.5F };

         Assert.AreEqual(1, sut.IndexOfNextHighest(0.5F));
         Assert.AreEqual(2, sut.IndexOfNextHighest(0.6F));
         Assert.AreEqual(4, sut.IndexOfNextHighest(1.6F));
         Assert.AreEqual(0, sut.IndexOfNextHighest(-3F));
      }

      [Observation]
      public void TestLeftIndexOf()
      {
         sut.Values = new[] { -2.0F, 0.5F, 1.0F, 1.5F };

         Assert.AreEqual(1, sut.IndexOfNextLowest(0.5F));
         Assert.AreEqual(1, sut.IndexOfNextLowest(0.6F));
         Assert.AreEqual(3, sut.IndexOfNextLowest(1.6F));
         Assert.AreEqual(-1, sut.IndexOfNextLowest(-3F));
      }
   }
}
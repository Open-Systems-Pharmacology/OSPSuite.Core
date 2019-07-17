using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_DataInfo : ContextSpecification<DataInfo>
   {
      protected override void Context()
      {
         sut = new DataInfo(ColumnOrigins.Calculation, AuxiliaryType.ArithmeticStdDev, "ml", DateTime.Today, "Journal", "cat", 155d)
         {
            LLOQ = 25,
            ComparisonThreshold = 1e-3f
         };

         sut.ExtendedProperties.Add(new ExtendedProperty<string> {Name = "Test", Value = "hello"});
      }
   }

   public class When_cloning_a_data_info : concern_for_DataInfo
   {
      private DataInfo _clone;

      protected override void Because()
      {
         _clone = sut.Clone();
      }

      [Observation]
      public void should_clone_all_the_properties_of_the_data_info()
      {
         _clone.ComparisonThreshold.ShouldBeEqualTo(sut.ComparisonThreshold);
         _clone.LLOQ.ShouldBeEqualTo(sut.LLOQ);
         _clone.AuxiliaryType.ShouldBeEqualTo(sut.AuxiliaryType);
         _clone.Origin.ShouldBeEqualTo(sut.Origin);
         _clone.DisplayUnitName.ShouldBeEqualTo(sut.DisplayUnitName);
         _clone.Date.ShouldBeEqualTo(sut.Date);
         _clone.Source.ShouldBeEqualTo(sut.Source);
         _clone.Category.ShouldBeEqualTo(sut.Category);
         _clone.MolWeight.ShouldBeEqualTo(sut.MolWeight);

         _clone.ExtendedProperties["Test"].ValueAsObject.ShouldBeEqualTo("hello");
      }
   }
}
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;

namespace OSPSuite.Core.DiffBuilders
{
   public class When_comparing_observers_with_for_all_false_and_differences_in_the_include_list :  concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var t1 = new ObserverBuilder().WithName("Passive");
         t1.ForAll = false;
         t1.AddMoleculeName("Drug");

         var t2 = new ObserverBuilder().WithName("Passive");
         t2.ForAll = false;
         t2.AddMoleculeName("Metab");

         _object1 = t1;
         _object2 = t2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_observers_with_for_all_false_and_differences_in_the_exclude_list :  concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var t1 = new ObserverBuilder().WithName("Passive");
         t1.ForAll = false;
         t1.AddMoleculeNameToExclude("Drug");

         var t2 = new ObserverBuilder().WithName("Passive");
         t2.ForAll = false;
         t2.AddMoleculeNameToExclude("Metab");

         _object1 = t1;
         _object2 = t2;
      }

      [Observation]
      public void should_report_no_differences()
      {
         _report.ShouldBeEmpty();
      }
   }

   public class When_comparing_observers_with_different_monitor_Formulas : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var t1 = new ObserverBuilder().WithName("Passive");
         t1.Formula = new ExplicitFormula("a+b");

         var t2 = new ObserverBuilder().WithName("Passive");
         t2.Formula = new ExplicitFormula("a-b");

         _object1 = t1;
         _object2 = t2;
      }

      [Observation]
      public void should_report_no_differences()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_observers_with_different_ContainerCriteria : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var t1 = new ObserverBuilder().WithName("Passive");
         t1.ContainerCriteria = Create.Criteria(x => x.With("Tada").And.With("Toto"));

         var t2 = new ObserverBuilder().WithName("Passive");
         t2.ContainerCriteria = Create.Criteria(x => x.Not("Tada").And.With("Toto"));

         _object1 = t1;
         _object2 = t2;
      }

      [Observation]
      public void should_report_no_differences()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_observers_with_different_dimensions : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var t1 = new ObserverBuilder()
            .WithName("Passive")
            .WithDimension(DimensionFactoryForSpecs.ConcentrationDimension);

         var t2 = new ObserverBuilder()
            .WithName("Passive")
            .WithDimension(Constants.Dimension.NO_DIMENSION);

         _object1 = t1;
         _object2 = t2;
      }

      [Observation]
      public void should_report_one_difference()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }
}
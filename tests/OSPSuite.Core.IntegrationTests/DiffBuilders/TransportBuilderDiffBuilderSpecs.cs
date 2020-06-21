using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core.DiffBuilders
{
   public class When_comparing_Transports_with_different_for_targetCriteria : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var t1 = new TransportBuilder().WithName("Passive");
         t1.TargetCriteria = Create.Criteria(x => x.With("Liver").And.Not("Plasma"));

         var t2 = new TransportBuilder().WithName("Passive");
         t2.TargetCriteria = Create.Criteria(x => x.With("Liver").And.With("Plasma"));

         _object1 = t1;
         _object2 = t2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_Transports_with_different_for_all_settings : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var t1 = new TransportBuilder().WithName("Passive");
         t1.ForAll = true;

         var t2 = new TransportBuilder().WithName("Passive");
         t2.ForAll = false;

         _object1 = t1;
         _object2 = t2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_Transports_with_for_all_true_and_differences_in_the_exclude_list : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var t1 = new TransportBuilder().WithName("Passive");
         t1.ForAll = true;
         t1.AddMoleculeNameToExclude("Drug");

         var t2 = new TransportBuilder().WithName("Passive");
         t2.ForAll = true;
         t2.AddMoleculeNameToExclude("Metab");

         _object1 = t1;
         _object2 = t2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_Transports_with_for_all_false_and_differences_in_the_include_list : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var t1 = new TransportBuilder().WithName("Passive");
         t1.ForAll = false;
         t1.AddMoleculeName("Drug");

         var t2 = new TransportBuilder().WithName("Passive");
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

   public class When_comparing_Transports_with_for_all_false_and_differences_in_the_exclude_list : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var t1 = new TransportBuilder().WithName("Passive");
         t1.ForAll = false;
         t1.AddMoleculeNameToExclude("Drug");

         var t2 = new TransportBuilder().WithName("Passive");
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

   public class When_comparing_Transports_withdiffernt_parameters : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var t1 = new TransportBuilder().WithName("Passive");
         t1.ForAll = false;
         t1.AddMoleculeName("Drug");
         t1.AddParameter(new Parameter().WithName("P1"));

         var t2 = new TransportBuilder().WithName("Passive");
         t2.ForAll = false;
         t2.AddMoleculeName("Drug");
         t2.AddParameter(new Parameter().WithName("P2"));

         _object1 = t1;
         _object2 = t2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }
}
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_ObjectComparer : ContextForIntegration<IObjectComparer>
   {
      protected object _object1;
      protected object _object2;
      protected ComparerSettings _comparerSettings;
      protected DiffReport _report;

      protected override void Context()
      {
         sut = IoC.Resolve<IObjectComparer>();
         _comparerSettings = new ComparerSettings {CompareHiddenEntities = true};
      }

      protected override void Because()
      {
         _report = sut.Compare(_object1, _object2, _comparerSettings);
      }
   }

   public class When_comparing_two_null_objects : concern_for_ObjectComparer
   {
      [Observation]
      public void should_return_an_empty_report()
      {
         sut.Compare(null, null).IsEmpty.ShouldBeTrue();
      }
   }

   public class When_comparing_one_object_not_null_with_another_one_null : concern_for_ObjectComparer
   {
      private DiffReport _report1;
      private IObjectBase _oneObject;
      private DiffReport _report2;

      protected override void Because()
      {
         _oneObject = new ARootContainer();
         _report1 = sut.Compare(null, _oneObject);
         _report2 = sut.Compare(_oneObject, null);
      }

      [Observation]
      public void should_return_a_report_stipulating_that_one_object_is_null_and_the_other_one_is_not()
      {
         _report1.Count.ShouldBeEqualTo(1);
         _report1.ElementAt(0).Object1.ShouldBeNull();
         _report1.ElementAt(0).Object2.ShouldBeEqualTo(_oneObject);
         _report1.ElementAt(0).Description.ShouldBeEqualTo(Captions.Diff.OneObjectIsNull);

         _report2.Count.ShouldBeEqualTo(1);
         _report2.ElementAt(0).Object1.ShouldBeEqualTo(_oneObject);
         _report2.ElementAt(0).Object2.ShouldBeNull();
         _report2.ElementAt(0).Description.ShouldBeEqualTo(Captions.Diff.OneObjectIsNull);
      }
   }

   public class When_comparing_two_objects_having_one_property_null_and_the_other_an_empty_string : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         _object1 = new Container {ContainerType = ContainerType.Event, Name = ""};
         _object2 = new Container {ContainerType = ContainerType.Event, Name = null};
      }

      [Observation]
      public void should_not_report_a_difference()
      {
         _report.Count().ShouldBeEqualTo(0);
      }
   }

   public class When_comparing_two_objects_having_one_property_null_and_the_other_not_null : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         _object1 = new Container {ContainerType = ContainerType.Event, Name = "xxx"};
         _object2 = new Container {ContainerType = ContainerType.Event, Name = null};
         _comparerSettings.OnlyComputingRelevant = false;

      }

      [Observation]
      public void should_report_a_difference()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_objects_having_the_same_property_null : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         _object1 = new Container {ContainerType = ContainerType.Event, Name = null};
         _object2 = new Container {ContainerType = ContainerType.Event, Name = null};
      }

      [Observation]
      public void should_not_report_a_difference()
      {
         _report.Count().ShouldBeEqualTo(0);
      }
   }

   public class When_comparing_two_object_base_having_different_name_and_id_without_comparing_the_id :
      concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         _object1 = new Container {ContainerType = ContainerType.Event, Name = "O1", Id = "Id1"};
         _object2 = new Container {ContainerType = ContainerType.Event, Name = "O2", Id = "Id2"};
         _comparerSettings.OnlyComputingRelevant = false;

      }

      [Observation]
      public void should_return_a_report_containing_the_name_difference()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_objects_having_the_differnt_tags : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var c1 = new Container {ContainerType = ContainerType.Event, Name = null};
         c1.AddTag("Tag");
         c1.AddTag("Tug");
         _object1 = c1;

         var c2 = new Container {ContainerType = ContainerType.Event, Name = null};
         c2.AddTag("Tag");
         c2.AddTag("Tig");
         _object2 = c2;
      }

      [Observation]
      public void should_report_a_missing_difference()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_two_objects_having_the_same_tags : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var c1 = new Container {ContainerType = ContainerType.Event, Name = null};
         c1.AddTag("Tag");
         c1.AddTag("Tug");
         _object1 = c1;

         var c2 = new Container {ContainerType = ContainerType.Event, Name = null};
         c2.AddTag("Tag");
         c2.AddTag("Tug");
         _object2 = c2;
      }

      [Observation]
      public void should_not_report_a_difference()
      {
         _report.ShouldBeEmpty();
      }
   }

   public class When_comparing_two_Container_containing_different_molecule_amounts : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var c1 = new Container { ContainerType = ContainerType.Event, Name = null };
         c1.Add(new MoleculeAmount().WithName("Drug").WithFormula(new ConstantFormula(50)));
         _object1 = c1;

         var c2 = new Container { ContainerType = ContainerType.Event, Name = null };
         c2.Add(new MoleculeAmount().WithName("Drug").WithFormula(new ConstantFormula(75)));
         _object2 = c2;
      }

      [Observation]
      public void should_report_a_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
}
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.DiffBuilders
{
   public class When_comparing_two_container_having_different_base_properties_and_container_properties : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         _comparerSettings.OnlyComputingRelevant = false;
         _object1 = new Container {ContainerType = ContainerType.Formulation, Name = "O1"};
         _object2 = new Container {ContainerType = ContainerType.Event, Name = "O2"};
      }

      [Observation]
      public void should_return_a_report_containing_the_name_and_the_container_type_difference()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_two_container_having_the_same_number_of_children_but_with_differences : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var container1 = new Container {Name = "O1"};
         container1.Add(new Parameter {BuildMode = ParameterBuildMode.Global, Name = "P1"});

         var container2 = new Container {Name = "O2"};
         container2.Add(new Parameter {BuildMode = ParameterBuildMode.Local, Name = "P1"});

         _object1 = container1;
         _object2 = container2;
      }

      [Observation]
      public void should_return_a_report_containing_the_build_mode_difference_for_the_parameter()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_containers_having_different_children : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var container1 = new Container {Name = "O1"};
         container1.Add(new Parameter {BuildMode = ParameterBuildMode.Global, Name = "P1"});

         var container2 = new Container {Name = "O2"};
         container2.Add(new Parameter {BuildMode = ParameterBuildMode.Local, Name = "P1"});
         container2.Add(new Parameter {BuildMode = ParameterBuildMode.Local, Name = "P2"});

         _object1 = container1;
         _object2 = container2;
      }

      [Observation]
      public void should_return_a_report_containing_the_diff_for_the_common_children_an_one_entry_for_each_missing_child()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_two_containers_having_a_difference_in_the_3rd_level_of_children : concern_for_ObjectComparer
   {
      private Container _c111;

      protected override void Context()
      {
         base.Context();
         var c1 = new Container {Name = "O"};
         var c11 = new Container {Name = "OO"}.WithParentContainer(c1);
         _c111 = new Container { Name = "OOO" }.WithParentContainer(c11);
         _c111.Add(new Parameter { BuildMode = ParameterBuildMode.Global, Name = "P1" });

         var c2 = new Container {Name = "O"};
         var c22 = new Container {Name = "OO"}.WithParentContainer(c2);
         var c222 = new Container {Name = "OOO"}.WithParentContainer(c22);
         c222.Add(new Parameter {BuildMode = ParameterBuildMode.Local, Name = "P1"});

         _object1 = c1;
         _object2 = c2;
      }

      [Observation]
      public void should_have_set_the_parent_to_the_common_3rd_level()
      {
         _report.Count().ShouldBeEqualTo(1);
         _report.ElementAt(0).CommonAncestor.ShouldBeEqualTo(_c111);
      }
   }
}
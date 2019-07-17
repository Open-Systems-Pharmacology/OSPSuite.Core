using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.DiffBuilders
{
   public class When_comparring_two_similar_neighborhood_builder : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var n1 = new NeighborhoodBuilder().WithName("cell2int");
         var n2 = new NeighborhoodBuilder().WithName("cell2int");

         var tc1 = new Container().WithName("Root");
         var cell1 = new Container().WithName("Cell").WithParentContainer(tc1);
         var int1 = new Container().WithName("int").WithParentContainer(tc1);
         n1.FirstNeighbor = cell1;
         n1.SecondNeighbor = int1;

         var tc2 = new Container().WithName("Root");
         var cell2 = new Container().WithName("Cell").WithParentContainer(tc2);
         var int2 = new Container().WithName("int").WithParentContainer(tc2);
         n2.FirstNeighbor = cell2;
         n2.SecondNeighbor = int2;

         _object1 = n1;
         _object2 = n2;
      }

      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }

   public class When_comparring_two_neighborhood_builder_with_swapped_neighbors : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var n1 = new NeighborhoodBuilder().WithName("cell2int");
         var n2 = new NeighborhoodBuilder().WithName("cell2int");

         var tc1 = new Container().WithName("Root");
         var cell1 = new Container().WithName("Cell").WithParentContainer(tc1);
         var int1 = new Container().WithName("int").WithParentContainer(tc1);
         n1.FirstNeighbor = cell1;
         n1.SecondNeighbor = int1;

         var tc2 = new Container().WithName("Root");
         var cell2 = new Container().WithName("Cell").WithParentContainer(tc2);
         var int2 = new Container().WithName("int").WithParentContainer(tc2);
         n2.FirstNeighbor = int2;
         n2.SecondNeighbor = cell2;

         _object1 = n1;
         _object2 = n2;
      }

      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }
   public class When_comparring_two_neighborhood_builder_with_different_neighbors : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var n1 = new NeighborhoodBuilder().WithName("cell2int");
         var n2 = new NeighborhoodBuilder().WithName("cell2int");

         var tc1 = new Container().WithName("Root");
         var cell1 = new Container().WithName("Cell").WithParentContainer(tc1);
         var int1 = new Container().WithName("int").WithParentContainer(tc1);
         n1.FirstNeighbor = cell1;
         n1.SecondNeighbor = int1;

         var tc2 = new Container().WithName("Root");
         var cell2 = new Container().WithName("Cell").WithParentContainer(tc2);
         var int2 = new Container().WithName("pls").WithParentContainer(tc2);
         n2.FirstNeighbor = int2;
         n2.SecondNeighbor = cell2;

         _object1 = n1;
         _object2 = n2;
      }

      [Observation]
      public void should_report_one_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
}
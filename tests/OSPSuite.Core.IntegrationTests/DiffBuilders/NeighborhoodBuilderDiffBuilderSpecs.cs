using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.DiffBuilders
{
   public abstract class concern_for_NeighborhoodBuilderDiffBuilder  : concern_for_ObjectComparer
   {
      protected IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         base.Context();
         _objectPathFactory = IoC.Resolve<IObjectPathFactory>();
      }
   }
   public class When_comparing_two_similar_neighborhood_builder : concern_for_NeighborhoodBuilderDiffBuilder
   {
      protected override void Context()
      {
         base.Context();
         var n1 = new NeighborhoodBuilder().WithName("cell2int");
         var n2 = new NeighborhoodBuilder().WithName("cell2int");

         var tc1 = new Container().WithName("Root");
         var cell1 = new Container().WithName("Cell").WithParentContainer(tc1);
         var int1 = new Container().WithName("int").WithParentContainer(tc1);
         n1.FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(cell1);
         n1.SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(int1);

         var tc2 = new Container().WithName("Root");
         var cell2 = new Container().WithName("Cell").WithParentContainer(tc2);
         var int2 = new Container().WithName("int").WithParentContainer(tc2);
         n2.FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(cell2);
         n2.SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(int2);

         _object1 = n1;
         _object2 = n2;
      }

      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }

   public class When_comparing_two_neighborhood_builder_with_swapped_neighbors : concern_for_NeighborhoodBuilderDiffBuilder
   {
      protected override void Context()
      {
         base.Context();
         var n1 = new NeighborhoodBuilder().WithName("cell2int");
         var n2 = new NeighborhoodBuilder().WithName("cell2int");

         var tc1 = new Container().WithName("Root");
         var cell1 = new Container().WithName("Cell").WithParentContainer(tc1);
         var int1 = new Container().WithName("int").WithParentContainer(tc1);
         n1.FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(cell1);
         n1.SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(int1);

         var tc2 = new Container().WithName("Root");
         var cell2 = new Container().WithName("Cell").WithParentContainer(tc2);
         var int2 = new Container().WithName("int").WithParentContainer(tc2);

         n2.FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(int2);
         n2.SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(cell2);

         _object1 = n1;
         _object2 = n2;
      }

      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }
   public class When_comparing_two_neighborhood_builder_with_different_neighbors : concern_for_NeighborhoodBuilderDiffBuilder
   {
      protected override void Context()
      {
         base.Context();
         var n1 = new NeighborhoodBuilder().WithName("cell2int");
         var n2 = new NeighborhoodBuilder().WithName("cell2int");

         var tc1 = new Container().WithName("Root");
         var cell1 = new Container().WithName("Cell").WithParentContainer(tc1);
         var int1 = new Container().WithName("int").WithParentContainer(tc1);
         n1.FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(cell1);
         n1.SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(int1);

         var tc2 = new Container().WithName("Root");
         var cell2 = new Container().WithName("Cell").WithParentContainer(tc2);
         var int2 = new Container().WithName("pls").WithParentContainer(tc2);
         n2.FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(int2);
         n2.SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(cell2);

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
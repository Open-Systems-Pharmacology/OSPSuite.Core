using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.DiffBuilders
{
   public class concern_for_ContainerDiffBuilder : ContextSpecification<ContainerDiffBuilder>
   {
      protected IComparison<IContainer> _comparison;
      protected DiffReport _report;

      protected override void Context()
      {
         var entityDiffBuilder = A.Fake<EntityDiffBuilder>();
         var enumerableComparer = A.Fake<EnumerableComparer>();
         var settings = new ComparerSettings();
         _report = new DiffReport();
         var container1 = new Container().WithName("Container1");
         var container2 = new Container().WithName("Container2");
         container1.ParentPath = new ObjectPath("A", "B");
         container2.ParentPath = new ObjectPath("C", "D");

         _comparison = new Comparison<IContainer>(container1, container2, settings, _report, null);
         sut = new ContainerDiffBuilder(entityDiffBuilder, enumerableComparer);
      }
   }

   public class when_comparing_two_containers_with_different_parent_paths : concern_for_ContainerDiffBuilder
   {
      protected override void Because()
      {
         sut.Compare(_comparison);
      }

      [Observation]
      public void should_report_path_changes()
      {
         _report.Count.ShouldBeEqualTo(1);
         Assert.IsTrue(_report.First().Description.StartsWith("Parent Paths are not equal"));
      }
   }
}
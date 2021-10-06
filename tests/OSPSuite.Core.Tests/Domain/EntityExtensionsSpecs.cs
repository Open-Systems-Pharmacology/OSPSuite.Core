using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_EntityExtensions : StaticContextSpecification
   {
      protected Container _sim;
      protected Container _organ;
      protected Container _cell;
      private ARootContainer _rootContainer;
      private Container _liver;
      protected Container _liverCell;

      protected override void Context()
      {
         _sim = new Container().WithName("S1");
         _organ = new Container().WithName("Liver");
         _cell = new Container().WithName("Cell");
         _sim.Add(_organ);
         _organ.Add(_cell);

         _rootContainer = new ARootContainer().WithName("ROOT").WithParentContainer(new Container().WithName("SHOULD_NOT_BE_PART_OF_PATH"));
         _liver = new Container().WithName("Liver");
         _liverCell = new Container().WithName("Cell");
         _liver.Add(_liverCell);
         _rootContainer.Add(_liver);

      }
   }

   public class When_resolving_the_entity_path_of_an_entity : concern_for_EntityExtensions
   {
      [Observation]
      public void should_return_the_expected_path()
      {
         _cell.EntityPath().ShouldBeEqualTo(new List<string>(new[] {"S1", "Liver", "Cell"}).ToPathString());
         _organ.EntityPath().ShouldBeEqualTo(new List<string>(new[] {"S1", "Liver"}).ToPathString());
         _sim.EntityPath().ShouldBeEqualTo(new List<string>(new[] {"S1"}).ToPathString());
      }
   }

   public class When_resolving_the_entity_path_of_an_entity_under_a_root_container : concern_for_EntityExtensions
   {
      [Observation]
      public void should_return_the_expected_path()
      {
         _liverCell.EntityPath().ShouldBeEqualTo(new List<string>(new[] { "ROOT", "Liver", "Cell" }).ToPathString());
      }
   }

   public class When_resolving_the_consolidated_path_of_an_entity : concern_for_EntityExtensions
   {
      [Observation]
      public void should_return_the_expected_path()
      {
         _cell.ConsolidatedPath().ShouldBeEqualTo(new List<string>(new[] {"Liver", "Cell"}).ToPathString());
         _organ.ConsolidatedPath().ShouldBeEqualTo(new List<string>(new[] {"Liver"}).ToPathString());
         _sim.ConsolidatedPath().ShouldBeEqualTo(new List<string>(new[] {"S1"}).ToPathString());
      }
   }
}
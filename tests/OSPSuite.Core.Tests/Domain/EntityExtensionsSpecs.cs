using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_EntityExtensions : StaticContextSpecification
   {
      protected Container _sim;
      protected Container _organ;
      protected Container _cell;

      protected override void Context()
      {
         _sim = new Container().WithName("S1");
         _organ = new Container().WithName("Liver");
         _cell = new Container().WithName("Cell");
         _sim.Add(_organ);
         _organ.Add(_cell);
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
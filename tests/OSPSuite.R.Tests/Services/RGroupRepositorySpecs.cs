using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.CLI.Core.MinimalImplementations;
using OSPSuite.Core.Domain;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_RGroupRepository : ContextSpecification<IGroupRepository>
   {
      protected override void Context()
      {
         sut = new GroupRepository();
      }
   }

   public class When_retrieving_a_group_by_id_or_name : concern_for_RGroupRepository
   {
      [Observation]
      public void should_return_the_same_instance()
      {
         var group1 = sut.GroupById("ID");
         var group2 = sut.GroupByName("ID");
         var group3 = sut.GroupById("ID");

         Assert.AreSame(group1, group2);
         Assert.AreSame(group2, group3);
      }
   }

   public class When_retrieving_a_groups_with_different_id_or_name : concern_for_RGroupRepository
   {
      [Observation]
      public void should_return_the_different_instance()
      {
         var group1 = sut.GroupById("ID1");
         var group2 = sut.GroupByName("ID2");
         var group3 = sut.GroupById("ID3");

         Assert.AreNotSame(group1, group2);
         Assert.AreNotSame(group2, group3);
         Assert.AreNotSame(group1, group3);
      }
   }
}
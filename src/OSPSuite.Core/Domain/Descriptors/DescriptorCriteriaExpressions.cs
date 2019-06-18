using System;

namespace OSPSuite.Core.Domain.Descriptors
{
   public static class Create
   {
      public static DescriptorCriteria Criteria(Action<DescriptorCriteriaBuilder> descriptorAction)
      {
         var descriptorBuilder = new DescriptorCriteriaBuilder();
         descriptorAction(descriptorBuilder);
         return descriptorBuilder.Build();
      }
   }

   public class DescriptorCriteriaBuilder
   {
      private readonly DescriptorCriteria _criteria;

      public DescriptorCriteriaBuilder()
      {
         _criteria = new DescriptorCriteria();
      }

      public DescriptorCriteriaBuilder And => this;

      public DescriptorCriteriaBuilder With(string match)
      {
         _criteria.Add(new MatchTagCondition(match));
         return this;
      }

      public DescriptorCriteriaBuilder Not(string match)
      {
         _criteria.Add(new NotMatchTagCondition(match));
         return this;
      }

      public DescriptorCriteriaBuilder InContainer(string containerName)
      {
         _criteria.Add(new InContainerCondition(containerName));
         return this;
      }

      public DescriptorCriteriaBuilder NotInContainer(string containerName)
      {
         _criteria.Add(new NotInContainerCondition(containerName));
         return this;
      }

      public DescriptorCriteria Build()
      {
         return _criteria;
      }
   }
}
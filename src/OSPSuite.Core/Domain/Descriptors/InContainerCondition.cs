using System;

namespace OSPSuite.Core.Domain.Descriptors
{
   public class InContainerCondition : TagCondition
   {
      [Obsolete("For serialization")]
      public InContainerCondition() : base(Constants.IN_CONTAINER)
      {
      }

      public InContainerCondition(string tag) : base(tag, Constants.IN_CONTAINER)
      {
      }

      public override IDescriptorCondition CloneCondition() => new InContainerCondition(Tag);

      public override bool IsSatisfiedBy(EntityDescriptor entityDescriptor)
      {
         return entityDescriptor.ParentContainerTags.Contains(Tag);
      }
   }
}
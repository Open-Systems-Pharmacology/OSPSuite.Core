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

      public override ITagCondition CloneCondition() => new InContainerCondition(Tag);

      public override bool IsSatisfiedBy(EntityDescriptor entityDescriptor)
      {
         return entityDescriptor.Tags.Contains(Tag) || entityDescriptor.ParentContainerTags.Contains(Tag);
      }
   }
}
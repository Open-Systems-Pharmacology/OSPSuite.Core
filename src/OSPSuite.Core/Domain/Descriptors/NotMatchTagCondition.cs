using System;

namespace OSPSuite.Core.Domain.Descriptors
{
   public class NotMatchTagCondition : TagCondition
   {
      [Obsolete("For serialization")]
      public NotMatchTagCondition():base(Constants.NOT)
      {
      }

      public NotMatchTagCondition(string tag) : base(tag, Constants.NOT)
      {
      }

      public override bool IsSatisfiedBy(EntityDescriptor entityDescriptor)
      {
         return !entityDescriptor.Tags.Contains(Tag);
      }

      public override IDescriptorCondition CloneCondition() => new NotMatchTagCondition(Tag);
   }
}
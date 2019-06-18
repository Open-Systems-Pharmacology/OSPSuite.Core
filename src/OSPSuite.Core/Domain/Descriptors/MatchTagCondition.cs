using System;

namespace OSPSuite.Core.Domain.Descriptors
{
   public class MatchTagCondition : TagCondition
   {
      [Obsolete("For serialization")]
      public MatchTagCondition():base(string.Empty)
      {
      }

      public MatchTagCondition(string tag) : base(tag, string.Empty)
      {
      }

      public override bool IsSatisfiedBy(EntityDescriptor entityDescriptor)
      {
         return entityDescriptor.Tags.Contains(Tag);
      }

      public override IDescriptorCondition CloneCondition()
      {
         return new MatchTagCondition(Tag);
      }

      public override string Condition => Tag;
   }
}
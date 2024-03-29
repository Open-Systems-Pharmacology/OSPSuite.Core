﻿using System;

namespace OSPSuite.Core.Domain.Descriptors
{
   public class NotInContainerCondition : TagCondition
   {
      [Obsolete("For serialization")]
      public NotInContainerCondition() : base(Constants.NOT_IN_CONTAINER)
      {
      }

      public NotInContainerCondition(string tag) : base(tag, Constants.NOT_IN_CONTAINER)
      {
      }

      public override ITagCondition CloneCondition() => new NotInContainerCondition(Tag);

      public override bool IsSatisfiedBy(EntityDescriptor entityDescriptor)
      {
         return !entityDescriptor.Tags.Contains(Tag) && !entityDescriptor.ParentContainerTags.Contains(Tag);
      }
   }
}
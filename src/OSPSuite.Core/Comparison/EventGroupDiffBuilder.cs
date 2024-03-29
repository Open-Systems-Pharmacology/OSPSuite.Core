﻿using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class EventGroupDiffBuilder : DiffBuilder<EventGroup>
   {
      private readonly ContainerDiffBuilder _containerDiffBuilder;

      public EventGroupDiffBuilder(ContainerDiffBuilder containerDiffBuilder)
      {
         _containerDiffBuilder = containerDiffBuilder;
      }

      public override void Compare(IComparison<EventGroup> comparison)
      {
         _containerDiffBuilder.Compare(comparison);
         CompareStringValues(x => x.EventGroupType, x => x.EventGroupType, comparison);
      }
   }
}
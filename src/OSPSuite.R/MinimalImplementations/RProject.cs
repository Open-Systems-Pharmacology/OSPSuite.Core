﻿using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.R.MinimalImplementations
{
   public class RProject : Project
   {
      public override bool HasChanged { get; set; }

      public override IReadOnlyCollection<T> All<T>()
      {
         throw new NotImplementedException();
      }

      public override IEnumerable<IUsesObservedData> AllUsersOfObservedData { get; } = new List<IUsesObservedData>();
   }
}
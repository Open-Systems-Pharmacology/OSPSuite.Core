﻿using NUnit.Framework;
using OSPSuite.BDDHelper;
using System;

namespace OSPSuite.Core.Extensions
{
   public abstract class concern_for_FunctionExtensions : ContextSpecification<Func<int, int>>
   {
      protected Func<int, int> inc;
      protected Func<int, int> dup;
      protected override void Context()
      {
         inc = x => x + 1;
         dup = x => x * 2;
      }
   }

   public class FunctionComposition_inc_dup : concern_for_FunctionExtensions
   {
      protected override void Because()
      {
         sut = x => inc.Compose(dup, 5);
      }

      [Observation]
      public void should_increment_then_duplicate()
      {
         Assert.AreEqual(sut.Invoke(5), 12);
      }
   }

   public class FunctionComposition_dup_inc : concern_for_FunctionExtensions
   {
      protected override void Because()
      {
         sut = x => dup.Compose(inc, 5);
      }

      [Observation]
      public void should_duplicate_then_increment()
      {
         Assert.AreEqual(sut.Invoke(5), 11);
      }
   }
}

﻿using System;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Builder
{
   public abstract class MergeCache<TKey, TValue> : Cache<TKey, TValue> where TValue : class
   {
      protected MergeCache(Func<TValue, TKey> getKey) : base(getKey, x => null)
      {
      }

      public override void Add(TValue value)
      {
         //override the Add to ensure that we are replacing an existing key
         this[GetKey(value)] = value;
      }
   }

   public class ObjectBaseCache<T> : MergeCache<string, T> where T : class, IObjectBase
   {
      public ObjectBaseCache() : base(x => x.Name)
      {
      }
   }

   public class StartValueCache<T> : MergeCache<ObjectPath, T> where T : PathAndValueEntity
   {
      public StartValueCache() : base(x => x.Path)
      {
      }
   }
}
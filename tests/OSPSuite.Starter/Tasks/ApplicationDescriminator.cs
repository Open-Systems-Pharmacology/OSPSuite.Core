using System;
using System.Collections.Generic;
using OSPSuite.Core;
using OSPSuite.Core.Domain;

namespace OSPSuite.Starter.Tasks
{
   public class ApplicationDiscriminator : IApplicationDiscriminator
   {
      public string DiscriminatorFor<T>(T item) where T : IObjectBase
      {
         throw new NotSupportedException();
      }

      public IReadOnlyCollection<IObjectBase> AllFor(string discriminator)
      {
         throw new NotSupportedException();
      }
   }
}

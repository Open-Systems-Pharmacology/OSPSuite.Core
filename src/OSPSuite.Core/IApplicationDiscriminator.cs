using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public interface IApplicationDiscriminator
   {
      /// <summary>
      /// Returns an application specfic discriminator allowing for the given <paramref name="item"/>
      /// </summary>
      string DiscriminatorFor<T>(T item) where T : IObjectBase;

      /// <summary>
      /// Returns all objects defined in the application that have the given <paramref name="discriminator"/>
      /// </summary>
      /// <param name="discriminator">Use to filter specific objects in the application</param>
      IReadOnlyCollection<IObjectBase> AllFor(string discriminator);
   }
}
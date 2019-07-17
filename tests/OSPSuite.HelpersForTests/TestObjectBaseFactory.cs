using System;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Helpers
{
   public class TestObjectBaseFactory : IObjectBaseFactory
   {
      // other methods are not necessary
      public T Create<T>() where T : class,  IObjectBase
      {

         if (typeof(T).IsAnImplementationOf<IMoleculeAmount>())
            return new MoleculeAmount().DowncastTo<T>();
         
         if (typeof(T).IsAnImplementationOf<INeighborhoodBuilder>())
            return new NeighborhoodBuilder().DowncastTo<T>();

         return A.Fake<T>();
      }

      public T Create<T>(string id) where T : class, IObjectBase
      {
         throw new NotSupportedException();
      }

      public T CreateObjectBaseFrom<T>(Type objectType) 
      {
         throw new NotSupportedException();
      }

      public T CreateObjectBaseFrom<T>(Type objectType, string id) 
      {
         throw new NotSupportedException();
      }

      public T CreateObjectBaseFrom<T>(T sourceObject) 
      {
         throw new NotSupportedException();
      }
   }
}
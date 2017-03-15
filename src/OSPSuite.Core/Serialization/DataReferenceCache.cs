using System.Collections.Generic;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Serialization
{
   public class DataColumnCollectionReference
   {
      private readonly ICache<AuxiliaryType, DataColumn> _cache;
      private readonly IWithIdRepository _withIdRepository;
      private readonly List<string> _ids;

      public DataColumnCollectionReference(ICache<AuxiliaryType, DataColumn> cache, IWithIdRepository withIdRepository)
      {
         _cache = cache;
         _withIdRepository = withIdRepository;
         _ids = new List<string>();
      }

      public void Add(string id)
      {
         _ids.Add(id);
      }

      public void ResolveReferences()
      {
         _ids.Each(id => _cache.Add(_withIdRepository.Get<DataColumn>(id)));
      }
   }

   public class DataReferenceCache
   {
      private readonly IWithIdRepository _withIdRepository;
      private readonly List<DataColumnCollectionReference> _collectionReferences;

      public DataReferenceCache(IWithIdRepository withIdRepository)
      {
         _withIdRepository = withIdRepository;
         _collectionReferences = new List<DataColumnCollectionReference>();
      }

      public DataColumnCollectionReference AddCollectionReference(ICache<AuxiliaryType, DataColumn> cache)
      {
         var collectionReference = new DataColumnCollectionReference(cache, _withIdRepository);
         _collectionReferences.Add(collectionReference);
         return collectionReference;
      }

      public void ResolveReferences()
      {
         _collectionReferences.Each(x => x.ResolveReferences());
      }

      public void Clear()
      {
         _collectionReferences.Clear();
      }
   }
}
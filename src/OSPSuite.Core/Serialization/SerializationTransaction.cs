using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core.Serialization
{
   /// <summary>
   ///    Encapsulate the initialization of globals for serialization/deserialization.
   ///    This class should be used with the using construct.
   /// </summary>
   public static class SerializationTransaction
   {
      /// <summary>
      ///    Use this constructor when deserializing
      /// </summary>
      public static SerializationContext Create(
         IContainer container,
         IDimensionFactory dimensionFactory = null, 
         IObjectBaseFactory objectBaseFactory = null, 
         IWithIdRepository withIdRepository = null, 
         ICloneManagerForModel cloneManagerForModel = null, 
         IEnumerable<DataRepository> dataRepositories = null)
      {
         return new SerializationContext(dimensionFactory, objectBaseFactory, withIdRepository, dataRepositories, cloneManagerForModel, container);
      }
   }
}
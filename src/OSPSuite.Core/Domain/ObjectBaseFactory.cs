using System;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class ObjectBaseFactory : IObjectBaseFactory
   {
      private readonly Utility.Container.IContainer _container;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IIdGenerator _idGenerator;
      private readonly ICreationMetaDataFactory _creationMetaDataFactory;

      public ObjectBaseFactory(Utility.Container.IContainer container, IDimensionFactory dimensionFactory,
         IIdGenerator idGenerator, ICreationMetaDataFactory creationMetaDataFactory)
      {
         _container = container;
         _dimensionFactory = dimensionFactory;
         _idGenerator = idGenerator;
         _creationMetaDataFactory = creationMetaDataFactory;
      }

      public virtual T Create<T>() where T : class, IObjectBase
      {
         return Create<T>(GetId());
      }

      public virtual T Create<T>(string id) where T : class, IObjectBase
      {
         var newObject = _container.Resolve<T>().WithId(id);
         updateDimension(newObject);
         updateCreationMetaData(newObject);
         return newObject;
      }

      private void updateCreationMetaData(IObjectBase newObject)
      {
         var withCreationMetaData = newObject as IWithCreationMetaData;
         if (withCreationMetaData != null)
            withCreationMetaData.Creation = _creationMetaDataFactory.Create();
      }

      private void updateDimension(object newObject)
      {
         var hasDimension = newObject as IWithDimension;
         if (hasDimension != null)
            hasDimension.Dimension = _dimensionFactory.NoDimension;
      }

      public virtual T CreateObjectBaseFrom<T>(Type objectType)
      {
         return CreateObjectBaseFrom<T>(objectType, GetId());
      }

      public virtual T CreateObjectBaseFrom<T>(Type objectType, string id)
      {
         var objectBase = Activator.CreateInstance(objectType)
            .ConvertedTo<T>();

         var withId = objectBase as IWithId;
         if (withId != null)
            withId.Id = id;

         updateDimension(objectBase);
         return objectBase;
      }

      public virtual T CreateObjectBaseFrom<T>(T sourceObject)
      {
         return CreateObjectBaseFrom<T>(sourceObject.GetType());
      }

      protected virtual string GetId()
      {
         return _idGenerator.NewId();
      }
   }
}
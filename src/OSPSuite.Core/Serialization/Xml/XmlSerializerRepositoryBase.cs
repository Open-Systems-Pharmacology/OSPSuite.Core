using System;
using System.Diagnostics;
using System.Reflection;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Attributes;
using OSPSuite.Serializer.Xml;

namespace OSPSuite.Core.Serialization.Xml
{
   /// <summary>
   ///    Abstract base class for XmlSerializerRepositories
   /// </summary>
   public abstract class XmlSerializerRepositoryBase : XmlSerializerRepository<SerializationContext>
   {
      public IAttributeMapperRepository<SerializationContext> AttributeMapperRepository { get; private set; }

      protected XmlSerializerRepositoryBase()
      {
         initialize();
      }

      /// <summary>
      ///    Initializes XmlSerializerRepository with DefaultMappers, InitialMappers, InitialSerializers
      /// </summary>
      private void initialize()
      {
         AttributeMapperRepository = new AttributeMapperRepository<SerializationContext>();
         AttributeMapperRepository.AddDefaultAttributeMappers();
         AddInitialMappers();

         try
         {
            AddInitialSerializer();
         }
         catch (ReflectionTypeLoadException ex)
         {
            foreach (var item in ex.LoaderExceptions)
            {
               Debug.Print(item.Message);
            }
         }
      }

      /// <summary>
      ///    Adds all serializers from the assambly of T, which implement T
      /// </summary>
      protected virtual void AddSerializersSimple<T>() where T : class, IXmlSerializer
      {
         this.AddSerializers(x =>
         {
            x.Implementing<T>();
            x.InAssemblyContainingType<T>();
            x.UsingAttributeRepository(AttributeMapperRepository);
         });
      }

      protected abstract void AddInitialMappers();
      protected abstract void AddInitialSerializer();
   }
}
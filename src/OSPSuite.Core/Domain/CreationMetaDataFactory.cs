namespace OSPSuite.Core.Domain
{
   public interface ICreationMetaDataFactory
   {
      CreationMetaData Create();
   }

   public class CreationMetaDataFactory : ICreationMetaDataFactory
   {
      private readonly IApplicationConfiguration _applicationConfiguration;

      public CreationMetaDataFactory(IApplicationConfiguration applicationConfiguration)
      {
         _applicationConfiguration = applicationConfiguration;
      }

      public CreationMetaData Create()
      {
         return new CreationMetaData
         {
            Origin = _applicationConfiguration.Product,
            CreationMode = CreationMode.New,
            Version = _applicationConfiguration.Version,
            InternalVersion = _applicationConfiguration.InternalVersion
         };
      }
   }
}
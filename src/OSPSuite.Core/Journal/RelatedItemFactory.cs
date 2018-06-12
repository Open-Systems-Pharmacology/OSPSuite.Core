using System;
using System.IO;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility;

namespace OSPSuite.Core.Journal
{
   public interface IRelatedItemFactory
   {
      /// <summary>
      ///    Creates and reaturns a related item containing the given <paramref name="relatedObject" /> as data.
      /// </summary>
      RelatedItem Create<T>(T relatedObject) where T : class, IObjectBase;

      /// <summary>
      ///    Creates and reaturns a related item containing the content of the file located at <paramref name="fileFullPath" />
      ///    as data.
      /// </summary>
      RelatedItem CreateFromFile(string fileFullPath);
   }

   public class RelatedItemFactory : IRelatedItemFactory
   {
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IApplicationConfiguration _applicationConfiguration;
      private readonly IProjectRetriever _projectRetriever;
      private readonly IApplicationDiscriminator _applicationDiscriminator;
      private readonly IRelatedItemSerializer _relatedItemSerializer;
      private readonly IRelatedItemDescriptionCreator _relatedItemDescriptionCreator;
      private readonly IRelatedItemTypeRetriever _relatedItemTypeRetriever;
      private readonly IFileExtensionToIconMapper _iconMapper;

      public RelatedItemFactory(IOSPSuiteExecutionContext executionContext,
         IApplicationConfiguration applicationConfiguration,
         IProjectRetriever projectRetriever,
         IApplicationDiscriminator applicationDiscriminator,
         IRelatedItemSerializer relatedItemSerializer,
         IRelatedItemDescriptionCreator relatedItemDescriptionCreator,
         IRelatedItemTypeRetriever relatedItemTypeRetriever,
         IFileExtensionToIconMapper iconMapper)
      {
         _executionContext = executionContext;
         _applicationConfiguration = applicationConfiguration;
         _projectRetriever = projectRetriever;
         _applicationDiscriminator = applicationDiscriminator;
         _relatedItemSerializer = relatedItemSerializer;
         _relatedItemDescriptionCreator = relatedItemDescriptionCreator;
         _relatedItemTypeRetriever = relatedItemTypeRetriever;
         _iconMapper = iconMapper;
      }

      public RelatedItem Create<T>(T relatedObject) where T : class, IObjectBase
      {
         _executionContext.Load(relatedObject);

         var name = relatedObject.Name;
         var data = _relatedItemSerializer.Serialize(relatedObject);
         var itemType = _relatedItemTypeRetriever.TypeFor(relatedObject);

         return createRelatedItem(name, itemType, data, relatedItem =>
         {
            relatedItem.Origin = _applicationConfiguration.Product;
            relatedItem.IconName = relatedObject.Icon;
            relatedItem.FullPath = _projectRetriever.CurrentProject.FilePath;
            relatedItem.Discriminator = _applicationDiscriminator.DiscriminatorFor(relatedObject);
            relatedItem.Description = _relatedItemDescriptionCreator.DescriptionFor(relatedObject);
         });
      }

      public RelatedItem CreateFromFile(string fileFullPath)
      {
         var fileInfo = new FileInfo(fileFullPath);
         var name = fileInfo.Name;
         var data = File.ReadAllBytes(fileFullPath);
         var itemType = Constants.RELATIVE_ITEM_FILE_ITEM_TYPE;

         return createRelatedItem(name, itemType, data, x =>
         {
            x.Origin = Origins.Other;
            x.IconName = _iconMapper.MapFrom(fileInfo.Extension).IconName;
            x.FullPath = fileFullPath;
            x.Discriminator = itemType;
         });
      }

      private RelatedItem createRelatedItem(string name, string itemType, byte[] data, Action<RelatedItem> configurationFunc)
      {
         var relatedItem = new RelatedItem
         {
            CreatedAt = SystemTime.UtcNow(),
            Name = name,
            ItemType = itemType,
            Content = new Content
            {
               Data = data
            },
            Version = _applicationConfiguration.FullVersion,
         };

         configurationFunc(relatedItem);
         return relatedItem;
      }
   }
}
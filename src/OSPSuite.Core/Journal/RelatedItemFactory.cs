using OSPSuite.Utility;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Journal
{
   public interface IRelatedItemFactory
   {
      RelatedItem Create<T>(T relatedObject) where T : class, IObjectBase;
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

      public RelatedItemFactory(IOSPSuiteExecutionContext executionContext, IApplicationConfiguration applicationConfiguration,
         IProjectRetriever projectRetriever, IApplicationDiscriminator applicationDiscriminator, 
         IRelatedItemSerializer relatedItemSerializer,IRelatedItemDescriptionCreator relatedItemDescriptionCreator,
         IRelatedItemTypeRetriever relatedItemTypeRetriever)
      {
         _executionContext = executionContext;
         _applicationConfiguration = applicationConfiguration;
         _projectRetriever = projectRetriever;
         _applicationDiscriminator = applicationDiscriminator;
         _relatedItemSerializer = relatedItemSerializer;
         _relatedItemDescriptionCreator = relatedItemDescriptionCreator;
         _relatedItemTypeRetriever = relatedItemTypeRetriever;
      }

      public RelatedItem Create<T>(T relatedObject) where T : class, IObjectBase
      {
         _executionContext.Load(relatedObject);

         return new RelatedItem
         {
            CreatedAt = SystemTime.UtcNow(),
            Name = relatedObject.Name,
            ItemType = _relatedItemTypeRetriever.TypeFor(relatedObject),
            Content = new Content
            {
               Data = _relatedItemSerializer.Serialize(relatedObject)
            },
            Origin = _applicationConfiguration.Product,
            Version = _applicationConfiguration.FullVersion,
            IconName = relatedObject.Icon,
            FullPath = _projectRetriever.CurrentProject.FilePath,
            Discriminator = _applicationDiscriminator.DiscriminatorFor(relatedObject),
            Description = _relatedItemDescriptionCreator.DescriptionFor(relatedObject)
         };
      }
   }
}
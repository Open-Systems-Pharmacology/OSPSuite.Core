using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Journal
{
   public interface IRelatedItemSerializer
   {
      byte[] Serialize(IObjectBase relatedObject);
      IObjectBase Deserialize(RelatedItem relatedItem);
   }

   public class RelatedItemSerializer : IRelatedItemSerializer
   {
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IApplicationConfiguration _applicationConfiguration;

      public RelatedItemSerializer(IOSPSuiteExecutionContext executionContext, IApplicationConfiguration applicationConfiguration)
      {
         _executionContext = executionContext;
         _applicationConfiguration = applicationConfiguration;
      }

      public byte[] Serialize(IObjectBase relatedObject)
      {
         return _executionContext.Serialize(relatedObject);
      }

      public IObjectBase Deserialize(RelatedItem relatedItem)
      {
         if (_applicationConfiguration.Product == relatedItem.Origin)
            return _executionContext.Deserialize<IObjectBase>(relatedItem.Content.Data);

         throw new OSPSuiteException(Error.CannotLoadRelatedItemCreatedWithAnotherApplication(relatedItem.ItemType, relatedItem.Name, relatedItem.Origin.DisplayName, _applicationConfiguration.Product.DisplayName));
      }
   }
}
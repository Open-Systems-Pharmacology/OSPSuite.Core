using OSPSuite.Assets;
using OSPSuite.Assets.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Services
{
   public interface IRelatedItemTypeRetriever
   {
      string TypeFor<T>(T relatedObject) where T : class;
   }

   public class RelatedItemTypeRetriever : IRelatedItemTypeRetriever
   {
      private readonly IOSPSuiteExecutionContext _context;

      public RelatedItemTypeRetriever(IOSPSuiteExecutionContext context)
      {
         _context = context;
      }

      public string TypeFor<T>(T relatedObject) where T : class
      {
         var type = _context.TypeFor(relatedObject);

         type = type.Replace(ObjectTypes.BuildingBlock, string.Empty).Trim();

         if (isStartValueBuildingBlock(relatedObject))
            return type.ToAcronym().Pluralize();

         return type.PluralizeIf(shouldPluralize(relatedObject));
      }

      private static bool isStartValueBuildingBlock<T>(T relatedObject) where T : class
      {
         return relatedObject.IsAnImplementationOf<IParameterStartValuesBuildingBlock>() ||
                relatedObject.IsAnImplementationOf<IMoleculeStartValuesBuildingBlock>();
      }

      private static bool shouldPluralize<T>(T relatedObject) where T : class
      {
         return relatedObject.IsAnImplementationOf<IMoleculeBuildingBlock>() ||
                relatedObject.IsAnImplementationOf<IReactionBuildingBlock>() ||
                relatedObject.IsAnImplementationOf<IPassiveTransportBuildingBlock>() ||
                relatedObject.IsAnImplementationOf<IObserverBuildingBlock>() ||
                relatedObject.IsAnImplementationOf<IEventGroupBuildingBlock>();
      }
   }
}
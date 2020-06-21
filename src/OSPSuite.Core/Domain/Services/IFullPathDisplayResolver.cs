using OSPSuite.Core.Domain.Mappers;

namespace OSPSuite.Core.Domain.Services
{
   public interface IFullPathDisplayResolver
   {
      string FullPathFor(IObjectBase objectBase, bool addSimulationName = false);
   }

   public class FullPathDisplayResolver : IFullPathDisplayResolver
   {
      private readonly IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;

      public FullPathDisplayResolver(IQuantityPathToQuantityDisplayPathMapper quantityDisplayPathMapper)
      {
         _quantityDisplayPathMapper = quantityDisplayPathMapper;
      }

      public virtual string FullPathFor(IObjectBase objectBase, bool addSimulationName = false)
      {
         switch (objectBase)
         {
            case IQuantity quantity:
               return _quantityDisplayPathMapper.DisplayPathAsStringFor(quantity, addSimulationName);
            case IEntity entity:
               return entity.EntityPath();
         }

         return DisplayFor(objectBase);
      }

      protected virtual string DisplayFor(IObjectBase objectBase) => objectBase.Name;
   }
}
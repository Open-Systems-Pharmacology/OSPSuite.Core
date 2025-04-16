namespace OSPSuite.Core.Domain.Builder
{
   public interface ILookupBuildingBlock<T> : IBuildingBlock<T> where T : IBuilder
   {
      T ByPath(ObjectPath path);
   }
}
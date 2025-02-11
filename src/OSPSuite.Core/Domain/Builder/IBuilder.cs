namespace OSPSuite.Core.Domain.Builder
{
   public interface IBuilder : IObjectBase
   {
      //Reference to building block containing this entity. This does not have to be serialized
      IBuildingBlock BuildingBlock { get; set; }
   }
}
namespace OSPSuite.Core.Diagram
{
   public interface IBaseObject : IWithVisible, IWithColor
   {
      IContainerBase GetParent();
   }
}
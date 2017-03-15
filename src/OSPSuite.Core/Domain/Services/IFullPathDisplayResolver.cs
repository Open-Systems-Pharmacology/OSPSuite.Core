namespace OSPSuite.Core.Domain.Services
{
   public interface IFullPathDisplayResolver
   {
      string FullPathFor(IObjectBase objectBase, bool addSimulationName = false);
   }
}
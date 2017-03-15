namespace OSPSuite.Core.Services
{
   public interface IChangePropagator
   {
      /// <summary>
      ///    Ensure that all values being currently edited are saved in the model. This should be call before starting for
      ///    instance a simulation
      /// </summary>
      void SaveChanges(); 
   }
}
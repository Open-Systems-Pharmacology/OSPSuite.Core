using OSPSuite.Utility.Events;

namespace OSPSuite.R.MinimalImplementations
{
   public class ProgressManager : IProgressManager
   {
      public IProgressUpdater Create()
      {
         return new ProgressUpdater();
      }
   }
}
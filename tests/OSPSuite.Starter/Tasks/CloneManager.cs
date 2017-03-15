using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Starter.Tasks
{
   internal class CloneManager : ICloneManager
   {
      public T Clone<T>(T objectToClone) where T : class, IUpdatable
      {
         return objectToClone;
      }
   }
}
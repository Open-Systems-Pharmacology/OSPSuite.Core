using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public interface IUpdatable
   {
      /// <summary>
      /// Update current element with the properties of the entities given as parameter.
      /// This function should typically be overridden in all sub classes.
      /// </summary>
      /// <param name="source">Object from which the properties should be updated</param>
      /// <param name="cloneManager">Clone Manager used to clone referenced object</param>
      void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager);
   }
}
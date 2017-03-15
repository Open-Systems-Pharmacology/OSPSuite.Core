namespace OSPSuite.Core.Domain.Services
{
   public interface ICloneManager
   {
      /// <summary>
      ///    Creates a clone of object passed
      /// </summary>
      /// <typeparam name="T">Any class inherited from <see cref="IUpdatable" /></typeparam>
      /// <param name="objectToClone">Source object to be cloned</param>
      /// <returns>Cloned object</returns>
      T Clone<T>(T objectToClone) where T : class, IUpdatable;
   }
}
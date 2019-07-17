namespace OSPSuite.Core.Services
{
   public interface IDisplayNameProvider
   {
      /// <summary>
      /// Retrieves the display name for the object given as parameter
      /// </summary>
      string DisplayNameFor(object objectToDisplay);
   }

}
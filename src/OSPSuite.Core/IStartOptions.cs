namespace OSPSuite.Core
{
   public interface IStartOptions
   {
      /// <summary>
      /// Returns <c>true</c> if the application was started in developer mode otherwise <c>false</c>
      /// </summary>
      bool IsDeveloperMode { get; }
   }
}
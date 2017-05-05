namespace OSPSuite.Core.Services
{
   public interface IStartableProcessFactory
   {
      /// <summary>
      ///    Starts the application located at <paramref name="applicationPath" /> with the given <paramref name="arguments" />
      /// </summary>
      /// <param name="applicationPath">Full path of application to start</param>
      /// <param name="arguments">Command line arguments</param>
      /// <returns>A <see cref="StartableProcess"/> monitoring the application located at <paramref name="applicationPath"/></returns>
      StartableProcess CreateStartableProcess(string applicationPath, params string[] arguments);
   }
}
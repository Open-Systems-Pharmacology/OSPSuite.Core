namespace OSPSuite.Core.Services
{
   public interface IActiveSubjectRetriever
   {
      T Active<T>() where T : class;
   }
}
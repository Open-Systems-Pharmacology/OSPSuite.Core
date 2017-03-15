namespace OSPSuite.Presentation.Services
{
   public interface ISingleStartPresenterTask
   {
      void StartForSubject<T>(T subject);
   }
}
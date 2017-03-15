using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Core
{
   public class PresenterSubject
   {
      public object Subject { get; set; }

      public bool Matches(IPresenter presenter)
      {
         var singleStartPresenter = presenter as ISubjectPresenter;
         if (singleStartPresenter == null)
            return false;

         return Equals(singleStartPresenter.Subject, Subject);
      }
   }
}
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.UICommands
{
   public abstract class ActiveObjectUICommand<T> : ObjectUICommand<T> where T : class
   {
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      protected ActiveObjectUICommand(IActiveSubjectRetriever activeSubjectRetriever)
      {
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      public override T Subject => base.Subject ?? _activeSubjectRetriever.Active<T>();
   }
}
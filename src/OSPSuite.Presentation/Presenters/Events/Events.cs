using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.Presenters.Events
{
   public class ApplicationInitializedEvent
   {
   }

   public abstract class HeavyWorkEvent
   {
      public bool ForceCursorChange { get; private set; }

      protected HeavyWorkEvent(bool forceHourGlassCursor = false)
      {
         ForceCursorChange = forceHourGlassCursor;
      }
   }

   public class HeavyWorkFinishedEvent : HeavyWorkEvent
   {
      public HeavyWorkFinishedEvent(bool forceHourGlassCursor = false)
         : base(forceHourGlassCursor)
      {
      }
   }

   public class HeavyWorkStartedEvent : HeavyWorkEvent
   {
      public HeavyWorkStartedEvent(bool forceHourGlassCursor = false)
         : base(forceHourGlassCursor)
      {
      }
   }

   public class DisableUIEvent
   {
   }

   public class EnableUIEvent
   {
      public bool ProjectLoaded { get; private set; }
      public IProject Project { get; private set; }

      public EnableUIEvent(IProject project, bool projectLoaded)
      {
         ProjectLoaded = projectLoaded;
         Project = project;
      }
   }

   public class ScreenActivatedEvent
   {
      public ISingleStartPresenter Presenter { get; private set; }

      public ScreenActivatedEvent(ISingleStartPresenter presenter)
      {
         Presenter = presenter;
      }
   }

   public class NoActiveScreenEvent
   {
   }
}
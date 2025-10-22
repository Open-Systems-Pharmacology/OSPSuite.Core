using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Events
{
   public abstract class ProjectEvent
   {
      public IProject Project { get; private set; }

      protected ProjectEvent(IProject project)
      {
         Project = project;
      }
   }

   public class ProjectChangedEvent : ProjectEvent
   {
      public ProjectChangedEvent(IProject project)
         : base(project)
      {
      }
   }

   public class ProjectLoadingEvent
   {
   }

   public class ProjectClosingEvent
   {
   }

   public class ProjectClosedEvent
   {
   }

   public class ProjectSavingEvent : ProjectEvent
   {
      public ProjectSavingEvent(IProject project)
         : base(project)
      {
      }
   }

   public class ProjectSavedEvent : ProjectEvent
   {
      public ProjectSavedEvent(IProject project)
         : base(project)
      {
      }
   }

   public class ProjectLoadedEvent : ProjectEvent
   {
      public ProjectLoadedEvent(IProject project)
         : base(project)
      {
      }
   }

   public class ProjectCreatedEvent : ProjectEvent
   {
      public ProjectCreatedEvent(IProject project)
         : base(project)
      {
      }
   }

   public class ObservedDataAddedEvent : ProjectEvent
   {
      public ObservedDataAddedEvent(IReadOnlyList<DataRepository> dataRepositories, IProject project)
         : base(project)
      {
         DataRepositories = dataRepositories;
      }

      public IReadOnlyList<DataRepository> DataRepositories { get; private set; }
   }

   public class ObservedDataRemovedEvent : ProjectEvent
   {
      public ObservedDataRemovedEvent(IReadOnlyList<DataRepository> dataRepositories, IProject project)
         : base(project)
      {
         DataRepositories = dataRepositories;
      }

      public IReadOnlyList<DataRepository> DataRepositories { get; private set; }
   }
}
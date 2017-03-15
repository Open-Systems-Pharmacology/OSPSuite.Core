using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Services
{
   public interface IPresentationSettingsTask
   {
      TPresentationSettings PresentationSettingsFor<TPresentationSettings>(IPresenterWithSettings presenter, IWithId subject) where TPresentationSettings : class, IPresentationSettings, new();
      TPresentationSettings PresentationSettingsFor<TPresentationSettings>(string presentationKey, IWithId subject) where TPresentationSettings : class, IPresentationSettings, new();
      void RemovePresentationSettingsFor(IWithId subject);
   }

   public class PresentationSettingsTask : IPresentationSettingsTask
   {
      private readonly IWithWorkspaceLayout _withWorkspaceLayout;

      public PresentationSettingsTask(IWithWorkspaceLayout withWorkspaceLayout)
      {
         _withWorkspaceLayout = withWorkspaceLayout;
      }

      public TPresentationSettings PresentationSettingsFor<TPresentationSettings>(string presentationKey, IWithId subject) where TPresentationSettings : class, IPresentationSettings, new()
      {
         var layoutItem = getMatchingLayoutItem(presentationKey, subject);

         if (layoutItem != null)
            return getPresenterSettingsFromLayout<TPresentationSettings>(layoutItem);

         layoutItem = new WorkspaceLayoutItem
         {
            PresentationKey = presentationKey,
            SubjectId = subject.Id,
            PresentationSettings = new TPresentationSettings()
         };
         _withWorkspaceLayout.WorkspaceLayout.AddLayoutItem(layoutItem);

         return layoutItem.PresentationSettings as TPresentationSettings;
      }

      public void RemovePresentationSettingsFor(IWithId subject)
      {
         getLayoutItemsForSubject(subject).ToList().Each(removeLayoutItem);
      }

      private void removeLayoutItem(WorkspaceLayoutItem item)
      {
         _withWorkspaceLayout.WorkspaceLayout.RemoveLayoutItem(item);
      }

      private IEnumerable<WorkspaceLayoutItem> getLayoutItemsForSubject(IWithId subject)
      {
         return _withWorkspaceLayout.WorkspaceLayout.LayoutItems.Where(x => subjectMatchesLayoutItem(subject, x));
      }

      public TPresenterSettings PresentationSettingsFor<TPresenterSettings>(IPresenterWithSettings presenter, IWithId subject) where TPresenterSettings : class, IPresentationSettings, new()
      {
         return PresentationSettingsFor<TPresenterSettings>(presenter.PresentationKey, subject);
      }

      private static TPresenterSettings getPresenterSettingsFromLayout<TPresenterSettings>(WorkspaceLayoutItem layoutItem) where TPresenterSettings : class, IPresentationSettings, new()
      {
         if (!layoutItem.PresentationSettings.IsAnImplementationOf<TPresenterSettings>())
         {
            layoutItem.PresentationSettings = new TPresenterSettings();
         }
         return layoutItem.PresentationSettings as TPresenterSettings;
      }

      private WorkspaceLayoutItem getMatchingLayoutItem(string presentationKey, IWithId subject)
      {
         return _withWorkspaceLayout.WorkspaceLayout.LayoutItems.FirstOrDefault(x => string.Equals(x.PresentationKey, presentationKey) && subjectMatchesLayoutItem(subject, x));
      }

      private static bool subjectMatchesLayoutItem(IWithId subject, WorkspaceLayoutItem x)
      {
         return string.Equals(x.SubjectId, subject.Id);
      }
   }

   public interface IWithWorkspaceLayout
   {
      IWorkspaceLayout WorkspaceLayout { get; set; }
   }
}
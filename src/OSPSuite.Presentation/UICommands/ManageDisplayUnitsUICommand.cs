using System;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.UICommands
{
   public abstract class ManageDisplayUnitsUICommand : IUICommand
   {
      private readonly IApplicationController _applicationController;
      private readonly Func<DisplayUnitsManager> _displayUnitsManagerRetriever;
      private readonly string _displayUnitsType;

      protected ManageDisplayUnitsUICommand(IApplicationController applicationController, Func<DisplayUnitsManager> displayUnitsManagerRetriever, string displayUnitsType)
      {
         _applicationController = applicationController;
         _displayUnitsManagerRetriever = displayUnitsManagerRetriever;
         _displayUnitsType = displayUnitsType;
      }

      public void Execute()
      {
         using (var displayUnitsPresenter = _applicationController.Start<IEditDisplayUnitsPresenter>())
         {
            displayUnitsPresenter.Edit(_displayUnitsManagerRetriever(), _displayUnitsType);
         }
      }
   }

   public class ManageUserDisplayUnitsUICommand : ManageDisplayUnitsUICommand
   {
      public ManageUserDisplayUnitsUICommand(IApplicationController applicationController, IPresentationUserSettings userSettings)
         : base(applicationController, () => userSettings.DisplayUnits, Captions.User)
      {
      }
   }
   public class ManageProjectDisplayUnitsUICommand : ManageDisplayUnitsUICommand
   {
      public ManageProjectDisplayUnitsUICommand(IApplicationController applicationController, IProjectRetriever projectRetriever)
         : base(applicationController, () => projectRetriever.CurrentProject.DisplayUnits, Captions.Project)
      {
      }
   }
}
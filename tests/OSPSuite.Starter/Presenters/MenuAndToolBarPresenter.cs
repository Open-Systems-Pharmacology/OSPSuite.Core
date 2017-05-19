using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Repositories;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Starter.Presenters
{
   public interface IMenuAndToolBarPresenter : IMainViewItemPresenter
   {
   }

   public class MenuAndToolBarPresenter : AbstractMenuAndToolBarPresenter, IMenuAndToolBarPresenter
   {
      public MenuAndToolBarPresenter(IMenuAndToolBarView view, IMenuBarItemRepository menuBarItemRepository, IMRUProvider mruProvider) : base(view, menuBarItemRepository, mruProvider)
      {
      }

      protected override void DisableMenuBarItemsForPogramStart()
      {
      }

      protected override void AddRibbonPages()
      {
         _view.AddPageGroupToPage(createGroupModelling(), "Modeling");
         _view.AddPageGroupToPage(createParmaeterIdentification(), "Modeling");
      }

      private IButtonGroup createParmaeterIdentification()
      {
         var button = ParameterIdentificationMenuBarButtons.RunParameterIdentification(new MenuBarItemId("Run", 0));

         return CreateButtonGroup.WithCaption("ParameterIdentification")
            .WithButton(CreateRibbonButton.From(button))
            .WithId("TEST");
      }

      private IButtonGroup createGroupModelling()
      {
         return CreateButtonGroup.WithCaption("TEST")
            .WithButton(createButton(ApplicationIcons.PopulationSimulationComparison, "Current"))
            .WithId("TEST");
      }

      private IRibbonBarItem createButton(ApplicationIcon icon, string caption)
      {
         return CreateRibbonButton.From(createMenuButton(icon,caption));
      }

      private IMenuBarButton createMenuButton(ApplicationIcon icon, string caption)
      {
         return CreateMenuButton.WithCaption(caption)
            .WithIcon(icon);
      }

   }
}
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
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
         _view.AddPageGroupToPage(createGroup(), "Modeling");
      }

      private IButtonGroup createGroup()
      {
         return CreateButtonGroup.WithCaption("TEST")
            .WithButton(createButton(ApplicationIcons.PopulationSimulationComparison, "Current"))
            .WithId("TEST");
      }

      private IRibbonBarItem createButton(ApplicationIcon icon, string caption)
      {
         return CreateRibbonButton.From(
            CreateMenuButton.WithCaption(caption)
               .WithIcon(icon)
            );
      }
   }
}
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Views.ContextMenus;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_CurveChartContextMenu : ContextSpecification<CurveChartContextMenu>
   {
      protected IContainer _container;
      protected IContextMenuView _contextMenuView;
      protected IChartDisplayPresenter _chartDisplayPresenter;
      protected CurveChart _curveChart;
      protected List<IMenuBarItem> _allMenuItems;

      protected override void Context()
      {
         _container = A.Fake<IContainer>();
         _contextMenuView = A.Fake<IContextMenuView>();
         _chartDisplayPresenter = A.Fake<IChartDisplayPresenter>();
         _curveChart = new CurveChart();
         _allMenuItems = new List<IMenuBarItem>();

         A.CallTo(() => _container.Resolve<IContextMenuView>()).Returns(_contextMenuView);
         A.CallTo(() => _contextMenuView.AddMenuItem(A<IMenuBarItem>._))
            .Invokes(x => _allMenuItems.Add(x.GetArgument<IMenuBarItem>(0)));
      }

      protected IMenuBarCheckItem AutoUpdateCheckItem =>
         _allMenuItems.OfType<IMenuBarCheckItem>().First(x => string.Equals(x.Caption, MenuNames.AutoUpdateChart));
   }

   public class When_creating_the_context_menu_for_a_chart_with_automatic_update_enabled : concern_for_CurveChartContextMenu
   {
      protected override void Context()
      {
         base.Context();
         _curveChart.AutoUpdateEnabled = true;
         sut = new CurveChartContextMenu(_curveChart, _chartDisplayPresenter, _container);
      }

      [Observation]
      public void should_show_the_automatic_update_entry_as_checked()
      {
         AutoUpdateCheckItem.Checked.ShouldBeTrue();
      }
   }

   public class When_creating_the_context_menu_for_a_chart_with_automatic_update_disabled : concern_for_CurveChartContextMenu
   {
      protected override void Context()
      {
         base.Context();
         _curveChart.AutoUpdateEnabled = false;
         sut = new CurveChartContextMenu(_curveChart, _chartDisplayPresenter, _container);
      }

      [Observation]
      public void should_show_the_automatic_update_entry_as_unchecked()
      {
         AutoUpdateCheckItem.Checked.ShouldBeFalse();
      }
   }

   public class When_disabling_the_automatic_update_from_the_chart_context_menu : concern_for_CurveChartContextMenu
   {
      protected override void Context()
      {
         base.Context();
         _curveChart.AutoUpdateEnabled = true;
         sut = new CurveChartContextMenu(_curveChart, _chartDisplayPresenter, _container);
      }

      protected override void Because()
      {
         AutoUpdateCheckItem.Checked = false;
      }

      [Observation]
      public void should_let_the_display_presenter_update_the_automatic_update_mode()
      {
         A.CallTo(() => _chartDisplayPresenter.UpdateAutoUpdateMode(false)).MustHaveHappened();
      }

      [Observation]
      public void should_not_update_the_chart_directly()
      {
         _curveChart.AutoUpdateEnabled.ShouldBeTrue();
      }
   }
}

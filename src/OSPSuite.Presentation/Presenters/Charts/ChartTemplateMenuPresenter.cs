using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Services.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IChartTemplateMenuPresenter : ICommandCollector
   {
      /// <summary>
      ///    Creates the chart template menu button with all sub menus filled in according to the subject
      ///    <paramref name="withChartTemplates" />
      /// </summary>
      /// <param name="withChartTemplates">This is the active simulation settings object containing the templates</param>
      /// <param name="retrieveActiveCurveChartFunc">retrieves the current CurveChart</param>
      /// <param name="applyTemplateAction">How to apply the template to the active curve chart</param>
      /// <returns>The menu button that should be added to the chart presenter</returns>
      IMenuBarSubMenu CreateChartTemplateButton(IWithChartTemplates withChartTemplates, Func<CurveChart> retrieveActiveCurveChartFunc, Action<CurveChartTemplate> applyTemplateAction);
   }

   public class ChartTemplateMenuPresenter : IChartTemplateMenuPresenter
   {
      private readonly IChartTemplatingTask _chartTemplatingTask;
      private Func<CurveChart> _curveChart;
      private Action<CurveChartTemplate> _loadMenuAction;
      private readonly List<ICommand> _commands;
      private IWithChartTemplates _withChartTemplates;

      public ChartTemplateMenuPresenter(IChartTemplatingTask chartTemplatingTask)
      {
         _chartTemplatingTask = chartTemplatingTask;
         _commands = new List<ICommand>();
      }

      private IEnumerable<CurveChartTemplate> allSimulationTemplates()
      {
         return _withChartTemplates.ChartTemplates;
      }

      protected CurveChartTemplate CreateNewTemplateFromCurrent(IEnumerable<CurveChartTemplate> existingTemplates)
      {
         return _chartTemplatingTask.CreateNewTemplateFromChart(_curveChart(), existingTemplates);
      }

      private void createNewTemplate()
      {
         var template = CreateNewTemplateFromCurrent(allSimulationTemplates());
         if (template == null) return;
         AddCommand(_chartTemplatingTask.AddChartTemplateCommand(template, _withChartTemplates));
      }

      private void updateTemplate(string templateName)
      {
         AddCommand(_chartTemplatingTask.UpdateChartTemplateCommand(_chartTemplatingTask.TemplateFrom(_curveChart()), _withChartTemplates, templateName));
      }

      private IMenuBarButton updateMenuFor(CurveChartTemplate chartTemplate)
      {
         return CreateMenuButton.WithCaption(chartTemplate.Name)
            .WithActionCommand(() => updateTemplate(chartTemplate.Name));
      }

      private IMenuBarButton loadMenuFor(CurveChartTemplate chartTemplate)
      {
         return CreateMenuButton.WithCaption(chartTemplate.Name)
            .WithActionCommand(() => _loadMenuAction(chartTemplate));
      }

      public IMenuBarSubMenu CreateChartTemplateButton(IWithChartTemplates withChartTemplates, Func<CurveChart> retrieveActiveCurveChartFunc,
         Action<CurveChartTemplate> applyTemplateAction)
      {
         _curveChart = retrieveActiveCurveChartFunc;
         _withChartTemplates = withChartTemplates;
         _loadMenuAction = applyTemplateAction;

         var chartTemplate = CreateSubMenu.WithCaption(MenuNames.ChartTemplate);

         chartTemplate.AddItem(createLoadButton(loadMenuFor));

         var fromChart = createFromChartMenu();

         var createNew = createCreateMenu();
         fromChart.AddItem(createNew);

         createUpdateMenu(fromChart);

         chartTemplate.AddItem(fromChart);

         var manageTemplatesMenu = createManageMenu();

         chartTemplate.AddItem(manageTemplatesMenu);

         return chartTemplate;
      }

      private IMenuBarSubMenu createFromChartMenu()
      {
         return CreateSubMenu.WithCaption(MenuNames.FromCurrentChart);
      }

      private IMenuBarButton createManageMenu()
      {
         var manageTemplatesMenu = CreateMenuButton.WithCaption(MenuNames.ManageTemplates)
            .WithIcon(ApplicationIcons.Settings)
            .WithActionCommand(manageTemplates)
            .AsGroupStarter();
         return manageTemplatesMenu;
      }

      private IMenuBarButton createCreateMenu()
      {
         var createNew = CreateMenuButton.WithCaption(MenuNames.CreateNewTemplate)
            .WithIcon(ApplicationIcons.Add)
            .WithActionCommand(createNewTemplate)
            .AsGroupStarter();
         return createNew;
      }

      private void createUpdateMenu(IMenuBarSubMenu chartTemplate)
      {
         var chartTemplates = allSimulationTemplates().ToList();
         if (chartTemplates.Any())
         {
            var overwrite = CreateSubMenu.WithCaption(MenuNames.UpdateExistingTemplate)
               .WithIcon(ApplicationIcons.SaveAsTemplate);

            chartTemplates.Each(t => overwrite.AddItem(updateMenuFor(t)));
            chartTemplate.AddItem(overwrite);
         }
      }

      private void manageTemplates()
      {
         AddCommand(_chartTemplatingTask.ManageTemplates(_withChartTemplates));
      }

      private IMenuBarItem createLoadButton(Func<CurveChartTemplate, IMenuBarButton> loadMenuFor)
      {
         var loadTemplate = CreateSubMenu.WithCaption(MenuNames.ApplyChartTemplate)
            .WithIcon(ApplicationIcons.Load);

         var noTemplateAvailable = CreateMenuButton.WithCaption(MenuNames.NoTemplateAvailable).WithActionCommand(() => { });
         var chartTemplates = _withChartTemplates.ChartTemplates.ToList();
         if (chartTemplates.Any())
            chartTemplates.Each(t => loadTemplate.AddItem(loadMenuFor(t)));
         else
            loadTemplate.AddItem(noTemplateAvailable);

         return loadTemplate;
      }

      public void AddCommand(ICommand command)
      {
         _commands.Add(command);
      }

      public IEnumerable<ICommand> All()
      {
         return _commands;
      }
   }
}
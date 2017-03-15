using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public class ChartTemplateManagerPresenter : AbstractDisposableContainerPresenter<IChartTemplateManagerView, IChartTemplateManagerPresenter>, IChartTemplateManagerPresenter
   {
      private readonly IChartTemplatingTask _chartTemplatingTask;
      private readonly IChartTemplateDetailsPresenter _chartTemplateDetailsPresenter;
      private readonly IDialogCreator _dialogCreator;
      private List<CurveChartTemplate> _chartTemplatesToEdit;
      public bool HasChanged { private set; get; }

      public ChartTemplateManagerPresenter(IChartTemplateManagerView view, IChartTemplatingTask chartTemplatingTask,
         IChartTemplateDetailsPresenter chartTemplateDetailsPresenter, IDialogCreator dialogCreator) : base(view)
      {
         _chartTemplatingTask = chartTemplatingTask;
         _chartTemplateDetailsPresenter = chartTemplateDetailsPresenter;
         _dialogCreator = dialogCreator;
         HasChanged = false;
         _view.SetChartTemplateView(_chartTemplateDetailsPresenter.View);
         AddSubPresenters(_chartTemplateDetailsPresenter);
      }

      public void SetDefaultTemplateValue(CurveChartTemplate template, bool isDefault)
      {
         _chartTemplatesToEdit.Each(x => x.IsDefault = false);
         rebindAction(() => template.IsDefault = isDefault);
      }

      public void EditTemplates(IEnumerable<CurveChartTemplate> chartTemplates)
      {
         _chartTemplatesToEdit = chartTemplates.OrderBy(x => x.Name).ToList();
         rebind();
         showFirstTemplateIfAvailable();
         HasChanged = false;
      }

      private void showFirstTemplateIfAvailable()
      {
         ShowTemplateDetails(_chartTemplatesToEdit.FirstOrDefault());
      }

      public override void ViewChanged()
      {
         base.ViewChanged();
         HasChanged = true;
      }

      public void ShowTemplateDetails(CurveChartTemplate chartTemplate)
      {
         _chartTemplateDetailsPresenter.Edit(chartTemplate);
      }

      public void CloneTemplate(CurveChartTemplate templateToClone)
      {
         var newTemplate = _chartTemplatingTask.CloneTemplate(templateToClone, _chartTemplatesToEdit);
         if (newTemplate == null)
            return;

         addTemplate(newTemplate);
      }

      public void DeleteTemplate(CurveChartTemplate chartTemplate)
      {
         rebindAction(() => _chartTemplatesToEdit.Remove(chartTemplate));
         showFirstTemplateIfAvailable();
      }

      public IEnumerable<CurveChartTemplate> EditedTemplates => _chartTemplatesToEdit;

      public void SaveTemplateToFile(CurveChartTemplate template)
      {
         var file = _dialogCreator.AskForFileToSave(Captions.SaveChartTemplateToFile, Constants.Filter.CHART_TEMPLATE_FILTER, Constants.DirectoryKey.MODEL_PART, template.Name);
         if (string.IsNullOrEmpty(file)) return;
         _chartTemplatingTask.SaveTemplateToFile(template, file);
      }

      public void LoadTemplateFromFile()
      {
         var file = _dialogCreator.AskForFileToOpen(Captions.LoadChartTemplateFromFile, Constants.Filter.CHART_TEMPLATE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (string.IsNullOrEmpty(file)) return;

         var template = _chartTemplatingTask.LoadTemplateFromFile(file, _chartTemplatesToEdit);
         if (template == null)
            return;

         addTemplate(template);
      }

      private void addTemplate(CurveChartTemplate template)
      {
         rebindAction(() => _chartTemplatesToEdit.Add(template));
         ShowTemplateDetails(template);
      }

      private void rebind()
      {
         _view.BindTo(_chartTemplatesToEdit);
      }

      private void rebindAction(Action action)
      {
         action();
         HasChanged = true;
         rebind();
      }
   }
}
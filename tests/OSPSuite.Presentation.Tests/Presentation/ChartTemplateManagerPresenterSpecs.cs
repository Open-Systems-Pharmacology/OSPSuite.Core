using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ChartTemplateManagerPresenter : ContextSpecification<IChartTemplateManagerPresenter>
   {
      protected IChartTemplateDetailsPresenter _chartTemplateDetailsPresenter;
      protected IChartTemplateManagerView _view;
      protected IChartTemplatingTask _chartTemplatingTask;
      protected List<CurveChartTemplate> _templatesToManage;
      protected CurveChartTemplate _template1;
      protected CurveChartTemplate _template2;
      protected IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _chartTemplatingTask = A.Fake<IChartTemplatingTask>();
         _chartTemplateDetailsPresenter = A.Fake<IChartTemplateDetailsPresenter>();
         _view = A.Fake<IChartTemplateManagerView>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _template1 = new CurveChartTemplate { Name = "Template1" };
         _template2 = new CurveChartTemplate { Name = "Template2" };
         _templatesToManage = new List<CurveChartTemplate> { _template1, _template2 };

         sut = new ChartTemplateManagerPresenter(_view, _chartTemplatingTask, _chartTemplateDetailsPresenter, _dialogCreator);
      }
   }

   public class When_setting_the_default_curve_template : concern_for_ChartTemplateManagerPresenter
   {
      protected override void Context()
      {
         base.Context();
         _template2.IsDefault = true;
         _template1.IsDefault = false;

         sut.EditTemplates(_templatesToManage);
      }

      protected override void Because()
      {
         sut.SetDefaultTemplateValue(_template1, true);
      }

      [Observation]
      public void should_make_the_other_templates_non_default()
      {
         _template2.IsDefault.ShouldBeFalse();
      }

      [Observation]
      public void should_make_the_indicated_template_the_default()
      {
         _template1.IsDefault.ShouldBeTrue();
      }
   }

   public class When_the_user_is_adding_a_new_template_to_the_manage_template : concern_for_ChartTemplateManagerPresenter
   {
      private CurveChartTemplate _newTemplate;
      private CurveChartTemplate _templateToClone;

      protected override void Context()
      {
         base.Context();
         _newTemplate = A.Fake<CurveChartTemplate>();
         _templateToClone = A.Fake<CurveChartTemplate>();

         A.CallTo(_chartTemplatingTask).WithReturnType<CurveChartTemplate>().Returns(_newTemplate);
         sut.EditTemplates(_templatesToManage);
      }

      protected override void Because()
      {
         sut.CloneTemplate(_templateToClone);
      }

      [Observation]
      public void should_create_a_new_template_with_the_predefined_settings()
      {
         A.CallTo(() => _chartTemplatingTask.CloneTemplate(_templateToClone, A<IEnumerable<CurveChartTemplate>>._)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_template_to_the_list_of_edited_templates()
      {
         sut.EditedTemplates.ShouldContain(_newTemplate);
      }

      [Observation]
      public void should_show_the_details_of_the_selected_template()
      {
         A.CallTo(() => _chartTemplateDetailsPresenter.Edit(_newTemplate)).MustHaveHappened();
      }
   }

   public class When_the_user_wants_to_save_a_given_template_to_a_file : concern_for_ChartTemplateManagerPresenter
   {
      private CurveChartTemplate _template;
      private string _filePath;

      protected override void Context()
      {
         base.Context();
         _template = new CurveChartTemplate().WithName("TOTO");
         _filePath = "FilePath";
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(_filePath);
      }

      protected override void Because()
      {
         sut.SaveTemplateToFile(_template);
      }

      [Observation]
      public void should_ask_the_user_to_select_a_file_name()
      {
         A.CallTo(() => _dialogCreator.AskForFileToSave(Captions.SaveChartTemplateToFile, Constants.Filter.CHART_TEMPLATE_FILTER,  Constants.DirectoryKey.MODEL_PART,_template.Name,null)).MustHaveHappened();
      }

      [Observation]
      public void should_save_the_template_to_the_selected_file()
      {
         A.CallTo(() => _chartTemplatingTask.SaveTemplateToFile(_template, _filePath)).MustHaveHappened();
      }
   }
}
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ModalChartTemplateManagerPresenter : ContextSpecification<IModalChartTemplateManagerPresenter>
   {
      protected IModalChartTemplateManagerView _view;
      protected ICloneManager _cloneManager;
      protected IChartTemplateManagerPresenter _chartTemplateManagerPresenter;

      protected override void Context()
      {
         _view = A.Fake<IModalChartTemplateManagerView>();
         _cloneManager = A.Fake<ICloneManager>();
         _chartTemplateManagerPresenter = A.Fake<IChartTemplateManagerPresenter>();
         sut = new ModalChartTemplateManagerPresenter(_view, _chartTemplateManagerPresenter, _cloneManager);
      }
   }
   public class When_editing_templates_and_the_user_clicks_ok : concern_for_ModalChartTemplateManagerPresenter
   {
      private List<CurveChartTemplate> _editedTemplates;
      private IEnumerable<CurveChartTemplate> _templatesToManage;
      private CurveChartTemplate _template1;
      private CurveChartTemplate _template2;
      private CurveChartTemplate _cloneTemplate1;
      private CurveChartTemplate _cloneTemplate2;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _chartTemplateManagerPresenter.EditTemplates(A<IEnumerable<CurveChartTemplate>>._))
            .Invokes(x => _editedTemplates = x.GetArgument<IEnumerable<CurveChartTemplate>>(0).ToList());

         _template1 = new CurveChartTemplate { Name = "Template1" };
         _cloneTemplate1 = new CurveChartTemplate { Name = "Template1" };
         _template2 = new CurveChartTemplate { Name = "Template2" };
         _cloneTemplate2 = new CurveChartTemplate { Name = "Template1" };
         A.CallTo(() => _cloneManager.Clone(_template1)).Returns(_cloneTemplate1);
         A.CallTo(() => _cloneManager.Clone(_template2)).Returns(_cloneTemplate2);

         _templatesToManage = new List<CurveChartTemplate> { _template1, _template2 };
      }

      protected override void Because()
      {
         sut.EditTemplates(_templatesToManage);
      }

      [Observation]
      public void should_clone_the_given_templates_and_display_them_in_the_view()
      {
         _editedTemplates.ShouldOnlyContain(_cloneTemplate1, _cloneTemplate2);
      }
   }

   public class When_editing_templates_for_a_given_chart_type : concern_for_ModalChartTemplateManagerPresenter
   {
      private List<CurveChartTemplate> _editedTemplates;
      private List<string> _forbiddenNames;
      private CurveChartTemplate _timeProfileTemplate;
      private CurveChartTemplate _predictedVsObservedTemplate;
      private CurveChartTemplate _cloneTimeProfileTemplate;
      private CurveChartTemplate _clonePredictedVsObservedTemplate;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _chartTemplateManagerPresenter.EditTemplates(A<IEnumerable<CurveChartTemplate>>._, A<IReadOnlyList<string>>._, CurveChartTypes.PredictedVsObserved))
            .Invokes(x =>
            {
               _editedTemplates = x.GetArgument<IEnumerable<CurveChartTemplate>>(0).ToList();
               _forbiddenNames = x.GetArgument<IReadOnlyList<string>>(1).ToList();
            });

         _timeProfileTemplate = new CurveChartTemplate {Name = "TimeProfileTemplate", ChartType = CurveChartTypes.TimeProfile};
         _predictedVsObservedTemplate = new CurveChartTemplate {Name = "PredictedVsObservedTemplate", ChartType = CurveChartTypes.PredictedVsObserved};
         _cloneTimeProfileTemplate = new CurveChartTemplate {Name = "TimeProfileTemplate", ChartType = CurveChartTypes.TimeProfile};
         _clonePredictedVsObservedTemplate = new CurveChartTemplate {Name = "PredictedVsObservedTemplate", ChartType = CurveChartTypes.PredictedVsObserved};
         A.CallTo(() => _cloneManager.Clone(_timeProfileTemplate)).Returns(_cloneTimeProfileTemplate);
         A.CallTo(() => _cloneManager.Clone(_predictedVsObservedTemplate)).Returns(_clonePredictedVsObservedTemplate);

         A.CallTo(() => _chartTemplateManagerPresenter.EditedTemplates).Returns(new[] {_clonePredictedVsObservedTemplate});
      }

      protected override void Because()
      {
         sut.EditTemplates(new[] {_timeProfileTemplate, _predictedVsObservedTemplate}, CurveChartTypes.PredictedVsObserved);
      }

      [Observation]
      public void should_only_edit_the_templates_matching_the_chart_type()
      {
         _editedTemplates.ShouldOnlyContain(_clonePredictedVsObservedTemplate);
      }

      [Observation]
      public void should_pass_the_names_of_the_templates_of_other_chart_types_as_forbidden_names()
      {
         _forbiddenNames.ShouldOnlyContain(_cloneTimeProfileTemplate.Name);
      }

      [Observation]
      public void should_return_the_templates_of_other_chart_types_unchanged_together_with_the_edited_templates()
      {
         sut.EditedTemplates.ShouldOnlyContain(_clonePredictedVsObservedTemplate, _cloneTimeProfileTemplate);
      }
   }
}
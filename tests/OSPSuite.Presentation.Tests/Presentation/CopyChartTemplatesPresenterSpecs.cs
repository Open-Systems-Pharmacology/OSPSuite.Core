using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_CopyChartTemplatesPresenter : ContextSpecification<ICopyChartTemplatesPresenter>
   {
      protected ICopyChartTemplatesView _view;
      protected IProjectRetriever _projectRetriever;
      protected ILazyLoadTask _lazyLoadTask;
      protected IProject _project;
      protected IWithChartTemplates _templateOwner;
      protected IWithChartTemplates _otherSimulation;
      protected CurveChartTemplate _projectTimeProfileTemplate;
      protected CurveChartTemplate _projectPredictedVsObservedTemplate;
      protected CurveChartTemplate _simulationPredictedVsObservedTemplate;
      protected IReadOnlyList<ChartTemplateSourceDTO> _sources;
      protected IReadOnlyList<ChartTemplateSelectionDTO> _templates;

      protected override void Context()
      {
         _view = A.Fake<ICopyChartTemplatesView>();
         _projectRetriever = A.Fake<IProjectRetriever>();
         _lazyLoadTask = A.Fake<ILazyLoadTask>();
         _project = A.Fake<IProject>();
         A.CallTo(() => _projectRetriever.CurrentProject).Returns(_project);

         _projectTimeProfileTemplate = new CurveChartTemplate {Name = "ProjectTimeProfile", ChartType = CurveChartTypes.TimeProfile};
         _projectPredictedVsObservedTemplate = new CurveChartTemplate {Name = "ProjectPredictedVsObserved", ChartType = CurveChartTypes.PredictedVsObserved};
         _simulationPredictedVsObservedTemplate = new CurveChartTemplate {Name = "SimulationPredictedVsObserved", ChartType = CurveChartTypes.PredictedVsObserved};
         A.CallTo(() => _project.ChartTemplates).Returns(new[] {_projectTimeProfileTemplate, _projectPredictedVsObservedTemplate});

         _templateOwner = simulationNamed("Owner");
         _otherSimulation = simulationNamed("Other");
         A.CallTo(() => _otherSimulation.ChartTemplates).Returns(new[] {_simulationPredictedVsObservedTemplate});
         A.CallTo(() => _project.All<IWithChartTemplates>()).Returns(new[] {_templateOwner, _otherSimulation});

         A.CallTo(() => _view.BindToSources(A<IReadOnlyList<ChartTemplateSourceDTO>>._))
            .Invokes(x => _sources = x.GetArgument<IReadOnlyList<ChartTemplateSourceDTO>>(0));
         A.CallTo(() => _view.BindToTemplates(A<IReadOnlyList<ChartTemplateSelectionDTO>>._))
            .Invokes(x => _templates = x.GetArgument<IReadOnlyList<ChartTemplateSelectionDTO>>(0));

         sut = new CopyChartTemplatesPresenter(_view, _projectRetriever, _lazyLoadTask);
      }

      private static IWithChartTemplates simulationNamed(string name)
      {
         var simulation = A.Fake<IWithChartTemplates>(x => x.Implements<IWithName>().Implements<ILazyLoadable>());
         A.CallTo(() => ((IWithName) simulation).Name).Returns(name);
         return simulation;
      }

      protected ChartTemplateSourceDTO sourceFor(IWithChartTemplates source) => _sources.Single(x => Equals(x.Source, source));
   }

   public class When_selecting_the_templates_to_copy_from_the_project : concern_for_CopyChartTemplatesPresenter
   {
      protected override void Because()
      {
         sut.SelectTemplatesToCopy(_templateOwner, CurveChartTypes.PredictedVsObserved);
      }

      [Observation]
      public void should_offer_the_project_and_the_other_template_owners_of_the_project_but_not_the_owner_of_the_edited_templates()
      {
         _sources.Select(x => x.Source).ShouldOnlyContainInOrder(_project, _otherSimulation);
      }

      [Observation]
      public void should_display_the_project_with_a_generic_caption_and_the_other_owners_with_their_name()
      {
         _sources.Select(x => x.DisplayName).ShouldOnlyContainInOrder(Captions.Project, "Other");
      }

      [Observation]
      public void should_not_allow_the_user_to_confirm_before_a_template_is_selected()
      {
         _view.OkEnabled.ShouldBeFalse();
      }
   }

   public class When_the_user_selects_a_source_of_templates : concern_for_CopyChartTemplatesPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.SelectTemplatesToCopy(_templateOwner, CurveChartTypes.PredictedVsObserved);
      }

      protected override void Because()
      {
         sut.SelectSource(sourceFor(_otherSimulation));
      }

      [Observation]
      public void should_load_the_source_before_accessing_its_templates()
      {
         A.CallTo(() => _lazyLoadTask.Load(_otherSimulation as ILazyLoadable)).MustHaveHappened();
      }

      [Observation]
      public void should_show_the_templates_of_the_source()
      {
         _templates.Select(x => x.Template).ShouldOnlyContain(_simulationPredictedVsObservedTemplate);
      }
   }

   public class When_the_user_selects_the_project_as_source_of_templates : concern_for_CopyChartTemplatesPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.SelectTemplatesToCopy(_templateOwner, CurveChartTypes.PredictedVsObserved);
      }

      protected override void Because()
      {
         sut.SelectSource(sourceFor(_project));
      }

      [Observation]
      public void should_only_show_the_templates_created_for_the_chart_type_of_the_edited_templates()
      {
         _templates.Select(x => x.Template).ShouldOnlyContain(_projectPredictedVsObservedTemplate);
      }
   }

   public class When_selecting_templates_to_copy_for_all_chart_types : concern_for_CopyChartTemplatesPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.SelectTemplatesToCopy(templateOwner: null, chartType: null);
      }

      protected override void Because()
      {
         sut.SelectSource(sourceFor(_project));
      }

      [Observation]
      public void should_offer_all_template_owners_of_the_project()
      {
         _sources.Select(x => x.Source).ShouldOnlyContain(_project, _templateOwner, _otherSimulation);
      }

      [Observation]
      public void should_show_the_templates_of_all_chart_types()
      {
         _templates.Select(x => x.Template).ShouldOnlyContain(_projectTimeProfileTemplate, _projectPredictedVsObservedTemplate);
      }
   }

   public class When_the_user_confirms_the_templates_selected_in_different_sources : concern_for_CopyChartTemplatesPresenter
   {
      private IReadOnlyList<CurveChartTemplate> _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
         A.CallTo(() => _view.Display()).Invokes(() =>
         {
            sut.SelectSource(sourceFor(_project));
            _templates.Each(x => x.Selected = true);
            sut.SelectSource(sourceFor(_otherSimulation));
            _templates.Each(x => x.Selected = true);
            // Selecting a source again shows the same selection
            sut.SelectSource(sourceFor(_project));
         });
      }

      protected override void Because()
      {
         _result = sut.SelectTemplatesToCopy(_templateOwner, CurveChartTypes.PredictedVsObserved);
      }

      [Observation]
      public void should_return_the_templates_selected_in_all_sources()
      {
         _result.ShouldOnlyContain(_projectPredictedVsObservedTemplate, _simulationPredictedVsObservedTemplate);
      }

      [Observation]
      public void should_keep_the_selection_when_selecting_a_source_again()
      {
         _templates.All(x => x.Selected).ShouldBeTrue();
      }

      [Observation]
      public void should_only_load_each_source_once()
      {
         A.CallTo(() => _lazyLoadTask.Load(_project as ILazyLoadable)).MustHaveHappenedOnceExactly();
      }
   }

   public class When_the_user_cancels_the_selection_of_templates_to_copy : concern_for_CopyChartTemplatesPresenter
   {
      private IReadOnlyList<CurveChartTemplate> _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
         A.CallTo(() => _view.Display()).Invokes(() =>
         {
            sut.SelectSource(sourceFor(_project));
            _templates.Each(x => x.Selected = true);
         });
      }

      protected override void Because()
      {
         _result = sut.SelectTemplatesToCopy(_templateOwner, CurveChartTypes.PredictedVsObserved);
      }

      [Observation]
      public void should_not_return_any_template()
      {
         _result.ShouldBeEmpty();
      }
   }

   public class When_selecting_templates_to_copy_while_no_project_is_open : concern_for_CopyChartTemplatesPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _projectRetriever.CurrentProject).Returns(null);
      }

      protected override void Because()
      {
         sut.SelectTemplatesToCopy(_templateOwner, CurveChartTypes.PredictedVsObserved);
      }

      [Observation]
      public void should_not_offer_any_source()
      {
         _sources.ShouldBeEmpty();
      }
   }
}

using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_CurveTemplatePresenter : ContextSpecification<CurveTemplatePresenter>
   {
      protected IChartTemplatingTask _chartTemplatingTask;
      private ICurveTemplateView _curveTemplateView;
      private ICommandCollector _commandCollector;
      protected CurveTemplate _curveTemplate;

      protected override void Context()
      {
         _chartTemplatingTask = A.Fake<IChartTemplatingTask>();
         _curveTemplateView = A.Fake<ICurveTemplateView>();
         _commandCollector = A.Fake<ICommandCollector>();
         sut = new CurveTemplatePresenter(_curveTemplateView );
         sut.InitializeWith(_commandCollector);

         _curveTemplate = new CurveTemplate();
      }
   }

   public class When_adding_templates_to_list_of_curve_templates : concern_for_CurveTemplatePresenter
   {
      private IList<CurveTemplate> _curveTemplates;

      protected override void Context()
      {
         base.Context();
         _curveTemplates = new List<CurveTemplate> {_curveTemplate};
         sut.Edit(_curveTemplates);
      }

      protected override void Because()
      {
         sut.AddCurve();
      }

      [Observation]
      public void number_of_curves_in_the_list_should_have_incremented()
      {
         _curveTemplates.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_removing_templates_from_list_of_curve_templates : concern_for_CurveTemplatePresenter
   {
      private IList<CurveTemplate> _curveTemplates;
      private CurveTemplateDTO _curveTemplateDTO;

      protected override void Context()
      {
         base.Context();

         _curveTemplates = new List<CurveTemplate> { _curveTemplate };
         _curveTemplateDTO = new CurveTemplateDTO(_curveTemplate);
         sut.Edit(_curveTemplates);
      }

      protected override void Because()
      {
         sut.DeleteCurve(_curveTemplateDTO);
      }

      [Observation]
      public void number_of_curves_in_the_list_should_have_decremented()
      {
         _curveTemplates.Count.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_not_be_able_to_delete_when_no_curves_are_being_edited()
      {
         sut.CanDeleteCurves.ShouldBeEqualTo(false);
      }
   }
}

using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ICurveTemplatePresenter : IPresenter<ICurveTemplateView>
   {
      void Edit(IList<CurveTemplate> curveTemplates);
      void DeleteCurve(CurveTemplateDTO curveTemplateDTO);
      void AddCurve();
      bool CanDeleteCurves { get; }
   }

   public class CurveTemplatePresenter : AbstractSubPresenter<ICurveTemplateView, ICurveTemplatePresenter>, ICurveTemplatePresenter
   {
      private IList<CurveTemplate> _curveTemplates;

      public CurveTemplatePresenter(ICurveTemplateView view)
         : base(view)
      {
      }

      public void Edit(IList<CurveTemplate> curveTemplates)
      {
         _curveTemplates = curveTemplates;
         rebind();
      }

      public void DeleteCurve(CurveTemplateDTO curveTemplate)
      {
         _curveTemplates.Remove(curveTemplate.CurveTemplate);
         rebind();
      }

      public void AddCurve()
      {
         _curveTemplates.Add(new CurveTemplate());
         rebind();
      }

      public bool CanDeleteCurves => _curveTemplates.Count > 1;

      private void rebind()
      {
         _view.BindTo(_curveTemplates.Select(x => new CurveTemplateDTO(x)));
      }
   }
}
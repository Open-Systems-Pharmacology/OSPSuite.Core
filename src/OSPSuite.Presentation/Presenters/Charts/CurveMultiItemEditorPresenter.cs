using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ICurveMultiItemEditorPresenter : IDisposablePresenter, IPresenter<ICurveMultiItemEditorView>
   {
      SelectedCurveValues GetSelectedValues();
      IEnumerable<bool?> AllBooleanOptions { get; }
      IEnumerable<LineStyles?> AllLineStyles { get; }
      IEnumerable<Symbols?> AllSymbols { get; }
   }

   public class CurveMultiItemEditorPresenter : AbstractDisposablePresenter<ICurveMultiItemEditorView, ICurveMultiItemEditorPresenter>,
      ICurveMultiItemEditorPresenter
   {
      private readonly SelectedCurveValues _selectedCurveValues;

      public CurveMultiItemEditorPresenter(ICurveMultiItemEditorView view) : base(view)
      {
         _selectedCurveValues = new SelectedCurveValues();
      }

      public IEnumerable<bool?> AllBooleanOptions { get; } = Constants.MultiCurveOptions.AllBooleanOptions;

      public IEnumerable<LineStyles?> AllLineStyles { get; } = new List<LineStyles?>() {null}.Union(EnumHelper.AllValuesFor<LineStyles>().Cast<LineStyles?>());

      public IEnumerable<Symbols?> AllSymbols { get; } = new List<Symbols?>() {null}.Union(EnumHelper.AllValuesFor<Symbols>().Cast<Symbols?>());

      public SelectedCurveValues GetSelectedValues()
      {
         _view.BindTo(_selectedCurveValues);
         _view.Display();

         return _view.Canceled ? new SelectedCurveValues() : _selectedCurveValues;
      }
   }
}
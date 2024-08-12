using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Parameters;

namespace OSPSuite.Presentation.Views.Parameters
{
   public interface ITableFormulaView : IView<ITableFormulaPresenter>
   {
      void Clear();
      void BindTo(TableFormulaDTO tableFormulaDTO);
      void EditPoint(ValuePointDTO pointToEdit);
      bool ImportVisible { set; }
      string YCaption { set; }
      string XCaption { set; }
      bool Editable { get; set; }
      string Description { get; set; }
      string ImportToolTip { get; set; }
      void ShowUseDerivedValues(bool show);
      void ShowRestartSolver(bool show);
   }
}
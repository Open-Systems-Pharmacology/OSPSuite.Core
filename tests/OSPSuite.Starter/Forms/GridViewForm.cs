using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Starter.Forms
{
   public partial class GridViewForm : Form
   {
      private readonly GridViewBinder<Parameter> _gridViewBinder;

      public GridViewForm()
      {
         InitializeComponent();
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
         gridView.OptionsSelection.EnableAppearanceFocusedRow = true;
         gridView.OptionsSelection.EnableAppearanceFocusedCell = true;
         gridView.OptionsSelection.MultiSelect = true;
         _gridViewBinder = new GridViewBinder<Parameter>(gridView);

         initializeBinding();
         _gridViewBinder.BindToSource(generateDummyContent().ToList());
      }

      private void initializeBinding()
      {
         var gridViewBoundColumn = _gridViewBinder.Bind(x => x.Name)
            .AsReadOnly();

         gridViewBoundColumn.XtraColumn.OptionsColumn.AllowFocus = true;

         var boundColumn = _gridViewBinder.Bind(x => x.Value)
            .AsReadOnly();
         boundColumn.XtraColumn.OptionsColumn.AllowFocus = true;

      }

      private IEnumerable<Parameter> generateDummyContent()
      {

         for (var i = 0; i < 10; i++)
         {
            var parameter = new Parameter().WithName($"Prameter_{i}");
            parameter.Formula = new ConstantFormula(i);
            yield return parameter;
         }
      }
   }
}

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Binders;

namespace OSPSuite.Starter.Forms
{
   public partial class GridViewForm : Form
   {
      private readonly ValueOriginBinder<ParameterDTO> _valueOriginBinder;
      private readonly GridViewBinder<ParameterDTO> _gridViewBinder;

      public GridViewForm(ValueOriginBinder<ParameterDTO> valueOriginBinder)
      {
         _valueOriginBinder = valueOriginBinder;
         InitializeComponent();
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
         gridView.OptionsSelection.EnableAppearanceFocusedRow = true;
         gridView.OptionsSelection.EnableAppearanceFocusedCell = true;
         gridView.OptionsSelection.MultiSelect = true;
         _gridViewBinder = new GridViewBinder<ParameterDTO>(gridView);

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

         _valueOriginBinder.InitializeBinding(_gridViewBinder, onValueOriginUpdated);
      }

      private void onValueOriginUpdated(ParameterDTO parameterDTO, ValueOrigin newValueOrigin)
      {
      }

      private IEnumerable<ParameterDTO> generateDummyContent()
      {
         for (var i = 0; i < 10; i++)
         {
            var parameter = new ParameterDTO().WithName($"Prameter_{i}");
            if (i % 2 == 0)
            {
               parameter.ValueOrigin.Source = ValueOriginSources.Database;
               parameter.ValueOrigin.Method = ValueOriginDeterminationMethods.ManualFit;
               parameter.ValueOrigin.Description = "This is the best description ever";
            }

            parameter.Formula = new ConstantFormula(i);
            yield return parameter;
         }
      }
   }

   public class ParameterDTO : Parameter
   {
   }
}
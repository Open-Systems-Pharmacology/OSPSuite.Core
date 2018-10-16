using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Binders;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Starter.Forms
{
   public partial class GridViewForm : Form
   {
      private readonly ValueOriginBinder<ParameterDTO> _valueOriginBinder;
      private readonly GridViewBinder<ParameterDTO> _gridViewBinder;
      private List<ParameterDTO> _allParameters;
      private ParameterDTO _firstParmaeter;

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

         _allParameters = generateDummyContent().ToList();
         _gridViewBinder.BindToSource(_allParameters.ToBindingList());

         _firstParmaeter = _allParameters[0];
         _firstParmaeter.PropertyChanged += propertyChanged;
      }

      private void propertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName != "ValueOrigin")
            return;

         Debug.Print(sender.DowncastTo<ParameterDTO>().Name);
      }

      private void initializeBinding()
      {
         var gridViewBoundColumn = _gridViewBinder.Bind(x => x.Name)
            .AsReadOnly();

         gridViewBoundColumn.XtraColumn.OptionsColumn.AllowFocus = true;

         var boundColumn = _gridViewBinder.Bind(x => x.Value);


         boundColumn.XtraColumn.OptionsColumn.AllowFocus = true;

         _valueOriginBinder.InitializeBinding(_gridViewBinder, onValueOriginUpdated, valueOriginEditableFunc: canEditValueOrigin);
      }

      private bool canEditValueOrigin(ParameterDTO parameter)
      {
//         if (!parameter.NameIsOneOf("Prameter_2", "Prameter_4"))
//            return false;

         return true;
      }

      private void onValueOriginUpdated(ParameterDTO parameterDTO, ValueOrigin newValueOrigin)
      {
         parameterDTO.UpdateValueOriginFrom(newValueOrigin);

         //Also update first parmaeter with same value origin to test update
         _firstParmaeter.UpdateValueOriginFrom(newValueOrigin);
      }

      private IEnumerable<ParameterDTO> generateDummyContent()
      {
         for (var i = 0; i < 10; i++)
         {
            var parameter = new ParameterDTO().WithName($"Prameter_{i}");
            if (i % 2 == 0)
            {
               parameter.ValueOrigin.Source = ValueOriginSources.Database;

               if(i!=4)
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
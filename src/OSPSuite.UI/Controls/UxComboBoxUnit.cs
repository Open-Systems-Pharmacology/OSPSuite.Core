using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.UI.Controls
{
   public class UxComboBoxUnit<TWithUnit> : ComboBoxEdit where TWithUnit : IWithDisplayUnitDTO
   {
      private readonly ScreenBinder<TWithUnit> _unitBinder;
      public event Action<TWithUnit, Unit> ParameterUnitSet = delegate { };
      
      //This is an empiric value that seems to work fine when dealing with unit that do not have
      //enough letters such as ml or g
      private const int MINIMUM_NUMBER_OF_LETTERS = 6;

      public UxComboBoxUnit(Control parent)
      {
         _unitBinder = new ScreenBinder<TWithUnit>();
         Visible = false;
         BorderStyle = BorderStyles.NoBorder;
         Parent = parent;
         TabStop = true;
         initializeBinding();
      }

      private void initializeBinding()
      {
         _unitBinder.Bind(x => x.DisplayUnit)
            .To(this)
            .WithValues(dto => dto.AllUnits)
            .OnValueUpdating += (o, e) => ParameterUnitSet(o, e.NewValue);
      }

      public void UpdateUnitsFor(BaseEdit activeEditor, TWithUnit dto)
      {
         if (!shouldDisplayUnits(dto)) return;

         _unitBinder.BindToSource(dto);
         var bounds = activeEditor.Bounds;
         var newEditorBound = bounds;

         //Maximum width is 2/5 of the parent width. Again an empiric values that seems to give appropriate results
         int unitWidth = Math.Min(getUnitsMinWidth(dto.AllUnits.Select(x => x.Name), dto.DisplayUnit), bounds.Width * 2 / 5);
         newEditorBound.Width = bounds.Width - unitWidth;
         activeEditor.Bounds = newEditorBound;

         bounds.X += newEditorBound.Width;
         bounds.Width = unitWidth;
         Visible = true;
         Bounds = bounds;
      }

      private static bool shouldDisplayUnits(TWithUnit dto)
      {
         var units = dto.Dimension.Units.ToList();
         if (units.Count == 0)
            return false;

         if (units.Count == 1)
            return !string.IsNullOrEmpty(units[0].Name);

         return true;
      }

      private int getUnitsMinWidth(IEnumerable<string> allUnits, Unit selectedUnit)
      {
         int maxLetters = allUnits.Max(x => x.Length);
         if (selectedUnit != null)
            maxLetters = selectedUnit.Name.Length;

         maxLetters = Math.Max(maxLetters, MINIMUM_NUMBER_OF_LETTERS);

         var size = Properties.Appearance.GetFont().SizeInPoints + 0.5;

         return (int)Math.Ceiling(maxLetters * size);
      }
   }

}
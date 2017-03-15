using System.Windows.Forms;
using OSPSuite.Presentation.DTO;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views
{
   public class ComboBoxUnitParameter : UxComboBoxUnit<IParameterDTO>
   {
      public ComboBoxUnitParameter(Control parent) : base(parent)
      {
      }
   }
}
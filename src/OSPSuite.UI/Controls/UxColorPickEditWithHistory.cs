using System.ComponentModel;
using DevExpress.XtraEditors;

namespace OSPSuite.UI.Controls
{
   [ToolboxItem(true)]
   public class UxColorPickEditWithHistory : ColorPickEdit
   {
      public override string EditorTypeName
      {
         get
         {
            return
               UxRepositoryItemColorPickEditWithHistory.CustomEditName;
         }
      }

      [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
      public new UxRepositoryItemColorPickEditWithHistory Properties
      {
         get { return base.Properties as UxRepositoryItemColorPickEditWithHistory; }
      }

      static UxColorPickEditWithHistory()
      {
         UxRepositoryItemColorPickEditWithHistory.RegisterCustomEdit();
      }

      public UxColorPickEditWithHistory()
      {
         Properties.ShowSystemColors = false;
         Properties.ShowWebColors = false;


      }
   }
}
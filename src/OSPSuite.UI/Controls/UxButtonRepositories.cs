using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Controls
{
   public class UxAddButtonRepository : UxRepositoryItemButtonEdit
   {
      public UxAddButtonRepository()
         : base(ButtonPredefines.Plus)
      {
         Buttons[0].ToolTip = Captions.AddEntry;
      }
   }

   public class UxRemoveButtonRepository : UxRepositoryItemButtonEdit
   {
      public UxRemoveButtonRepository()
         : base(ButtonPredefines.Delete)
      {
         Buttons[0].ToolTip = Captions.DeleteEntry;
      }
   }

   public class UxAddAndRemoveButtonRepository : UxAddButtonRepository
   {
      public UxAddAndRemoveButtonRepository()
      {
         Buttons.Add(new EditorButton(ButtonPredefines.Delete) {ToolTip = Captions.DeleteEntry});
      }
   }

   public class UxAddAndDisabledRemoveButtonRepository : UxAddAndRemoveButtonRepository
   {
      public UxAddAndDisabledRemoveButtonRepository()
      {
         Buttons[1].Enabled = false;
      }
   }

   public class UxDisableAddAndDisableRemoveButtonRepository : UxAddAndDisabledRemoveButtonRepository
   {
      public UxDisableAddAndDisableRemoveButtonRepository()
      {
         Buttons[0].Enabled = false;
      }
   }
}
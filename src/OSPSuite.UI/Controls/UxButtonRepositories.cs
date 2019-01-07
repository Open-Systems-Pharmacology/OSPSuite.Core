using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Controls
{
   public enum ButtonType {
      Add,
      Remove,
      Update,
      Other
   }

   public class UxAddButtonRepository : UxRepositoryItemButtonEdit
   {
      public UxAddButtonRepository() : base(ButtonPredefines.Plus, Captions.AddEntry, ButtonType.Add)
      {
      }
   }

   public class UxRemoveButtonRepository : UxRepositoryItemButtonEdit
   {
      public UxRemoveButtonRepository() : base(ButtonPredefines.Delete, Captions.DeleteEntry, ButtonType.Remove)
      {
      }
   }

   public class UxAddAndRemoveButtonRepository : UxAddButtonRepository
   {
      public UxAddAndRemoveButtonRepository()
      {
         AddButton(ButtonPredefines.Delete, Captions.DeleteEntry, ButtonType.Remove);
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
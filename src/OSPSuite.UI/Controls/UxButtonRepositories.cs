using OSPSuite.Assets;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Controls
{
   public class UxAddButtonRepository : UxRepositoryItemButtonImage
   {
      public UxAddButtonRepository() : base(ApplicationIcons.Add, Captions.AddEntry)
      {
      }
   }

   public class UxRemoveButtonRepository : UxRepositoryItemButtonImage
   {
      public UxRemoveButtonRepository() : base(ApplicationIcons.Remove, Captions.DeleteEntry)
      {
      }
   }

   public class UxAddAndRemoveButtonRepository : UxAddButtonRepository
   {
      public UxAddAndRemoveButtonRepository()
      {
         AddButton(ApplicationIcons.Remove, Captions.DeleteEntry);
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
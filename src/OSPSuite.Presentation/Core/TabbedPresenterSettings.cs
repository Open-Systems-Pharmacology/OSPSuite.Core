namespace OSPSuite.Presentation.Core
{
   public class TabbedPresenterSettings : DefaultPresentationSettings
   {
      private const string SELECTED_TAB_INDEX = "SELECTED_TAB_INDEX";

      public virtual int SelectedTabIndex
      {
         get { return GetSetting<int>(SELECTED_TAB_INDEX); }
         set { SetSetting(SELECTED_TAB_INDEX, value); }
      }
   }
}
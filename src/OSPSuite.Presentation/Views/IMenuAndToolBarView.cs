using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.Views
{
   public interface IMenuAndToolBarView
   {
      void CreateDynamicPageCategory(string pageCategoryName, Color categoryColor);
      void CreateDynamicPageCategory(string pageCategoryName);
      void AddDynamicPageGroupToPageCategory(IButtonGroup buttonGroup, string pageName, string pageCategoryName);
      void SetPageCategoryVisibility(string pageCategoryName, bool visible);
      void AddPageGroupToPage(IButtonGroup buttonGroup, string pageName);
      void AddQuickAcccessButton(IMenuBarItem menuBarItem);
      void AddPageHeaderItemLinks(IMenuBarItem menuBarItem);
      void InitializeSkinGallery(ISkinManager skinManager, string pageGroupName, string pageName);
      void AddApplicationMenu(IButtonGroup buttonGroup);
      void AddMRUMenus(IEnumerable<IMenuBarItem> allMenus);
   }
}
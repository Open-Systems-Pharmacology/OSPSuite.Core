using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Mappers;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views
{
   public class MenuAndToolBarView : IMenuAndToolBarView
   {
      private readonly RibbonBarManager _barManager;
      private readonly IButtonGroupToRibbonPageGroupMapper _ribbonPageGroupMapper;
      private readonly IMenuBarItemToBarItemMapper _barItemMapper;
      private readonly IRibbonBarItemToBarItemMapper _ribbonItemMapper;
      private readonly ISkinManagerToSkinGalleryMapper _skinGalleryMapper;
      private readonly ApplicationMenu _applicationMenu;
      private readonly MRUArrayList _mruArrayList;
      private readonly ICache<string, IList<IButtonGroup>> _buttonGroupCache;

      public MenuAndToolBarView(RibbonBarManager barManager, IButtonGroupToRibbonPageGroupMapper ribbonPageGroupMapper, IMenuBarItemToBarItemMapper barItemMapper,
         IRibbonBarItemToBarItemMapper ribbonItemMapper, ISkinManagerToSkinGalleryMapper skinGalleryMapper,
         ApplicationMenu applicationMenu, PanelControl panelRecentItems, IToolTipCreator toolTipCreator)
      {
         _barManager = barManager;
         _ribbonPageGroupMapper = ribbonPageGroupMapper;
         _barItemMapper = barItemMapper;
         _ribbonItemMapper = ribbonItemMapper;
         _skinGalleryMapper = skinGalleryMapper;
         _applicationMenu = applicationMenu;
         _mruArrayList = new MRUArrayList(_applicationMenu, panelRecentItems, toolTipCreator);
         _buttonGroupCache = new Cache<string, IList<IButtonGroup>>(category => new List<IButtonGroup>());
      }

      public void AddPageGroupToPage(IButtonGroup buttonGroup, string pageName)
      {
         var page = pageFrom(_barManager.Ribbon.Pages, pageName);
         addButtonGroupToPage(buttonGroup, page);
      }

      public void CreateDynamicPageCategory(string pageCategoryName, Color categoryColor)
      {
         var page = new RibbonPageCategory(pageCategoryName, categoryColor, false);
         _barManager.Ribbon.PageCategories.Add(page);
      }

      public void CreateDynamicPageCategory(string pageCategoryName)
      {
         CreateDynamicPageCategory(pageCategoryName, defaultPageColor);
      }

      private void addButtonGroupToPage(IButtonGroup buttonGroup, RibbonPage page)
      {
         var pageGroup = _ribbonPageGroupMapper.MapFrom(buttonGroup);
         buttonGroup.Buttons.Each(btn => pageGroup.ItemLinks.Add(_ribbonItemMapper.MapFrom(btn), btn.BeginGroup));
         page.Groups.Add(pageGroup);
      }

      public void AddDynamicPageGroupToPageCategory(IButtonGroup buttonGroup, string pageName, string pageCategoryName)
      {
         var pageCategory = pageCategoryBy(pageCategoryName);
         var page = pageFrom(pageCategory.Pages, pageName);
         addButtonGroupToPage(buttonGroup, page);

         addButtonGroupToButtonGroupCache(buttonGroup, pageCategory);

         buttonGroup.WithLock(true);
      }

      private void addButtonGroupToButtonGroupCache(IButtonGroup buttonGroup, RibbonPageCategory pageCategory)
      {
         var buttonGroups = _buttonGroupCache[identifierForCategory(pageCategory)];
         buttonGroups.Add(buttonGroup);
         _buttonGroupCache[identifierForCategory(pageCategory)] = buttonGroups;
      }

      public void SetPageCategoryVisibility(string pageCategoryName, bool visible)
      {
         var pageCategory = pageCategoryBy(pageCategoryName);
         pageCategory.Visible = visible;
         setButtonLocks(!visible, pageCategory);

         if (!visible) return;

         _barManager.Ribbon.SelectedPage = pageCategory.Pages[0];
      }

      private void setButtonLocks(bool locked, RibbonPageCategory category)
      {
         var groups = _buttonGroupCache[identifierForCategory(category)];
         groups.Each(group => group.WithLock(locked));
      }

      private static string identifierForCategory(RibbonPageCategory category)
      {
         return category.Text;
      }

      private Color defaultPageColor
      {
         get
         {
            var page = _barManager.Ribbon.DefaultPageCategory;
            return page?.Color ?? Color.LightGreen;
         }
      }

      private RibbonPageCategory pageCategoryBy(string pageCategoryName)
      {
         var pageCategory = _barManager.Ribbon.PageCategories[pageCategoryName];
         if (pageCategory == null)
            throw new Exception($"Could not find page category named {pageCategoryName}");
         return pageCategory;
      }

      private RibbonPage pageFrom(RibbonPageCollection ribbonPageCollection, string pageName)
      {
         var page = ribbonPageCollection[pageName];
         if (page == null)
         {
            page = new RibbonPage(pageName) { KeyTip = pageName.Substring(0, 1).ToUpper() };
            ribbonPageCollection.Add(page);
         }
         return page;
      }

      public void AddQuickAcccessButton(IMenuBarItem menuBarItem)
      {
         _barManager.Ribbon.Toolbar.ItemLinks.Add(_barItemMapper.MapFrom(_barManager, menuBarItem));
      }

      public void AddPageHeaderItemLinks(IMenuBarItem menuBarItem)
      {
         _barManager.Ribbon.PageHeaderItemLinks.Add(_barItemMapper.MapFrom(_barManager, menuBarItem));
      }

      public void InitializeSkinGallery(ISkinManager skinManager, string pageGroupName, string pageName)
      {
         var pageGroup = _ribbonPageGroupMapper.MapFrom(new ButtonGroup { Caption = pageGroupName });
         pageGroup.ItemLinks.Add(_skinGalleryMapper.MapFrom(_barManager, skinManager));
         pageFrom(_barManager.Ribbon.Pages, pageName).Groups.Add(pageGroup);
      }

      public void AddApplicationMenu(IButtonGroup buttonGroup)
      {
         _barManager.Ribbon.ApplicationButtonText = buttonGroup.Caption;
         _barManager.Ribbon.ApplicationButtonKeyTip = buttonGroup.Caption.Substring(0, 1).ToUpper();

         foreach (var button in buttonGroup.Buttons)
         {
            var barItem = _ribbonItemMapper.MapFrom(button);
            _applicationMenu.ItemLinks.Add(barItem, button.BeginGroup);
         }
      }

      public void AddMRUMenus(IEnumerable<IMenuBarItem> allMenus)
      {
         _mruArrayList.ClearEntries();
         allMenus.Reverse().Each(_mruArrayList.InsertElement);
      }
   }
}
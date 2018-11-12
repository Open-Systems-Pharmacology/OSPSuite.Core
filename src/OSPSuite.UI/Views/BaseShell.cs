using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Alerter;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using DevExpress.XtraTabbedMdi;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views
{
   public partial class BaseShell : RibbonForm, IMainView, IViewWithPopup
   {
      public event EventHandler CaptionChanged = delegate { };
      public ApplicationIcon ApplicationIcon { get; set; }
      public event Action Loading = delegate { };
      private IMainViewPresenter _presenter;
      private readonly AlertButton _removeAlertButton;
      private RibbonControl _ribbon;
      private DockManager _dockManager;

      public BaseShell()
      {
         InitializeComponent();

         // Activates double buffering to avoid flickering
         SetStyle(ControlStyles.DoubleBuffer |
                  ControlStyles.OptimizedDoubleBuffer |
                  ControlStyles.UserPaint |
                  ControlStyles.AllPaintingInWmPaint, true);


         UpdateStyles();

         PopupBarManager = new BarManager {Form = this};
         _removeAlertButton = new AlertButton(ApplicationIcons.Cancel, AlertButtonStyle.Button) {Hint = ToolTips.DoNotShowVersionUpdate};
      }

      public virtual void Initialize()
      {
         InitializeTabManager();
         InitializeAlert();
         RegisterRegions();
      }

      protected void InitializeTabManager()
      {
         xtraTabbedMdiManager.PageAdded += pageAdded;
         xtraTabbedMdiManager.PageRemoved += pageRemoved;
         xtraTabbedMdiManager.BorderStyle = BorderStyles.NoBorder;
         xtraTabbedMdiManager.BorderStylePage = BorderStyles.NoBorder;
         xtraTabbedMdiManager.ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeader;
         xtraTabbedMdiManager.MouseDown += (o, e) => this.DoWithinExceptionHandler(() => onTabMouseDown(e));
         xtraTabbedMdiManager.AllowDragDrop = DefaultBoolean.True;
      }

      protected void InitializeAlert()
      {
         alertControl.AllowHtmlText = true;
         alertControl.Buttons.Add(_removeAlertButton);
         alertControl.AlertClick += (o, e) => this.DoWithinExceptionHandler(() => alertClick(e.Info.Tag.ToString()));
         alertControl.ButtonClick += (o, e) => this.DoWithinExceptionHandler(() => alertButtonClicked(e.Button));
      }

      protected void InitializeDockManager(DockManager dockManager)
      {
         _dockManager = dockManager;
         dockManager.DockingOptions.ShowCaptionImage = true;
         dockManager.DockingOptions.HideImmediatelyOnAutoHide = true;
         dockManager.AutoHideSpeed = 100;
      }

      protected void InitializeImages(IImageListRetriever imageListRetriever)
      {
         _dockManager.Images = imageListRetriever.AllImages16x16;
         alertControl.Images = imageListRetriever.AllImages16x16;
         _ribbon.Images = imageListRetriever.AllImages16x16;
         _ribbon.LargeImages = imageListRetriever.AllImages32x32;

         xtraTabbedMdiManager.Images = imageListRetriever.AllImagesForTabs;
         var toolTipController = defaultToolTipController.DefaultController;
         toolTipController.Initialize(imageListRetriever);
         PopupBarManager.Images = imageListRetriever.AllImagesForContextMenu;
      }

      private void onTabMouseDown(MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right)
            return;

         var hi = xtraTabbedMdiManager.CalcHitInfo(e.Location);
         if (hi == null || hi.HitTest != XtraTabHitTest.PageHeader)
            return;

         var view = viewFromPage(hi.Page as XtraMdiTabPage);

         _presenter.CreatePopupMenuFor(view).At(PointToClient(Cursor.Position));
      }

      private void activateView()
      {
         _presenter.Activate(ActiveView);
         if (ActiveView == null) return;
         var page = xtraTabbedMdiManager.Pages[ActiveMdiChild];
         if (page == null) return;
         page.Image = ActiveView.ApplicationIcon.ToImage(IconSizes.Size16x16);
      }

      private void pageRemoved(object sender, MdiTabPageEventArgs e)
      {
         var activeView = viewFromPage(e);
         if (activeView == null) return;
      }

      private void pageAdded(object sender, MdiTabPageEventArgs e)
      {
         var activeView = viewFromPage(e);
         if (activeView == null) return;
         e.Page.Image = activeView.ApplicationIcon.ToImage(IconSizes.Size16x16);
      }

      public void ShowHelp()
      {
         Help.ShowHelp(this, Constants.HELP_NAMESPACE, HelpNavigator.Topic);
      }

      public void DisplayNotification(string caption, string notification, string url)
      {
         var info = new AlertInfo(caption, notification, ApplicationIcon.ToImage(IconSizes.Size32x32)) {Tag = url};
         alertControl.FormMaxCount = 1;
         alertControl.Show(this, info);
      }

      private void alertButtonClicked(AlertButton button)
      {
         if (button.Name == _removeAlertButton.Name)
            _presenter.RemoveAlert();
      }

      private void alertClick(string urlToOpen)
      {
         FileHelper.TryOpenFile(urlToOpen);
      }

      private IMdiChildView viewFromPage(MdiTabPageEventArgs e)
      {
         return viewFromPage(e.Page);
      }

      private IMdiChildView viewFromPage(XtraMdiTabPage page)
      {
         return page?.MdiChild as IMdiChildView;
      }

      protected void RegisterRegion(DockPanel dockPanel, RegionName regionName)
      {
         IoC.RegisterImplementationOf((IRegion) dockPanel, regionName.Name);
         dockPanel.Text = regionName.Caption;
         dockPanel.ImageIndex = ApplicationIcons.IconIndex(regionName.Icon);
      }

      public void InWaitCursor(bool hourGlassVisible, bool forceCursorChange)
      {
         if (hourGlassVisible)
         {
            if (forceCursorChange)
               HourGlass.Enabled = true;
            else
               Cursor.Current = Cursors.WaitCursor;
         }
         else
         {
            HourGlass.Enabled = false;
            Cursor.Current = Cursors.Default;
         }
      }

      protected virtual void RegisterRegions()
      {
      }

      protected void RaiseLoadingEvent()
      {
         Loading();
      }

      public IMdiChildView ActiveView => ActiveMdiChild as IMdiChildView;

      public bool AllowChildActivation
      {
         set
         {
            MdiChildren.Where(mdi => !Equals(mdi, ActiveMdiChild))
               .Each(mdi => mdi.Enabled = value);
         }
      }

      protected override void OnMdiChildActivate(EventArgs e)
      {
         base.OnMdiChildActivate(e);
         try
         {
            //Memory leak in .NET Framework. FormerlyActiveMdiChild is not set to null
            typeof (Form).InvokeMember("FormerlyActiveMdiChild",
               BindingFlags.Instance | BindingFlags.SetProperty |
               BindingFlags.NonPublic, null,
               this, new object[] {null});
         }
         catch (Exception)
         {
            // Something went wrong. Maybe we don't have enough permissions
            // to perform this or the "FormerlyActiveMdiChild" property
            // no longer exists.
         }
      }

      public virtual void AttachPresenter(IPresenter presenter)
      {
         /*nothing to do here*/
      }

      public virtual void AttachPresenter(IMainViewPresenter presenter)
      {
         _presenter = presenter;
      }

      protected void InitializeRibbon(RibbonControl ribbon, ApplicationMenu applicationMenu, PopupControlContainer popupControlContainer)
      {
         _ribbon = ribbon;
         ribbon.ApplicationButtonDropDownControl = applicationMenu;
         applicationMenu.RightPaneControlContainer = popupControlContainer;
         applicationMenu.ShowRightPane = true;
         applicationMenu.MenuDrawMode = MenuDrawMode.LargeImagesTextDescription;
         applicationMenu.Ribbon = ribbon;
         popupControlContainer.Ribbon = ribbon;
      }

      public virtual void InitializeBinding()
      {
         MdiChildActivate += (o, e) => this.DoWithinExceptionHandler(activateView);
      }

      public virtual void InitializeResources()
      {
         /*nothing to do here*/
      }

      public virtual string Caption
      {
         get => Text;
         set
         {
            Text = value;
            CaptionChanged(this, EventArgs.Empty);
         }
      }

      public virtual bool HasError => false;

      public string ErrorMessage => string.Empty;

      public BarManager PopupBarManager { get; }
   }
}
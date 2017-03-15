using System;
using OSPSuite.Utility.Container;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Services;

namespace OSPSuite.Starter.Views
{
   public partial class ShellView : RibbonForm, IShellView
   {
      private IShellPresenter _presenter;

      public ShellView(IContainer container)
      {
         InitializeComponent();
         container.RegisterImplementationOf(ribbon.Manager);
         container.RegisterImplementationOf(ribbon.Manager as BarManager);
         container.RegisterImplementationOf(new ApplicationMenu());
         container.RegisterImplementationOf(new PanelControl());
      }

      public void AttachPresenter(IShellPresenter presenter)
      {
         _presenter = presenter;
      }

      public void InitializeBinding()
      {
         
      }

      public void InitializeResources()
      {
         
      }

      public string Caption { get; set; }
      public event EventHandler CaptionChanged = delegate { };

      public bool HasError
      {
         get { return false; }
      }

      public void AttachPresenter(IPresenter presenter)
      {
      }

      public ApplicationIcon ApplicationIcon { get; set; }
      public IMdiChildView ActiveView { get; private set; }

      public void InWaitCursor(bool hourGlassVisible, bool forceCursorChange)
      {
      }

      public void Initialize()
      {
         var imageListRetriever = IoC.Resolve<IImageListRetriever>();
         ribbon.Images = imageListRetriever.AllImages16x16;
         ribbon.LargeImages = imageListRetriever.AllImages32x32;
      }

      public void ShowHelp()
      {
      }

      public void DisplayNotification(string caption, string notification, string url)
      {
      }
   }
}
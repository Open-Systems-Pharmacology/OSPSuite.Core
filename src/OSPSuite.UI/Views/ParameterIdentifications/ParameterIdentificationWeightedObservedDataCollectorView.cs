using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraTab;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationWeightedObservedDataCollectorView : BaseUserControl, IParameterIdentificationWeightedObservedDataCollectorView, ITabbedView
   {
      private IParameterIdentificationWeightedObservedDataCollectorPresenter _presenter;
      public bool Updating { get; protected set; }

      public ParameterIdentificationWeightedObservedDataCollectorView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IParameterIdentificationWeightedObservedDataCollectorPresenter presenter)
      {
         _presenter = presenter;
         TabControl.SelectedPageChanging += onSelectedPageChanging;
      }

      private void onSelectedPageChanging(object sender, TabPageChangingEventArgs e)
      {
         OnEvent(() => _presenter.ObservedDataViewSelected(e.Page.Tag as IView));
      }

      public void AddObservedDataView(IView view)
      {
         try
         {
            TabControl.SelectedPageChanging -= onSelectedPageChanging;
            var page = this.AddTabbedView(TabControl.TabPages.Count, view);
            page.Tag = view;
            page.ShowCloseButton = DefaultBoolean.False;
         }
         finally
         {
            TabControl.SelectedPageChanging += onSelectedPageChanging;
         }
      }

      public void RemoveObservedDataView(IView view)
      {
         var tab = pageFor(view);
         removeTab(tab);
      }

      private XtraTabPage pageFor(IView view)
      {
         return TabControl.TabPages.FirstOrDefault(x => Equals(x.Tag, view));
      }

      public void SelectObservedDataView(IView view)
      {
         var page = pageFor(view);
         selectPage(page);
      }

      public void Clear()
      {
         TabControl.TabPages.Clear();
      }

      private void selectPage(XtraTabPage page)
      {
         if (page == null) return;
         TabControl.SelectedTabPage = page;
      }

      private void removeTab(XtraTabPage page)
      {
         if (page == null) return;
         TabControl.TabPages.Remove(page);
      }

      public void BeginUpdate()
      {
         TabControl.BeginUpdate();
         Updating = true;
      }

      public void EndUpdate()
      {
         TabControl.EndUpdate();
         Updating = false;
      }

      public XtraTabControl TabControl { get; private set; }

   }
}
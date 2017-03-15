using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ObservedData
{
   public partial class EditDataRepositoryView : BaseMdiChildView, IEditDataRepositoryView
   {
      public EditDataRepositoryView(IShell shell) : base(shell)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditDataRepositoryPresenter presenter)
      {
         _presenter = presenter;
      }

      public override ApplicationIcon ApplicationIcon
      {
         get { return ApplicationIcons.ObservedData; }
      }

      public override void AddSubItemView(ISubPresenterItem subPresenterItem, IView viewToAdd)
      {
         if (subPresenterItem == ObservedDataItems.Chart)
            splitControlDataToChart.Panel2.FillWith(viewToAdd);

         else if (subPresenterItem == ObservedDataItems.Data)
            splitControlDataToChart.Panel1.FillWith(viewToAdd);

         else if (subPresenterItem == ObservedDataItems.MetaData)
            splitControlMetaDataToData.Panel1.FillWith(viewToAdd);
      }
   }
}
using System.Collections.Generic;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Assets;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class MultipleParameterIdentificationFeedbackView : BaseUserControl, IMultipleParameterIdentificationFeedbackView
   {
      private readonly IImageListRetriever _imageListRetriever;
      private IMultipleParameterIdentificationFeedbackPresenter _presenter;
      private readonly GridViewBinder<MultiOptimizationRunResultDTO> _gridViewBinder;
      private readonly RepositoryItemMemoEdit _repositoryItemDescription;
      private readonly UxRepositoryItemImageComboBox _statusRepositoryItem;

      public MultipleParameterIdentificationFeedbackView(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<MultiOptimizationRunResultDTO>(gridView);
         _repositoryItemDescription = new RepositoryItemMemoEdit();
         _statusRepositoryItem = new UxRepositoryItemImageComboBox(gridView, imageListRetriever);
         gridView.AllowsFiltering = false;
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.MultiSelect = true;
         gridView.OptionsView.RowAutoHeight = true;
      }

      public void AttachPresenter(IMultipleParameterIdentificationFeedbackPresenter presenter)
      {
         _presenter = presenter;
      }

      public void RefreshData()
      {
         gridControl.RefreshDataSource();
      }

      public void BindTo(IEnumerable<MultiOptimizationRunResultDTO> allRunResultDTO)
      {
         _gridViewBinder.BindToSource(allRunResultDTO);
         gridView.BestFitColumns();
      }

      public void DeleteBinding()
      {
         _gridViewBinder.DeleteBinding();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridViewBinder.Bind(x => x.Index)
            .WithCaption(Captions.ParameterIdentification.RunIndex)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.Description)
            .WithRepository(x => _repositoryItemDescription)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.BestError)
            .WithCaption(Captions.ParameterIdentification.Best)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.CurrentError)
             .WithCaption(Captions.ParameterIdentification.Current)
             .AsReadOnly();

         _gridViewBinder.Bind(x => x.Status)
            .WithCaption(Captions.ParameterIdentification.Status)
            .WithRepository(statusRepositoryFor)
            .AsReadOnly();

         gridView.CustomDrawEmptyForeground += (o, e) => OnEvent(addMessageInEmptyArea, e);
      }

      private RepositoryItem statusRepositoryFor(MultiOptimizationRunResultDTO runResultDTO)
      {
         _statusRepositoryItem.Items.Clear();
         _statusRepositoryItem.Items.Add(new ImageComboBoxItem(runResultDTO.Status, _imageListRetriever.ImageIndex(runResultDTO.StatusIcon.IconName)));
         return _statusRepositoryItem;
      }

      private void addMessageInEmptyArea(CustomDrawEventArgs e)
      {
         gridView.AddMessageInEmptyArea(e, Captions.ParameterIdentification.MultipleRunsAreBeingCreated);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemParameterIdentificationRunResults.TextVisible = false;
      }
   }
}
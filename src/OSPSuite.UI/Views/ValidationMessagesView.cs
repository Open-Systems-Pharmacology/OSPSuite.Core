using System.Collections.Generic;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views
{
   public partial class ValidationMessagesView : BaseModalView, IValidationMessagesView
   {
      private readonly IImageListRetriever _imageListRetriever;
      private readonly IToolTipCreator _toolTipCreator;
      private GridViewBinder<ValidationMessageDTO> _gridViewBinder;
      private readonly RepositoryItemMemoEdit _messageRepositoryItem;

      public ValidationMessagesView(IShell shell, IImageListRetriever imageListRetriever, IToolTipCreator toolTipCreator) : base(shell)
      {
         InitializeComponent();
         _imageListRetriever = imageListRetriever;
         _toolTipCreator = toolTipCreator;
         gridView.ShouldUseColorForDisabledCell = false;
         _messageRepositoryItem = new RepositoryItemMemoEdit {AutoHeight = true};
         gridView.OptionsView.RowAutoHeight = true;
         var toolTipController = new ToolTipController {ImageList = imageListRetriever.AllImages16x16};
         toolTipController.GetActiveObjectInfo += onToolTipControllerGetActiveObjectInfo;
         gridControl.ToolTipController = toolTipController;
      }

      public override void InitializeBinding()
      {
         _gridViewBinder = new GridViewBinder<ValidationMessageDTO>(gridView);

         _gridViewBinder.Bind(x => x.Status)
            .WithCaption(Captions.ParameterIdentification.Status)
            .WithRepository(statusRepositoryFor)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.Message)
            .WithRepository(x => _messageRepositoryItem)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.ObjectDescription)
            .WithCaption(Captions.InvalidObject)
            .WithToolTip(Captions.InvalidObjectToolTip)
            .AsReadOnly();
      }

      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (e.SelectedControl != gridView.GridControl) return;

         var validationMessage = _gridViewBinder.ElementAt(e);
         if (validationMessage == null) return;

         var superToolTip = _toolTipCreator.ToolTipFor(validationMessage);
         if (superToolTip == null)
            return;

         e.Info = _toolTipCreator.ToolTipControlInfoFor(validationMessage, superToolTip);
      }

      public void AttachPresenter(IValidationMessagesPresenter presenter)
      {
      }

      public void BindTo(IEnumerable<ValidationMessageDTO> validationMessageDtos)
      {
         _gridViewBinder.BindToSource(validationMessageDtos);
         gridView.BestFitColumns();
      }

      private RepositoryItem statusRepositoryFor(ValidationMessageDTO runResultDTO)
      {
         var statusRepositoryItem = new UxRepositoryItemImageComboBox(gridView, _imageListRetriever);
         return statusRepositoryItem.AddItem(runResultDTO.Status, runResultDTO.Icon);
      }
   }
}
using System.Collections.Generic;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationLinkedParametersView : BaseUserControl, IParameterIdentificationLinkedParametersView
   {
      private IParameterIdentificationLinkedParametersPresenter _presenter;
      private readonly GridViewBinder<LinkedParameterDTO> _gridViewBinder;
      private readonly PathElementsBinder<LinkedParameterDTO> _pathElementsBinder;
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRemoveButtonRepository();
      private readonly RepositoryItemButtonEdit _disableRemoveButtonRepository = new UxRemoveButtonRepository();
      private readonly RepositoryItemButtonEdit _unlinkButtonRepository = new UxRepositoryItemButtonImage(ApplicationIcons.Redo, MenuDescriptions.UnlinkParameter);
      private readonly RepositoryItemButtonEdit _disabledUnlinkButtonRepository = new UxRepositoryItemButtonImage(ApplicationIcons.Redo);

      public ParameterIdentificationLinkedParametersView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         gridView.AllowsFiltering = false;
         _gridViewBinder = new GridViewBinder<LinkedParameterDTO>(gridView);
         _pathElementsBinder = new PathElementsBinder<LinkedParameterDTO>(imageListRetriever);
         gridView.ShouldUseColorForDisabledCell = false;
         _disableRemoveButtonRepository.Buttons[0].Enabled = false;
         _disabledUnlinkButtonRepository.Buttons[0].Enabled = false;
      }

      public void AttachPresenter(IParameterIdentificationLinkedParametersPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<LinkedParameterDTO> linkedParameterDTOs)
      {
         _gridViewBinder.BindToSource(linkedParameterDTOs);
         gridView.RefreshData();
      }

      public void SetVisibility(PathElement pathElement, bool visible)
      {
         _pathElementsBinder.SetVisibility(pathElement, visible);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _pathElementsBinder.InitializeBinding(_gridViewBinder);

         _gridViewBinder.Bind(x => x.InitialValue)
            .WithCaption(Captions.ParameterIdentification.InitialValue)
            .WithFormat(dto => new UnitFormatter(dto.DisplayUnit))
            .AsReadOnly();

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(unlinkButtonRepository)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(removedButtonRepository)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.RemoveParameter(_gridViewBinder.FocusedElement));
         _unlinkButtonRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.UnlinkParameter(_gridViewBinder.FocusedElement));
         _gridViewBinder.Changed += NotifyViewChanged;
      }

      private RepositoryItem removedButtonRepository(LinkedParameterDTO linkedParameterDTO)
      {
         return _presenter.CanRemove(linkedParameterDTO) ? _removeButtonRepository : _disableRemoveButtonRepository;
      }

      private RepositoryItem unlinkButtonRepository(LinkedParameterDTO linkedParameterDTO)
      {
         return _presenter.CanUnlink(linkedParameterDTO) ? _unlinkButtonRepository : _disabledUnlinkButtonRepository;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemLinkedParameters.TextVisible = false;
      }
   }
}
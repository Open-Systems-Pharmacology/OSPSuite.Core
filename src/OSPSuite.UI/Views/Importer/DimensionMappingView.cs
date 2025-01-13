using System.Collections.Generic;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Views.Importer
{
   public partial class DimensionMappingView : BaseModalView, IDimensionMappingView
   {
      private IDimensionMappingPresenter _presenter;
      private readonly GridViewBinder<DimensionSelectionDTO> _gridViewBinder;
      private readonly UxRepositoryItemButtonEdit _applyToAllButtonRepository;

      public DimensionMappingView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<DimensionSelectionDTO>(gridView);
         _gridViewBinder.Changed += changed;

         _applyToAllButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Glyph);
         _applyToAllButtonRepository.Buttons[0].Caption = Captions.Importer.ApplyToAll;
         _applyToAllButtonRepository.ButtonClick += (o, e) => OnEvent(applyToAllClick);
      }

      private void applyToAllClick()
      {
         var templateDTO = _gridViewBinder.FocusedElement;
         _presenter.ApplyToAllMatching(templateDTO);
         _gridViewBinder.Update();
      }

      private void changed()
      {
         OkEnabled = _presenter.CanClose;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = Captions.Importer.DimensionSelect;
         descriptionLabel.Text = Captions.Importer.SetDimensionsForAmbiguousUnits;
      }

      public override void InitializeBinding()
      {
         _gridViewBinder.Bind(x => x.DisplayName);
         _gridViewBinder.Bind(x => x.SelectedDimension).WithRepository(repositoryFor);
         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithFixedWidth(UIConstants.Size.APPLY_TO_ALL_BUTTON_WIDTH)
            .WithRepository(x => getButton());
      }

      private RepositoryItem getButton()
      {
         return _applyToAllButtonRepository;
      }

      private RepositoryItem repositoryFor(DimensionSelectionDTO dto)
      {
         var uxRepositoryItemComboBox = new UxRepositoryItemComboBox(_gridViewBinder.GridView);
         uxRepositoryItemComboBox.FillComboBoxRepositoryWith(dto.Dimensions);
         return uxRepositoryItemComboBox;
      }

      public void AttachPresenter(IDimensionMappingPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IReadOnlyList<DimensionSelectionDTO> dimensionDTOs)
      {
         _gridViewBinder.BindToSource(dimensionDTOs);
         changed();
      }

      private void disposeBinders()
      {
         _gridViewBinder.Dispose();
      }
   }
}

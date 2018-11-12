using System.Collections.Generic;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Assets;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Views
{
   public partial class DisplayUnitsView : BaseUserControl, IDisplayUnitsView
   {
      private IDisplayUnitsPresenter _presenter;
      private GridViewBinder<DefaultUnitMapDTO> _gridViewBinder;
      private readonly UxRepositoryItemComboBox _allDimensionsRepository;
      private readonly UxRepositoryItemComboBox _allUnitsRepository;
      private readonly UxRepositoryItemButtonEdit _deleteRepository;

      public DisplayUnitsView()
      {
         InitializeComponent();
         _allDimensionsRepository = new UxRepositoryItemComboBox(gridView);
         _allUnitsRepository = new UxRepositoryItemComboBox(gridView);
         _deleteRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Delete);
         gridView.AllowsFiltering = false;
         gridView.OptionsMenu.EnableColumnMenu = false;
      }

      public override void InitializeBinding()
      {
         _gridViewBinder = new GridViewBinder<DefaultUnitMapDTO>(gridView);
         _gridViewBinder.AutoBind(x => x.Dimension)
            .WithRepository(dto => _allDimensionsRepository)
            .WithEditorConfiguration(editDimensionForCurrentMap)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _gridViewBinder.AutoBind(x => x.DisplayUnit)
            .WithCaption(Captions.DisplayUnit)
            .WithRepository(dto => _allUnitsRepository)
            .WithEditorConfiguration(editUnitForCurrentMap)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithRepository(x => _deleteRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _gridViewBinder.Changed += NotifyViewChanged;
         _deleteRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.RemoveDefaultUnit(_gridViewBinder.FocusedElement));
         btnAddUnitMap.Click += (o, e) => OnEvent(_presenter.AddDefaultUnit);
         btnSaveUnits.Click += (o, e) => OnEvent(_presenter.SaveUnitsToFile);
         btnLoadUnits.Click += (o, e) => OnEvent(_presenter.LoadUnitsFromFile);
      }

      private void editDimensionForCurrentMap(BaseEdit baseEdit, DefaultUnitMapDTO unitMap)
      {
         baseEdit.FillComboBoxEditorWith(_presenter.AllPossibleDimensionsFor(unitMap));
      }

      private void editUnitForCurrentMap(BaseEdit baseEdit, DefaultUnitMapDTO unitMap)
      {
         baseEdit.FillComboBoxEditorWith(_presenter.AllUnitsFor(unitMap.Dimension));
      }

      public void AttachPresenter(IDisplayUnitsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<DefaultUnitMapDTO> defaultUnits)
      {
         _gridViewBinder.BindToSource(defaultUnits);
      }

      public override bool HasError => _gridViewBinder.HasError;

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemAddUnitMap.AdjustTallButtonSize();
         layoutItemLoadUnits.AdjustTallButtonSize();
         layoutItemSaveUnits.AdjustTallButtonSize();
         btnAddUnitMap.InitWithImage(ApplicationIcons.Add, Captions.AddUnitMap, toolTip: ToolTips.AddUnitMap);
         btnLoadUnits.InitWithImage(ApplicationIcons.Load, Captions.LoadUnits, toolTip: ToolTips.LoadUnits);
         btnSaveUnits.InitWithImage(ApplicationIcons.Save, Captions.SaveUnits, toolTip: ToolTips.SaveUnits);
      }

   }
}
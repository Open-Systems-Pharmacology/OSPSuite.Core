using System.Collections.Generic;
using System.Linq;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Views.Charts
{
   public partial class CurveTemplateView : BaseUserControl, ICurveTemplateView
   {
      private ICurveTemplatePresenter _presenter;
      private readonly GridViewBinder<CurveTemplateDTO> _gridViewBinder;
      private readonly RepositoryItemColorEdit _colorRepository;
      private readonly RepositoryItemComboBox _lineStyleRepository;
      private readonly RepositoryItemComboBox _lineThicknessRepository;
      private readonly RepositoryItemComboBox _symbolRepository;
      private readonly RepositoryItemCheckedComboBoxEdit _quantityTypeRepository;
      private readonly UxRepositoryItemButtonEdit _buttonRepository;
      private readonly EditorButton _deleteButton;

      public CurveTemplateView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<CurveTemplateDTO>(mainView);
         _lineStyleRepository = new UxRepositoryItemComboBox(mainView);
         _lineThicknessRepository = new UxRepositoryItemComboBox(mainView);
         _symbolRepository = new UxRepositoryItemComboBox(mainView);
         _colorRepository = new UxRepositoryItemColorPickEditWithHistory(mainView);
         _buttonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Delete);
         _quantityTypeRepository = new UxRepositoryItemCheckedComboBoxEdit(mainView, typeof(QuantityType));
         _deleteButton = _buttonRepository.Buttons[0];
         mainView.AllowsFiltering = false;
      }

      public void AttachPresenter(ICurveTemplatePresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         _lineStyleRepository.FillComboBoxRepositoryWith(EnumHelper.AllValuesFor<LineStyles>());
         _lineThicknessRepository.FillComboBoxRepositoryWith(new[] { 1, 2, 3 });
         _symbolRepository.FillComboBoxRepositoryWith(EnumHelper.AllValuesFor<Symbols>());

         _gridViewBinder.AutoBind(x => x.Name)
            .WithCaption(Captions.CurveName);

         _gridViewBinder.AutoBind(x => x.xRepositoryName)
               .WithCaption(Captions.XRepositoryName);
         
         _gridViewBinder.AutoBind(x => x.xDataPath)
               .WithCaption(Captions.XDataPath);

         _gridViewBinder.AutoBind(x => x.xQuantityType)
            .WithRepository(x => _quantityTypeRepository)
            .WithCaption(Captions.XQuantityType);

         _gridViewBinder.AutoBind(x => x.yRepositoryName)
               .WithCaption(Captions.YRepositoryName);

         _gridViewBinder.AutoBind(x => x.yDataPath)
            .WithCaption(Captions.YDataPath);

         _gridViewBinder.AutoBind(x => x.yQuantityType)
            .WithRepository(x => _quantityTypeRepository)
            .WithCaption(Captions.YQuantityType);

         _gridViewBinder.AutoBind(x => x.Color)
            .WithRepository(x => _colorRepository);

         _gridViewBinder.AutoBind(x => x.LineStyle)
            .WithRepository(x => _lineStyleRepository);

         _gridViewBinder.AutoBind(x => x.LineThickness)
            .WithRepository(x => _lineThicknessRepository);

         _gridViewBinder.AutoBind(x => x.Symbol)
            .WithRepository(x => _symbolRepository);

         _gridViewBinder.AutoBind(x => x.Visible);

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH * _buttonRepository.Buttons.Count)
            .WithRepository(getButtonRepository);

         _gridViewBinder.Changed += NotifyViewChanged;

         _buttonRepository.ButtonClick += (o, e) => OnEvent(buttonClick, e, notifyViewChanged:true);
      }

      private RepositoryItem getButtonRepository(CurveTemplateDTO dto)
      {
         _buttonRepository.Enabled = _presenter.CanDeleteCurves;
         _buttonRepository.Buttons.Cast<EditorButton>().Each(b => b.Enabled = _buttonRepository.Enabled);
         return _buttonRepository;
      }

      private void buttonClick(ButtonPressedEventArgs e)
      {
         if (e.Button.Index == _deleteButton.Index)
            _presenter.DeleteCurve(_gridViewBinder.FocusedElement);
      }

      public void BindTo(IEnumerable<CurveTemplateDTO> curveTemplates)
      {
         _gridViewBinder.BindToSource(curveTemplates);
         mainView.BestFitColumns();
         mainView.RefreshData();
      }
   }
}

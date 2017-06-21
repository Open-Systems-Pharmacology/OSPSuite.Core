using System;
using System.Collections.Generic;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Charts
{
   public partial class ChartTemplateManagerView : BaseUserControl, IChartTemplateManagerView
   {
      private readonly IToolTipCreator _toolTipCreator;
      private IChartTemplateManagerPresenter _presenter;
      private readonly GridViewBinder<CurveChartTemplate> _gridViewBinder;
      private readonly UxRepositoryItemButtonEdit _buttonRepository;
      private readonly EditorButton _deleteButton;
      private readonly EditorButton _cloneButton;
      private readonly EditorButton _saveButton;
      private readonly UxRepositoryItemCheckEdit _checkEditRepository;

      public ChartTemplateManagerView(IToolTipCreator toolTipCreator)
      {
         _toolTipCreator = toolTipCreator;
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<CurveChartTemplate>(gridViewTemplates);
         gridViewTemplates.AllowsFiltering = false;
         gridViewTemplates.ShowRowIndicator = false;
         gridViewTemplates.FocusedRowChanged += (o, e) => OnEvent(showSelectedTemplate, e);
         _checkEditRepository = new UxRepositoryItemCheckEdit(gridViewTemplates);
         _buttonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Glyph);
         _cloneButton = _buttonRepository.Buttons[0];
         _saveButton = _buttonRepository.AddButton(ButtonPredefines.Glyph);
         _deleteButton = _buttonRepository.AddButton(ButtonPredefines.Glyph);
      }

      private void buttonClicked(ButtonPressedEventArgs e)
      {
         int index = e.Button.Index;
         var template = _gridViewBinder.FocusedElement;
         if (index == _deleteButton.Index)
            _presenter.DeleteTemplate(template);
         else if (index == _cloneButton.Index)
            _presenter.CloneTemplate(template);
         else if (index == _saveButton.Index)
            _presenter.SaveTemplateToFile(template);
      }

      private void showSelectedTemplate(FocusedRowChangedEventArgs e)
      {
         _presenter.ShowTemplateDetails(_gridViewBinder.ElementAt(e.FocusedRowHandle));
      }

      public override void InitializeBinding()
      {
         _gridViewBinder.AutoBind(x => x.Name)
            .AsReadOnly();

         _gridViewBinder.AutoBind(x => x.IsDefault)
            .WithCaption(Captions.Default).WithRepository(template => _checkEditRepository)
            .WithOnValueUpdating((template,propertyValueSetEventArgs) => OnEvent(() => setDefaultForTemplate(template, propertyValueSetEventArgs.NewValue)));

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH * _buttonRepository.Buttons.Count)
            .WithRepository(x => _buttonRepository);

         _buttonRepository.ButtonClick += (o, e) => OnEvent(buttonClicked, e);

         loadTemplateButton.Click += (o, e) => OnEvent(loadTemplates);
      }

      private void setDefaultForTemplate(CurveChartTemplate template, bool set)
      {
         _presenter.SetDefaultTemplateValue(template, set);
         gridViewTemplates.RefreshData();
      }

      private void loadTemplates()
      {
         _presenter.LoadTemplateFromFile();
      }

      public void AttachPresenter(IChartTemplateManagerPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<CurveChartTemplate> chartTemplates)
      {
         _gridViewBinder.BindToSource(chartTemplates);
         gridViewTemplates.RefreshData();
      }

      public void SetChartTemplateView(IView view)
      {
         panelTemplateView.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         initButton(_cloneButton, ApplicationIcons.Clone, ToolTips.ManageTemplates.Clone);
         initButton(_deleteButton, ApplicationIcons.Delete, ToolTips.ManageTemplates.Delete);
         initButton(_saveButton, ApplicationIcons.Save, ToolTips.ManageTemplates.SaveToFile);

         gridViewTemplates.OptionsMenu.EnableColumnMenu = true;
         gridViewTemplates.ShowColumnChooser = true;
         gridViewTemplates.ShowRowIndicator = true;
         gridViewTemplates.OptionsSelection.EnableAppearanceFocusedRow = true;

         loadTemplateButton.InitWithImage(ApplicationIcons.Load, Captions.LoadTemplate);
         loadTemplateControlItem.AdjustLongButtonSize();

         Caption = Captions.ManageChartTemplates;
      }

      private void initButton(EditorButton button, ApplicationIcon icon, string toolTip)
      {
         button.Image = icon.ToImage(IconSizes.Size16x16);
         button.SuperTip = _toolTipCreator.CreateToolTip(toolTip,image: button.Image);
      }

      public event EventHandler<ViewResizedEventArgs> HeightChanged;
      public void AdjustHeight()
      {
         HeightChanged(this, new ViewResizedEventArgs(OptimalHeight));
      }

      public void Repaint()
      {
         
      }

      public int OptimalHeight
      {
         get { return layoutControl.Size.Height; }
      }
   }
}

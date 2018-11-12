using System;
using System.Drawing;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.XtraLayout;
using OSPSuite.Assets;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Views.SensitivityAnalyses;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   public partial class SensitivityAnalysisSetValueView : BaseUserControl, ISensitivityAnalysisSetValueView
   {
      private ISensitivityAnalysisSetValuePresenter _presenter;
      private readonly ScreenBinder<IParameterDTO> _screenBinder;
      private TextEditElementBinder<IParameterDTO, double> _textEditElementBinder;

      public SensitivityAnalysisSetValueView()
      {
         _screenBinder = new ScreenBinder<IParameterDTO>();
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnApplyAll.InitWithImage(ApplicationIcons.RefreshAll, Captions.SensitivityAnalysis.All);
         btnApplySelection.InitWithImage(ApplicationIcons.RefreshSelected, Captions.SensitivityAnalysis.Selection);

         alignButtons();
         textEdit.Properties.Mask.UseMaskAsDisplayFormat = true;

         layoutControlItemValueText.TextVisible = false;
         verticalAlignTextEdit();
         uxLayoutControl.BestFit();
      }

      private void alignButtons()
      {
         btnApplyAll.AutoWidthInLayoutControl = true;
         btnApplySelection.AutoWidthInLayoutControl = true;
         layoutControlItemAllButton.SizeConstraintsType = SizeConstraintsType.Custom;
         layoutControlItemAllButton.ControlAlignment = ContentAlignment.MiddleLeft;
         layoutControlItemSelectionButton.SizeConstraintsType = SizeConstraintsType.Custom;
         layoutControlItemSelectionButton.ControlAlignment = ContentAlignment.MiddleLeft;
      }

      private void verticalAlignTextEdit()
      {
         layoutControlItemValueText.SizeConstraintsType = SizeConstraintsType.Custom;
         layoutControlItemValueText.FillControlToClientArea = false;
         layoutControlItemValueText.Size = new Size(100, 0);
         layoutControlItemValueText.ControlAlignment = ContentAlignment.MiddleLeft;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnApplyAll.Click += (o, e) => OnEvent(() => _presenter.ApplyAll(_presenter.ParameterDTO));
         btnApplySelection.Click += (o, e) => OnEvent(() => _presenter.ApplySelection(_presenter.ParameterDTO));
         btnApplyAll.ToolTip = Captions.SensitivityAnalysis.ApplyValueToAllSensitivityParameters;
         btnApplySelection.ToolTip = Captions.SensitivityAnalysis.ApplyValueToSelectedSensitivityParameters;

         _textEditElementBinder = _screenBinder.Bind(x => x.Value).To(textEdit);
         _textEditElementBinder.OnValueUpdating += (parameterDTO, value) => OnEvent(() => setParameterValue(parameterDTO, value));

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      private void setParameterValue(IParameterDTO parameterDTO, PropertyValueSetEventArgs<double> value)
      {
         _presenter.SetParameterValue(parameterDTO, value.NewValue);
      }

      public void AttachPresenter(ISensitivityAnalysisSetValuePresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IParameterDTO parameterDTO)
      {
         _screenBinder.BindToSource(parameterDTO);
      }

      public void ConfigureForUInt()
      {
         textEdit.Properties.ConfigureWith(typeof(uint));
         _textEditElementBinder.WithFormat(value => Convert.ToUInt32(value).ToString());
      }
   }
}

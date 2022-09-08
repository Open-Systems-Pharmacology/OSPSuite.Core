using System;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
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
         tablePanel.AdjustControlSize(btnApplyAll, height: tbValue.Height);
         tablePanel.AdjustControlSize(btnApplySelection, height: tbValue.Height);
         tbValue.Properties.Mask.UseMaskAsDisplayFormat = true;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnApplyAll.Click += (o, e) => OnEvent(() => _presenter.ApplyAll(_presenter.ParameterDTO));
         btnApplySelection.Click += (o, e) => OnEvent(() => _presenter.ApplySelection(_presenter.ParameterDTO));
         btnApplyAll.ToolTip = Captions.SensitivityAnalysis.ApplyValueToAllSensitivityParameters;
         btnApplySelection.ToolTip = Captions.SensitivityAnalysis.ApplyValueToSelectedSensitivityParameters;

         _textEditElementBinder = _screenBinder.Bind(x => x.Value).To(tbValue);
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
         tbValue.Properties.ConfigureWith(typeof(uint));
         _textEditElementBinder.WithFormat(value => Convert.ToUInt32(value).ToString());
      }
   }
}
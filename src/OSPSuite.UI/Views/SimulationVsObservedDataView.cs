﻿using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Format;

namespace OSPSuite.UI.Views
{
   public partial class SimulationVsObservedDataView : BaseUserControl, ISimulationVsObservedDataView
   {
      private ISimulationVsObservedDataPresenter _presenter;
      private readonly IFormatter<double> _doubleFormatter;

      public SimulationVsObservedDataView()
      {
         InitializeComponent();
         totalErrorLayoutControlItem.TextVisible = true;
         totalErrorLayoutControlItem.Visibility = LayoutVisibility.Never;
         totalErrorLayoutControlItem.Text = Captions.ParameterIdentification.TotalError; ;
         totalErrorTextEdit.ReadOnly = true;
         _doubleFormatter = new DoubleFormatter();
      }

      public void SetAnalysisView(IView view)
      {
         panelChart.FillWith(view);
      }

      public void SetTotalError(double totalError)
      {
         totalErrorLayoutControlItem.Visibility = LayoutVisibility.Always;
         totalErrorTextEdit.Text = _doubleFormatter.Format(totalError);
      }

      public void AttachPresenter(ISimulationVsObservedDataPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
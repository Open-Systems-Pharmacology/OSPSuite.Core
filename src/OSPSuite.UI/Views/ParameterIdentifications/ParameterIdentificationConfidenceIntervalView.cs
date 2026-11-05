using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationConfidenceIntervalView : BaseUserControl, IParameterIdentificationConfidenceIntervalView
   {
      private readonly GridViewBinder<ParameterConfidenceIntervalDTO> _gridViewBinder;
      private IParameterIdentificationConfidenceIntervalPresenter _presenter;
      private readonly DoubleFormatter _doubleFormatter = new DoubleFormatter();

      public ParameterIdentificationConfidenceIntervalView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<ParameterConfidenceIntervalDTO>(gridView);
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.AllowsFiltering = false;
         gridView.MultiSelect = true;
      }

      public void AttachPresenter(IParameterIdentificationConfidenceIntervalPresenter presenter)
      {
         _presenter = presenter;
      }

      public void DeleteBinding()
      {
         _gridViewBinder.DeleteBinding();
      }

      public void BindTo(IEnumerable<ParameterConfidenceIntervalDTO> parameterConfidenceIntervalDTOs)
      {
         _gridViewBinder.BindToSource(parameterConfidenceIntervalDTOs);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridViewBinder.Bind(x => x.Name)
            .WithCaption(ObjectTypes.IdentificationParameter)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.Value)
            .WithCaption(Captions.ParameterIdentification.OptimalValue)
            .WithFormat(dto => new UnitFormatter(dto.Unit))
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.ConfidenceInterval)
            .WithCaption(Captions.ParameterIdentification.ConfidenceInterval)
            .WithFormat(dto => new UnitFormatter(dto.Unit))
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.RelativeConfidenceInterval)
            .WithCaption(Captions.ParameterIdentification.RelativeConfidenceInterval)
            .WithFormat(_doubleFormatter)
            .AsReadOnly();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemConfidenceIntervals.TextVisible = false;
      }
   }
}
using System.Collections.Generic;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Formatters;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationParametersFeedbackView : BaseUserControl, IParameterIdentificationParametersFeedbackView
   {
      private readonly IImageListRetriever _imageListRetriever;
      private IParameterIdentificationParametersFeedbackPresenter _presenter;
      private readonly GridViewBinder<ParameterFeedbackDTO> _parametersBinder;
      private readonly GridViewBinder<IRunPropertyDTO> _runPropertiesBinder;
      private readonly ValueDTOFormatter _valueDTOFormatter;
      private readonly RepositoryItemTextEdit _textRepositoryItem;

      public ParameterIdentificationParametersFeedbackView(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         _parametersBinder = new GridViewBinder<ParameterFeedbackDTO>(gridViewParameters);
         _runPropertiesBinder = new GridViewBinder<IRunPropertyDTO>(gridViewProperties);
         initGridView(gridViewParameters);
         initGridView(gridViewProperties);
         gridViewProperties.ShowRowIndicator = false;
         gridViewProperties.ShowColumnHeaders = false;
         _valueDTOFormatter = new ValueDTOFormatter();
         _textRepositoryItem = new RepositoryItemTextEdit();
      }

      private void initGridView(UxGridView gridView)
      {
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.AllowsFiltering = false;
         gridView.MultiSelect = true;
      }

      public void AttachPresenter(IParameterIdentificationParametersFeedbackPresenter presenter)
      {
         _presenter = presenter;
      }

      public void RefreshData()
      {
         gridParameters.RefreshDataSource();
         gridProperties.RefreshDataSource();
      }

      public void BindTo(IEnumerable<ParameterFeedbackDTO> parametersDTO, IEnumerable<IRunPropertyDTO> propertiesDTO)
      {
         _parametersBinder.BindToSource(parametersDTO);
         _runPropertiesBinder.BindToSource(propertiesDTO);
      }

      public bool CanExportParametersHistory
      {
         set
         {
            btnExportParametersHistory.Enabled = value;
            layoutItemExportParametersHistory.Enabled = value;
         }
         get => layoutItemExportParametersHistory.Enabled;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _parametersBinder.Bind(x => x.Name)
            .WithCaption(Captions.Name)
            .WithRepository(nameRepositoryFor)
            .AsReadOnly();

         _parametersBinder.Bind(x => x.Best)
            .WithFormat(_valueDTOFormatter)
            .WithCaption(Captions.ParameterIdentification.Best)
            .AsReadOnly();

         _parametersBinder.Bind(x => x.Current)
            .WithCaption(Captions.ParameterIdentification.Current)
            .WithFormat(_valueDTOFormatter)
            .AsReadOnly();

         _runPropertiesBinder.Bind(x => x.Name)
            .AsReadOnly();

         _runPropertiesBinder.Bind(x => x.FormattedValue)
            .WithRepository(propertyRepositoryFor)
            .AsReadOnly();

         btnExportParametersHistory.Click += (o, e) => OnEvent(_presenter.ExportParametersHistory);
      }

      private RepositoryItem propertyRepositoryFor(IRunPropertyDTO propertyDTO)
      {
         if (propertyDTO.Icon == null)
            return _textRepositoryItem;

         var textIconRepositoryItem = new UxRepositoryItemImageComboBox(gridViewProperties, _imageListRetriever);
         return textIconRepositoryItem.AddItem(propertyDTO.FormattedValue, propertyDTO.Icon);
      }

      private RepositoryItem nameRepositoryFor(ParameterFeedbackDTO parameterFeedbackDTO)
      {
         var nameRepository = new UxRepositoryItemImageComboBox(gridViewParameters, _imageListRetriever);
         return nameRepository.AddItem(parameterFeedbackDTO.Name, parameterFeedbackDTO.BoundaryCheckIcon);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemParameters.TextVisible = false;
         layoutItemRunProperties.TextVisible = false;
         layoutItemExportParametersHistory.AdjustLargeButtonSize();
         btnExportParametersHistory.InitWithImage(ApplicationIcons.Excel, Captions.ParameterIdentification.ExportParametersHistory);
      }
   }
}
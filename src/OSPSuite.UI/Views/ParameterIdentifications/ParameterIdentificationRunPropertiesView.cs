using System;
using System.Collections.Generic;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationRunPropertiesView : BaseUserControl, IParameterIdentificationRunPropertiesView
   {
      private readonly IImageListRetriever _imageListRetriever;
      private IParameterIdentificationRunPropertiesPresenter _presenter;
      private readonly GridViewBinder<IRunPropertyDTO> _gridViewBinder;
      public event EventHandler<ViewResizedEventArgs> HeightChanged = delegate { };
      private readonly RepositoryItemTextEdit _textRepositoryItem;

      public ParameterIdentificationRunPropertiesView(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         gridView.AllowsFiltering = false;
         gridView.MultiSelect = true;
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.ShowColumnHeaders = false;
         _gridViewBinder = new GridViewBinder<IRunPropertyDTO>(gridView);
         _textRepositoryItem = new RepositoryItemTextEdit();
      }

      public void BindTo(IEnumerable<IRunPropertyDTO> properties)
      {
         _gridViewBinder.BindToSource(properties);
         AdjustHeight();
      }

      public void AttachPresenter(IParameterIdentificationRunPropertiesPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridViewBinder.Bind(x => x.Name)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.FormattedValue)
            .WithRepository(propertyRepositoryFor)
            .AsReadOnly();
      }

      private RepositoryItem propertyRepositoryFor(IRunPropertyDTO propertyDTO)
      {
         if (propertyDTO.Icon == null)
            return _textRepositoryItem;

         var textIconRepositoryItem = new UxRepositoryItemImageComboBox(gridView, _imageListRetriever);
         return textIconRepositoryItem.AddItem(propertyDTO.FormattedValue, propertyDTO.Icon);
      }

      public void AdjustHeight()
      {
         HeightChanged(this, new ViewResizedEventArgs(OptimalHeight));
         Repaint();
      }

      public void Repaint()
      {
         gridView.LayoutChanged();
      }

      public int OptimalHeight => gridView.OptimalHeight;
   }
}
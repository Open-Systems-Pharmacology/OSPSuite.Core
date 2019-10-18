using System;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Formatters;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class ExtendedPropertiesView : BaseUserControl, IExtendedPropertiesView
   {
      private readonly IToolTipCreator _toolTipCreator;
      private readonly IImageListRetriever _imageListRetriever;
      private IExtendedPropertiesPresenter _presenter;
      private GridViewBinder<IExtendedProperty> _gridBinder;
      private readonly Cache<Type, RepositoryItem> _repositoryCache;
      private RepositoryItemTextEdit _repositoryItemTextEdit;
      private readonly SplitToUpperCaseFormatter _splitToUpperCaseFormatter;
      private IGridViewColumn _valueColumn;
      public event EventHandler<ViewResizedEventArgs> HeightChanged = delegate { };

      public ExtendedPropertiesView(IToolTipCreator toolTipCreator, IImageListRetriever imageListRetriever)
      {
         _toolTipCreator = toolTipCreator;
         _imageListRetriever = imageListRetriever;
         InitializeComponent();

         _repositoryCache = new Cache<Type, RepositoryItem>();
         _splitToUpperCaseFormatter = new SplitToUpperCaseFormatter();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _repositoryCache[typeof(bool)] = createBoolEditRepositoryItem();

         _repositoryCache[typeof(int)] = createIntegerEditRepositoryItem();
         var decimalEditRepositoryItem = createDecimalEditRepositoryItem();
         _repositoryCache[typeof(float)] = decimalEditRepositoryItem;
         _repositoryCache[typeof(double)] = decimalEditRepositoryItem;
         _repositoryItemTextEdit = new RepositoryItemTextEdit();
         _repositoryCache[typeof(string)] = _repositoryItemTextEdit;

         _gridBinder = new GridViewBinder<IExtendedProperty>(gridView);

         _gridBinder.Bind(x => x.DisplayName)
            .WithCaption(Captions.Name)
            .WithFormat(_splitToUpperCaseFormatter)
            .AsReadOnly();

         _valueColumn = _gridBinder.Bind(x => x.ValueAsObject)
            .WithCaption(Captions.Value)
            .WithRepository(repositoryFor)
            .WithOnValueUpdating((property, e) => OnEvent(() => setValue(property, e)));

         _gridBinder.Changed += NotifyViewChanged;

         gridControl.ToolTipController = new ToolTipController().Initialize(_imageListRetriever);
         gridControl.ToolTipController.GetActiveObjectInfo += (o, e) => OnEvent(() => createToolTip(e));
      }

      private void createToolTip(ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         var property = _gridBinder.ElementAt(e.ControlMousePosition);

         if (string.IsNullOrEmpty(property?.Description)) return;

         var superTooltip = _toolTipCreator.CreateToolTip(property.Description, property.DisplayName);
         e.Info = _toolTipCreator.ToolTipControlInfoFor(property, superTooltip);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         gridView.AllowsFiltering = false;
         gridView.MultiSelect = true;
      }

      private RepositoryItem createRepositoryItemFor<T>()
      {
         var repositoryItem = new RepositoryItemTextEdit();
         repositoryItem.ConfigureWith(typeof(T));
         return repositoryItem;
      }

      private RepositoryItem createDecimalEditRepositoryItem()
      {
         return createRepositoryItemFor<double>();
      }

      private void setValue(IExtendedProperty extendedProperty, PropertyValueSetEventArgs<object> e)
      {
         _presenter.SetPropertyValue(extendedProperty, e.NewValue);
      }

      private RepositoryItem createBoolEditRepositoryItem()
      {
         var comboBox = new UxRepositoryItemComboBox(gridView);
         comboBox.Items.AddRange(new[] {true, false});
         return comboBox;
      }

      private RepositoryItem createIntegerEditRepositoryItem()
      {
         return createRepositoryItemFor<int>();
      }

      private RepositoryItem repositoryFor(IExtendedProperty option)
      {
         if (option.ListOfValuesAsObjects.Any())
         {
            var comboBox = new UxRepositoryItemComboBox(gridView);
            option.ListOfValuesAsObjects.Each(value => comboBox.Items.Add(value));
            return comboBox;
         }

         if (_repositoryCache.Contains(option.Type))
            return _repositoryCache[option.Type];

         return _repositoryItemTextEdit;
      }

      public void AttachPresenter(IExtendedPropertiesPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ExtendedProperties algorithmProperties)
      {
         _gridBinder.BindToSource(algorithmProperties);
         AdjustHeight();
      }

      public bool ReadOnly
      {
         set
         {
            _valueColumn.ReadOnly = value;
            gridView.ShouldUseColorForDisabledCell = !value;
         }
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
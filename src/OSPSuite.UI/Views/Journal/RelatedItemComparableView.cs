using System.Collections.Generic;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Views.Journal
{
   public partial class RelatedItemComparableView : BaseUserControl, IRelatedItemComparableView
   {
      private IRelatedItemComparablePresenter _presenter;
      private readonly GridViewBinder<ObjectSelectionDTO> _gridViewBinder;
      private readonly UxRepositoryItemCheckEdit _selectEditor;

      public RelatedItemComparableView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<ObjectSelectionDTO>(gridView);
         _selectEditor = new UxRepositoryItemCheckEdit(gridView);
      }

      public void AttachPresenter(IRelatedItemComparablePresenter presenter)
      {
         _presenter = presenter;
      }

      public void ShowWarning(string warning)
      {
         warningVisible = true;
         lblWarning.Text = warning.FormatForDescription();
      }

      private bool warningVisible
      {
         set
         {
            layoutItemWarning.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
            layoutItemComparableItems.Visibility = LayoutVisibilityConvertor.FromBoolean(!value);
            layoutItemRunComparison.Visibility = layoutItemComparableItems.Visibility;
            emptySpaceItem.Visibility = layoutItemComparableItems.Visibility;
         }
      }

      public override string Caption
      {
         set { layoutItemComparableItems.Text = value.FormatForLabel(); }
      }

      public void BindTo(IEnumerable<ObjectSelectionDTO> allComparables)
      {
         warningVisible = false;
         //Since we are using autobind, we need the usage of a bindng list as well to react to code change of bound properties
         _gridViewBinder.BindToSource(allComparables.ToBindingList());
         gridView.RefreshData();
         gridView.BestFitColumns();
      }

      public bool RunComparisonEnabled
      {
         get { return btnRunComparison.Enabled; }
         set { btnRunComparison.Enabled = value; }
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _gridViewBinder.AutoBind(x => x.Selected)
            .WithRepository(dto => _selectEditor)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH)
            .WithOnChanged(x => _presenter.ItemSelectionChanged(x));

         _gridViewBinder.Bind(x => x.Name)
            .AsReadOnly();

         btnRunComparison.Click += (o, e) => OnEvent(_presenter.RunComparison);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         lblWarning.AsDescription();
         gridView.AllowsFiltering = false;
         gridView.ShowColumnHeaders = false;
         gridView.ShowRowIndicator = false;
         btnRunComparison.InitWithImage(ApplicationIcons.Run, Captions.Journal.RunComparison);
      }
   }
}
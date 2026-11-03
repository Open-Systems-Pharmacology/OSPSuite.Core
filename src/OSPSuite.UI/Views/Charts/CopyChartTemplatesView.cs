using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Views.Charts
{
   public partial class CopyChartTemplatesView : BaseModalView, ICopyChartTemplatesView
   {
      private ICopyChartTemplatesPresenter _presenter;
      private readonly GridViewBinder<ChartTemplateSelectionDTO> _gridViewBinder;
      private readonly UxRepositoryItemCheckEdit _selectEditor;

      public CopyChartTemplatesView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<ChartTemplateSelectionDTO>(gridView);
         _selectEditor = new UxRepositoryItemCheckEdit(gridView);
         gridView.AllowsFiltering = false;
         gridView.ShowRowIndicator = false;
         gridView.ShowColumnHeaders = false;
         cbSource.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
      }

      public void AttachPresenter(ICopyChartTemplatesPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         _gridViewBinder.Bind(x => x.Selected)
            .WithRepository(x => _selectEditor)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH);

         _gridViewBinder.Bind(x => x.Name)
            .AsReadOnly();

         _gridViewBinder.Changed += NotifyViewChanged;

         cbSource.SelectedIndexChanged += (o, e) => OnEvent(() => _presenter.SelectSource(cbSource.SelectedItem as ChartTemplateSourceDTO));
      }

      public void BindToSources(IReadOnlyList<ChartTemplateSourceDTO> sources)
      {
         cbSource.FillComboBoxEditorWith(sources);
         cbSource.SelectedItem = sources.FirstOrDefault();
      }

      public void BindToTemplates(IReadOnlyList<ChartTemplateSelectionDTO> templates)
      {
         _gridViewBinder.BindToSource(templates);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemSource.Text = Captions.CopyFrom.FormatForLabel();
         layoutItemGridTemplates.TextVisible = false;
         Caption = Captions.CopyChartTemplates;
         ApplicationIcon = ApplicationIcons.Copy;
      }

      public override bool HasError => _gridViewBinder.HasError;
   }
}

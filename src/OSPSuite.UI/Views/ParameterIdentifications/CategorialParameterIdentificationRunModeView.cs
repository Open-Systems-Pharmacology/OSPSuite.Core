using System;
using System.Collections.Generic;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraPivotGrid;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class CategorialParameterIdentificationRunModeView : BaseUserControl, ICategorialParameterIdentificationRunModeView
   {
      private ICategorialParameterIdentificationRunModePresenter _presenter;
      private PivotGridField _valueField;
      private readonly Cache<CategoryDTO, ScreenBinder<CategoryDTO>> _categoryScreenBinderCache = new Cache<CategoryDTO, ScreenBinder<CategoryDTO>>();
      private readonly ScreenBinder<CategorialRunModeDTO> _categorialRunModeScreenBinder = new ScreenBinder<CategorialRunModeDTO>();

      public CategorialParameterIdentificationRunModeView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(ICategorialParameterIdentificationRunModePresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         pivotGridControl.EditValueChanged += (o, e) => OnEvent(() => handleEditorChanged(e), notifyViewChanged: true);

         _presenter.RowAreaColumns.Each(columnName => { addField(pivotGridControl.CreateRowAreaNamed(columnName)); });
         _presenter.VisibleColumnAreaColumns.Each(columnName => { addField(pivotGridControl.CreateColumnAreaNamed(columnName)); });
         _presenter.HiddenColumnAreaColumns.Each(columnName => { addField(pivotGridControl.CreateColumnAreaNamed(columnName), visible: false); });

         _valueField = pivotGridControl.CreateDataAreaNamed(_presenter.ValueAreaColumn);
         addField(_valueField);

         pivotGridControl.TotalsVisible = false;
         pivotGridControl.ShowHeaders = false;
         pivotGridControl.OptionsSelection.MultiSelect = false;

         var repositoryItemCheckEdit = new RepositoryItemCheckEdit();
         pivotGridControl.RepositoryItems.Add(repositoryItemCheckEdit);
         pivotGridControl.OptionsCustomization.AllowEdit = true;
         pivotGridControl.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
         _valueField.FieldEdit = repositoryItemCheckEdit;
         _valueField.Options.AllowEdit = true;
         repositoryItemCheckEdit.EditValueChanged += (o, e) => OnEvent(() => pivotGridControl.PostEditor());

         pivotGridControl.BestFitDataHeaders(considerRowArea: true);

         _categorialRunModeScreenBinder.Bind(x => x.AllTheSame)
            .To(chkAllTheSame)
            .Changed += () => OnEvent(_presenter.AllTheSameChanged, notifyViewChanged: true);
      }

      private void addField(PivotGridField field, bool visible = true)
      {
         pivotGridControl.Fields.Add(field);
         field.Visible = visible;
      }

      private void handleEditorChanged(EditValueChangedEventArgs e)
      {
         var ds = e.CreateDrillDownDataSource();
         for (int i = 0; i < ds.RowCount; i++)
         {
            var dr = ds[i];
            var compoundName = dr.StringValue(Constants.CategoryOptimizations.COMPOUND);
            var calculationMethodName = dr.StringValue(Constants.CategoryOptimizations.CALCULATION_METHOD);
            var categoryName = dr.StringValue(Constants.CategoryOptimizations.CATEGORY);
            var selected = Convert.ToBoolean(e.Editor.EditValue);
            ds.SetValue(i, _valueField, selected);
            _presenter.CalculationMethodSelectionChanged(compoundName, categoryName, calculationMethodName, selected);
         }
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         chkAllTheSame.Text = Captions.ParameterIdentification.AllTheSame;
         chkAllTheSame.AutoSizeInLayoutControl = true;
         chkAllTheSame.AutoSize = true;
      }

      public void BindTo(IEnumerable<CategoryDTO> categories)
      {
         addCategories(categories);
      }

      public void BindTo(CategorialRunModeDTO categorialRunModeDTO)
      {
         pivotGridControl.DataSource = categorialRunModeDTO.CalculationMethodSelectionTable;
         displayCalculationMethodConfiguration(categorialRunModeDTO.ShouldShowSelection);
         _categorialRunModeScreenBinder.BindToSource(categorialRunModeDTO);
      }

      public void UpdateParameterIdentificationCount(int parameterIdentificationCount)
      {
         uxHintPanel.NoteText = Captions.ParameterIdentification.OptimizationsCount(parameterIdentificationCount);
         uxHintPanel.Image = parameterIdentificationCount < _presenter.WarningThreshold ? ApplicationIcons.Info : ApplicationIcons.Warning;
      }

      private void displayCalculationMethodConfiguration(bool shouldShowConfiguration)
      {
         layoutItemPivotGrid.Visibility = LayoutVisibilityConvertor.FromBoolean(shouldShowConfiguration);
      }

      private void addCategories(IEnumerable<CategoryDTO> categories)
      {
         categories.Each(addCategoryToGrid);
         layoutGroupCalculationMethodCategories.BestFit();
      }

      private void addCategoryToGrid(CategoryDTO categoryDTO)
      {
         var layoutItem = layoutGroupCalculationMethodCategories.AddItem();
         layoutItem.Move(emptySpaceItem, InsertType.Left);

         var checkEdit = new CheckEdit
         {
            AutoSize = true,
            AutoSizeInLayoutControl = true
         };

         var categoryScreenBinder = new ScreenBinder<CategoryDTO>();
         categoryScreenBinder.Bind(x => x.Selected)
            .To(checkEdit)
            .WithCaption(categoryDTO.DisplayName)
            .Changed += () => OnEvent(() => categorySelectionChanged(categoryDTO), notifyViewChanged: true);

         categoryScreenBinder.BindToSource(categoryDTO);
         _categoryScreenBinderCache.Add(categoryDTO, categoryScreenBinder);
         layoutItem.Control = checkEdit;
         layoutItem.TextVisible = false;
      }

      private void categorySelectionChanged(CategoryDTO categoryDTO)
      {
         _presenter.CategorySelectionChanged(categoryDTO);
      }
   }
}
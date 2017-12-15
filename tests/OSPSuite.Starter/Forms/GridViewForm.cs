using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Starter.Forms
{
   public partial class GridViewForm : Form
   {
      private readonly IImageListRetriever _imageListRetriever;
      private readonly GridViewBinder<ParameterDTO> _gridViewBinder;

      public GridViewForm(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
         gridView.OptionsSelection.EnableAppearanceFocusedRow = true;
         gridView.OptionsSelection.EnableAppearanceFocusedCell = true;
         gridView.OptionsSelection.MultiSelect = true;
         _gridViewBinder = new GridViewBinder<ParameterDTO>(gridView);

         initializeBinding();
         _gridViewBinder.BindToSource(generateDummyContent().ToList());
      }

      private void initializeBinding()
      {
         var gridViewBoundColumn = _gridViewBinder.Bind(x => x.Name)
            .AsReadOnly();

         gridViewBoundColumn.XtraColumn.OptionsColumn.AllowFocus = true;

         var boundColumn = _gridViewBinder.Bind(x => x.Value)
            .AsReadOnly();


         boundColumn.XtraColumn.OptionsColumn.AllowFocus = true;


         _gridViewBinder.Bind(x => x.ValueDescription);

         _gridViewBinder.Bind(x => x.ValueOriginType)
            .WithRepository(getRepository);
      }

      private RepositoryItem getRepository(ParameterDTO arg)
      {
         var repositoryItem = new UxRepositoryItemImageComboBox(gridView, _imageListRetriever);
         repositoryItem.FillImageComboBoxRepositoryWith(ValueOriginTypes.AllValueOrigins, x => x.Icon);
         return repositoryItem;
      }

      private IEnumerable<ParameterDTO> generateDummyContent()
      {
         for (var i = 0; i < 10; i++)
         {
            var parameter = new ParameterDTO().WithName($"Prameter_{i}");
            parameter.Formula = new ConstantFormula(i);
            yield return parameter;
         }
      }
   }

   class ParameterDTO : Parameter
   {
      public string ValueDescription
      {
         get => ValueOrigin.Description;
         set => ValueOrigin.Description = value;
      }

      public ValueOriginType ValueOriginType
      {
         get => ValueOrigin.Type;
         set => ValueOrigin.Type = value;
      }
   }
}
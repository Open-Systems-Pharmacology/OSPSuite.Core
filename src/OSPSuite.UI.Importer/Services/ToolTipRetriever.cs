using DevExpress.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.DeprecatedImporter.Services;

namespace OSPSuite.UI.Importer.Services
{
   public interface IToolTipRetriever
   {
      /// <summary>
      ///    This method builds a super tool tip for a import data column.
      /// </summary>
      /// <param name="column">Import data column.</param>
      /// <returns>A super tool tip with information about the given column.</returns>
      SuperToolTip GetToolTipForImportDataColumn(ImportDataColumn column);

   }

   public class ToolTipRetriever : IToolTipRetriever
   {
      private readonly IImporterTask _importerTask;

      public ToolTipRetriever(IImporterTask importerTask)
      {
         _importerTask = importerTask;
      }

      public SuperToolTip GetToolTipForImportDataColumn(ImportDataColumn column)
      {
         if (column == null) return null;
         var retValue = new SuperToolTip();
         retValue.Items.AddTitle(column.DisplayName);
         retValue.Items.Add(column.Description);
         if (!string.IsNullOrEmpty(column.Source))
            retValue.Items.Add($"Source = {column.Source}");
         if (column.Dimensions != null)
         {
            retValue.Items.AddSeparator();
            var item = new ToolTipTitleItem { Text = Captions.Importer.UnitInformation, Image = ApplicationIcons.UnitInformation.ToImage() };
            retValue.Items.Add(item);
            retValue.Items.Add($"Dimension = {column.ActiveDimension.DisplayName}[{column.ActiveDimension.Name}]");
            retValue.Items.Add($"Unit = {column.ActiveUnit.DisplayName}[{column.ActiveUnit.Name}]");
            if (column.ActiveDimension.InputParameters != null && column.ActiveDimension.InputParameters.Count > 0)
            {
               retValue.Items.AddTitle("Input Parameters");
               foreach (var inputParameter in column.ActiveDimension.InputParameters)
                  retValue.Items.Add($"{inputParameter.DisplayName} = {inputParameter.Value} {inputParameter.Unit.Name}");
            }
         }
         if (column.MetaData != null)
         {
            retValue.Items.AddSeparator();
            var item = new ToolTipTitleItem { Text = Captions.Importer.MetaData, Image = ApplicationIcons.MetaData.ToImage() };
            retValue.Items.Add(item);
            foreach (MetaDataColumn metaData in column.MetaData.Columns)
               retValue.Items.Add($"{metaData.DisplayName} = {((column.MetaData.Rows.Count > 0) ? column.MetaData.Rows.ItemByIndex(0)[metaData.ColumnName] : string.Empty)}");
         }

         //Add help text when data is missed
         var imageindex = _importerTask.GetImageIndex(column);
         if (imageindex == ApplicationIcons.IconIndex(ApplicationIcons.MissingUnitInformation))
         {
            retValue.Items.AddSeparator();
            retValue.Items.Add(new ToolTipItem
            {
               Image = ApplicationIcons.MissingUnitInformation.ToImage(),
               Text = Captions.Importer.TheUnitInformationMustBeEnteredOrConfirmed
            });
         }
         if (imageindex == ApplicationIcons.IconIndex(ApplicationIcons.MissingMetaData))
         {
            retValue.Items.AddSeparator();
            retValue.Items.Add(new ToolTipItem
            {
               Image = ApplicationIcons.MissingMetaData.ToImage(),
               Text = Captions.Importer.TheMetaDataInformationMustBeEnteredOrConfirmed
            });
         }
         return retValue;
      }

   }
}
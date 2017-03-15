using System;
using System.Drawing;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ColorPickEditControl;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Presentation.Core;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Controls
{
   [UserRepositoryItem("RegisterCustomEdit")]
   public class UxRepositoryItemColorPickEditWithHistory : RepositoryItemColorPickEdit
   {
      private readonly CustomColorArrayManager _customColorArrayManager;

      static UxRepositoryItemColorPickEditWithHistory() { RegisterCustomEdit(); }

      public static string CustomEditName = typeof(UxColorPickEditWithHistory).Name;

      public override string EditorTypeName { get { return CustomEditName; } }

      public static void RegisterCustomEdit()
      {
         EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomEditName,
           typeof(UxColorPickEditWithHistory), typeof(UxRepositoryItemColorPickEditWithHistory),
           typeof(ColorEditViewInfo), new ColorEditPainter(), true, null));
      }

      protected override string GetColorDisplayText(Color editValue)
      {
         return string.Empty;
      }

      public UxRepositoryItemColorPickEditWithHistory()
      {
         ShowSystemColors = false;
         ShowWebColors = false;
         _customColorArrayManager = new CustomColorArrayManager();
         ColorChanged += addColorToRecentlyUsed;
      }

      protected override Matrix CreateStandardColors()
      {
         var i = 0;
         var standardColors = new Matrix(10, 3);
         CommonColors.OrderedForPicker().Each(color =>
         {
            addColor(color, i / 10, i % 10, standardColors);
            i++;
         });

         return standardColors;
      }

      private void addColor(Color color, int x, int y, Matrix matrix)
      {
         matrix[x, y] = color;
      }

      public UxRepositoryItemColorPickEditWithHistory(BaseView view)
         : this()
      {
         EditValueChanged += (o, e) => view.PostEditor();
      }

      private void addColorToRecentlyUsed(object sender, EventArgs e)
      {
         var colorEdit = sender as ColorPickEdit;

         if (colorEdit == null) return;

         // If this color is picked by the color pick modal dialog, it will be inserted properly
         // No need to add manually.
         if (isStandardColor(colorEdit.Color) || isThemeColor(colorEdit.Color))
         {
            _customColorArrayManager.PushRecentColor(RecentColors, colorEdit.Color);
         }
      }
      
      private bool isStandardColor(Color newColor)
      {
         return foundColorInMatrix(newColor, StandardColors);
      }

      private bool isThemeColor(Color newColor)
      {
         return foundColorInMatrix(newColor, ThemeColors);
      }

      private bool foundColorInMatrix(Color newColor, Matrix matrix)
      {
         var found = false;
         matrix.ForEachItem(themeColor =>
         {
            if (newColor.IsEqualTo(themeColor.Value))
               found = true;
         });
         return found;
      }
   }
}

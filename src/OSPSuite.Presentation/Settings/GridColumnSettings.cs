using OSPSuite.Core;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Presentation.Settings
{
   public class GridColumnSettings : Notifier
   {
      public string ColumnName { get; }

      private string _caption;
      private int _groupIndex;
      private bool _visible;
      private int _visibleIndex;
      private int _width;
      private string _sortColumnName;
      public const int INDEX_NOT_DEFINED = 100;

      public GridColumnSettings()
      {
         _visible = true;
         _groupIndex = -1;
         _visibleIndex = INDEX_NOT_DEFINED;
      }

      public GridColumnSettings(string columnName) : this()
      {
         ColumnName = columnName;
         _caption = columnName;
      }

      public GridColumnSettings(GridColumnSettings settings)
      {
         ColumnName = settings.ColumnName;
         _visible = settings.Visible;
         _caption = settings.Caption;
         _groupIndex = settings.GroupIndex;
         _visibleIndex = settings.VisibleIndex;
         _width = settings.Width;
         _sortColumnName = settings.SortColumnName;
      }

      public GridColumnSettings WithVisible(bool visible)
      {
         Visible = visible;
         if (!Visible && VisibleIndex >= 0)
            VisibleIndex = -1;

         return this;
      }

      public GridColumnSettings WithCaption(string caption)
      {
         Caption = caption;
         return this;
      }

      public GridColumnSettings WithSortColumnName(string sortColumnName)
      {
         SortColumnName = sortColumnName;
         return this;
      }

      public bool Visible
      {
         get => _visible;
         set
         {
            if (_visible != value)
            {
               _visible = value;
               if (!_visible && VisibleIndex >= 0)
                  VisibleIndex = -1;

               OnPropertyChanged();
            }
         }
      }

      public int VisibleIndex
      {
         get => _visibleIndex;
         set
         {
            if (_visibleIndex != value)
            {
               _visibleIndex = value;
               if (_visibleIndex >= 0 && !Visible)
                  Visible = true;

               if (_visibleIndex < 0 && Visible)
                  Visible = false;

               OnPropertyChanged();
            }
         }
      }

      public int Width
      {
         get => _width;
         set => SetProperty(ref _width, value);
      }

      public string Caption
      {
         get => _caption;
         set => SetProperty(ref _caption, value);
      }

      public int GroupIndex
      {
         get => _groupIndex;
         set => SetProperty(ref _groupIndex, value);
      }

      public string SortColumnName
      {
         get => _sortColumnName;
         set => SetProperty(ref _sortColumnName, value);
      }

      public void CopyFrom(GridColumnSettings settings)
      {
         _visible = settings.Visible;
         _caption = settings.Caption;
         _groupIndex = settings.GroupIndex;
         _visibleIndex = settings.VisibleIndex;
         _width = settings.Width;
         _sortColumnName = settings.SortColumnName;
         OnChanged();
      }
   }
}
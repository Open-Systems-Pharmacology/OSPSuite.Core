using OSPSuite.Utility.Reflection;

namespace OSPSuite.Presentation.Settings
{
   public class GridColumnSettings : Notifier
   {
      public string ColumnName { get; private set; }

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
         if (!Visible && VisibleIndex >= 0) VisibleIndex = -1;
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
         get { return _visible; }
         set
         {
            if (_visible != value)
            {
               _visible = value;
               if (!_visible && VisibleIndex >= 0) VisibleIndex = -1;
               OnPropertyChanged(() => Visible);
            }
         }
      }
      public int VisibleIndex
      {
         get { return _visibleIndex; }
         set
         {
            if (_visibleIndex != value)
            {
               _visibleIndex = value;
               if (_visibleIndex >= 0 && !Visible) Visible = true;
               if (_visibleIndex < 0 && Visible) Visible = false;
               OnPropertyChanged(() => VisibleIndex);
            }
         }
      }

      public int Width
      {
         get { return _width; }
         set {
            if (_width != value)
            {
               _width = value;
               OnPropertyChanged(() => Width);
            }
         }
      }

      public string Caption
      {
         get { return _caption; }
         set
         {
            if (_caption != value)
            {
               _caption = value;
               OnPropertyChanged(() => Caption);
            }
         }
      }

      public int GroupIndex
      {
         get { return _groupIndex; }
         set
         {
            if (_groupIndex != value)
            {
               _groupIndex = value;
               OnPropertyChanged(() => GroupIndex);
            }
         }
      }

      public string SortColumnName
      {
         get { return _sortColumnName; }
         set
         {
            if (_sortColumnName != value)
            {
               _sortColumnName = value;
               OnPropertyChanged(() => SortColumnName);
            }
         }
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
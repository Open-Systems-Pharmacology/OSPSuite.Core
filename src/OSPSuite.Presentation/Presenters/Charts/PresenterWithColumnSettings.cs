using System;
using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Settings;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IPresenterWithColumnSettings
   {
      IEnumerable<GridColumnSettings> AllColumnSettings();
      GridColumnSettings ColumnSettings(string columnName);
      event Action<GridColumnSettings> ColumnSettingsChanged;
      void ApplyAllColumnSettings();
   }

   internal abstract class PresenterWithColumnSettings<TView, TPresenter> : AbstractPresenter<TView, TPresenter>, IPresenterWithColumnSettings
      where TView : IView<TPresenter>, IViewWithColumnSettings
      where TPresenter : IPresenter
   {
      protected IItemNotifyCache<string, GridColumnSettings> _columnSettings;
      public event Action<GridColumnSettings> ColumnSettingsChanged = delegate { };

      protected PresenterWithColumnSettings(TView view) : base(view)
      {
         _columnSettings = new ItemNotifyCache<string, GridColumnSettings>(x => x.ColumnName);
         SetDefaultColumnSettings();
         _columnSettings.ItemChanged += onItemChanged;
      }

      public void ApplyAllColumnSettings()
      {
         _view.ApplyAllColumnSettings();
      }

      private void onItemChanged(object sender, ItemChangedEventArgs args)
      {
         ColumnSettingsChanged(args.Item as GridColumnSettings); //sender is the cache
      }

      protected abstract void SetDefaultColumnSettings();

      protected GridColumnSettings AddColumnSettings(Enum enumValue)
      {
         var columnSettings = new GridColumnSettings(enumValue.ToString());
         _columnSettings.Add(columnSettings);
         return columnSettings;
      }

      public IEnumerable<GridColumnSettings> AllColumnSettings()
      {
         return _columnSettings;
      }

      public GridColumnSettings ColumnSettings(string columnName)
      {
         return _columnSettings.Contains(columnName) ? _columnSettings[columnName] : null;
      }
   }
}
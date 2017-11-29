using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.Settings;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IPresenterWithColumnSettings
   {
      IReadOnlyCollection<GridColumnSettings> AllColumnSettings { get; }
      GridColumnSettings ColumnSettings(string columnName);
      event Action<IReadOnlyCollection<GridColumnSettings>> ColumnSettingsChanged;
      void ApplyAllColumnSettings();
      void ApplyColumnSettings(GridColumnSettings columnSettings);
      void NotifyColumnSettingsChanged();
   }

   public abstract class PresenterWithColumnSettings<TView, TPresenter> : AbstractPresenter<TView, TPresenter>, IPresenterWithColumnSettings
      where TView : IView<TPresenter>, IViewWithColumnSettings
      where TPresenter : IPresenter
   {
      private readonly Cache<string, GridColumnSettings> _columnSettings;
      public event Action<IReadOnlyCollection<GridColumnSettings>> ColumnSettingsChanged = delegate { };

      protected PresenterWithColumnSettings(TView view) : base(view)
      {
         _columnSettings = new Cache<string, GridColumnSettings>(x => x.ColumnName);
         SetDefaultColumnSettings();
      }

      public void ApplyAllColumnSettings()
      {
         _view.ApplyAllColumnSettings();
      }

      public void ApplyColumnSettings(GridColumnSettings columnSettings)
      {
         if (!AllColumnSettings.Contains(columnSettings))
            return;

         _view.ApplyColumnSettings(columnSettings);
      }

      public void NotifyColumnSettingsChanged()
      {
         ColumnSettingsChanged(_columnSettings);
      }

      protected abstract void SetDefaultColumnSettings();

      protected GridColumnSettings AddColumnSettings(Enum enumValue)
      {
         var columnSettings = new GridColumnSettings(enumValue.ToString());
         _columnSettings.Add(columnSettings);
         return columnSettings;
      }

      public IReadOnlyCollection<GridColumnSettings> AllColumnSettings=> _columnSettings;

      public GridColumnSettings ColumnSettings(string columnName)
      {
         return _columnSettings.Contains(columnName) ? _columnSettings[columnName] : null;
      }
   }
}
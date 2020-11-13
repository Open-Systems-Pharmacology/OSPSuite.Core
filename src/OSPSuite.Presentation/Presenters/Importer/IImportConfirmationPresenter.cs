using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class DataSetSelectedEventArgs : EventArgs
   {
      public string Key { get; set; }
      public int Index { get; set; }
   }

   public class NamingConventionChangedEventArgs : EventArgs
   {
      public string NamingConvention { get; set; }
   }
   public interface IImportConfirmationPresenter : IPresenter<IImportConfirmationView>
   {
      void TriggerNamingConventionChanged(string namingConvention);
      void SetKeys(IEnumerable<string> keys);
      void SetNamingConventions(IEnumerable<string> namingConventions);
      void ImportData();
      void Refresh();
      void DataSetSelected(string key, int index);
      void PlotDataRepository(DataRepository dataRepository);
      void SetDataSetNames(IEnumerable<string> names);

      event EventHandler<EventArgs> OnImportData;

      event EventHandler<DataSetSelectedEventArgs> OnDataSetSelected;

      event EventHandler<NamingConventionChangedEventArgs> OnNamingConventionChanged;
      
   }
}

using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class DataSetSelectedEventArgs : EventArgs
   {
      public int Index { get; set; }
   }

   public class NamingConventionChangedEventArgs : EventArgs
   {
      public string NamingConvention { get; set; }
   }
   public interface IImportPreviewPresenter : IPresenter<IImportPreviewView>
   {
      void TriggerNamingConventionChanged(string namingConvention);
      //void SetSelectedNamingConvention(string namingConvention);
      void SetKeys(IReadOnlyList<string> keys);
      void SetNamingConventions(IReadOnlyList<string> namingConventions, string selectedNamingConvention);
      void ImportData();
      void DataSetSelected(int index);
      void PlotDataRepository(DataRepository dataRepository);
      void SetDataSetNames(IEnumerable<string> names);

      event EventHandler<EventArgs> OnImportData;

      event EventHandler<DataSetSelectedEventArgs> OnDataSetSelected;

      event EventHandler<NamingConventionChangedEventArgs> OnNamingConventionChanged;
      void SetViewingStateToError(string invalidExceptionMessage);
      void SetViewingStateToNormal();
   }
}

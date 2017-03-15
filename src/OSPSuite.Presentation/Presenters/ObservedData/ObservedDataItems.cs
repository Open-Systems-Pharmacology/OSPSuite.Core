using System.Collections.Generic;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public class ObservedDataItem<TObservedDataItemPresenter> : SubPresenterItem<TObservedDataItemPresenter> where TObservedDataItemPresenter : IDataRepositoryItemPresenter
   {
   }

   public static class ObservedDataItems
   {
      public static readonly ObservedDataItem<IDataRepositoryDataPresenter> Data = new ObservedDataItem<IDataRepositoryDataPresenter>();
      public static readonly ObservedDataItem<IDataRepositoryChartPresenter> Chart = new ObservedDataItem<IDataRepositoryChartPresenter>();
      public static readonly ObservedDataItem<IDataRepositoryMetaDataPresenter> MetaData = new ObservedDataItem<IDataRepositoryMetaDataPresenter>();
      public static IReadOnlyList<ISubPresenterItem> All = new List<ISubPresenterItem> { Data, Chart, MetaData };

   }
}
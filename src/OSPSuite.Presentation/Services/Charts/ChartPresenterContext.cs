using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Services.Charts
{
   public class ChartPresenterContext
   {
      public virtual IChartEditorAndDisplayPresenter ChartEditorAndDisplayPresenter { get;  }
      public virtual IQuantityPathToQuantityDisplayPathMapper QuantityDisplayPathMapper { get;  }
      public virtual IDataColumnToPathElementsMapper DataColumnToPathElementsMapper { get;  }
      public virtual IChartTemplatingTask TemplatingTask { get;  }
      public virtual IPresentationSettingsTask PresenterSettingsTask { get;  }
      public virtual IChartEditorLayoutTask EditorLayoutTask { get; }
      public virtual IProjectRetriever ProjectRetriever { get;  }
      public virtual IDimensionFactory DimensionFactory { get;  }

      public virtual IChartEditorPresenter EditorPresenter => ChartEditorAndDisplayPresenter.EditorPresenter;
      public virtual IChartDisplayPresenter DisplayPresenter => ChartEditorAndDisplayPresenter.DisplayPresenter;

      public ChartPresenterContext(IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter, IQuantityPathToQuantityDisplayPathMapper quantityDisplayPathMapper,
         IDataColumnToPathElementsMapper dataColumnToPathElementsMapper, IChartTemplatingTask chartTemplatingTask, IPresentationSettingsTask presentationSettingsTask,
         IChartEditorLayoutTask chartEditorLayoutTask, IProjectRetriever projectRetriever, IDimensionFactory dimensionFactory)
      {
         ChartEditorAndDisplayPresenter = chartEditorAndDisplayPresenter;
         QuantityDisplayPathMapper = quantityDisplayPathMapper;
         DataColumnToPathElementsMapper = dataColumnToPathElementsMapper;
         TemplatingTask = chartTemplatingTask;
         PresenterSettingsTask = presentationSettingsTask;
         EditorLayoutTask = chartEditorLayoutTask;
         ProjectRetriever = projectRetriever;
         DimensionFactory = dimensionFactory;
      }
   }
}
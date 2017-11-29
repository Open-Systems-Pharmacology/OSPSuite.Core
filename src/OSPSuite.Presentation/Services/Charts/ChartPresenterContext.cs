using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Services.Charts
{
   public class ChartPresenterContext
   {
      public virtual IChartEditorAndDisplayPresenter EditorAndDisplayPresenter { get; }
      public virtual IDataColumnToPathElementsMapper DataColumnToPathElementsMapper { get; }
      public virtual IChartTemplatingTask TemplatingTask { get; }
      public virtual IPresentationSettingsTask PresenterSettingsTask { get; }
      public virtual IChartEditorLayoutTask EditorLayoutTask { get; }
      public virtual IProjectRetriever ProjectRetriever { get; }
      public virtual IDimensionFactory DimensionFactory { get; }
      public virtual ICurveNamer CurveNamer { get; }

      public virtual IChartEditorPresenter EditorPresenter => EditorAndDisplayPresenter.EditorPresenter;
      public virtual IChartDisplayPresenter DisplayPresenter => EditorAndDisplayPresenter.DisplayPresenter;

      public ChartPresenterContext(IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter, ICurveNamer curveNamer,
         IDataColumnToPathElementsMapper dataColumnToPathElementsMapper, IChartTemplatingTask chartTemplatingTask, IPresentationSettingsTask presentationSettingsTask,
         IChartEditorLayoutTask chartEditorLayoutTask, IProjectRetriever projectRetriever, IDimensionFactory dimensionFactory)
      {
         EditorAndDisplayPresenter = chartEditorAndDisplayPresenter;
         DataColumnToPathElementsMapper = dataColumnToPathElementsMapper;
         TemplatingTask = chartTemplatingTask;
         PresenterSettingsTask = presentationSettingsTask;
         EditorLayoutTask = chartEditorLayoutTask;
         ProjectRetriever = projectRetriever;
         DimensionFactory = dimensionFactory;
         CurveNamer = curveNamer;
      }

      public void Refresh()
      {
         EditorPresenter.Refresh();
         DisplayPresenter.Refresh();
      }

      public void NotifyProjectChanged()
      {
         ProjectRetriever.CurrentProject.HasChanged = true;
      }

      public void Clear()
      {
         EditorPresenter.Clear();
         DisplayPresenter.Clear();
      }
   }
}
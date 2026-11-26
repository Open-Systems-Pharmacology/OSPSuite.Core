using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ICopyChartTemplatesPresenter : IDisposablePresenter
   {
      /// <summary>
      ///    Lets the user select templates owned by the current project or by any other object of the project owning chart
      ///    templates (e.g. simulations), except <paramref name="templateOwner" />. Only templates created for charts of
      ///    type <paramref name="chartType" /> are offered, or all templates if <paramref name="chartType" /> is
      ///    <c>null</c>.
      /// </summary>
      /// <returns>The selected templates (not copied) or an empty list if the user canceled the selection</returns>
      IReadOnlyList<CurveChartTemplate> SelectTemplatesToCopy(IWithChartTemplates templateOwner, CurveChartTypes? chartType);

      /// <summary>
      ///    Shows the templates of the <paramref name="source" />. Templates selected in other sources remain selected
      /// </summary>
      void SelectSource(ChartTemplateSourceDTO source);
   }

   public class CopyChartTemplatesPresenter : AbstractDisposablePresenter<ICopyChartTemplatesView, ICopyChartTemplatesPresenter>, ICopyChartTemplatesPresenter
   {
      private readonly IProjectRetriever _projectRetriever;
      private readonly ILazyLoadTask _lazyLoadTask;
      private readonly Dictionary<ChartTemplateSourceDTO, IReadOnlyList<ChartTemplateSelectionDTO>> _templatesBySource = new Dictionary<ChartTemplateSourceDTO, IReadOnlyList<ChartTemplateSelectionDTO>>();
      private CurveChartTypes? _chartType;

      public CopyChartTemplatesPresenter(ICopyChartTemplatesView view, IProjectRetriever projectRetriever, ILazyLoadTask lazyLoadTask) : base(view)
      {
         _projectRetriever = projectRetriever;
         _lazyLoadTask = lazyLoadTask;
      }

      public IReadOnlyList<CurveChartTemplate> SelectTemplatesToCopy(IWithChartTemplates templateOwner, CurveChartTypes? chartType)
      {
         _chartType = chartType;
         _templatesBySource.Clear();
         _view.BindToSources(allSourcesExcept(templateOwner));
         _view.OkEnabled = CanClose;
         _view.Display();

         if (_view.Canceled)
            return new List<CurveChartTemplate>();

         return selectedTemplates.ToList();
      }

      public void SelectSource(ChartTemplateSourceDTO source)
      {
         _view.BindToTemplates(source == null ? new List<ChartTemplateSelectionDTO>() : templatesFor(source));
      }

      private IReadOnlyList<ChartTemplateSelectionDTO> templatesFor(ChartTemplateSourceDTO source)
      {
         if (_templatesBySource.TryGetValue(source, out var templates))
            return templates;

         // Simulations might not be loaded yet, in which case their templates are not available
         _lazyLoadTask.Load(source.Source as ILazyLoadable);

         templates = source.Source.ChartTemplates
            .Where(matchesChartType)
            .OrderBy(x => x.Name)
            .Select(x => new ChartTemplateSelectionDTO(x))
            .ToList();

         _templatesBySource[source] = templates;
         return templates;
      }

      private bool matchesChartType(CurveChartTemplate template) => _chartType == null || template.ChartType == _chartType.Value;

      private IReadOnlyList<ChartTemplateSourceDTO> allSourcesExcept(IWithChartTemplates templateOwner)
      {
         var project = _projectRetriever.CurrentProject;
         if (project == null)
            return new List<ChartTemplateSourceDTO>();

         var projectSource = new ChartTemplateSourceDTO(project, Captions.Project);
         var otherSources = project.All<IWithChartTemplates>()
            .Select(x => new ChartTemplateSourceDTO(x, (x as IWithName)?.Name))
            .OrderBy(x => x.DisplayName);

         return new[] {projectSource}.Concat(otherSources)
            .Where(x => !Equals(x.Source, templateOwner))
            .ToList();
      }

      private IEnumerable<CurveChartTemplate> selectedTemplates => _templatesBySource.Values.SelectMany(x => x).Where(x => x.Selected).Select(x => x.Template);

      public override void ViewChanged()
      {
         base.ViewChanged();
         _view.OkEnabled = CanClose;
      }

      public override bool CanClose => base.CanClose && selectedTemplates.Any();
   }
}

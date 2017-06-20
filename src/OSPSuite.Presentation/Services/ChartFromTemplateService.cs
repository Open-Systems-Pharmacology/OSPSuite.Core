using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Services
{
   public class ChartFromTemplateService : IChartFromTemplateService
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IKeyPathMapper _keyPathMapper;
      private readonly IDialogCreator _dialogCreator;
      private readonly ICloneManager _cloneManager;
      private readonly IChartUpdater _chartUpdater;
      private List<ColumnPath> _allColumnsByPath;
      private Func<DataColumn, string> _curveNameDefinition;

      public ChartFromTemplateService(IDimensionFactory dimensionFactory, IKeyPathMapper keyPathMapper, IDialogCreator dialogCreator, ICloneManager cloneManager, IChartUpdater chartUpdater)
      {
         _dimensionFactory = dimensionFactory;
         _keyPathMapper = keyPathMapper;
         _dialogCreator = dialogCreator;
         _cloneManager = cloneManager;
         _chartUpdater = chartUpdater;
      }

      public void InitializeChartFromTemplate(CurveChart chart, IEnumerable<DataColumn> dataColumns, CurveChartTemplate template, 
         Func<DataColumn, string> curveNameDefinition = null, 
         bool warnIfNumberOfCurvesAboveThreshold = false,
         bool propogateChartChangeEvent = true,
         int warningThreshold = Constants.DEFAULT_TEMPLATE_WARNING_THRESHOLD)
      {
         try
         {
            _allColumnsByPath = dataColumns.Select(x => new ColumnPath(x)).ToList();
            _curveNameDefinition = curveNameDefinition ?? (x => x.Name);

            //Retrieve all possible curves for each curve template defined in the given template 
            var curvesForTemplates = new Cache<CurveTemplate, TemplateToColumnsMatch>(x => x.CurveTemplate);
            template.Curves.Each(curveTemplate => curvesForTemplates.Add(allPossibleCurvesForTemplate(curveTemplate)));

            var bestTemplateForCurves = findBestTemplateForCurves(curvesForTemplates);

            //Ensure that the user wants to continue if the threshold is exceeded
            var numberOfCreatedColumns = bestTemplateForCurves.Sum(x => x.Count);
            if (numberOfCreatedColumns > warningThreshold && warnIfNumberOfCurvesAboveThreshold)
            {
               var shouldContinue = _dialogCreator.MessageBoxYesNo(Captions.NumberOfSelectedCurveWouldGoOverThreshold(numberOfCreatedColumns));
               if (shouldContinue == ViewResult.No)
                  return;
            }

            //Last but not least, update the chart
            updateChartFromTemplateWithMatchingCurves(chart, template, bestTemplateForCurves, propogateChartChangeEvent);
         }
         finally
         {
            _curveNameDefinition = null;
            _allColumnsByPath.Clear();
         }
      }

      private void updateChartFromTemplateWithMatchingCurves(CurveChart chart, CurveChartTemplate template, ICache<CurveTemplate, IReadOnlyList<ColumnMap>> bestTemplateForCurves, bool propogateChartChangeEvent)
      {
         using (_chartUpdater.UpdateTransaction(chart, propogateChartChangeEvent))
         {
            chart.Clear();
            chart.CopyChartSettingsFrom(template);
            chart.FontAndSize.UpdatePropertiesFrom(template.FontAndSize, _cloneManager);
            template.Axes.Each(axis => chart.AddAxis(axis.Clone()));
            bestTemplateForCurves.KeyValues.Each(kv => addCurvesToChart(kv.Key, kv.Value, chart));
         }
      }

      private ICache<CurveTemplate, IReadOnlyList<ColumnMap>> findBestTemplateForCurves(ICache<CurveTemplate, TemplateToColumnsMatch> curvesForTemplates)
      {
         var prunedCache = new Cache<CurveTemplate, IReadOnlyList<ColumnMap>>();
         var columnsByBestTemplate = new Cache<ColumnMap, CurveTemplate>();
         var allTemplatesByColumns = allPossibleTemplatesByColumn(curvesForTemplates);

         foreach (var templateByColumn in allTemplatesByColumns.GroupBy(x => x.ColumnMap))
         {
            columnsByBestTemplate[templateByColumn.Key] = bestTemplateForColumn(templateByColumn.Key, templateByColumn, columnsByBestTemplate);
         }

         foreach (var allColumnsForTemplate in columnsByBestTemplate.KeyValues.GroupBy(x => x.Value))
         {
            prunedCache.Add(allColumnsForTemplate.Key, allColumnsForTemplate.Select(x => x.Key).ToList());
         }

         return prunedCache;
      }

      private CurveTemplate bestTemplateForColumn(ColumnMap columnMap, IEnumerable<TemplateColumnMatch> templateByColumn, IReadOnlyCollection<CurveTemplate> alreadyMappedTemplates)
      {
         var allTemplates = templateByColumn.ToList();
         var templateColumnMatch = allTemplates.First();
         var firstTemplate = templateColumnMatch.CurveTemplate;
         if (allTemplates.Count == 1)
            return firstTemplate;

         //find the one with exact matching
         var allTemplateExactMatching = allTemplates.Where(x => x.ColumnMap.MatchType == MatchType.Exact).Select(x => x.CurveTemplate).ToList();

         //Only one exact match or none at all. return
         if (allTemplateExactMatching.Count <= 1)
            return allTemplateExactMatching.FirstOrDefault() ?? firstTemplate;

         //more than one exact matching. Check if we can find a matching repo name
         var repoName = columnMap.YColumn.Repository?.Name;
         var allTemplateExactMatchingWithRepo = allTemplateExactMatching.Where(x => string.Equals(x.yData.RepositoryName, repoName)).ToList();
         if (allTemplateExactMatchingWithRepo.Any())
            return allTemplateExactMatchingWithRepo[0];

         return firstUnusedOrLast(allTemplateExactMatching, alreadyMappedTemplates);
      }

      private CurveTemplate firstUnusedOrLast(IReadOnlyList<CurveTemplate> availableTemplates, IReadOnlyCollection<CurveTemplate> templatesAlreadyUsed)
      {
         return availableTemplates.FirstOrDefault(x => !templatesAlreadyUsed.Contains(x)) ?? availableTemplates.Last();
      }

      private IEnumerable<TemplateColumnMatch> allPossibleTemplatesByColumn(IEnumerable<TemplateToColumnsMatch> curvesForTemplates)
      {
         return from curvesForTemplate in curvesForTemplates
            from columnMap in curvesForTemplate.ColumnMaps
            select new TemplateColumnMatch(curvesForTemplate.CurveTemplate, columnMap);
      }

      private TemplateToColumnsMatch allPossibleCurvesForTemplate(CurveTemplate curveTemplate)
      {
         var allColumns = retrieveAllColumnsExactlyMatching(curveTemplate);
         return allColumns.Any()
            ? new TemplateToColumnsMatch(curveTemplate, allColumns)
            : new TemplateToColumnsMatch(curveTemplate, retriveAllColumnsPatternMatching(curveTemplate));
      }

      private void addCurvesToChart(CurveTemplate templateCurve, IReadOnlyList<ColumnMap> columnsToAdd, CurveChart chart)
      {
         foreach (var columnMap in columnsToAdd)
         {
            var curve = chart.CreateCurve(columnMap.XColumn, columnMap.YColumn, templateCurve.Name, _dimensionFactory);
            curve.CurveOptions.UpdateFrom(templateCurve.CurveOptions);
            chart.AddCurve(curve, useAxisDefault: false);
            curve.Name = bestPossibleCurveName(columnsToAdd, columnMap, templateCurve.Name);
         }
      }

      private string bestPossibleCurveName(IReadOnlyList<ColumnMap> columnsToAdd, ColumnMap columnMap, string templateName)
      {
         //more than one column for a given template curve, use default templating name instead of predefined curves
         if (columnsToAdd.Count > 1)
            return _curveNameDefinition(columnMap.YColumn);

         //only one column to add: Don't rename for an exact pattern match of if we could not retrieve the name of the molecule dynamically
         if (columnMap.MatchType == MatchType.Exact || string.IsNullOrEmpty(columnMap.TemplateMoleculeName))
            return templateName;

         var moleculeName = _keyPathMapper.MoleculeNameFrom(columnMap.YColumn);
         if (string.IsNullOrEmpty(moleculeName))
            return templateName;

         return templateName.Replace(columnMap.TemplateMoleculeName, moleculeName);
      }

      private IReadOnlyList<ColumnMap> retriveAllColumnsPatternMatching(CurveTemplate curveTemplate)
      {
         return retrieveAllColumnsMatching(curveTemplate, columnsMatchingPattern);
      }

      private IReadOnlyList<ColumnMap> retrieveAllColumnsExactlyMatching(CurveTemplate curveTemplate)
      {
         return retrieveAllColumnsMatching(curveTemplate, columnsByPath);
      }

      private IReadOnlyList<ColumnMap> retrieveAllColumnsMatching(CurveTemplate curveTemplate, Func<CurveDataTemplate, IReadOnlyList<ColumnPathMolecule>> matchingStrategy)
      {
         var notFound = new List<ColumnMap>();

         var allYMatching = matchingStrategy(curveTemplate.yData);

         //Y columns found using the base grid. Returns them all!
         if (curveTemplate.IsBaseGrid)
            return allYMatching.Select(x => x.ToColumnMap()).ToList();

         var allXMatching = matchingStrategy(curveTemplate.xData);

         //only returns this columns if there is one x matching (otherwise how do we deal with it?)
         return allXMatching.Count == 1
            ? allYMatching.Select(x => x.ToColumnMap(allXMatching[0].ColumnPath.Column)).ToList()
            : notFound;
      }

      private IReadOnlyList<ColumnPathMolecule> columnsByPath(CurveDataTemplate dataTemplate)
      {
         Func<ColumnPath, bool> match = x => string.Equals(x.Path, dataTemplate.Path) &&
                                             x.QuantityType.Is(dataTemplate.QuantityType);

         return _allColumnsByPath.Where(match).Select(x => x.ToColumnPathMolecule(MatchType.Exact)).ToList();
      }

      private IReadOnlyList<ColumnPathMolecule> columnsMatchingPattern(CurveDataTemplate dataTemplate)
      {
         var pattern = _keyPathMapper.MapFrom(dataTemplate.Path, dataTemplate.QuantityType, removeFirstEntry: false);
         Func<DataColumn, bool> match = c => string.Equals(_keyPathMapper.MapFrom(c), pattern.Path) &&
                                             c.QuantityInfo.Type.Is(dataTemplate.QuantityType);

         return _allColumnsByPath.Where(x => match(x.Column)).Select(x => x.ToColumnPathMolecule(MatchType.Pattern, pattern.Molecule)).ToList();
      }

      private class ColumnPath
      {
         public DataColumn Column { get; }
         public string Path { get; }

         public ColumnPath(DataColumn column)
         {
            Column = column;
            Path = column.TemplatePath;
         }

         public QuantityType QuantityType => Column.QuantityInfo.Type;

         public ColumnPathMolecule ToColumnPathMolecule(MatchType matchType, string templateMoleculeName = null)
         {
            return new ColumnPathMolecule(this, templateMoleculeName, matchType);
         }
      }

      private class ColumnPathMolecule
      {
         public ColumnPath ColumnPath { get; }
         private readonly string _templateMoleculeName;
         private readonly MatchType _matchType;

         public ColumnPathMolecule(ColumnPath columnPath, string templateMoleculeName, MatchType matchType)
         {
            _matchType = matchType;
            ColumnPath = columnPath;
            _templateMoleculeName = templateMoleculeName;
         }

         public ColumnMap ToColumnMap(DataColumn xColumn = null)
         {
            return new ColumnMap(xColumn ?? ColumnPath.Column.BaseGrid, ColumnPath.Column, _templateMoleculeName, _matchType);
         }
      }

      private class ColumnMap
      {
         public DataColumn XColumn { get; }
         public DataColumn YColumn { get; }
         public string TemplateMoleculeName { get; }
         public MatchType MatchType { get; }

         public ColumnMap(DataColumn xColumn, DataColumn yColumn, string templateMoleculeName, MatchType matchType)
         {
            XColumn = xColumn;
            YColumn = yColumn;
            TemplateMoleculeName = templateMoleculeName;
            MatchType = matchType;
         }

         public override bool Equals(object obj)
         {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ColumnMap) obj);
         }

         public bool Equals(ColumnMap other)
         {
            return Equals(XColumn, other.XColumn) && Equals(YColumn, other.YColumn);
         }

         public override int GetHashCode()
         {
            unchecked
            {
               return ((XColumn?.GetHashCode() ?? 0) * 397) ^ (YColumn?.GetHashCode() ?? 0);
            }
         }
      }

      /// <summary>
      ///    Possible match for a given column and a template
      /// </summary>
      private class TemplateColumnMatch
      {
         public CurveTemplate CurveTemplate { get; }
         public ColumnMap ColumnMap { get; }

         public TemplateColumnMatch(CurveTemplate curveTemplate, ColumnMap columnMap)
         {
            CurveTemplate = curveTemplate;
            ColumnMap = columnMap;
         }
      }

      /// <summary>
      ///    All columns potentially matching a given template
      /// </summary>
      private class TemplateToColumnsMatch
      {
         public CurveTemplate CurveTemplate { get; }
         public IReadOnlyList<ColumnMap> ColumnMaps { get; }

         public TemplateToColumnsMatch(CurveTemplate curveTemplate, IReadOnlyList<ColumnMap> columnMaps)
         {
            CurveTemplate = curveTemplate;
            ColumnMaps = columnMaps;
         }
      }

      private enum MatchType
      {
         Exact,
         Pattern
      }
   }
}
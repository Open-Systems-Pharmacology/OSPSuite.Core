using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IModelReportCreator
   {
      /// <summary>
      ///    Generates a structural report as for the given model (DUMP).
      /// </summary>
      /// <param name="model">Model that should be reported</param>
      /// <param name="reportFormulaReferences">Should the references defined in the formula be generated?</param>
      /// <param name="reportFormulaObjectPaths">Should the object paths used by the formula be generated?</param>
      /// <param name="reportDescriptions">Should the description of object base be generated?</param>
      string ModelReport(IModel model, bool reportFormulaReferences = false, bool reportFormulaObjectPaths = false, bool reportDescriptions = false);

      /// <summary>
      ///    Generates a structural report for the given container (DUMP).
      /// </summary>
      string ContainerReport(IContainer container, bool reportFormulaReferences = false, bool reportFormulaObjectPaths = false, bool reportDescriptions = false);
   }

   public class ModelReportCreator : IModelReportCreator
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private bool _reportFormulaReferences;
      private bool _reportFormulaObjectPaths;
      private StringBuilder _report;
      private IList<IEntity> _allChildren;
      private bool _reportDescriptions;

      public ModelReportCreator(IObjectPathFactory objectPathFactory)
      {
         _objectPathFactory = objectPathFactory;
         _reportFormulaReferences = false;
         _reportFormulaObjectPaths = false;
         _reportDescriptions = false;
      }

      public string ContainerReport(IContainer container, bool reportFormulaReferences = false, bool reportFormulaObjectPaths = false, bool reportDescriptions = false)
      {
         _reportFormulaReferences = reportFormulaReferences;
         _reportFormulaObjectPaths = reportFormulaObjectPaths;
         _reportDescriptions = reportDescriptions;
         return containerReport(container);
      }

      public string ModelReport(IModel model, bool reportFormulaReferences = false, bool reportFormulaObjectPaths = false, bool reportDescriptions = false)
      {
         if (model.Root == null)
            return string.Empty;

         _reportFormulaReferences = reportFormulaReferences;
         _reportFormulaObjectPaths = reportFormulaObjectPaths;
         _reportDescriptions = reportDescriptions;


         return containerReport(model.Root, model);
      }

      private string containerReport(IContainer container, IModel model = null)
      {
         _report = new StringBuilder();
         _allChildren = container.GetAllChildren<IEntity>().ToList();

         try
         {
            transportsReport();
            reactionsReport();
            eventsReport();
            parametersReport();
            observersReport();
            neighborhoodsReportFor(model);
            containersReport();
            moleculesStartValuesReport();
            return _report.ToString();
         }
         finally
         {
            _allChildren.Clear();
         }
      }

      private void reportFor(IContainer container)
      {
         if (container.IsAnImplementationOf<INeighborhood>())
            return;
         if (container.IsAnImplementationOf<ITransport>())
            return;
         if (container.IsAnImplementationOf<IReaction>())
            return;
         if (container.IsAnImplementationOf<IParameter>())
            return;
         if (container.IsAnImplementationOf<IMoleculeAmount>())
            return;

         if (container.Name.Equals(Constants.MOLECULE_PROPERTIES) || container.ParentContainer.Name.Equals(Constants.MOLECULE_PROPERTIES))
            return;

         _report.AppendFormat("Container: {0}", _objectPathFactory.CreateAbsoluteObjectPath(container));
         _report.AppendLine();
         _report.AppendFormat("\tType: {0}", container.ContainerType);
         _report.AppendLine();
         _report.AppendFormat("\tMode: {0}", container.Mode);
         _report.AppendLine();
         _report.AppendFormat("\tTags: ");

         bool firstEntry = true;

         foreach (var tag in container.Tags)
         {
            if (firstEntry)
               firstEntry = false;
            else
               _report.Append("|");

            _report.Append(tag.Value);
         }

         _report.AppendLine();
         reportDescription(container);
         _report.AppendLine();
      }

      private void reportFor(INeighborhood neighborhood)
      {
         _report.AppendFormat("Neighborhood: {0}", neighborhood.Name);
         _report.AppendLine();

         _report.AppendFormat("\t1st neighbor: {0}", _objectPathFactory.CreateAbsoluteObjectPath(neighborhood.FirstNeighbor));
         _report.AppendLine();

         _report.AppendFormat("\t2nd neighbor: {0}", _objectPathFactory.CreateAbsoluteObjectPath(neighborhood.SecondNeighbor));
         _report.AppendLine();
         reportDescription(neighborhood);
         _report.AppendLine();
      }

      private void reportFor(IMoleculeAmount moleculeAmount)
      {
         if (moleculeAmount.Formula.IsAnImplementationOf<ConstantFormula>() && (moleculeAmount.Value == 0))
            return;

         _report.AppendFormat("Molecule: {0}", _objectPathFactory.CreateAbsoluteObjectPath(moleculeAmount));
         _report.AppendLine();
         _report.AppendFormat("\tValue: {0}{1}", moleculeAmount.Value, getUnit(moleculeAmount.Dimension));
         _report.AppendLine();
         reportFor(moleculeAmount.Formula);
         reportDescription(moleculeAmount);
         _report.AppendLine();
      }

      private void reportFor(IObserver observer)
      {
         _report.AppendFormat("Observer: {0}", observer.Name);
         _report.AppendLine();
         _report.AppendFormat("\tPath: {0}", _objectPathFactory.CreateAbsoluteObjectPath(observer));
         _report.AppendLine();
         reportFor(observer.Formula);
         reportDescription(observer);
         _report.AppendLine();
      }

      private void reportFor(IParameter parameter)
      {
         if (parameter.Name.Equals(Constants.Distribution.MEAN) ||
             parameter.Name.Equals(Constants.Distribution.DEVIATION) ||
             parameter.Name.Equals(Constants.Distribution.GEOMETRIC_DEVIATION) ||
             parameter.Name.Equals(Constants.Distribution.PERCENTILE))
            return;

         _report.AppendFormat("Parameter: {0}", _objectPathFactory.CreateAbsoluteObjectPath(parameter));
         _report.AppendLine();
         _report.AppendFormat("\tIsFixed: {0}", parameter.IsFixedValue);
         _report.AppendLine();
         _report.AppendFormat("\tValue: {0}{1}", parameter.Value, getUnit(parameter.Dimension));
         _report.AppendLine();


         if (parameter.IsAnImplementationOf<IDistributedParameter>())
         {
            var distributedParameter = parameter.DowncastTo<IDistributedParameter>();
            _report.AppendFormat("\tPercentile: {0}", distributedParameter.Percentile);
            _report.AppendLine();
         }
         else
         {
            reportFor(parameter.Formula, 1);
            if (parameter.RHSFormula != null)
               reportFor(parameter.RHSFormula, "RHS_Formula", 1);
         }

         if (parameter.Tags.Any())
            _report.AppendFormat("\tTags: {0}", parameter.Tags.Select(x => x.Value).ToString(", "));

         reportDescription(parameter);
         _report.AppendLine();
      }

      private void reportFor(IEvent modelEvent)
      {
         _report.AppendFormat("Event: {0}", modelEvent.Name);
         _report.AppendLine();
         _report.AppendFormat("\tPath: {0}", _objectPathFactory.CreateAbsoluteObjectPath(modelEvent));
         _report.AppendLine();
         reportFor(modelEvent.Formula, "Condition");
         _report.AppendLine("\tAssignments:");
         foreach (var assignment in modelEvent.Assignments)
         {
            _report.AppendFormat("\t\tPath: {0}", _objectPathFactory.CreateAbsoluteObjectPath(assignment.ChangedEntity));
            _report.AppendLine();
            _report.AppendFormat("\t\tUseAsValue: {0}", assignment.UseAsValue);
            _report.AppendLine();
            reportFor(assignment.Formula, 3);
         }

         reportDescription(modelEvent);
         _report.AppendLine();
      }

      private void reportFor(IReaction reaction)
      {
         _report.AppendFormat("Reaction: {0}", reaction.Name);
         _report.AppendLine();
         _report.AppendLine("\tEducts:");
         foreach (var reactionPartner in reaction.Educts)
         {
            _report.AppendFormat("\t\t{0} * {1}", reactionPartner.StoichiometricCoefficient,
               _objectPathFactory.CreateAbsoluteObjectPath(reactionPartner.Partner));
            _report.AppendLine();
         }

         _report.AppendLine("\tProducts:");
         foreach (var reactionPartner in reaction.Products)
         {
            _report.AppendFormat("\t\t{0} * {1}", reactionPartner.StoichiometricCoefficient,
               _objectPathFactory.CreateAbsoluteObjectPath(reactionPartner.Partner));
            _report.AppendLine();
         }

         reportFor(reaction.Formula);
         reportDescription(reaction);
         _report.AppendLine();
      }

      private void reportFor(ITransport transport)
      {
         _report.AppendFormat("Transport: {0}", transport.Name);
         _report.AppendLine();
         _report.AppendFormat("\tSource: {0}", _objectPathFactory.CreateAbsoluteObjectPath(transport.SourceAmount));
         _report.AppendLine();
         _report.AppendFormat("\tTarget: {0}", _objectPathFactory.CreateAbsoluteObjectPath(transport.TargetAmount));
         _report.AppendLine();

         reportFor(transport.Formula);
         reportDescription(transport);
         _report.AppendLine();
      }

      private void reportFor(IFormula formula, string caption, int noOfTabs)
      {
         string formulaString = string.Empty;

         if (formula.IsAnImplementationOf<ExplicitFormula>())
            formulaString = formula.DowncastTo<ExplicitFormula>().FormulaString;

         if (formula.IsAnImplementationOf<ConstantFormula>())
            formulaString = formula.DowncastTo<ConstantFormula>().Value.ToString();

         if (formula.IsAnImplementationOf<TableFormula>())
            formulaString = "<TABLE>";

         _report.AppendFormat("{0}{1}: {2}", tabs(noOfTabs), caption, formulaString);
         _report.AppendLine();

         if (formula.IsAnImplementationOf<TableFormula>())
            reportFor(formula.DowncastTo<TableFormula>(), noOfTabs + 1);

         if (_reportFormulaReferences)
            reportFormulaReferences(formula, noOfTabs + 1);

         if (_reportFormulaObjectPaths)
            reportFormulaObjectPaths(formula, noOfTabs + 1);
      }

      private void reportFor(TableFormula tableFormula, int noOfTabs)
      {
         foreach (var valuePoint in tableFormula.AllPoints())
         {
            _report.AppendFormat("{0}{1}; {2}   RestartSolver = {3}", tabs(noOfTabs), valuePoint.X, valuePoint.Y, valuePoint.RestartSolver);
            _report.AppendLine();
         }
      }

      private void reportFor(IFormula formula, string caption)
      {
         reportFor(formula, caption, 1);
      }

      private void reportFor(IFormula formula, int noOfTabs)
      {
         reportFor(formula, "Formula", noOfTabs);
      }

      private void reportFor(IFormula formula)
      {
         reportFor(formula, "Formula", 1);
      }

      private void addSeparatorFor(string name)
      {
         _report.AppendFormat("--------------- {0} ----------", name);
         _report.AppendLine();
      }

      private void reportDescription(IObjectBase objectBase)
      {
         if (!_reportDescriptions) return;
         if (objectBase.Description.IsNullOrEmpty()) return;

         _report.AppendFormat("\tDescription: {0}", objectBase.Description);
         _report.AppendLine();
      }

      private void reportFormulaReferences(IFormula formula, int noOfTabs)
      {
         foreach (var formulaRef in formula.ObjectReferences)
         {
            string unit = getUnit(formulaRef.Object.Dimension);
            _report.AppendFormat("{0}{1}: {2}({3}{4})", tabs(noOfTabs), formulaRef.Alias,
               _objectPathFactory.CreateAbsoluteObjectPath(formulaRef.Object), formulaRef.Object.Value, unit);

            _report.AppendLine();
         }
      }

      private static string getUnit(IDimension dimension)
      {
         if (dimension == null)
            return " DIMENSION IS NULL!";

         string unit = dimension.BaseUnit.Name;
         if (!string.IsNullOrEmpty(unit))
            unit = $" [{unit}]";

         return unit;
      }

      private void reportFormulaObjectPaths(IFormula formula, int noOfTabs)
      {
         foreach (var path in formula.ObjectPaths)
         {
            _report.AppendFormat("{0}{1}: {2}", tabs(noOfTabs), path.Alias, path);
            _report.AppendLine();
         }
      }

      private static string tabs(int noOfTabs)
      {
         string tabs = "";

         for (int i = 1; i <= noOfTabs; i++)
            tabs += "\t";

         return tabs;
      }

      private void containersReport()
      {
         createEntityReport<IContainer>("CONTAINERS", reportFor);
      }

      private void neighborhoodsReportFor(IModel model)
      {
         if (model == null) return;
         addSeparatorFor("NEIGHBORHOODS");

         foreach (var neighborhood in model.Neighborhoods.GetAllChildren<INeighborhood>())
         {
            reportFor(neighborhood);
         }
      }

      private void moleculesStartValuesReport()
      {
         createEntityReport<IMoleculeAmount>("MOLECULE_START_FORMULAS (Non zero only)", reportFor);
      }

      private void transportsReport()
      {
         createEntityReport<ITransport>("TRANSPORTS", reportFor);
      }

      private void observersReport()
      {
         createEntityReport<IObserver>("OBSERVER (Non zero only)", reportFor);
      }

      private void parametersReport()
      {
         createEntityReport<IParameter>("PARAMETER", reportFor);
      }

      private void eventsReport()
      {
         createEntityReport<IEvent>("SWITCHES", reportFor);
      }

      private void reactionsReport()
      {
         createEntityReport<IReaction>("REACTIONS", reportFor);
      }

      private void createEntityReport<T>(string separatorName, Action<T> reportFor)
      {
         var allItems = all<T>().ToList();
         if (!allItems.Any()) return;

         addSeparatorFor(separatorName);
         foreach (var itemToReport in allItems)
         {
            reportFor(itemToReport);
         }
      }

      private IEnumerable<T> all<T>()
      {
         return from c in _allChildren
            where c.IsAnImplementationOf<T>()
            select c.DowncastTo<T>();
      }
   }
}
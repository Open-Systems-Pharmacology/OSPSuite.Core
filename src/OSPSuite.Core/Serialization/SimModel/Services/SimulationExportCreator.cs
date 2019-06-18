using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Serialization.SimModel.Services
{
   public interface ISimulationExportCreator
   {
      /// <summary>
      ///    Create the sim model export model using the Full mode
      /// </summary>
      SimulationExport CreateExportFor(IModel model);

      /// <summary>
      ///    Create the sim model export model using the export mode given as parameter
      /// </summary>
      SimulationExport CreateExportFor(IModel model, SimModelExportMode exportMode);
   }

   public class SimulationExportCreator :
      IVisitor<IReaction>,
      IVisitor<IObserver>,
      IVisitor<IParameter>,
      IVisitor<IMoleculeAmount>,
      IVisitor<ITransport>,
      IVisitor<IEvent>,
      ISimulationExportCreator
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private ICache<string, int> _idMap;
      private SimulationExport _modelExport;
      private SimModelExportMode _exportMode;
      private readonly ITableFormulaToTableFormulaExportMapper _tableFormulaExportMapper;
      private readonly IConcentrationBasedFormulaUpdater _concentrationBasedFormulaUpdater;
      private IReadOnlyList<IProcess> _allProcesses;

      public SimulationExportCreator(
         IObjectPathFactory objectPathFactory, 
         ITableFormulaToTableFormulaExportMapper tableFormulaExportMapper,
         IConcentrationBasedFormulaUpdater concentrationBasedFormulaUpdater)
      {
         _objectPathFactory = objectPathFactory;
         _tableFormulaExportMapper = tableFormulaExportMapper;
         _concentrationBasedFormulaUpdater = concentrationBasedFormulaUpdater;
      }

      public SimulationExport CreateExportFor(IModel model)
      {
         return CreateExportFor(model, SimModelExportMode.Full);
      }

      public SimulationExport CreateExportFor(IModel model, SimModelExportMode exportMode)
      {
         try
         {
            _modelExport = new SimulationExport();
            _idMap = new Cache<string, int> {{Constants.TIME, 0}};
            _exportMode = exportMode;
            _allProcesses = model.Root.GetAllChildren<IProcess>();
            _modelExport.ObjectPathDelimiter = ObjectPath.PATH_DELIMITER;
            model.Root.AcceptVisitor(this);
            return _modelExport;
         }

         finally
         {
            _allProcesses = null;
            _modelExport = null;
         }
      }

      public void Visit(IObserver observer)
      {
         var observerExport = new QuantityExport();
         mapQuantity(observer, observerExport);
         _modelExport.ObserverList.Add(observerExport);
      }

      public void Visit(IParameter parameter)
      {
         //SubParameter of DistributedParameter are not needed in SimModel(distributed Values are Mapped to constant for SimModel)
         if (parameter.ParentContainer.IsAnImplementationOf<IDistributedParameter>()) return;
         if (parameter.RHSFormula != null)
         {
            createVariableExport(parameter, 1.0);
            createRHSForParameter(parameter);
         }
         else
         {
            createParameterExport(parameter);
         }
      }

      private void createRHSForParameter(IParameter parameter)
      {
         createRHS(parameter, parameter, parameter.RHSFormula, mapFormula);
      }

      private void createRHSforAmount(IProcess process, IMoleculeAmount moleculeAmount, double stoichiometricCoefficient)
      {
         createVariableExport(moleculeAmount);

         var rhsExport = createProcessRHS(moleculeAmount, process);
         if (stoichiometricCoefficient == 1.0)
            return;

         rhsExport.Equation = stoichiometricCoefficient == -1.0 ? $"-({rhsExport.Equation})" : $"{stoichiometricCoefficient}*({rhsExport.Equation})";
      }

      private ExplicitFormulaExport createProcessRHS(IMoleculeAmount moleculeAmount, IProcess process)
      {
         return createRHS(moleculeAmount, process, process.Formula, (p, f) => mapProcessFormula(process));
      }

      private TFormulaExport createRHS<TFormulaExport>(IQuantity quantity, IUsingFormula usingFormula, IFormula formula, Func<IUsingFormula, IFormula, TFormulaExport> formulaMapper)
         where TFormulaExport : FormulaExport
      {
         var variableExport = _modelExport.VariableList[idFor(quantity)];
         var rhsExport = formulaMapper(usingFormula, formula);
         variableExport.RHSIds.Add(rhsExport.Id);
         return rhsExport;
      }

      private void createParameterExport(IParameter parameter)
      {
         createParameterExport(parameter, parameter.CanBeVaried);
      }

      private void createParameterExport(IQuantity quantity, bool canBeVaried)
      {
         if (isSystemParameter(quantity))
            return;

         var paraExport = new ParameterExport();
         mapQuantity(quantity, paraExport);
         paraExport.CanBeVaried = canBeVaried;
         _modelExport.ParameterList.Add(paraExport.Id, paraExport);
      }

      public void Visit(IMoleculeAmount moleculeAmount)
      {
         if (canBeExportedAsParameter(moleculeAmount))
            createParameterExport(moleculeAmount, canBeVaried: false);
         else
            createVariableExport(moleculeAmount);
      }

      private bool canBeExportedAsParameter(IMoleculeAmount moleculeAmount)
      {
         if (isSystemVariable(moleculeAmount))
            return false;

         return !_allProcesses.Any(x => x.Uses(moleculeAmount));
      }

      private void createVariableExport(IMoleculeAmount moleculeAmount)
      {
         createVariableExport(moleculeAmount, moleculeAmount.ScaleDivisor);
      }

      private void createVariableExport(IQuantity quantity, double scaleFactor)
      {
         if (isSystemVariable(quantity))
            return;

         var variableExport = new VariableExport();
         mapQuantity(quantity, variableExport);
         variableExport.ScaleFactor = scaleFactor;
         variableExport.NegativeValuesAllowed = quantity.NegativeValuesAllowed;
         _modelExport.VariableList.Add(variableExport.Id, variableExport);
      }

      private bool isSystemParameter(IObjectBase objectBase)
      {
         if (objectBase == null) return false;
         return _modelExport.ParameterList.Contains(idFor(objectBase));
      }

      private bool isSystemVariable(IObjectBase objectBase)
      {
         if (objectBase == null) return false;
         return _modelExport.VariableList.Contains(idFor(objectBase));
      }

      public void Visit(ITransport transport)
      {
         createRHSforAmount(transport, transport.SourceAmount, -1);
         createRHSforAmount(transport, transport.TargetAmount, 1);
      }

      public void Visit(IReaction reaction)
      {
         foreach (var reactionPartner in reaction.Educts)
         {
            createRHSforAmount(reaction, reactionPartner.Partner, reactionPartner.StoichiometricCoefficient * -1);
         }

         foreach (var reactionPartner in reaction.Products)
         {
            createRHSforAmount(reaction, reactionPartner.Partner, reactionPartner.StoichiometricCoefficient);
         }
      }

      /// <summary>
      ///    Visit the specified object.
      /// </summary>
      /// <param name="eventToVisit">The object to visit.</param>
      public void Visit(IEvent eventToVisit)
      {
         var eventExport = new EventExport
         {
            Id = idFor(eventToVisit),
            EntityId = eventToVisit.Id,
            ConditionFormulaId = mapFormula(eventToVisit, eventToVisit.Formula).Id,
            OneTime = eventToVisit.OneTime
         };

         foreach (var assignment in eventToVisit.Assignments)
         {
            var alternateFormula = assignment.Formula;
            var assigmentExport = new AssigmentExport
            {
               ObjectId = idFor(assignment.ChangedEntity),
               NewFormulaId = mapFormula(assignment, alternateFormula).Id,
               UseAsValue = assignment.UseAsValue
            };
            eventExport.AssignmentList.Add(assigmentExport);
         }

         _modelExport.EventList.Add(eventExport);
      }

      private void mapQuantity(IQuantity quantity, QuantityExport quantityExport)
      {
         quantityExport.Id = idFor(quantity);
         quantityExport.EntityId = quantity.Id;
         quantityExport.Name = quantity.Name;
         quantityExport.Persistable = quantity.Persistable;

         //path is required also in optimized mode!
         //otherwise results-DataRepository will contain invalid quantity infos
         quantityExport.Path = _objectPathFactory.CreateAbsoluteObjectPath(quantity).PathAsString;

         if (_exportMode == SimModelExportMode.Full && quantity.Dimension != null)
            quantityExport.Unit = quantity.Dimension.BaseUnit.Name;

         if (quantity.IsFixedValue || quantity.Formula.IsConstant())
            quantityExport.Value = quantity.Value;
         else
            quantityExport.FormulaId = mapFormula(quantity, quantity.Formula).Id;
      }

      private void addObjectReferencesTo(ExplicitFormula explicitFormula, IDictionary<string, int> list)
      {
         explicitFormula.ObjectReferences.Each(objectReference => addObjectReferenceToList(explicitFormula, list, objectReference));
      }

      private void addObjectReferenceToList(IFormula formula, IDictionary<string, int> list, IObjectReference objectReference)
      {
         var alias = Regex.Replace(objectReference.Alias, @"\s", "_");
         try
         {
            list.Add(alias, idFor(objectReference.Object));
         }
         catch (ArgumentException ex)
         {
            throw new ArgumentException(Error.AliasAlreadyUsedInFormula(alias, formula.ToString()), ex);
         }
      }

      private ExplicitFormulaExport mapProcessFormula(IProcess process)
      {
         //already in amount per time... nothing to do
         if (process.IsAmountBased())
            return mapFormula(process, process.Formula).DowncastTo<ExplicitFormulaExport>();

         var amountKinetic = _concentrationBasedFormulaUpdater.CreateAmountBaseFormulaFor(process);
         //resolve to ensure that mapping works as expected
         amountKinetic.ResolveObjectPathsFor(process);

         return mapFormula(process, amountKinetic).DowncastTo<ExplicitFormulaExport>();
      }

      private FormulaExport mapFormula(IUsingFormula usingFormula, IFormula formula)
      {
         var formulaExport = createFormulaExport(usingFormula, formula);
         _modelExport.FormulaList.Add(formulaExport);
         formulaExport.Id = _modelExport.NewId();
         return formulaExport;
      }

      private FormulaExport createFormulaExport(IUsingFormula usingFormula, IFormula formula)
      {
         switch (formula)
         {
            case ExplicitFormula explicitFormula:
               var explicitFormulaExport = new ExplicitFormulaExport {Equation = explicitFormula.FormulaString};
               addObjectReferencesTo(explicitFormula, explicitFormulaExport.ReferenceList);
               return explicitFormulaExport;

            case TableFormulaWithXArgument tableFormulaWithXArgument:
               var tableFormulaWithXArgumentExport = new TableFormulaWithXArgumentExport()
               {
                  TableObjectId = idFor(tableFormulaWithXArgument.GetTableObject(usingFormula)),
                  XArgumentObjectId = idFor(tableFormulaWithXArgument.GetXArgumentObject(usingFormula))
               };
               return tableFormulaWithXArgumentExport;

            case TableFormulaWithOffset tableFormulaWithOffset:
               var tableFormulaWithOffsetExport = new TableFormulaWithOffsetExport
               {
                  TableObjectId = idFor(tableFormulaWithOffset.GetTableObject(usingFormula)),
                  OffsetObjectId = idFor(tableFormulaWithOffset.GetOffsetObject(usingFormula))
               };
               return tableFormulaWithOffsetExport;

            case TableFormula tableFormula:
               return _tableFormulaExportMapper.MapFrom(tableFormula);

            default:
               return new ExplicitFormulaExport {Equation = formula.Calculate(usingFormula).ToString(NumberFormatInfo.InvariantInfo)};
         }
      }

      private int idFor(IObjectBase objectBase)
      {
         if (!_idMap.Contains(objectBase.Id))
            _idMap.Add(objectBase.Id, _modelExport.NewId());

         return _idMap[objectBase.Id];
      }
   }
}
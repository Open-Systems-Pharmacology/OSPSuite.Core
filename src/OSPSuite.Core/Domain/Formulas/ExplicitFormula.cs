using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Formulas
{
   public class ExplicitFormula : FormulaWithFormulaString
   {
      /// <summary>
      ///   Id of formula from which the current formula came from. This is only usefull for serialization 
      ///   optimization when serializing a model
      /// </summary>
      public string OriginId { get; set; }

      public ExplicitFormula() : this(string.Empty)
      {
      }

      public ExplicitFormula(string formulaString)
      {
         FormulaString = formulaString;
         OriginId = string.Empty;
      }

      protected override double CalculateFor(IEnumerable<IObjectReference> usedObjects, IUsingFormula dependentObject)
      {
         var allUsedObjects = usedObjects.ToList();
         var formulaParser = createFormulaParser(allUsedObjects);

         var parameterValues = new double[] {};
         var variableValues = allUsedObjects.Select(x => x.Object.Value).ToArray();

         return formulaParser.Compute(variableValues, parameterValues);
      }

      private IExplicitFormulaParser createFormulaParser(IEnumerable<IObjectReference> references)
      {
         IEnumerable<string> parameterNames = new Collection<string>();
         var variableNames = references.Select(x => x.Alias);
         var formulaParser = ExplicitFormulaParserCreator(variableNames, parameterNames);
         formulaParser.FormulaString = FormulaString;
         return formulaParser;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var explicitFormula = source as ExplicitFormula;
         if (explicitFormula == null) return;
         FormulaString = explicitFormula.FormulaString;
         OriginId = explicitFormula.OriginId;
      }

      public override string ToString()
      {
         return FormulaString;
      }
   }
}
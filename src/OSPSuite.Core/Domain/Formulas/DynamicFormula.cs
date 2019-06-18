using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Formulas
{
   /// <summary>
   ///    Base class for dynamic formula. It defines a condition that should be met by some IFormulaUsable objects
   ///    Default value of the formula string is set to the variable used for expansion
   /// </summary>
   public abstract class DynamicFormula : FormulaWithFormulaString
   {
      /// <summary>
      ///    Pattern use to recognize a variable in the formula string
      /// </summary>
      protected const string ITERATION_PATTERN = "#i";

      /// <summary>
      ///    Condition to be fulfilled by any IFormulaUsable that will be used in the formula
      /// </summary>
      public DescriptorCriteria Criteria { get; set; }

      /// <summary>
      ///    Name of the variable used when expanding the formula. By default, variable is set to P
      /// </summary>
      public string Variable { get; set; }

      protected DynamicFormula()
      {
         Criteria = new DescriptorCriteria();
         Variable = "P";
         FormulaString = VariablePattern;
      }

      protected override double CalculateFor(IEnumerable<IObjectReference> usedObjects, IUsingFormula dependentObject)
      {
         //this formula cannot be evaluated 
         return double.NaN;
      }

      /// <summary>
      ///    Operation performed bu the Dynamic Formula (Typically + or *)
      /// </summary>
      protected abstract string Operation { get; }

      /// <summary>
      ///    Returns the pattern representing the variable in the formula string. (e.g P_#i)
      /// </summary>
      public string VariablePattern => $"{Variable}_{ITERATION_PATTERN}";

      /// <summary>
      ///    Expands the dynamic formula using the list of available object that can be used in the formula
      /// </summary>
      public IFormula ExpandUsing(EntityDescriptorMapList<IFormulaUsable> allFormulaUsable, IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory)
      {
         var explicitFormula = objectBaseFactory.Create<ExplicitFormula>().WithName(Name).WithFormulaString("0");
         var allEntityToUse = allFormulaUsable.AllSatisfiedBy(Criteria);

         explicitFormula.Dimension = Dimension;

         //no entity to use in the dynamic formula? return
         if (allEntityToUse.Count == 0)
            return explicitFormula;

         var stringBuilder = new StringBuilder();
         for (int i = 0; i < allEntityToUse.Count; i++)
         {
            addDynamicVariablesToFormula(explicitFormula, stringBuilder, allEntityToUse[i], i, objectPathFactory);

            if (i < allEntityToUse.Count - 1)
               stringBuilder.AppendFormat(" {0} ", Operation);
         }

         explicitFormula.FormulaString = stringBuilder.ToString();

         return explicitFormula;
      }

      private void addDynamicVariablesToFormula(ExplicitFormula explicitFormula, StringBuilder stringBuilder, IFormulaUsable formulaUsable, int index0, IObjectPathFactory objectPathFactory)
      {
         string index1 = $"{index0 + 1}";
         string currentVariable = $"{Variable}_{index1}";

         //replace current variable in formula string and add reference to the explicit formula
         string formulaStringPart = FormulaString.Replace(VariablePattern, currentVariable);
         explicitFormula.AddObjectPath(objectPathFactory.CreateAbsoluteFormulaUsablePath(formulaUsable).WithAlias(currentVariable));

         foreach (var objectPath in ObjectPaths)
         {
            if (explicitFormula.ObjectPaths.Contains(objectPath))
               continue;

            if (objectPath.Alias.Contains(ITERATION_PATTERN))
            {
               var referenceVariable = objectPath.TryResolve<IFormulaUsable>(formulaUsable, out var success);
               if (!success)
                  throw new UnableToResolvePathException(objectPath, formulaUsable);

               var referenceAlias = objectPath.Alias.Replace(ITERATION_PATTERN, index1);
               formulaStringPart = formulaStringPart.Replace(objectPath.Alias, referenceAlias);
               explicitFormula.AddObjectPath(objectPathFactory.CreateAbsoluteFormulaUsablePath(referenceVariable).WithAlias(referenceAlias));
            }
            else
               //it is an usable path that is not dynamic. Simply add it
               explicitFormula.AddObjectPath(objectPath);
         }

         stringBuilder.Append(formulaStringPart);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceDynamicFormula = source as DynamicFormula;
         if (sourceDynamicFormula == null) return;
         Criteria = sourceDynamicFormula.Criteria.Clone();
         Variable = sourceDynamicFormula.Variable;
         FormulaString = sourceDynamicFormula.FormulaString;
      }

      protected override IEnumerable<string> UsedVariableNames => base.UsedVariableNames.Union(new[] {VariablePattern});
   }
}
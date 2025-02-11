using System;
using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    A calculation method represents the way a set of parameters will be calculated.
   ///    For instance, a calculation Method RR will describes for the category Partition Coefficient how
   ///    the parameters of the Rodger And Roland Model are calculated
   /// </summary>
   public class CoreCalculationMethod : BuildingBlock
   {
      /// <summary>
      ///    Name of the category where the calculation methods belongs
      /// </summary>
      public string Category { get; set; }

      private readonly Cache<IFormula, ParameterDescriptor> _outputFormulas = new Cache<IFormula, ParameterDescriptor>();
      private readonly Cache<IParameter, DescriptorCriteria> _helpParameters = new Cache<IParameter, DescriptorCriteria>();

      public CoreCalculationMethod()
      {
         Icon = IconNames.CALCULATION_METHOD;
      }

      /// <summary>
      ///    Defines a formula that will be defined for all parameter matching the descriptor.
      ///    This is typically the generic calculation one partition coefficient (formula)
      ///    and the descriptor of the parameter
      /// </summary>
      public void AddOutputFormula(IFormula formula, ParameterDescriptor parameterDescriptor)
      {
         //Add in formula cache
         _outputFormulas.Add(formula, parameterDescriptor);
      }

      /// <summary>
      ///    Adds a parameter that is only used as "help" parameter in order to calculate the formulas defined
      ///    in the calculation method. The parameter will be created in all containers matching the decriptor criteria condition
      /// </summary>
      /// <param name="parameter">Parameter that will be created</param>
      /// <param name="containerCriteria">Criteria of container where the parameter will be created</param>
      public void AddHelpParameter(IParameter parameter, DescriptorCriteria containerCriteria)
      {
         if (parameter == null)
            throw new ArgumentException(Error.UndefinedHelpParameter(Name, Category));

         if (parameter.Formula == null)
            throw new ArgumentException(Error.UndefinedFormulaInHelpParameter(parameter.Name, Name, Category));

         if (!FormulaCache.Contains(parameter.Formula))
            FormulaCache.Add(parameter.Formula);

         _helpParameters.Add(parameter, containerCriteria);
      }

      /// <summary>
      ///    Returns all output formulas (formula describing the parameter of the calculation method)
      /// </summary>
      public IEnumerable<IFormula> AllOutputFormulas() => _outputFormulas.Keys;

      /// <summary>
      ///    Returns the parameter descriptor describing the criteria where the formula should be used
      /// </summary>
      public ParameterDescriptor DescriptorFor(IFormula outputFormula) => _outputFormulas[outputFormula];

      /// <summary>
      ///    Returns all help parameters defined in the calculation method
      /// </summary>
      public IEnumerable<IParameter> AllHelpParameters() => _helpParameters.Keys;

      /// <summary>
      ///    Returns the descriptor describing the location were the given help parameter should be created
      /// </summary>
      public DescriptorCriteria DescriptorFor(IParameter helpParameter) => _helpParameters[helpParameter];

      public ICache<IFormula, ParameterDescriptor> OutputFormulas => _outputFormulas;

      public ICache<IParameter, DescriptorCriteria> HelpParameters => _helpParameters;

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceCalculationMethod = source as CoreCalculationMethod;
         if (sourceCalculationMethod == null) return;

         Category = sourceCalculationMethod.Category;
         sourceCalculationMethod.OutputFormulas.KeyValues.Each(kv => AddOutputFormula(cloneManager.Clone(kv.Key), kv.Value.Clone()));
         sourceCalculationMethod.HelpParameters.KeyValues.Each(kv => AddHelpParameter(cloneManager.Clone(kv.Key), kv.Value.Clone()));
      }
   }
}
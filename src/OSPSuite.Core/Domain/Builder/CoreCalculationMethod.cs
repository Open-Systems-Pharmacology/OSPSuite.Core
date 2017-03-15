using System;
using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    A calculation method represents the way a set of parameters will be calculated.
   ///    For instance, a calculation Method RR will describes for the category Partition Coefficient how
   ///    the parameters of the Rodger And Roland Model are calculated
   /// </summary>
   public interface ICoreCalculationMethod : IBuildingBlock
   {
      /// <summary>
      ///    Name of the category where the calculation methods belongs
      /// </summary>
      string Category { get; set; }

      /// <summary>
      ///    Defines a formula that will be defined for all parameter matching the descriptor.
      ///    This is typically the generic calculation one partition coefficient (formula)
      ///    and the descriptor of the parameter
      /// </summary>
      void AddOutputFormula(IFormula formula, ParameterDescriptor parameterDescriptor);

      /// <summary>
      ///    Adds a parameter that is only used as "help" parameter in order to calculate the formulas defined
      ///    in the calculation method. The parameter will be created in all containers matching the decriptor criteria condition
      /// </summary>
      /// <param name="parameter">Parameter that will be created</param>
      /// <param name="containerCriteria">Criteria of container where the parameter will be created</param>
      void AddHelpParameter(IParameter parameter, DescriptorCriteria containerCriteria);

      /// <summary>
      ///    Returns all output formulas (formula describing the parameter of the calculation method)
      /// </summary>
      IEnumerable<IFormula> AllOutputFormulas();

      /// <summary>
      ///    Returns the parameter descriptor describing the criteria where the formula should be used
      /// </summary>
      ParameterDescriptor DescriptorFor(IFormula outputFormula);

      /// <summary>
      ///    Returns all help parameters defined in the calculation method
      /// </summary>
      IEnumerable<IParameter> AllHelpParameters();

      /// <summary>
      ///    Returns the descriptor describing the location were the given help parameter should be created
      /// </summary>
      DescriptorCriteria DescriptorFor(IParameter helpParameter);
   }

   public class CoreCalculationMethod : BuildingBlock, ICoreCalculationMethod
   {
      public string Category { get; set; }
      private readonly ICache<IFormula, ParameterDescriptor> _outputFormulas;
      private readonly ICache<IParameter, DescriptorCriteria> _helpParameters;

      public CoreCalculationMethod()
      {
         Icon = IconNames.CALCULATION_METHOD;
         _outputFormulas = new Cache<IFormula, ParameterDescriptor>();
         _helpParameters = new Cache<IParameter, DescriptorCriteria>();
      }

      public void AddOutputFormula(IFormula formula, ParameterDescriptor parameterDescriptor)
      {
         //Add in formula cache
         _outputFormulas.Add(formula, parameterDescriptor);
      }

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

      public IEnumerable<IFormula> AllOutputFormulas()
      {
         return _outputFormulas.Keys;
      }

      public ParameterDescriptor DescriptorFor(IFormula outputFormula)
      {
         return _outputFormulas[outputFormula];
      }

      public IEnumerable<IParameter> AllHelpParameters()
      {
         return _helpParameters.Keys;
      }

      public DescriptorCriteria DescriptorFor(IParameter helpParameter)
      {
         return _helpParameters[helpParameter];
      }

      public ICache<IFormula, ParameterDescriptor> OutputFormulas
      {
         get { return _outputFormulas; }
      }

      public ICache<IParameter, DescriptorCriteria> HelpParameters
      {
         get { return _helpParameters; }
      }

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
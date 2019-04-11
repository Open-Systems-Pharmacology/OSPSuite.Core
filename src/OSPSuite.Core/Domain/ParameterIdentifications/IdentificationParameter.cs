using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class IdentificationParameter : Container
   {
      private readonly List<ParameterSelection> _allLinkedParameters = new List<ParameterSelection>();
      private string _linkedPath;
      public virtual IParameter StartValueParameter => this.Parameter(Constants.Parameters.START_VALUE);
      public virtual double StartValue => StartValueParameter.Value;

      public virtual IParameter MinValueParameter => this.Parameter(Constants.Parameters.MIN_VALUE);
      public virtual double MinValue => MinValueParameter.Value;

      public virtual IParameter MaxValueParameter => this.Parameter(Constants.Parameters.MAX_VALUE);
      public virtual double MaxValue => MaxValueParameter.Value;

      public virtual IDimension Dimension => _allLinkedParameters.FirstOrDefault()?.Dimension;
      public virtual bool UseAsFactor { get; set; }
      public virtual bool IsFixed { get; set; }
      public virtual Scalings Scaling { get; set; }

      /// <summary>
      ///    Reference to the <see cref="ParameterIdentification" /> containing the field
      /// </summary>
      public virtual ParameterIdentification ParameterIdentification { get; set; }

      public IdentificationParameter()
      {
         Rules?.AddRange(IdentificationParameterRules.All);
      }

      public void AddLinkedParameter(ParameterSelection parameterSelection)
      {
         if (parameterSelection == null)
            return;

         if (LinksParameter(parameterSelection))
            return;

         if (Dimension != null && !Equals(parameterSelection.Dimension, Dimension))
            throw new DimensionMismatchException(new[] {parameterSelection.Dimension, Dimension});

         _allLinkedParameters.Add(parameterSelection);
      }

      public virtual Unit DisplayUnit => StartValueParameter?.DisplayUnit;

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceIdentificationParameter = source as IdentificationParameter;
         if (sourceIdentificationParameter == null) return;
         UseAsFactor = sourceIdentificationParameter.UseAsFactor;
         Scaling = sourceIdentificationParameter.Scaling;
         IsFixed = sourceIdentificationParameter.IsFixed;
         _allLinkedParameters.Clear();
         _allLinkedParameters.AddRange(sourceIdentificationParameter.AllLinkedParameters.Select(x => x.Clone()));
      }

      public IReadOnlyList<ParameterSelection> AllLinkedParameters => _allLinkedParameters;

      public string LinkedPath
      {
         get
         {
            if (_linkedPath != null)
               return _linkedPath;

            var distinctPaths = AllLinkedParameters.Select(x => x.QuantitySelection.Path).Distinct().ToList();
            if (distinctPaths.Count == 1)
               _linkedPath = distinctPaths[0];

            return _linkedPath;
         }
      }

      public bool LinksParameter(ParameterSelection parameterSelection)
      {
         return parameterSelection.Simulation != null && _allLinkedParameters.Contains(parameterSelection);
      }

      public void RemovedLinkedParameter(IQuantity quantity)
      {
         RemovedLinkedParameter(LinkedParameterFor(quantity));
      }

      public ParameterSelection LinkedParameterFor(IQuantity quantity)
      {
         return _allLinkedParameters.Find(x => Equals(x.Quantity, quantity));
      }

      public IEnumerable<ParameterSelection> LinkedParametersFor(ISimulation simulation)
      {
         return _allLinkedParameters.Where(x => Equals(x.Simulation, simulation));
      }

      public void RemovedLinkedParameter(ParameterSelection linkedParameter)
      {
         if (linkedParameter == null) return;
         _allLinkedParameters.Remove(linkedParameter);
      }

      public void RemovedLinkedParametersForSimulation(ISimulation simulation)
      {
         LinkedParametersFor(simulation).ToList().Each(RemovedLinkedParameter);
      }

      public void SwapSimulation(ISimulation oldSimulation, ISimulation newSimulation)
      {
         LinkedParametersFor(oldSimulation).Each(x => x.UpdateSimulation(newSimulation));
      }

      public double OptimizedParameterValueFor(OptimizedParameterValue optimizedParameterValue, ParameterSelection linkedParameter)
      {
         return OptimizedParameterValueFor(optimizedParameterValue.Value, linkedParameter);
      }

      public double OptimizedParameterValueFor(double optimalValue, ParameterSelection linkedParameter)
      {
         if (UseAsFactor)
            return optimalValue * linkedParameter.Parameter.Value;

         return optimalValue;
      }
   }
}
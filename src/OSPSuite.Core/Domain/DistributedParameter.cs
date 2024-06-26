﻿using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Maths.Random;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public interface IDistributedParameter : IParameter, IQuantityAndContainer
   {
      double Percentile { get; set; }
      new DistributionFormula Formula { get; set; }
      IParameter MeanParameter { get; }
      IParameter DeviationParameter { get; }
      IParameter PercentileParameter { get; }
      double ProbabilityDensityFor(double value);
      double ValueFor(double percentile);
      void RefreshPercentile();
   }

   public class DistributedParameter : QuantityAndContainer, IDistributedParameter
   {
      public virtual ParameterBuildMode BuildMode { get; set; }
      public IFormula RHSFormula { get; set; }
      public ParameterInfo Info { get; set; }
      public ParameterOrigin Origin { get; private set; }
      public double? DefaultValue { get; set; }
      public DescriptorCriteria ContainerCriteria { get; set; }

      /// <inheritdoc />
      public bool IsDefault { get; set; }

      public DistributedParameter()
      {
         Persistable = false;
         QuantityType = QuantityType.Parameter;
         Info = new ParameterInfo();
         Origin = new ParameterOrigin();
         Rules.AddRange(ParameterRules.All());
         NegativeValuesAllowed = true;
      }

      public virtual void ResetToDefault()
      {
         if (DefaultValue != null)
            Value = DefaultValue.Value;

         //Is Fixed value should always be set after setting the value (Distributed parameter)
         IsFixedValue = false;
      }

      public override double Value
      {
         //This is required to ensure that R can read the value
         get => base.Value;
         set
         {
            _cachedValue = value;
            if (Formula.IsAnImplementationOf<DistributionFormula>())
            {
               percentile = Formula.DowncastTo<DistributionFormula>().CalculatePercentileForValue(value, this);
            }

            _cachedValueValid = true;
            IsFixedValue = true;
         }
      }

      public new DistributionFormula Formula
      {
         get => base.Formula as DistributionFormula;
         set => base.Formula = value;
      }

      public double ProbabilityDensityFor(double value)
      {
         return Formula.ProbabilityDensityFor(value, this);
      }

      public double RandomDeviateIn(RandomGenerator randomGenerator, double? min = null, double? max = null)
      {
         var minValue = min ?? MinValue;
         var maxValue = max ?? MaxValue;

         if (minValue.HasValue && maxValue.HasValue)
            return Formula.RandomDeviate(randomGenerator, this, minValue.Value, maxValue.Value);

         //no min and max value defined? Use the default interval in the distribution
         if (!(minValue.HasValue || maxValue.HasValue))
            return Formula.RandomDeviate(randomGenerator, this);

         //Min value is available
         if (minValue.HasValue)
            return Formula.RandomDeviate(randomGenerator, this, minValue.Value, double.PositiveInfinity);

         //Max value is available
         return Formula.RandomDeviate(randomGenerator, this, 0, maxValue.Value);
      }

      public void ClearRHSFormula() => RHSFormula = null;

      public double Percentile
      {
         get => percentile;
         set
         {
            IsFixedValue = false;
            percentile = value;
            _cachedValueValid = false;
            IsFixedValue = true;
         }
      }

      public double ValueFor(double percentileValue)
      {
         return Formula.CalculateValueFromPercentile(percentileValue, this);
      }

      public void RefreshPercentile()
      {
         //this triggers a recalculation of the percentile value
         Value = Value;
      }

      public IParameter MeanParameter => this.Parameter(Constants.Distribution.MEAN);

      public IParameter DeviationParameter => this.Parameter(Constants.Distribution.DEVIATION) ?? this.Parameter(Constants.Distribution.GEOMETRIC_DEVIATION);

      public override bool IsFixedValue
      {
         get => base.IsFixedValue;
         set
         {
            //percentile set first so that correct value is available if a value event is raised
            //Note: Percentile parameter may be null when constructing the distributed parameter dynamically
            if (PercentileParameter != null)
               PercentileParameter.IsFixedValue = value;

            base.IsFixedValue = value;

            if (PercentileParameter != null)
               OnPropertyChanged(() => Percentile);
         }
      }

      private double percentile
      {
         get => PercentileParameter.Value;
         set => PercentileParameter.Value = value;
      }

      public IParameter PercentileParameter => this.Parameter(Constants.Distribution.PERCENTILE);

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceDistributedParameter = source as IDistributedParameter;
         if (sourceDistributedParameter == null) return;
         BuildMode = sourceDistributedParameter.BuildMode;
         Info = sourceDistributedParameter.Info.Clone();
         DefaultValue = sourceDistributedParameter.DefaultValue;
         Origin = sourceDistributedParameter.Origin.Clone();
         IsDefault = sourceDistributedParameter.IsDefault;
         ContainerCriteria = sourceDistributedParameter.ContainerCriteria?.Clone();
      }

      #region Parameter Info

      public bool IsChangedByCreateIndividual
      {
         get => Info.IsChangedByCreateIndividual;
         set => Info.IsChangedByCreateIndividual = value;
      }

      public bool CanBeVaried
      {
         get => Info.CanBeVaried;
         set => Info.CanBeVaried = value;
      }

      public string GroupName
      {
         get => Info.GroupName;
         set => Info.GroupName = value;
      }

      public bool Visible
      {
         get => Info.Visible;
         set => Info.Visible = value;
      }

      public bool Editable
      {
         get => !Info.ReadOnly;
         set => Info.ReadOnly = !value;
      }

      public int Sequence
      {
         get => Info.Sequence;
         set => Info.Sequence = value;
      }

      public bool MinIsAllowed
      {
         get => Info.MinIsAllowed;
         set => Info.MinIsAllowed = value;
      }

      public double? MinValue
      {
         get => Info.MinValue;
         set => Info.MinValue = value;
      }

      public double? MaxValue
      {
         get => Info.MaxValue;
         set => Info.MaxValue = value;
      }

      public bool MaxIsAllowed
      {
         get => Info.MaxIsAllowed;
         set => Info.MaxIsAllowed = value;
      }

      public bool CanBeVariedInPopulation
      {
         get => Info.CanBeVariedInPopulation;
         set => Info.CanBeVariedInPopulation = value;
      }

      public PKSimBuildingBlockType BuildingBlockType
      {
         get => Info.BuildingBlockType;
         set => Info.BuildingBlockType = value;
      }

      #endregion
   }
}
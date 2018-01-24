using OSPSuite.Assets;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Maths.Random;

namespace OSPSuite.Core.Domain
{
   public interface IParameter : IQuantity, IWithDefaultState
   {
      ParameterBuildMode BuildMode { get; set; }
      IFormula RHSFormula { get; set; }
      bool Visible { get; set; }
      bool CanBeVaried { get; set; }
      string GroupName { get; set; }

      /// <summary>
      ///    Can this parameter be edited?
      /// </summary>
      bool Editable { get; set; }

      /// <summary>
      ///    Sequence of parameters in the hierarchy
      /// </summary>
      int Sequence { get; set; }

      /// <summary>
      ///    Min value of the parameter. Null if not available
      /// </summary>
      double? MinValue { get; set; }

      /// <summary>
      ///    Max value of the parameter. Null if not available
      /// </summary>
      double? MaxValue { get; set; }

      /// <summary>
      ///    Is the minimum value allowed?
      /// </summary>
      bool MinIsAllowed { get; set; }

      /// <summary>
      ///    Is the maximum value allowed?
      /// </summary>
      bool MaxIsAllowed { get; set; }

      /// <summary>
      ///    Can this parameter be varied in a population simulation?
      /// </summary>
      bool CanBeVariedInPopulation { get; set; }

      /// <summary>
      ///    Meta Information for this parameter
      /// </summary>
      ParameterInfo Info { get; set; }

      /// <summary>
      ///    PK-Sim Building block type of building block in which this parameter is defined.
      ///    For parameter defined outside of PK-Sim, the flag will be set to the default value
      ///    <see cref="PKSimBuildingBlockType.Simulation" />"/>
      /// </summary>
      PKSimBuildingBlockType BuildingBlockType { get; set; }

      /// <summary>
      ///    Origin of a simulation parameter
      /// </summary>
      ParameterOrigin Origin { get; }

      /// <summary>
      ///    Default value of parameter. This is typically only set for parameters having a specified  building block such as
      ///    individual or population
      /// </summary>
      double? DefaultValue { get; set; }

      /// <summary>
      ///    Can this parameter be changed by the create individual algorithm?
      ///    Default false
      /// </summary>
      bool IsChangedByCreateIndividual { get; }

      /// <summary>
      ///    Reset the parameter values to its default as defined when created
      /// </summary>
      void ResetToDefault();

      /// <summary>
      ///    returns a random deviate for the parameter between min and max if specified. Otherwise, the min and max
      ///    value of the parameter are taken
      /// </summary>
      double RandomDeviateIn(RandomGenerator randomGenerator, double? min = null, double? max = null);
   }

   public class Parameter : Quantity, IParameter
   {
      public virtual ParameterBuildMode BuildMode { get; set; }
      public virtual IFormula RHSFormula { get; set; }
      public virtual ParameterInfo Info { get; set; }
      public virtual ParameterOrigin Origin { get; private set; }
      public virtual double? DefaultValue { get; set; }

      /// <inheritdoc />
      public bool IsDefault { get; set; }

      public Parameter()
      {
         Persistable = false;
         BuildMode = ParameterBuildMode.Local;
         QuantityType = QuantityType.Parameter;
         Info = new ParameterInfo();
         Icon = IconNames.PARAMETER;
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

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceParameter = source as IParameter;
         if (sourceParameter == null) return;
         BuildMode = sourceParameter.BuildMode;
         Info = sourceParameter.Info.Clone();
         Origin = sourceParameter.Origin.Clone();
         DefaultValue = sourceParameter.DefaultValue;
         IsDefault = sourceParameter.IsDefault;
      }

      public double RandomDeviateIn(RandomGenerator randomGenerator, double? min = null, double? max = null)
      {
         var minValue = min ?? MinValue;
         var maxValue = max ?? MaxValue;
         if (!(minValue.HasValue && maxValue.HasValue))
            return double.NaN;

         //return a unfiorm deviate between min and max
         return randomGenerator.UniformDeviate(minValue.Value, maxValue.Value);
      }

      public virtual bool IsChangedByCreateIndividual => false;

      #region Parameter Info

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
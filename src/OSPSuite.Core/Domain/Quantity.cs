using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public interface IQuantity : IFormulaUsable, IUsingFormula
   {
      /// <summary>
      ///    Gets or sets a value indicating whether this <see cref="IQuantity" /> values are persisted during simulation run.
      /// </summary>
      /// <value>
      ///    <c>true</c> if persistable; otherwise, <c>false</c> .
      /// </value>
      bool Persistable { get; set; }

      /// <summary>
      ///    Gets or sets a value indicating whether this instance  fixed value is used or not.
      /// </summary>
      /// <value>
      ///    <c>true</c> if this instance uses the fixed value; otherwise, <c>false</c> .
      /// </value>
      bool IsFixedValue { get; set; }

      /// <summary>
      ///    Type of quantity. This is defined as flag so that type can be combined.
      ///    For instance, observer + Drug would be an observer observing a drug molecule
      /// </summary>
      QuantityType QuantityType { get; set; }

      /// <summary>
      ///    The value in the displayed unit
      /// </summary>
      double ValueInDisplayUnit { get; set; }

      /// <summary>
      ///    Optional description explaining the value of the parameter
      /// </summary>
      string ValueDescription { get; set; }

      /// <summary>
      ///    Specifies whether negative values are allowed or not for this quantity
      /// </summary>
      bool NegativeValuesAllowed { get; set; }
   }

   public abstract class Quantity : Entity, IQuantity
   {
      protected double _cachedValue;
      protected bool _cachedValueValid;
      private IFormula _formula;
      private bool _isFixedValue;
      private Unit _displayUnit;
      private string _valueDescription;
      private bool _persistable;
      public IDimension Dimension { get; set; }
      public QuantityType QuantityType { get; set; }
      public bool NegativeValuesAllowed { get; set; }

      protected Quantity()
      {
         Persistable = true;
         QuantityType = QuantityType.Undefined;
         Dimension = Constants.Dimension.NO_DIMENSION;
         _valueDescription = string.Empty;
         NegativeValuesAllowed = false;
      }

      public IFormula Formula
      {
         get { return _formula; }
         set
         {
            if (_formula != null)
               _formula.Changed -= onFormulaChanged;

            _formula = value;
            if (_formula != null)
               _formula.Changed += onFormulaChanged;

            //Formula was changed: The cached value is not valid anymore
            _cachedValueValid = false;
            OnPropertyChanged(() => Value);
         }
      }

      public string ValueDescription
      {
         get { return _valueDescription; }
         set
         {
            _valueDescription = value;
            OnPropertyChanged(() => ValueDescription);
         }
      }

      /// <summary>
      ///    Gets or sets a value indicating whether this <see cref="Quantity" /> values are persisted during simulation run.
      /// </summary>
      /// <value>
      ///    <c>true</c> if persistable; otherwise, <c>false</c> .
      /// </value>
      public bool Persistable
      {
         get { return _persistable; }
         set
         {
            //slight optimization here because this flag is set when running a simulation over all parameters. 
            //no need to raise event if not required
            if (_persistable == value)
               return;

            _persistable = value;
            OnPropertyChanged(() => Persistable);
         }
      }

      public virtual double Value
      {
         get
         {
            if (IsFixedValue || _cachedValueValid)
               return _cachedValue;

            if (Formula == null)
               _cachedValue = double.NaN;
            else
            {
               _cachedValue = Formula.Calculate(this);
               //Cached value is only valid if the Formula has updated it's references
               _cachedValueValid = Formula.AreReferencesResolved;
            }

            return _cachedValue;
         }
         set
         {
            _cachedValue = value;
            _isFixedValue = true;
            _cachedValueValid = false;
            OnPropertyChanged(() => Value);
         }
      }

      public double ValueInDisplayUnit
      {
         get { return this.ConvertToDisplayUnit(Value); }
         set { Value = this.ConvertToBaseUnit(value); }
      }

      public virtual Unit DisplayUnit
      {
         get
         {
            return _displayUnit ?? Dimension?.DefaultUnit;
         }
         set
         {
            _displayUnit = value;
            OnPropertyChanged(() => DisplayUnit);
         }
      }

      /// <summary>
      ///    Gets or sets a value indicating whether this instance  fixed value is used or not.
      /// </summary>
      /// <value>
      ///    <c>true</c> if this instance uses the fixed value; otherwise, <c>false</c> .
      /// </value>
      public virtual bool IsFixedValue
      {
         get { return _isFixedValue; }
         set
         {
            _cachedValue = value ? Value : double.NaN;
            _isFixedValue = value;
            _cachedValueValid = false;
            OnPropertyChanged(() => Value);
         }
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var sourceQuantity = source as IQuantity;
         if (sourceQuantity == null) return;

         Persistable = sourceQuantity.Persistable;
         Dimension = sourceQuantity.Dimension;
         DisplayUnit = sourceQuantity.DisplayUnit;
         ValueDescription = sourceQuantity.ValueDescription;
         QuantityType = sourceQuantity.QuantityType;
         NegativeValuesAllowed = sourceQuantity.NegativeValuesAllowed;

         if (sourceQuantity.IsFixedValue)
         {
            Value = sourceQuantity.Value;
         }
      }

      private void onFormulaChanged(object obj)
      {
         //Formula has changed but value is fixed. Nothing to do 
         if (IsFixedValue) return;

         _cachedValueValid = false;
         OnPropertyChanged(() => Value);
      }
   }
}
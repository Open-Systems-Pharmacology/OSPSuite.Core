using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public interface IQuantity : IFormulaUsable, IUsingFormula, IWithValueOrigin
   {
      /// <summary>
      ///    Gets or sets a value indicating whether this <see cref="IQuantity" /> values are persisted during simulation run.
      /// </summary>
      /// <value>
      ///    <c>true</c> if persistable; otherwise, <c>false</c> .
      /// </value>
      bool Persistable { get; set; }

      /// <summary>
      ///    Gets or sets a value indicating whether this instance fixed value is used or not.
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
      private bool _persistable;

      /// <inheritdoc />
      public virtual ValueOrigin ValueOrigin { get; }

      /// <inheritdoc />
      public IDimension Dimension { get; set; }

      /// <inheritdoc />
      public QuantityType QuantityType { get; set; }

      /// <inheritdoc />
      public bool NegativeValuesAllowed { get; set; }

      protected Quantity()
      {
         Persistable = true;
         QuantityType = QuantityType.Undefined;
         Dimension = Constants.Dimension.NO_DIMENSION;
         NegativeValuesAllowed = false;
         ValueOrigin = new ValueOrigin();
      }

      /// <inheritdoc />
      public IFormula Formula
      {
         get => _formula;
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

      /// <inheritdoc />
      public bool Persistable
      {
         get => _persistable;
         set => SetProperty(ref _persistable, value);
      }

      /// <inheritdoc />
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

      /// <inheritdoc />
      public double ValueInDisplayUnit
      {
         get => this.ConvertToDisplayUnit(Value);
         set => Value = this.ConvertToBaseUnit(value);
      }

      /// <inheritdoc />
      public virtual Unit DisplayUnit
      {
         get => _displayUnit ?? Dimension?.DefaultUnit;
         set => SetProperty(ref _displayUnit, value);
      }

      /// <inheritdoc />
      public virtual bool IsFixedValue
      {
         get => _isFixedValue;
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
         QuantityType = sourceQuantity.QuantityType;
         NegativeValuesAllowed = sourceQuantity.NegativeValuesAllowed;
         ValueOrigin.UpdateAllFrom(sourceQuantity.ValueOrigin);

         if (sourceQuantity.IsFixedValue)
         {
            Value = sourceQuantity.Value;
         }
      }

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         if (Equals(ValueOrigin, sourceValueOrigin))
            return;

         ValueOrigin.UpdateFrom(sourceValueOrigin);
         OnPropertyChanged(() => ValueOrigin);
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
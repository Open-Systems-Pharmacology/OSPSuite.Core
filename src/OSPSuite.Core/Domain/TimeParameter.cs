using System;
using System.ComponentModel;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Maths.Random;
using OSPSuite.Utility.Validation;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   internal class TimeParameter : IParameter
   {
      public IContainer ParentContainer { get; set; }
      public event PropertyChangedEventHandler PropertyChanged = delegate { };
      public event Action<object> Changed = delegate { };
      public IDimension Dimension { get; set; }
      public virtual bool Visible { get; set; }
      public bool CanBeVaried { get; set; }
      public string GroupName { get; set; }
      public bool Editable { get; set; }
      public int Sequence { get; set; }
      public double? MinValue { get; set; }
      public double? MaxValue { get; set; }
      public bool MinIsAllowed { get; set; }
      public bool MaxIsAllowed { get; set; }
      public bool CanBeVariedInPopulation { get; set; }
      public bool IsDefault { get; set; }
      public DescriptorCriteria ContainerCriteria { get; set; }

      public ParameterInfo Info { get; set; }
      public PKSimBuildingBlockType BuildingBlockType { get; set; }
      public ParameterOrigin Origin { get; }
      public ValueOrigin ValueOrigin { get; }
     

      public double? DefaultValue { get; set; }
      public bool IsChangedByCreateIndividual { get; set; }

      public void ResetToDefault()
      {
         //nothing to do
      }

      public double RandomDeviateIn(RandomGenerator randomGenerator, double? min = null, double? max = null)
      {
         return Value;
      }

      public void ClearRHSFormula()
      {
         //nothing to do
      }


      public string ValueDescription { get; set; }


      public (double value, bool success) TryGetValueInDisplayUnit()
      {
         return (ValueInDisplayUnit, true);
      }

      public (double value, bool success) TryGetValue()
      {
         return (Value, true);
      }

      public bool NegativeValuesAllowed { get; set; }

      public TimeParameter()
      {
         Tags = new Tags {new Tag(Constants.TIME)};
         Info = new ParameterInfo();
         Origin = new ParameterOrigin();
         ValueOrigin = new ValueOrigin();
         CanBeVaried = false;
         CanBeVariedInPopulation = false;
         NegativeValuesAllowed = true;
      }

      public void AcceptVisitor(IVisitor visitor)
      {
      }

      public string Id
      {
         get => Constants.TIME;
         set
         {
            /*nothing to do here*/
         }
      }

      public string Name
      {
         get => Constants.TIME;
         set
         {
            /*nothing to do here*/
         }
      }

      public void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         /*nothing to do here*/
      }

      public string Icon { get; set; }
      public string Description { get; set; }

      public Tags Tags { get; }

      public void AddTag(Tag tag)
      {
         /*nothing to do here*/
      }

      public void RemoveTag(Tag tag)
      {
         /*nothing to do here*/
      }

      public void RemoveTag(string tag)
      {
         /*nothing to do here*/
      }

      public void AddTag(string tagValue)
      {
         /*nothing to do here*/
      }

      public bool IsDescendant(IContainer container)
      {
         return false;
      }

      public bool HasAncestorNamed(string parentName)
      {
         return false;
      }

      public bool HasAncestorWith(Func<IContainer, bool> criteria)
      {
         return false;
      }

      public IContainer RootContainer => ParentContainer;

      public double Value
      {
         get => 0;
         set
         {
            /*nothing to do here*/
         }
      }

      /// <summary>
      ///    Method to add a IUsingFormulaEntity to the internal collection of using Objects.
      /// </summary>
      /// <remarks>
      ///    This method should be used only from IFormula Objects. make internal?
      /// </remarks>
      public void AddUsingObject(IUsingFormula usingObject)
      {
         /*nothing to do here*/
      }

      /// <summary>
      ///    Method to remove a IUsingFormulaEntity to the internal collection of using Objects.
      /// </summary>
      /// <remarks>
      ///    This method should be used only from IFormula Objects. make internal?
      /// </remarks>
      public void RemoveUsingObject(IUsingFormula usingObject)
      {
         /*nothing to do here*/
      }

      public bool Uses(IUsingFormula usingObject)
      {
         return false;
      }

      public double GetUnitValue(Unit unit)
      {
         return 0;
      }

      public IFormula Formula
      {
         get => null;
         set
         {
            ;
         }
      }

      /// <summary>
      ///    Gets or sets a value indicating whether this <see cref="IQuantity" /> values are persisted during simulation run.
      /// </summary>
      /// <value><c>true</c> if persistable; otherwise, <c>false</c>.</value>
      public bool Persistable
      {
         get => false;
         set
         {
            ;
         }
      }

      /// <summary>
      ///    Gets or sets a value indicating whether this instance  fixed value is used or not.
      /// </summary>
      /// <value>
      ///    <c>true</c> if this instance uses the fixed value; otherwise, <c>false</c>.
      /// </value>
      public bool IsFixedValue
      {
         get => true;
         set
         {
            ;
         }
      }

      public QuantityType QuantityType
      {
         get => QuantityType.Undefined;
         set { }
      }

      public string QuantityTypeAsString => QuantityType.ToString();

      public double ValueInDisplayUnit { get; set; }

      public ParameterBuildMode BuildMode
      {
         get => ParameterBuildMode.Global;
         set { }
      }

      public IFormula RHSFormula
      {
         get => null;
         set
         {
            ;
         }
      }

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         ValueOrigin.UpdateFrom(sourceValueOrigin);
      }


      public IBusinessRuleSet Rules => new BusinessRuleSet();

      public Unit DisplayUnit
      {
         get => Dimension.DefaultUnit;
         set
         {
            /* nothing to do */
         }
      }
   }
}
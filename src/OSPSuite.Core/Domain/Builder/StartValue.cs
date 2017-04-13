using System;
using System.Linq;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IStartValue : IUsingFormula, IWithDisplayUnit
   {
      /// <summary>
      ///    Path of parent container in spatial structure
      /// </summary>
      IObjectPath ContainerPath { get; set; }

      /// <summary>
      ///    Default start value. Will be used unless <see cref="Formula" /> is set.
      /// </summary>
      double? StartValue { get; set; }

      /// <summary>
      ///    Full path of entity in spatial structure or model
      /// </summary>
      IObjectPath Path { get; set; }

      /// <summary>
      ///    Optional description explaining the value of the parameter
      /// </summary>
      string ValueDescription { get; set; }
   }

   public abstract class StartValueBase : Entity, IStartValue
   {
      private Unit _displayUnit;
      private double? _startValue;
      private IFormula _formula;
      private IDimension _dimension;
      private IObjectPath _containerPath;
      private string _valueDescription;

      protected StartValueBase()
      {
         Dimension = Constants.Dimension.NO_DIMENSION;
         StartValue = null;
         ContainerPath = ObjectPath.Empty;
         _valueDescription = string.Empty;
      }

      public IObjectPath Path
      {
         get { return ContainerPath.Clone<IObjectPath>().AndAdd(Name); }
         set { entityFullPathToComponents(value); }
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
      /// Tests whether or not the value is public-member-equivalent to the target
      /// </summary>
      /// <param name="target">The comparable object</param>
      /// <returns>True if all the public members are equal, otherwise false</returns>
      protected bool IsEquivalentTo(IStartValue target)
      {
         if (ReferenceEquals(this, target))
            return true;
         return
            NullableEqualsCheck(ContainerPath, target.ContainerPath) &&
            NullableEqualsCheck(Path, target.Path) &&
            StartValue.HasValue == target.StartValue.HasValue && 
            
            (!StartValue.HasValue || ValueComparer.AreValuesEqual(StartValue.GetValueOrDefault(), target.StartValue.GetValueOrDefault())) &&
            
            NullableEqualsCheck(Formula, target.Formula, x=> x.ToString()) &&
            NullableEqualsCheck(Dimension, target.Dimension, x => x.ToString()) &&
            NullableEqualsCheck(Icon, target.Icon) &&
            NullableEqualsCheck(Description, target.Description) &&
            NullableEqualsCheck(Name, target.Name);
      }

      /// <summary>
      /// Compares two objects of the same type first checking for null, then for .Equals
      /// </summary>
      /// <typeparam name="T">The type being compared</typeparam>
      /// <param name="first">The first element being compared</param>
      /// <param name="second">The second element being compared</param>
      /// <param name="transform">An optional transform done on the parameter before .Equals. Often this is .ToString
      /// making the the comparison the same as first.ToString().Equals(second.ToString())</param>
      /// <returns>The result of the transform and .Equals calls as outlined if first is not null. If first is null, returns true if second is null</returns>
      protected bool NullableEqualsCheck<T>(T first, T second, Func<T, object> transform = null) where T : class
      {
         if (first == null)
            return second == null;

         if (second == null)
            return false;

         return transform == null ? first.Equals(second) : transform(first).Equals(transform(second));

      }

      private void entityFullPathToComponents(IObjectPath fullPath)
      {
         if (fullPath.Any())
         {
            Name = fullPath.Last();
            ContainerPath = fullPath.Clone<IObjectPath>();
            if (ContainerPath.Count > 0)
               ContainerPath.RemoveAt(ContainerPath.Count - 1);
         }
         else
         {
            Name = string.Empty;
            ContainerPath = ObjectPath.Empty;
         }
      }

      public double? StartValue
      {
         get { return _startValue; }
         set
         {
            _startValue = value;
            OnPropertyChanged(() => StartValue);
         }
      }

      public IFormula Formula
      {
         get { return _formula; }
         set
         {
            _formula = value;
            OnPropertyChanged(() => Formula);
         }
      }

      public IDimension Dimension
      {
         get { return _dimension; }
         set
         {
            _dimension = value;
            OnPropertyChanged(() => Dimension);
         }
      }

      public IObjectPath ContainerPath
      {
         get { return _containerPath; }
         set
         {
            _containerPath = value;
            OnPropertyChanged(() => ContainerPath);
         }
      }

      public Unit DisplayUnit
      {
         get { return _displayUnit ?? Dimension.DefaultUnit; }
         set
         {
            _displayUnit = value;
            OnPropertyChanged(() => DisplayUnit);
         }
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceStartValue = source as StartValueBase;
         if (sourceStartValue == null) return;

         StartValue = sourceStartValue.StartValue;
         ValueDescription = sourceStartValue.ValueDescription;
         ContainerPath = sourceStartValue.ContainerPath.Clone<IObjectPath>();
         DisplayUnit = sourceStartValue.DisplayUnit;
         Dimension = sourceStartValue.Dimension;
         Formula = cloneManager.Clone(sourceStartValue.Formula);
      }

      public override string ToString()
      {
         return $"Path={ContainerPath}, Name={Name}";
      }
   }
}
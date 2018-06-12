using System;
using System.Linq;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IStartValue : IUsingFormula, IWithDisplayUnit, IWithValueOrigin
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
   }

   public abstract class StartValueBase : Entity, IStartValue
   {
      private Unit _displayUnit;
      private double? _startValue;
      private IFormula _formula;
      private IDimension _dimension;
      private IObjectPath _containerPath;
      public ValueOrigin ValueOrigin { get; }
 
      protected StartValueBase()
      {
         Dimension = Constants.Dimension.NO_DIMENSION;
         StartValue = null;
         ContainerPath = ObjectPath.Empty;
         ValueOrigin = new ValueOrigin();
      }

      public IObjectPath Path
      {
         get => ContainerPath.Clone<IObjectPath>().AndAdd(Name);
         set => entityFullPathToComponents(value);
      }

      /// <summary>
      ///    Tests whether or not the value is public-member-equivalent to the target
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
            NullableEqualsCheck(Formula, target.Formula, x => x.ToString()) &&
            NullableEqualsCheck(Dimension, target.Dimension, x => x.ToString()) &&
            NullableEqualsCheck(Icon, target.Icon) &&
            NullableEqualsCheck(Description, target.Description) &&
            NullableEqualsCheck(Name, target.Name);
      }

      /// <summary>
      ///    Compares two objects of the same type first checking for null, then for .Equals
      /// </summary>
      /// <typeparam name="T">The type being compared</typeparam>
      /// <param name="first">The first element being compared</param>
      /// <param name="second">The second element being compared</param>
      /// <param name="transform">
      ///    An optional transform done on the parameter before .Equals. Often this is .ToString
      ///    making the the comparison the same as first.ToString().Equals(second.ToString())
      /// </param>
      /// <returns>
      ///    The result of the transform and .Equals calls as outlined if first is not null. If first is null, returns true
      ///    if second is null
      /// </returns>
      protected bool NullableEqualsCheck<T>(T first, T second, Func<T, object> transform = null) where T : class
      {
         if (first == null)
            return second == null;

         if (second == null)
            return false;

         return transform?.Invoke(first).Equals(transform(second)) ?? first.Equals(second);
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
         get => _startValue;
         set => SetProperty(ref _startValue, value);
      }

      public IFormula Formula
      {
         get => _formula;
         set => SetProperty(ref _formula, value);
      }

      public IDimension Dimension
      {
         get => _dimension;
         set => SetProperty(ref _dimension, value);
      }

      public IObjectPath ContainerPath
      {
         get => _containerPath;
         set => SetProperty(ref _containerPath, value);
      }

      public Unit DisplayUnit
      {
         get => _displayUnit ?? Dimension.DefaultUnit;
         set => SetProperty(ref _displayUnit, value);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceStartValue = source as StartValueBase;
         if (sourceStartValue == null) return;

         StartValue = sourceStartValue.StartValue;
         ContainerPath = sourceStartValue.ContainerPath.Clone<IObjectPath>();
         DisplayUnit = sourceStartValue.DisplayUnit;
         Dimension = sourceStartValue.Dimension;
         Formula = cloneManager.Clone(sourceStartValue.Formula);
         ValueOrigin.UpdateAllFrom(sourceStartValue.ValueOrigin);
      }

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         if(Equals(ValueOrigin, sourceValueOrigin))
            return;

         ValueOrigin.UpdateFrom(sourceValueOrigin);
         OnPropertyChanged(() => ValueOrigin);
      }

      public override string ToString()
      {
         return $"Path={ContainerPath}, Name={Name}";
      }

   }
}
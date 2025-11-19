using System.Linq;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   public abstract class PathAndValueEntity : Entity, IUsingFormula, IWithDisplayUnit, IWithPath, IWithNullableValue, IWithValueOrigin, IBuilder
   {
      private ObjectPath _containerPath;
      protected IFormula _formula;
      private Unit _displayUnit;
      private IDimension _dimension;
      private double? _value;
      public IBuildingBlock BuildingBlock { get; set; }

      protected PathAndValueEntity()
      {
         Dimension = Constants.Dimension.NO_DIMENSION;
         Value = null;
         ContainerPath = ObjectPath.Empty;
         ValueOrigin = new ValueOrigin();
      }

      private void entityFullPathToComponents(ObjectPath fullPath)
      {
         if (fullPath.Any())
         {
            Name = fullPath.Last();
            var containerPath = fullPath.Clone<ObjectPath>();
            if (containerPath.Count > 0)
               containerPath.RemoveAt(containerPath.Count - 1);
            ContainerPath = containerPath; //this fires
         }
         else
         {
            Name = string.Empty;
            ContainerPath = ObjectPath.Empty;
         }
      }

      public ObjectPath ContainerPath
      {
         get => _containerPath;
         set => SetProperty(ref _containerPath, value);
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

      public Unit DisplayUnit
      {
         get => _displayUnit ?? Dimension.DefaultUnit;
         set => SetProperty(ref _displayUnit, value);
      }

      public ObjectPath Path
      {
         get => ContainerPath.Clone<ObjectPath>().AndAdd(Name);
         set => entityFullPathToComponents(value);
      }
      
      public double? Value
      {
         get => _value;
         set => SetProperty(ref _value, value);
      }

      public ValueOrigin ValueOrigin { get; }

      /// <summary>
      ///    Only set for a parameter that is a distributed parameter.
      ///    This is required in order to be able to create distributed parameter dynamically in the simulation
      /// </summary>
      public DistributionType? DistributionType { get; set; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         if (!(source is PathAndValueEntity sourcePathAndValueEntity)) 
            return;

         DistributionType = sourcePathAndValueEntity.DistributionType;
         Value = sourcePathAndValueEntity.Value;
         ContainerPath = sourcePathAndValueEntity.ContainerPath.Clone<ObjectPath>();
         DisplayUnit = sourcePathAndValueEntity.DisplayUnit;
         Dimension = sourcePathAndValueEntity.Dimension;
         Formula = cloneManager.Clone(sourcePathAndValueEntity.Formula);
         ValueOrigin.UpdateAllFrom(sourcePathAndValueEntity.ValueOrigin);
      }

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         if (Equals(ValueOrigin, sourceValueOrigin))
            return;

         ValueOrigin.UpdateFrom(sourceValueOrigin);
         OnPropertyChanged(() => ValueOrigin);
      }

      public override string ToString() => $"Path={ContainerPath}, Name={Name}";
   }
}
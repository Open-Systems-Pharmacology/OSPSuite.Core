using System.Collections.Generic;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using static OSPSuite.Core.Domain.Constants.ContainerName;

namespace OSPSuite.Core.Domain.Builder
{
   public class ExpressionProfileBuildingBlock : PathAndValueEntityBuildingBlockFromPKSim<ExpressionParameter>, ILookupBuildingBlock<InitialCondition>
   {
      private readonly PathAndValueEntityCache<InitialCondition> _initialConditions = new PathAndValueEntityCache<InitialCondition>();
      public override string Icon => Type.IconName;

      public virtual string MoleculeName { get; private set; }

      public string Species { get; private set; }

      public ExpressionType Type { set; get; }

      public virtual string Category { get; private set; }

      InitialCondition ILookupBuildingBlock<InitialCondition>.ByPath(ObjectPath path)
      {
         return _initialConditions[path];
      }

      public InitialCondition ByPath(string objectPath)
      {
         throw new System.NotImplementedException();
      }

      public override string Name
      {
         get => ExpressionProfileName(MoleculeName, Species, Category);
         set
         {
            if (string.Equals(Name, value))
            {
               return;
            }

            var (moleculeName, species, category) = NamesFromExpressionProfileName(value);
            if (string.IsNullOrEmpty(moleculeName))
               return;

            Species = species;
            Category = category;
            MoleculeName = moleculeName;
            OnPropertyChanged(() => Name);
         }
      }

      public void RemoveInitialCondition(InitialCondition initialCondition)
      {
         if (initialCondition == null)
            return;

         _initialConditions.Remove(initialCondition.Path);
         initialCondition.BuildingBlock = null;
      }

      public void AddInitialCondition(InitialCondition initialCondition)
      {
         _initialConditions.Add(initialCondition);
         initialCondition.BuildingBlock = this;
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         _initialConditions.Each(ic => ic.AcceptVisitor(visitor));
      }

      public IReadOnlyCollection<InitialCondition> InitialConditions => _initialConditions;

      public IReadOnlyCollection<ExpressionParameter> ExpressionParameters => _allValues;

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceExpressionProfile = source as ExpressionProfileBuildingBlock;
         if (sourceExpressionProfile == null) return;

         Type = sourceExpressionProfile.Type;

         // Name is required because our base objects will the private property
         // But in this case the name is decomposed and stored in 3 other properties
         Name = sourceExpressionProfile.Name;

         _initialConditions.Clear();
         sourceExpressionProfile.InitialConditions.Each(initialCondition =>
            AddInitialCondition(cloneManager.Clone(initialCondition)));
      }

      IEnumerator<InitialCondition> IEnumerable<InitialCondition>.GetEnumerator()
      {
         return _initialConditions.GetEnumerator();
      }

      public void Add(InitialCondition pathAndValueEntity)
      {
         _initialConditions.Add(pathAndValueEntity);
         pathAndValueEntity.BuildingBlock = this;
      }

      public void Remove(InitialCondition pathAndValueEntity)
      {
         if (pathAndValueEntity == null)
            return;

         _initialConditions.Remove(pathAndValueEntity.Path);
      }
   }
}
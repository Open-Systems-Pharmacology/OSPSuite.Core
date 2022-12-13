using OSPSuite.Core.Domain.Services;
using static OSPSuite.Core.Domain.Constants.ContainerName;

namespace OSPSuite.Core.Domain.Builder
{
   public class ExpressionProfileBuildingBlock : PathAndValueEntityBuildingBlockSourcedFromPKSimBuildingBlock<ExpressionParameter>
   {
      public override string Icon => Type.IconName;

      public virtual string MoleculeName { get; private set; }

      public string Species { get; private set; }

      public ExpressionType Type { set; get; }


      public virtual string Category { get; private set; }

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

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceExpressionProfile = source as ExpressionProfileBuildingBlock;
         if (sourceExpressionProfile == null) return;

         Type = sourceExpressionProfile.Type;

         // Name is required because our base objects will the private property
         // But in this case the name is decomposed and stored in 3 other properties
         Name = sourceExpressionProfile.Name;
      }
   }
}
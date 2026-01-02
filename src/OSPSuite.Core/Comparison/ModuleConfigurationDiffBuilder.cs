using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Comparison
{
   public class ModuleConfigurationDiffBuilder : ShallowDiffBuilder<ModuleConfiguration>
   {
      public ModuleConfigurationDiffBuilder(IObjectTypeResolver objectTypeResolver) : base(objectTypeResolver)
      {
      }

      public override void Compare(IComparison<ModuleConfiguration> comparison)
      {
         // We can consider the module configurations equal if they have the same module name
         // and the same selected initial conditions and parameter values
         // All equality checks are based on names. Content differences
         // of the building blocks are not reported here because they should already be reflected
         // in the model

         CompareStringValues(x => x.Module.Name, x => x.Module.Name, comparison);
         AddShallowDifference(x => x.SelectedInitialConditions, comparison);
         AddShallowDifference(x => x.SelectedParameterValues, comparison);
      }
   }
}
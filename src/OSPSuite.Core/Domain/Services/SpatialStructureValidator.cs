using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   internal class SpatialStructureValidator : IModelValidator
   {
      private ValidationResult _result;

      public ValidationResult Validate(ModelConfiguration modelConfiguration)
      {
         try
         {
            var model = modelConfiguration.Model;
            _result = new ValidationResult();
            validateNeighborhoods(model, model.Neighborhoods.GetAllChildren<Neighborhood>());
            return _result;
         }
         finally
         {
            _result = null;
         }
      }

      private void validateNeighborhoods(IModel model, IReadOnlyCollection<Neighborhood> neighborhoods)
      {
         var root = model.Root;
         neighborhoods.Each(x => validateNeighborhood(x, root));
      }

      private void validateNeighborhood(Neighborhood neighborhood, IContainer root)
      {
         if (neighborhood.FirstNeighbor == null)
            _result.AddMessage(NotificationType.Error, neighborhood, Error.FirstNeighborNotDefinedFor(neighborhood.Name));

         if (neighborhood.SecondNeighbor == null)
            _result.AddMessage(NotificationType.Error, neighborhood, Error.SecondNeighborNotDefinedFor(neighborhood.Name));

         if (neighborhood.FirstNeighbor.Mode == ContainerMode.Logical)
            _result.AddMessage(NotificationType.Warning, neighborhood, Error.NeighborIsLogical(neighborhood.FirstNeighbor.Name, neighborhood.Name));

         if (neighborhood.SecondNeighbor.Mode == ContainerMode.Logical)
            _result.AddMessage(NotificationType.Warning, neighborhood, Error.NeighborIsLogical(neighborhood.SecondNeighbor.Name, neighborhood.Name));
      }
   }
}
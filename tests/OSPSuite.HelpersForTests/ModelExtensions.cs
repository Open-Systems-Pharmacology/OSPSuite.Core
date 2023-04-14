using OSPSuite.Core.Domain;

namespace OSPSuite.Helpers
{
   public static class ModelExtensionsForSpecs
   {
      public static IContainer MoleculeContainerInNeighborhood(this IModel model, string neighborhoodName, string moleculeName)
      {
         return model.Neighborhoods.EntityAt<IContainer>(neighborhoodName, moleculeName);
      }

      public static IContainer ModelOrganCompartment(this IModel model, string organName, string compartmentName)
      {
         return model.Root.EntityAt<IContainer>(Constants.ORGANISM, organName, compartmentName);
      }

      public static MoleculeAmount ModelOrganCompartmentMolecule(this IModel model, string organName, string compartmentName, string moleculeName)
      {
         return model.ModelOrganCompartment(organName, compartmentName).GetSingleChildByName<MoleculeAmount>(moleculeName);
      }
   }
}
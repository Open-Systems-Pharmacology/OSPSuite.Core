namespace OSPSuite.Core.Domain.Builder
{
   public interface IMoleculeDependentBuilder : IEntity, IBuilder
   {
      MoleculeList MoleculeList { get; }
      bool ForAll { get; set; }
   }
}
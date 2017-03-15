namespace OSPSuite.Core.Domain.Builder
{
   public interface IMoleculeDependentBuilder : IEntity
   {
      MoleculeList MoleculeList { get; }
      bool ForAll { get; set; }
   }
}
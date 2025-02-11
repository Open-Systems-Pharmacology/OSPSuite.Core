using System;

namespace OSPSuite.Core.Domain
{
   public class ReactionPartner
   {
      public double StoichiometricCoefficient { get; }
      public MoleculeAmount Partner { get; }

      [Obsolete("For serialization")]
      public ReactionPartner()
      {
      }

      public ReactionPartner(double stoichiometricCoefficient, MoleculeAmount partner)
      {
         StoichiometricCoefficient = stoichiometricCoefficient;
         Partner = partner;
      }
   }
}
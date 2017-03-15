using System;

namespace OSPSuite.Core.Domain
{
   public interface IReactionPartner
   {
      double StoichiometricCoefficient { get; }
      IMoleculeAmount Partner { get; }
   }

    public class ReactionPartner : IReactionPartner
    {
       public double StoichiometricCoefficient { get; private set; }
       public IMoleculeAmount Partner { get; private set; }

       [Obsolete("For serialization")]
       public ReactionPartner()
       {
       }

       public ReactionPartner(double stoichiometricCoefficient, IMoleculeAmount partner)
       {
          StoichiometricCoefficient = stoichiometricCoefficient;
          Partner = partner;
       }

    }
}
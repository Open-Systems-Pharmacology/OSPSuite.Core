using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public class MoleculeAmount : QuantityAndContainer, IWithScaleDivisor
   {
      public double ScaleDivisor { get; set; }

      public MoleculeAmount()
      {
         ContainerType = ContainerType.Molecule;
         ScaleDivisor = Constants.DEFAULT_SCALE_DIVISOR;
         Rules.Add(ScaleDivisorRules.ScaleDivisorStrictlyPositive);
         NegativeValuesAllowed = false;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var sourceMoleculeAmount = source as MoleculeAmount;
         if (sourceMoleculeAmount == null) return;
         ScaleDivisor = sourceMoleculeAmount.ScaleDivisor;
         QuantityType = sourceMoleculeAmount.QuantityType;
         ContainerType = sourceMoleculeAmount.ContainerType;
      }
   }
}
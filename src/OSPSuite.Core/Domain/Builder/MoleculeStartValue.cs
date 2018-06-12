using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IMoleculeStartValue : IStartValue, IWithScaleDivisor
   {
      bool IsPresent { set; get; }

      string MoleculeName { get; }

      /// <summary>
      ///    Tests whether or not the value is public-member-equivalent to the target
      /// </summary>
      /// <param name="moleculeStartValue">The comparable object</param>
      /// <returns>True if all the public members are equal, otherwise false</returns>
      bool IsEquivalentTo(IMoleculeStartValue moleculeStartValue);

      bool NegativeValuesAllowed { get; set; }
   }

   public class MoleculeStartValue : StartValueBase, IMoleculeStartValue
   {
      private bool _isPresent;
      private double _scaleDivisor;
      public bool NegativeValuesAllowed { get; set; }

      public MoleculeStartValue()
      {
         Rules.Add(ScaleDivisorRules.ScaleDivisorStrictlyPositive);
         ScaleDivisor = Constants.DEFAULT_SCALE_DIVISOR;
         NegativeValuesAllowed = false;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceMoleculeStartValue = source as IMoleculeStartValue;
         if (sourceMoleculeStartValue == null) return;
         IsPresent = sourceMoleculeStartValue.IsPresent;
         ScaleDivisor = sourceMoleculeStartValue.ScaleDivisor;
         NegativeValuesAllowed = sourceMoleculeStartValue.NegativeValuesAllowed;
      }

      public bool IsPresent
      {
         get => _isPresent;
         set => SetProperty(ref _isPresent, value);
      }

      public string MoleculeName => Name;

      public bool IsEquivalentTo(IMoleculeStartValue moleculeStartValue)
      {
         var isBaseEquivalent = base.IsEquivalentTo(moleculeStartValue);

         var isEquivalent =
            (IsPresent == moleculeStartValue.IsPresent) &&
            NullableEqualsCheck(MoleculeName, moleculeStartValue.MoleculeName) &&
            ValueComparer.AreValuesEqual(ScaleDivisor, moleculeStartValue.ScaleDivisor);

         return isBaseEquivalent && isEquivalent;
      }

      public virtual double ScaleDivisor
      {
         get => _scaleDivisor;
         set => SetProperty(ref _scaleDivisor, value);
      }
   }
}
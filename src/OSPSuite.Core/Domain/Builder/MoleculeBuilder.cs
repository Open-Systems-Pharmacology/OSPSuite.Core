using System.Collections.Generic;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IMoleculeBuilder : IContainer, IContainsParameters, IWithDisplayUnit
   {
      /// <summary>
      ///    Default start formula. Dimension depends on settings used in the project (Amount or Concentration)
      /// </summary>
      IFormula DefaultStartFormula { get; set; }

      /// <summary>
      ///    Set to true, the molecule will be floating in the spatial structure. (e.g. Not stationary)
      /// </summary>
      bool IsFloating { get; set; }

      QuantityType QuantityType { get; set; }

      /// <summary>
      ///    Specifies the list of transporter by which the molecule will be transported.
      /// </summary>
      IEnumerable<TransporterMoleculeContainer> TransporterMoleculeContainerCollection { get; }

      void AddTransporterMoleculeContainer(TransporterMoleculeContainer transporterMolecule);
      void RemoveTransporterMoleculeContainer(TransporterMoleculeContainer transporterMoleculeToRemove);

      /// <summary>
      ///    Specifies the list of interactions that the molecule can trigger
      /// </summary>
      IEnumerable<InteractionContainer> InteractionContainerCollection { get; }

      void AddInteractionContainer(InteractionContainer interactionContainer);
      void RemoveInteractionContainer(InteractionContainer interactionContainer);

      /// <summary>
      ///    Add the calculation method (category and name) to the molecule builder.
      ///    If a method was already added for the same category, an exception will be thrown
      /// </summary>
      void AddUsedCalculationMethod(UsedCalculationMethod calculationMethod);

      /// <summary>
      ///    Add the calculation method (category and name) to the molecule builder.
      ///    If a method was already added for the same category, an exception will be thrown
      /// </summary>
      void AddUsedCalculationMethod(ICoreCalculationMethod calculationMethod);

      IEnumerable<UsedCalculationMethod> UsedCalculationMethods { get; }

      /// <summary>
      ///    Set to true, represents a molecule that is being simulated (such as Compound, Inhibitor, DrugComplex) etc
      ///    Set to false, represents a molecule that is individual specific such as en Enzyme, Protein, Transporter, FcRn etc.
      ///    Default value is true
      /// </summary>
      bool IsXenobiotic { get; set; }

      bool IsFloatingXenobiotic { get; }

      /// <summary>
      ///    Gets the default start value for the Molecule
      /// </summary>
      /// <returns>The default value if the formula is a constant, otherwise null</returns>
      double? GetDefaultMoleculeStartValue();
   }

   public class MoleculeBuilder : Container, IMoleculeBuilder
   {
      private readonly ICache<string, UsedCalculationMethod> _usedCalculationMethods;
      private Unit _displayUnit;
      public IFormula DefaultStartFormula { get; set; }
      public bool IsFloating { get; set; }
      public QuantityType QuantityType { get; set; }
      public bool IsXenobiotic { get; set; }
      public IDimension Dimension { get; set; }

      public MoleculeBuilder()
      {
         _usedCalculationMethods = new Cache<string, UsedCalculationMethod>(x => x.Category);
         IsXenobiotic = true;
         QuantityType = QuantityType.Undefined;
      }

      public void AddParameter(IParameter parameter)
      {
         Add(parameter);
      }

      public void RemoveParameter(IParameter parameterToRemove)
      {
         RemoveChild(parameterToRemove);
      }

      public IEnumerable<IParameter> Parameters
      {
         get { return GetChildren<IParameter>(); }
      }

      public IEnumerable<TransporterMoleculeContainer> TransporterMoleculeContainerCollection
      {
         get { return GetChildren<TransporterMoleculeContainer>(); }
      }

      public void AddTransporterMoleculeContainer(TransporterMoleculeContainer transporterMolecule)
      {
         Add(transporterMolecule);
      }

      public void RemoveTransporterMoleculeContainer(TransporterMoleculeContainer transporterMoleculeToRemove)
      {
         RemoveChild(transporterMoleculeToRemove);
      }

      public IEnumerable<InteractionContainer> InteractionContainerCollection
      {
         get { return GetChildren<InteractionContainer>(); }
      }

      public void AddInteractionContainer(InteractionContainer interactionContainer)
      {
         Add(interactionContainer);
      }

      public void RemoveInteractionContainer(InteractionContainer interactionContainer)
      {
         RemoveChild(interactionContainer);
      }

      public void AddUsedCalculationMethod(UsedCalculationMethod calculationMethod)
      {
         _usedCalculationMethods.Add(calculationMethod);
      }

      public void AddUsedCalculationMethod(ICoreCalculationMethod calculationMethod)
      {
         AddUsedCalculationMethod(new UsedCalculationMethod(calculationMethod.Category, calculationMethod.Name));
      }

      public IEnumerable<UsedCalculationMethod> UsedCalculationMethods
      {
         get { return _usedCalculationMethods; }
      }

      public bool IsFloatingXenobiotic
      {
         get { return IsFloating && IsXenobiotic; }
      }

      public double? GetDefaultMoleculeStartValue()
      {
         if (DefaultStartFormula.IsConstant())
            return DefaultStartFormula.Calculate(null);

         return null;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var sourceMoleculeBuilder = source as IMoleculeBuilder;
         if (sourceMoleculeBuilder == null) return;

         IsFloating = sourceMoleculeBuilder.IsFloating;
         QuantityType = sourceMoleculeBuilder.QuantityType;
         IsXenobiotic = sourceMoleculeBuilder.IsXenobiotic;
         DisplayUnit = sourceMoleculeBuilder.DisplayUnit;
         Dimension = sourceMoleculeBuilder.Dimension;

         DefaultStartFormula = cloneManager.Clone(sourceMoleculeBuilder.DefaultStartFormula);
         _usedCalculationMethods.Clear();
         sourceMoleculeBuilder.UsedCalculationMethods.Each(x => AddUsedCalculationMethod(x.Clone()));
      }

      public virtual Unit DisplayUnit
      {
         get
         {
            if (_displayUnit != null)
               return _displayUnit;

            return Dimension != null ? Dimension.DefaultUnit : null;
         }
         set
         {
            _displayUnit = value;
            OnPropertyChanged(() => DisplayUnit);
         }
      }
   }
}
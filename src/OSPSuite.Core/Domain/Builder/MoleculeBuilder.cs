using System.Collections.Generic;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Builder
{
   public class MoleculeBuilder : Container, IContainsParameters, IWithDisplayUnit, IBuilder
   {
      private readonly ICache<string, UsedCalculationMethod> _usedCalculationMethods;
      private Unit _displayUnit;

      /// <summary>
      ///    Default start formula. Dimension depends on settings used in the project (Amount or Concentration)
      /// </summary>
      public IFormula DefaultStartFormula { get; set; }

      /// <summary>
      ///    Set to true, the molecule will be floating in the spatial structure. (e.g. Not stationary)
      /// </summary>
      public bool IsFloating { get; set; }

      public QuantityType QuantityType { get; set; }

      /// <summary>
      ///    Set to true, represents a molecule that is being simulated (such as Compound, Inhibitor, DrugComplex) etc
      ///    Set to false, represents a molecule that is individual specific such as en Enzyme, Protein, Transporter, FcRn etc.
      ///    Default value is true
      /// </summary>
      public bool IsXenobiotic { get; set; }

      public IDimension Dimension { get; set; }
      public IBuildingBlock BuildingBlock { get; set; }

      public MoleculeBuilder()
      {
         _usedCalculationMethods = new Cache<string, UsedCalculationMethod>(x => x.Category);
         IsXenobiotic = true;
         QuantityType = QuantityType.Undefined;
      }

      public void AddParameter(IParameter parameter) => Add(parameter);

      public void RemoveParameter(IParameter parameterToRemove) => RemoveChild(parameterToRemove);

      public IEnumerable<IParameter> Parameters => GetChildren<IParameter>();

      /// <summary>
      ///    Specifies the list of transporter by which the molecule will be transported.
      /// </summary>
      public IEnumerable<TransporterMoleculeContainer> TransporterMoleculeContainerCollection => GetChildren<TransporterMoleculeContainer>();

      public void AddTransporterMoleculeContainer(TransporterMoleculeContainer transporterMolecule) => Add(transporterMolecule);

      public void RemoveTransporterMoleculeContainer(TransporterMoleculeContainer transporterMoleculeToRemove) => RemoveChild(transporterMoleculeToRemove);

      /// <summary>
      ///    Specifies the list of interactions that the molecule can trigger
      /// </summary>
      public IEnumerable<InteractionContainer> InteractionContainerCollection => GetChildren<InteractionContainer>();

      public void AddInteractionContainer(InteractionContainer interactionContainer) => Add(interactionContainer);

      public void RemoveInteractionContainer(InteractionContainer interactionContainer) => RemoveChild(interactionContainer);

      /// <summary>
      ///    Add the calculation method (category and name) to the molecule builder.
      ///    If a method was already added for the same category, an exception will be thrown
      /// </summary>
      public void AddUsedCalculationMethod(UsedCalculationMethod calculationMethod) => _usedCalculationMethods.Add(calculationMethod);

      /// <summary>
      ///    Add the calculation method (category and name) to the molecule builder.
      ///    If a method was already added for the same category, an exception will be thrown
      /// </summary>
      public void AddUsedCalculationMethod(CoreCalculationMethod calculationMethod)
      {
         AddUsedCalculationMethod(new UsedCalculationMethod(calculationMethod.Category, calculationMethod.Name));
      }

      public IEnumerable<UsedCalculationMethod> UsedCalculationMethods => _usedCalculationMethods;

      public bool IsFloatingXenobiotic => IsFloating && IsXenobiotic;

      /// <summary>
      ///    Gets the default start value for the Molecule
      /// </summary>
      /// <returns>The default value if the formula is a constant, otherwise null</returns>
      public double? GetDefaultInitialCondition()
      {
         if (DefaultStartFormula.IsConstant())
            return DefaultStartFormula.Calculate(null);

         return null;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var sourceMoleculeBuilder = source as MoleculeBuilder;
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
         get => _displayUnit ?? Dimension?.DefaultUnit;
         set => SetProperty(ref _displayUnit, value);
      }
   }
}
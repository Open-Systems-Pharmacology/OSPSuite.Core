using System;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps molecule builder instance on molecule amount
   ///    <para></para>
   ///    Only parameters with the build mode "Local" will be added;
   ///    <para></para>
   ///    parameters with other build modes will be added elsewhere
   ///    <para></para>
   ///    (by the global molecule properties mapper)
   /// </summary>
   internal interface IMoleculeBuilderToMoleculeAmountMapper
   {
      /// <summary>
      ///    Maps the <paramref name="moleculeBuilder" /> to a MoleculeAmount. <paramref name="targetContainer" /> is where the
      ///    molecule amount will be added to.
      /// </summary>
      /// <param name="moleculeBuilder">Molecule builder to map to a MoleculeAmount</param>
      /// <param name="targetContainer">
      ///    Container where the molecule amount will be added. This is required in order to evaluate
      ///    local parameters container criteria
      /// </param>
      /// <param name="simulationBuilder">Simulation builder</param>
      /// <returns></returns>
      MoleculeAmount MapFrom(MoleculeBuilder moleculeBuilder, IContainer targetContainer, SimulationBuilder simulationBuilder);
   }

   internal class MoleculeBuilderToMoleculeAmountMapper : IMoleculeBuilderToMoleculeAmountMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;
      private readonly IFormulaFactory _formulaFactory;
      private readonly IParameterFactory _parameterFactory;
      private readonly IDimension _amountDimension;
      private readonly IObjectTracker _objectTracker;

      public MoleculeBuilderToMoleculeAmountMapper(IObjectBaseFactory objectBaseFactory,
         IFormulaBuilderToFormulaMapper formulaMapper,
         IParameterBuilderToParameterMapper parameterMapper,
         IDimensionFactory dimensionFactory, 
         IFormulaFactory formulaFactory,
         IParameterFactory parameterFactory,
         IObjectTracker objectTracker)
      {
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
         _parameterMapper = parameterMapper;
         _formulaFactory = formulaFactory;
         _parameterFactory = parameterFactory;
         _objectTracker = objectTracker;
         _amountDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
      }

      public MoleculeAmount MapFrom(MoleculeBuilder moleculeBuilder, IContainer targetContainer, SimulationBuilder simulationBuilder)
      {
         //molecule amount always in amount
         var moleculeAmount = _objectBaseFactory.Create<MoleculeAmount>()
            .WithName(moleculeBuilder.Name)
            .WithDescription(moleculeBuilder.Description)
            .WithContainerType(ContainerType.Molecule)
            .WithIcon(moleculeBuilder.Icon)
            .WithQuantityType(moleculeBuilder.QuantityType)
            .WithDimension(_amountDimension)
            .WithDisplayUnit(_amountDimension.UnitOrDefault(_amountDimension.DefaultUnit.Name));

         simulationBuilder.AddBuilderReference(moleculeAmount, moleculeBuilder);
         _objectTracker.TrackObject(moleculeAmount, moleculeBuilder, simulationBuilder);

         createMoleculeAmountDefaultFormula(moleculeBuilder, simulationBuilder, moleculeAmount);

         //map parameters. Only parameters having BuildMode="Local" will
         //be added to the molecule amount. Global/Property-Parameters
         //will be filled in elsewhere (by the GlobalProperties-Mapper)
         var allLocalParameters = moleculeBuilder.Parameters
            .Where(x => x.BuildMode == ParameterBuildMode.Local)
            .Where(x => x.ContainerCriteria?.IsSatisfiedBy(targetContainer) ?? true);

         allLocalParameters.Each(x => moleculeAmount.Add(_parameterMapper.MapFrom(x, simulationBuilder)));

         return moleculeAmount;
      }

      private void createMoleculeAmountDefaultFormula(MoleculeBuilder moleculeBuilder, SimulationBuilder simulationBuilder, MoleculeAmount moleculeAmount)
      {
         //set start value formula to the default. If user has specified
         //a new start value in MoleculesStartValueCollection-BB, default formula
         //will be overwritten during setting of initial condition

         var modelFormula = _formulaMapper.MapFrom(moleculeBuilder.DefaultStartFormula, simulationBuilder);

         //amount based, we can just the formula as is
         if (moleculeBuilder.IsAmountBased())
         {
            moleculeAmount.Formula = modelFormula;
            return;
         }

         //create a start value parameter that will be referenced in the molecule formula 
         var startValueParameter = _parameterFactory.CreateStartValueParameter(moleculeAmount, modelFormula, moleculeBuilder.DisplayUnit);
         simulationBuilder.AddBuilderReference(startValueParameter, moleculeBuilder);
         moleculeAmount.Add(startValueParameter);
         moleculeAmount.Formula = _formulaFactory.CreateMoleculeAmountReferenceToStartValue(startValueParameter);
      }
   }
}
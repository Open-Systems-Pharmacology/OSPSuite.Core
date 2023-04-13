using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using static OSPSuite.Core.Domain.Constants;
using static OSPSuite.Helpers.ConstantsForSpecs;

namespace OSPSuite.Helpers
{
   public class ModuleHelperForSpecs
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ISolverSettingsFactory _solverSettingsFactory;
      private readonly IOutputSchemaFactory _outputSchemaFactory;
      private readonly ISpatialStructureFactory _spatialStructureFactory;
      private readonly IParameterFactory _parameterFactory;
      private readonly INeighborhoodBuilderFactory _neighborhoodFactory;

      public ModuleHelperForSpecs(IObjectBaseFactory objectBaseFactory, ISolverSettingsFactory solverSettingsFactory, IOutputSchemaFactory outputSchemaFactory, ISpatialStructureFactory spatialStructureFactory, IParameterFactory parameterFactory, INeighborhoodBuilderFactory neighborhoodFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _solverSettingsFactory = solverSettingsFactory;
         _outputSchemaFactory = outputSchemaFactory;
         _spatialStructureFactory = spatialStructureFactory;
         _parameterFactory = parameterFactory;
         _neighborhoodFactory = neighborhoodFactory;
      }

      public SimulationConfiguration CreateSimulationConfiguration()
      {
         var simulationConfiguration = new SimulationConfiguration
         {
            SimulationSettings = createSimulationConfiguration(),
         };

         var module1 = createModule1();
         var module2 = createModule2();

         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module1));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module2));
         return simulationConfiguration;
      }

      private Module createModule1()
      {
         var module = _objectBaseFactory.Create<Module>().WithName("Module1");
         module.SpatialStructure = getSpatialStructureModule1();
         return module;
      }

      private SpatialStructure getSpatialStructureModule1()
      {
         var spatialStructure = _spatialStructureFactory.Create().WithName("SPATIAL STRUCTURE");

         var organism = _objectBaseFactory.Create<IContainer>()
            .WithName(ORGANISM)
            .WithMode(ContainerMode.Logical);

         //Create a parameter with formula in Organism with absolute path
         var bw = newConstantParameter(BW, 20);
         organism.Add(bw);


         var organismMoleculeProperties = _objectBaseFactory.Create<IContainer>().WithName(MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Molecule);
         organism.Add(organismMoleculeProperties);

         //ART
         var art = createContainerWithName(ArterialBlood);

         var artPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         artPlasma.Add(newConstantParameter(Volume, 2));
         art.Add(artPlasma);
         art.Add(newConstantParameter(Q, 2));
         art.Add(newConstantParameter(P, 10));
         organism.Add(art);


         //LUNG
         var lung = createContainerWithName(Lung);

         var lngPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         lngPlasma.Add(newConstantParameter(Volume, 2));
         lngPlasma.Add(newConstantParameter(pH, 7.5));
         lung.Add(lngPlasma);

         var lngCell = createContainerWithName(Cell, ContainerMode.Physical);
         lngCell.Add(newConstantParameter(Volume, 1));
         lngCell.Add(newConstantParameter(pH, 7));
         lung.Add(lngCell);

         lung.Add(newConstantParameter(Q, 3));
         lung.Add(newConstantParameter(P, 2));
         organism.Add(lung);

         //BONE
         var bone = createContainerWithName(Bone);
         organism.Add(bone);

         var bonePlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         bonePlasma.Add(newConstantParameter(Volume, 2));
         bonePlasma.Add(newConstantParameter(pH, 7.5));
         bone.Add(bonePlasma);


         var boneCell = createContainerWithName(Cell, ContainerMode.Physical);
         boneCell.Add(newConstantParameter(Volume, 1));
         boneCell.Add(newConstantParameter(pH, 7));
         bone.Add(boneCell);


         //VEN
         var ven = createContainerWithName(VenousBlood);
         var venPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         venPlasma.Add(newConstantParameter(Volume, 2));
         ven.Add(venPlasma);
         ven.Add(newConstantParameter(Q, 2));
         organism.Add(ven);

         organism.Add(newConstantParameter(fu, 1));
         spatialStructure.AddTopContainer(organism);

         var neighborhood1 = _neighborhoodFactory.CreateBetween(artPlasma, bonePlasma).WithName("art_pls_to_bon_pls");
         //this is a constant parameter that will be referenced from arterial plasma compartment
         neighborhood1.AddParameter(newConstantParameter("K", 10));
         spatialStructure.AddNeighborhood(neighborhood1);
         var neighborhood2 = _neighborhoodFactory.CreateBetween(lngPlasma, artPlasma).WithName("lng_pls_to_art_pls");
         spatialStructure.AddNeighborhood(neighborhood2);
         var neighborhood3 = _neighborhoodFactory.CreateBetween(bonePlasma, venPlasma).WithName("bon_pls_to_ven_pls");
         spatialStructure.AddNeighborhood(neighborhood3);
         var neighborhood4 = _neighborhoodFactory.CreateBetween(venPlasma, lngPlasma).WithName("ven_pls_to_lng_pls");
         spatialStructure.AddNeighborhood(neighborhood4);

         var neighborhood5 = _neighborhoodFactory.CreateBetween(lngPlasma, lngCell).WithName("lng_pls_to_lng_cell");
         neighborhood5.AddTag("Cell2Plasma");
         neighborhood5.AddParameter(newConstantParameter("SA", 22));
         spatialStructure.AddNeighborhood(neighborhood5);

         var neighborhood6 = _neighborhoodFactory.CreateBetween(bonePlasma, boneCell).WithName("bon_pls_to_bon_cell");
         neighborhood6.AddTag("Cell2Plasma");
         neighborhood6.AddParameter(newConstantParameter("SA", 22));

         spatialStructure.AddNeighborhood(neighborhood6);

         spatialStructure.ResolveReferencesInNeighborhoods();
         return spatialStructure; 
      }

      private IContainer createContainerWithName(string containerName, ContainerMode containerMode = ContainerMode.Logical)
      {
         return _objectBaseFactory.Create<IContainer>()
            .WithName(containerName)
            .WithMode(containerMode);
      }

      private IParameter newConstantParameter(string name, double value, ParameterBuildMode parameterBuildMode = ParameterBuildMode.Local)
      {
         return _parameterFactory.CreateParameter(name, value)
            .WithMode(parameterBuildMode);
      }

      private Module createModule2()
      {
         var module = _objectBaseFactory.Create<Module>().WithName("Module2");

         return module;
      }

      private SimulationSettings createSimulationConfiguration()
      {
         return new SimulationSettings {Solver = _solverSettingsFactory.CreateCVODE(), OutputSchema = _outputSchemaFactory.CreateDefault(), OutputSelections = new OutputSelections()};
      }
   }
}
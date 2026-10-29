using System;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_BuildingBlockContentHasher : ContextForIntegration<IBuildingBlockContentHasher>
   {
      private const string LIVER = "Liver";
      private IObjectBaseFactory _objectBaseFactory;
      private IObjectPathFactory _objectPathFactory;
      private ISpatialStructureFactory _spatialStructureFactory;
      private IDimension _volumeDimension;
      private IDimension _amountDimension;
      private IDimension _amountPerTimeDimension;

      protected override void Context()
      {
         sut = IoC.Resolve<IBuildingBlockContentHasher>();
         _objectBaseFactory = IoC.Resolve<IObjectBaseFactory>();
         _objectPathFactory = IoC.Resolve<IObjectPathFactory>();
         _spatialStructureFactory = IoC.Resolve<ISpatialStructureFactory>();
         var dimensionFactory = IoC.Resolve<IDimensionFactory>();
         _volumeDimension = dimensionFactory.Dimension(Constants.Dimension.VOLUME);
         _amountDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         _amountPerTimeDimension = dimensionFactory.Dimension(Constants.Dimension.AMOUNT_PER_TIME);
      }

      /// <summary>
      ///    Every call creates a module with exactly the same content but with new ids and new creation meta data
      /// </summary>
      protected Module CreateModule(string name = "Module")
      {
         var module = _objectBaseFactory.Create<Module>().WithName(name);
         module.Add(createSpatialStructure());
         module.Add(createMolecules());
         module.Add(createReactions());
         module.Add(createParameterValues());
         module.Add(createInitialConditions());
         return module;
      }

      /// <summary>
      ///    Every call creates an individual with exactly the same content but with new ids and new creation meta data
      /// </summary>
      protected IndividualBuildingBlock CreateIndividual(string name = "Individual")
      {
         var individual = _objectBaseFactory.Create<IndividualBuildingBlock>().WithName(name);
         individual.PKSimVersion = "12.0";
         individual.OriginData.Add(new ExtendedProperty<string> {Name = "Species", Value = "Human"});
         individual.OriginData.Add(new ExtendedProperty<double> {Name = "Age", Value = 30});

         var weight = new IndividualParameter
         {
            ContainerPath = new ObjectPath(Constants.ORGANISM),
            Name = "Weight",
            Value = 70,
            DistributionType = DistributionType.Normal
         };
         weight.ValueOrigin.UpdateFrom(new ValueOrigin {Source = ValueOriginSources.Database, Method = ValueOriginDeterminationMethods.Assumption, Description = "Lots of people"});
         individual.Add(weight);

         var bmi = NewExplicitFormula(individual.FormulaCache, "BMI", "Weight / Height^2", Constants.Dimension.NO_DIMENSION);
         bmi.AddObjectPath(PathWithAlias("Weight", Constants.ORGANISM, "Weight"));
         bmi.AddObjectPath(PathWithAlias("Height", Constants.ORGANISM, "Height"));
         individual.Add(new IndividualParameter
         {
            ContainerPath = new ObjectPath(Constants.ORGANISM),
            Name = "BMI",
            Formula = bmi
         });

         return individual;
      }

      /// <summary>
      ///    Every call creates an expression profile with exactly the same content but with new ids and new creation meta data
      /// </summary>
      protected ExpressionProfileBuildingBlock CreateExpressionProfile()
      {
         var expressionProfile = _objectBaseFactory.Create<ExpressionProfileBuildingBlock>().WithName("CYP3A4|Human|Healthy");
         expressionProfile.Type = ExpressionTypes.MetabolizingEnzyme;
         expressionProfile.PKSimVersion = "12.0";

         expressionProfile.Add(new ExpressionParameter
         {
            ContainerPath = new ObjectPath(Constants.ORGANISM, LIVER, "CYP3A4"),
            Name = "Relative expression",
            Value = 1
         });

         var initialConcentration = NewExplicitFormula(expressionProfile.FormulaCache, "InitialConcentration", "RelExp * RefConc", Constants.Dimension.NO_DIMENSION);
         initialConcentration.AddObjectPath(PathWithAlias("RelExp", ObjectPath.PARENT_CONTAINER, "Relative expression"));
         initialConcentration.AddObjectPath(PathWithAlias("RefConc", Constants.ORGANISM, LIVER, "CYP3A4", "Reference concentration"));
         expressionProfile.Add(new ExpressionParameter
         {
            ContainerPath = new ObjectPath(Constants.ORGANISM, LIVER, "CYP3A4"),
            Name = "Initial concentration",
            Formula = initialConcentration
         });

         expressionProfile.AddInitialCondition(new InitialCondition
         {
            ContainerPath = new ObjectPath(Constants.ORGANISM, LIVER, "Intracellular"),
            Name = "CYP3A4",
            Value = 0,
            IsPresent = true,
            Dimension = _amountDimension
         });

         return expressionProfile;
      }

      protected ExplicitFormula NewExplicitFormula(IFormulaCache formulaCache, string name, string formulaString, IDimension dimension)
      {
         var formula = _objectBaseFactory.Create<ExplicitFormula>().WithName(name).WithFormulaString(formulaString).WithDimension(dimension);
         formulaCache.Add(formula);
         return formula;
      }

      protected FormulaUsablePath PathWithAlias(string alias, params string[] path)
      {
         return _objectPathFactory.CreateFormulaUsablePathFrom(path).WithAlias(alias);
      }

      protected IParameter NewConstantParameter(string name, double value, IDimension dimension, ParameterBuildMode buildMode = ParameterBuildMode.Local)
      {
         var parameter = _objectBaseFactory.Create<IParameter>().WithName(name).WithDimension(dimension).WithFormula(new ConstantFormula(value).WithDimension(dimension));
         parameter.BuildMode = buildMode;
         return parameter;
      }

      private SpatialStructure createSpatialStructure()
      {
         var spatialStructure = _spatialStructureFactory.Create().WithName("Spatial Structure");
         var organism = _objectBaseFactory.Create<IContainer>().WithName(Constants.ORGANISM).WithMode(ContainerMode.Logical);
         var liver = _objectBaseFactory.Create<IContainer>().WithName(LIVER).WithMode(ContainerMode.Physical);
         organism.Add(liver);

         liver.Add(NewConstantParameter("Volume", 2, _volumeDimension));

         var volumeTwice = NewExplicitFormula(spatialStructure.FormulaCache, "VolumeTwice", "Volume * 2", _volumeDimension);
         volumeTwice.AddObjectPath(PathWithAlias("Volume", Constants.ORGANISM, LIVER, "Volume"));
         liver.Add(_objectBaseFactory.Create<IParameter>().WithName("VolumeTwice").WithDimension(_volumeDimension).WithFormula(volumeTwice));

         spatialStructure.AddTopContainer(organism);
         return spatialStructure;
      }

      private MoleculeBuildingBlock createMolecules()
      {
         var molecules = _objectBaseFactory.Create<MoleculeBuildingBlock>().WithName("Molecules");

         var startFormula = NewExplicitFormula(molecules.FormulaCache, "StartFormula", "C0 * V", _amountDimension);
         startFormula.AddObjectPath(PathWithAlias("V", Constants.ORGANISM, LIVER, "Volume"));
         startFormula.AddObjectPath(PathWithAlias("C0", ObjectPath.PARENT_CONTAINER, "C0"));

         molecules.Add(createMolecule("A", startFormula));
         molecules.Add(createMolecule("B", _objectBaseFactory.Create<ConstantFormula>().WithValue(0).WithDimension(_amountDimension)));
         return molecules;
      }

      private MoleculeBuilder createMolecule(string name, IFormula defaultStartFormula)
      {
         var molecule = _objectBaseFactory.Create<MoleculeBuilder>().WithName(name).WithDimension(_amountDimension);
         molecule.DefaultStartFormula = defaultStartFormula;
         molecule.IsFloating = true;
         molecule.QuantityType = QuantityType.Drug;
         molecule.AddParameter(NewConstantParameter("MW", 300, Constants.Dimension.NO_DIMENSION, ParameterBuildMode.Global));
         molecule.AddParameter(NewConstantParameter("C0", 1, _amountDimension));
         return molecule;
      }

      private ReactionBuildingBlock createReactions()
      {
         var reactions = _objectBaseFactory.Create<ReactionBuildingBlock>().WithName("Reactions");
         var reaction = _objectBaseFactory.Create<ReactionBuilder>().WithName("R1").WithDimension(_amountPerTimeDimension);

         var kinetic = NewExplicitFormula(reactions.FormulaCache, "R1Kinetic", "k * A", _amountPerTimeDimension);
         kinetic.AddObjectPath(PathWithAlias("A", ObjectPath.PARENT_CONTAINER, "A"));
         kinetic.AddObjectPath(PathWithAlias("k", ObjectPath.PARENT_CONTAINER, "R1", "k"));
         reaction.Formula = kinetic;

         reaction.AddEduct(new ReactionPartnerBuilder("A", 1));
         reaction.AddProduct(new ReactionPartnerBuilder("B", 1));
         reaction.AddParameter(NewConstantParameter("k", 0.5, Constants.Dimension.NO_DIMENSION));
         reactions.Add(reaction);
         return reactions;
      }

      private ParameterValuesBuildingBlock createParameterValues()
      {
         var parameterValues = _objectBaseFactory.Create<ParameterValuesBuildingBlock>().WithName("Parameter Values");
         parameterValues.Add(new ParameterValue
         {
            ContainerPath = new ObjectPath(Constants.ORGANISM, LIVER),
            Name = "Volume",
            Value = 3,
            Dimension = _volumeDimension
         });

         var volumeFormula = NewExplicitFormula(parameterValues.FormulaCache, "VolumeFormula", "2 * V", _volumeDimension);
         volumeFormula.AddObjectPath(PathWithAlias("V", Constants.ORGANISM, LIVER, "Volume"));
         parameterValues.Add(new ParameterValue
         {
            ContainerPath = new ObjectPath(Constants.ORGANISM, LIVER),
            Name = "VolumeTwice",
            Formula = volumeFormula,
            Dimension = _volumeDimension
         });
         return parameterValues;
      }

      private InitialConditionsBuildingBlock createInitialConditions()
      {
         var initialConditions = _objectBaseFactory.Create<InitialConditionsBuildingBlock>().WithName("Initial Conditions");
         initialConditions.Add(new InitialCondition
         {
            ContainerPath = new ObjectPath(Constants.ORGANISM, LIVER),
            Name = "A",
            Value = 1,
            IsPresent = true,
            Dimension = _amountDimension
         });
         initialConditions.Add(new InitialCondition
         {
            ContainerPath = new ObjectPath(Constants.ORGANISM, LIVER),
            Name = "B",
            Value = 0,
            IsPresent = false,
            Dimension = _amountDimension
         });
         return initialConditions;
      }
   }

   public class When_hashing_two_modules_with_the_same_content_but_different_ids_names_and_meta_data : concern_for_BuildingBlockContentHasher
   {
      private Module _module1;
      private Module _module2;

      protected override void Context()
      {
         base.Context();
         _module1 = CreateModule("Module 1");
         _module2 = CreateModule("Module 2");
         _module2.Snapshot = "a snapshot of module 2";
         _module2.BuildingBlocks.Each(buildingBlock =>
         {
            buildingBlock.Version = 5;
            buildingBlock.Creation = new CreationMetaData
            {
               Version = "11.0",
               InternalVersion = 42,
               CreationMode = CreationMode.Clone,
               ClonedFrom = "Module 1",
               Origin = Origins.PKSim
            };
         });
      }

      [Observation]
      public void the_modules_should_have_different_ids()
      {
         _module1.Id.ShouldNotBeEqualTo(_module2.Id);
         _module1.Reactions.Id.ShouldNotBeEqualTo(_module2.Reactions.Id);
         _module1.Reactions.First().Formula.Id.ShouldNotBeEqualTo(_module2.Reactions.First().Formula.Id);
      }

      [Observation]
      public void should_return_the_same_hash()
      {
         sut.ContentHashFor(_module1).ShouldBeEqualTo(sut.ContentHashFor(_module2));
      }

      [Observation]
      public void should_return_a_non_empty_hash()
      {
         string.IsNullOrEmpty(sut.ContentHashFor(_module1)).ShouldBeFalse();
      }

      [Observation]
      public void should_return_the_same_hash_when_called_again_for_the_same_module()
      {
         sut.ContentHashFor(_module1).ShouldBeEqualTo(sut.ContentHashFor(_module1));
      }

      [Observation]
      public void should_consider_the_modules_to_have_the_same_content()
      {
         sut.HaveSameContent(_module1, _module2).ShouldBeTrue();
      }
   }

   public class When_hashing_two_modules_with_the_same_content_where_ids_are_equal_to_values_of_other_attributes : concern_for_BuildingBlockContentHasher
   {
      private Module _module1;
      private Module _module2;

      protected override void Context()
      {
         base.Context();
         _module1 = CreateModule();
         _module2 = CreateModule();

         //Ids are unrestricted strings. "0" and "1" are also serialized for flags, for values and for the ordinals of
         //object paths, aliases and dimensions in the string table of the formula cache.
         //An unreferenced id (the Liver container) ...
         var organism = _module2.SpatialStructure.TopContainers.First(x => x.Name == Constants.ORGANISM);
         organism.Container("Liver").Id = "0";

         //... and a referenced id (the reaction formula in the formula cache)
         var kinetic = _module2.Reactions.First().Formula;
         kinetic.Id = "1";
         _module2.Reactions.FormulaCache.Refresh();
      }

      [Observation]
      public void should_return_the_same_hash()
      {
         sut.ContentHashFor(_module1).ShouldBeEqualTo(sut.ContentHashFor(_module2));
      }

      [Observation]
      public void should_consider_the_modules_to_have_the_same_content()
      {
         sut.HaveSameContent(_module1, _module2).ShouldBeTrue();
      }
   }

   public class When_hashing_a_module_that_was_saved_to_pkml_and_loaded_again : concern_for_BuildingBlockContentHasher
   {
      private IPKMLPersistor _pkmlPersistor;
      private string _fileName;
      private Module _module;
      private Module _loadedModule;

      protected override void Context()
      {
         base.Context();
         _pkmlPersistor = IoC.Resolve<IPKMLPersistor>();
         _fileName = FileHelper.GenerateTemporaryFileName();
         _module = CreateModule();
         _pkmlPersistor.SaveToPKML(_module, _fileName);
      }

      protected override void Because()
      {
         _loadedModule = _pkmlPersistor.Load<Module>(_fileName);
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_fileName);
      }

      [Observation]
      public void should_return_the_same_hash_for_the_original_and_the_loaded_module()
      {
         ReferenceEquals(_module, _loadedModule).ShouldBeFalse();
         sut.ContentHashFor(_loadedModule).ShouldBeEqualTo(sut.ContentHashFor(_module));
         sut.HaveSameContent(_module, _loadedModule).ShouldBeTrue();
      }
   }

   public abstract class concern_for_modules_with_different_content : concern_for_BuildingBlockContentHasher
   {
      protected Module _module1;
      protected Module _module2;

      protected override void Context()
      {
         base.Context();
         _module1 = CreateModule();
         _module2 = CreateModule();
         ChangeContentOf(_module2);
      }

      protected abstract void ChangeContentOf(Module module);

      [Observation]
      public void should_return_different_hashes()
      {
         sut.ContentHashFor(_module1).ShouldNotBeEqualTo(sut.ContentHashFor(_module2));
      }

      [Observation]
      public void should_not_consider_the_modules_to_have_the_same_content()
      {
         sut.HaveSameContent(_module1, _module2).ShouldBeFalse();
      }
   }

   public class When_hashing_two_modules_that_differ_in_a_parameter_value : concern_for_modules_with_different_content
   {
      protected override void ChangeContentOf(Module module)
      {
         module.ParameterValuesCollection[0].First(x => x.Name == "Volume").Value = 4;
      }
   }

   public class When_hashing_two_modules_that_differ_in_a_formula_string : concern_for_modules_with_different_content
   {
      protected override void ChangeContentOf(Module module)
      {
         module.Reactions.First().Formula.DowncastTo<ExplicitFormula>().FormulaString = "k * A * 2";
      }
   }

   public class When_hashing_two_modules_that_differ_in_a_molecule_flag : concern_for_modules_with_different_content
   {
      protected override void ChangeContentOf(Module module)
      {
         module.Molecules.First().IsFloating = false;
      }
   }

   public class When_hashing_two_modules_that_differ_in_the_alias_bound_to_a_formula_path : concern_for_modules_with_different_content
   {
      protected override void ChangeContentOf(Module module)
      {
         var volumeTwice = module.SpatialStructure.FormulaCache.First(x => x.Name == "VolumeTwice");
         volumeTwice.ObjectPaths.First().Alias = "Vol";
      }
   }

   public class When_hashing_two_modules_that_differ_in_the_path_bound_to_a_formula_alias : concern_for_modules_with_different_content
   {
      protected override void ChangeContentOf(Module module)
      {
         var kinetic = module.Reactions.First().Formula;
         kinetic.ObjectPaths.First(x => x.Alias == "A").Replace("A", "B");
      }
   }

   public class When_hashing_two_modules_that_differ_in_a_reaction_stoichiometry : concern_for_modules_with_different_content
   {
      protected override void ChangeContentOf(Module module)
      {
         module.Reactions.First().Educts.First().StoichiometricCoefficient = 2;
      }
   }

   public class When_hashing_two_modules_that_differ_in_an_initial_condition : concern_for_modules_with_different_content
   {
      protected override void ChangeContentOf(Module module)
      {
         module.InitialConditionsCollection[0].First(x => x.Name == "B").IsPresent = true;
      }
   }

   public class When_hashing_two_modules_where_a_building_block_is_missing : concern_for_modules_with_different_content
   {
      protected override void ChangeContentOf(Module module)
      {
         module.Remove(module.Reactions);
      }
   }

   public class When_hashing_two_modules_that_differ_in_a_parameter_of_the_spatial_structure : concern_for_modules_with_different_content
   {
      protected override void ChangeContentOf(Module module)
      {
         module.SpatialStructure.TopContainers.First(x => x.Name == Constants.ORGANISM).Container("Liver").Parameter("Volume").Value = 20;
      }
   }

   public class When_hashing_two_modules_that_differ_in_the_merge_behavior : concern_for_modules_with_different_content
   {
      protected override void ChangeContentOf(Module module)
      {
         module.MergeBehavior = MergeBehavior.Extend;
      }
   }

   public class When_hashing_two_individuals_with_the_same_content_but_different_ids_names_and_meta_data : concern_for_BuildingBlockContentHasher
   {
      private IndividualBuildingBlock _individual1;
      private IndividualBuildingBlock _individual2;

      protected override void Context()
      {
         base.Context();
         _individual1 = CreateIndividual("Individual 1");
         _individual2 = CreateIndividual("Individual 2");
         _individual2.Version = 3;
         _individual2.Snapshot = "a snapshot";
         _individual2.Creation = new CreationMetaData {Version = "11.0", CreationMode = CreationMode.Clone, ClonedFrom = "Individual 1", Origin = Origins.PKSim};
         _individual2.Each(x => x.Origin = new ParameterOrigin {BuilingBlockId = "another building block", ParameterId = "another parameter"});
      }

      [Observation]
      public void the_individuals_should_have_different_ids()
      {
         _individual1.Id.ShouldNotBeEqualTo(_individual2.Id);
         _individual1.FormulaCache.First().Id.ShouldNotBeEqualTo(_individual2.FormulaCache.First().Id);
      }

      [Observation]
      public void should_return_the_same_hash()
      {
         sut.ContentHashFor(_individual1).ShouldBeEqualTo(sut.ContentHashFor(_individual2));
         sut.HaveSameContent(_individual1, _individual2).ShouldBeTrue();
      }
   }

   public class When_hashing_an_individual_that_was_saved_to_pkml_and_loaded_again : concern_for_BuildingBlockContentHasher
   {
      private IPKMLPersistor _pkmlPersistor;
      private string _fileName;
      private IndividualBuildingBlock _individual;
      private IndividualBuildingBlock _loadedIndividual;

      protected override void Context()
      {
         base.Context();
         _pkmlPersistor = IoC.Resolve<IPKMLPersistor>();
         _fileName = FileHelper.GenerateTemporaryFileName();
         _individual = CreateIndividual();
         _pkmlPersistor.SaveToPKML(_individual, _fileName);
      }

      protected override void Because()
      {
         _loadedIndividual = _pkmlPersistor.Load<IndividualBuildingBlock>(_fileName);
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_fileName);
      }

      [Observation]
      public void should_return_the_same_hash_for_the_original_and_the_loaded_individual()
      {
         sut.ContentHashFor(_loadedIndividual).ShouldBeEqualTo(sut.ContentHashFor(_individual));
      }
   }

   public abstract class concern_for_individuals_with_different_content : concern_for_BuildingBlockContentHasher
   {
      protected IndividualBuildingBlock _individual1;
      protected IndividualBuildingBlock _individual2;

      protected override void Context()
      {
         base.Context();
         _individual1 = CreateIndividual();
         _individual2 = CreateIndividual();
         ChangeContentOf(_individual2);
      }

      protected abstract void ChangeContentOf(IndividualBuildingBlock individual);

      [Observation]
      public void should_return_different_hashes()
      {
         sut.ContentHashFor(_individual1).ShouldNotBeEqualTo(sut.ContentHashFor(_individual2));
         sut.HaveSameContent(_individual1, _individual2).ShouldBeFalse();
      }
   }

   public class When_hashing_two_individuals_that_differ_in_a_parameter_value : concern_for_individuals_with_different_content
   {
      protected override void ChangeContentOf(IndividualBuildingBlock individual)
      {
         individual.First(x => x.Name == "Weight").Value = 71;
      }
   }

   public class When_hashing_two_individuals_that_differ_in_a_parameter_formula : concern_for_individuals_with_different_content
   {
      protected override void ChangeContentOf(IndividualBuildingBlock individual)
      {
         individual.FormulaCache.First().DowncastTo<ExplicitFormula>().FormulaString = "Weight / Height";
      }
   }

   public class When_hashing_two_individuals_that_differ_in_the_origin_data : concern_for_individuals_with_different_content
   {
      protected override void ChangeContentOf(IndividualBuildingBlock individual)
      {
         individual.OriginData["Age"].DowncastTo<ExtendedProperty<double>>().Value = 31;
      }
   }

   public class When_hashing_two_individuals_that_differ_in_a_value_origin : concern_for_individuals_with_different_content
   {
      protected override void ChangeContentOf(IndividualBuildingBlock individual)
      {
         individual.First(x => x.Name == "Weight").ValueOrigin.Description = "Only a few people";
      }
   }

   public class When_hashing_two_individuals_where_a_parameter_is_missing : concern_for_individuals_with_different_content
   {
      protected override void ChangeContentOf(IndividualBuildingBlock individual)
      {
         individual.Remove(individual.First(x => x.Name == "BMI"));
      }
   }

   public class When_hashing_two_expression_profiles_with_the_same_content_but_different_ids_and_meta_data : concern_for_BuildingBlockContentHasher
   {
      private ExpressionProfileBuildingBlock _expressionProfile1;
      private ExpressionProfileBuildingBlock _expressionProfile2;

      protected override void Context()
      {
         base.Context();
         _expressionProfile1 = CreateExpressionProfile();
         _expressionProfile2 = CreateExpressionProfile();
         _expressionProfile2.Version = 3;
         _expressionProfile2.Snapshot = "a snapshot";
         _expressionProfile2.Creation = new CreationMetaData {Version = "11.0", CreationMode = CreationMode.Clone, Origin = Origins.PKSim};
      }

      [Observation]
      public void should_return_the_same_hash()
      {
         _expressionProfile1.Id.ShouldNotBeEqualTo(_expressionProfile2.Id);
         sut.ContentHashFor(_expressionProfile1).ShouldBeEqualTo(sut.ContentHashFor(_expressionProfile2));
         sut.HaveSameContent(_expressionProfile1, _expressionProfile2).ShouldBeTrue();
      }
   }

   public abstract class concern_for_expression_profiles_with_different_content : concern_for_BuildingBlockContentHasher
   {
      protected ExpressionProfileBuildingBlock _expressionProfile1;
      protected ExpressionProfileBuildingBlock _expressionProfile2;

      protected override void Context()
      {
         base.Context();
         _expressionProfile1 = CreateExpressionProfile();
         _expressionProfile2 = CreateExpressionProfile();
         ChangeContentOf(_expressionProfile2);
      }

      protected abstract void ChangeContentOf(ExpressionProfileBuildingBlock expressionProfile);

      [Observation]
      public void should_return_different_hashes()
      {
         sut.ContentHashFor(_expressionProfile1).ShouldNotBeEqualTo(sut.ContentHashFor(_expressionProfile2));
         sut.HaveSameContent(_expressionProfile1, _expressionProfile2).ShouldBeFalse();
      }
   }

   public class When_hashing_two_expression_profiles_that_differ_in_the_expression_type : concern_for_expression_profiles_with_different_content
   {
      protected override void ChangeContentOf(ExpressionProfileBuildingBlock expressionProfile)
      {
         expressionProfile.Type = ExpressionTypes.TransportProtein;
      }
   }

   public class When_hashing_two_expression_profiles_that_differ_in_an_expression_parameter : concern_for_expression_profiles_with_different_content
   {
      protected override void ChangeContentOf(ExpressionProfileBuildingBlock expressionProfile)
      {
         expressionProfile.ExpressionParameters.First(x => x.Name == "Relative expression").Value = 0.5;
      }
   }

   public class When_hashing_two_expression_profiles_where_an_initial_condition_is_missing : concern_for_expression_profiles_with_different_content
   {
      protected override void ChangeContentOf(ExpressionProfileBuildingBlock expressionProfile)
      {
         expressionProfile.RemoveInitialCondition(expressionProfile.InitialConditions.First());
      }
   }

   public class When_hashing_two_building_blocks_of_different_types : concern_for_BuildingBlockContentHasher
   {
      [Observation]
      public void should_not_consider_them_to_have_the_same_content()
      {
         sut.HaveSameContent(CreateIndividual(), CreateExpressionProfile()).ShouldBeFalse();
      }
   }

   public class When_hashing_a_null_object : concern_for_BuildingBlockContentHasher
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.ContentHashFor((Module) null)).ShouldThrowAn<ArgumentNullException>();
         The.Action(() => sut.ContentHashFor((IBuildingBlock) null)).ShouldThrowAn<ArgumentNullException>();
      }
   }
}

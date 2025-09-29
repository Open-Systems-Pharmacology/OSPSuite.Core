using System;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants;
using static OSPSuite.Core.Domain.Constants.Organ;
using static OSPSuite.Core.Domain.Constants.Parameters;
using static OSPSuite.Core.Domain.ObjectPath;
using static OSPSuite.Core.Domain.ObjectPathKeywords;

namespace OSPSuite.Core.Domain
{
   internal abstract class concern_for_FormulaTask : ContextSpecification<FormulaTask>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      private IObjectPathFactory _objectPathFactory;
      private IAliasCreator _aliasCreator;
      private IDimensionFactory _dimensionFactory;
      private IEntityPathResolver _entityPathResolver;
      protected IKeywordReplacerTask _keywordReplacerTask;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         _aliasCreator = new AliasCreator();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _entityPathResolver = new EntityPathResolverForSpecs();
         _keywordReplacerTask = new KeywordReplacerTask(_objectPathFactory);
         sut = new FormulaTask(_objectPathFactory, _objectBaseFactory, _aliasCreator, _dimensionFactory, _entityPathResolver);
      }
   }

   internal class When_checking_if_two_formula_of_different_types_are_the_same : concern_for_FormulaTask
   {
      [Observation]
      public void should_return_false()
      {
         sut.FormulasAreTheSame(new ExplicitFormula(), new ConstantFormula()).ShouldBeFalse();
         sut.FormulasAreTheSame(new ExplicitFormula(), new NormalDistributionFormula()).ShouldBeFalse();
         sut.FormulasAreTheSame(new ConstantFormula(), new LogNormalDistributionFormula()).ShouldBeFalse();
         sut.FormulasAreTheSame(new LogNormalDistributionFormula(), new NormalDistributionFormula()).ShouldBeFalse();
         sut.FormulasAreTheSame(new ExplicitFormula(), new TableFormula()).ShouldBeFalse();
         sut.FormulasAreTheSame(new BlackBoxFormula(), new TableFormula()).ShouldBeFalse();
         sut.FormulasAreTheSame(new BlackBoxFormula(), new ConstantFormula()).ShouldBeFalse();
      }
   }

   internal class When_checking_if_two_blackbox_formulas_are_the_same : concern_for_FormulaTask
   {
      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(new BlackBoxFormula(), new BlackBoxFormula()).ShouldBeTrue();
      }
   }

   internal class When_checking_if_two_distributed_formula_having_the_same_distribution_type_are_the_same : concern_for_FormulaTask
   {
      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(new LogNormalDistributionFormula(), new LogNormalDistributionFormula()).ShouldBeTrue();
         sut.FormulasAreTheSame(new NormalDistributionFormula(), new NormalDistributionFormula()).ShouldBeTrue();
         sut.FormulasAreTheSame(new DiscreteDistributionFormula(), new DiscreteDistributionFormula()).ShouldBeTrue();
         sut.FormulasAreTheSame(new UniformDistributionFormula(), new UniformDistributionFormula()).ShouldBeTrue();
      }
   }

   internal class When_checking_if_two_constant_formula_are_the_same : concern_for_FormulaTask
   {
      [Observation]
      public void should_return_true_if_the_value_are_the_same()
      {
         sut.FormulasAreTheSame(new ConstantFormula(5), new ConstantFormula(5)).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_value_are_not_the_same()
      {
         sut.FormulasAreTheSame(new ConstantFormula(5), new ConstantFormula(6)).ShouldBeFalse();
      }
   }

   internal class When_checking_if_two_explicit_formula_using_the_same_formula_string_and_the_references_are_the_same : concern_for_FormulaTask
   {
      private ExplicitFormula _formula1;
      private ExplicitFormula _formula2;

      protected override void Context()
      {
         base.Context();
         _formula1 = new ExplicitFormula("A+B");
         _formula2 = new ExplicitFormula("A+B");
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});

         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});
      }

      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(_formula1, _formula2).ShouldBeTrue();
      }
   }

   internal class When_checking_if_two_explicit_formula_using_the_same_formula_string_but_different_references_are_the_same : concern_for_FormulaTask
   {
      private ExplicitFormula _formula1;
      private ExplicitFormula _formula2;

      protected override void Context()
      {
         base.Context();
         _formula1 = new ExplicitFormula("A+B");
         _formula2 = new ExplicitFormula("A+B");
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});

         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"E", "F", "F"}) {Alias = "B"});
      }

      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(_formula1, _formula2).ShouldBeFalse();
      }
   }

   internal class When_checking_if_two_explicit_formula_using_diffent_formula_string_but_the_same_references_are_the_same : concern_for_FormulaTask
   {
      private ExplicitFormula _formula1;
      private ExplicitFormula _formula2;

      protected override void Context()
      {
         base.Context();
         _formula1 = new ExplicitFormula("A+B");
         _formula2 = new ExplicitFormula("A+C");
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});

         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});
      }

      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(_formula1, _formula2).ShouldBeFalse();
      }
   }

   internal class When_checking_if_two_explicit_formula_using_different_formula_string_and_different_references_are_the_same : concern_for_FormulaTask
   {
      private ExplicitFormula _formula1;
      private ExplicitFormula _formula2;

      protected override void Context()
      {
         base.Context();
         _formula1 = new ExplicitFormula("A+B");
         _formula2 = new ExplicitFormula("A+C");
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});

         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
      }

      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(_formula1, _formula2).ShouldBeFalse();
      }
   }

   internal class When_checking_if_two_explicit_formula_using_the_same_formula_string_and_the_references_but_diffent_aliases_are_the_same : concern_for_FormulaTask
   {
      private ExplicitFormula _formula1;
      private ExplicitFormula _formula2;

      protected override void Context()
      {
         base.Context();
         _formula1 = new ExplicitFormula("A+B");
         _formula2 = new ExplicitFormula("A+B");
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula1.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "B"});

         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"A", "B", "C"}) {Alias = "A"});
         _formula2.AddObjectPath(new FormulaUsablePath(new[] {"D", "E", "F"}) {Alias = "C"});
      }

      [Observation]
      public void should_return_true()
      {
         sut.FormulasAreTheSame(_formula1, _formula2).ShouldBeFalse();
      }
   }

   internal class When_expanding_all_dynamic_formula_in_a_model : concern_for_FormulaTask
   {
      private IModel _model;
      private IParameter _parameter;
      private IParameter _p1;
      private IParameter _p2;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _objectBaseFactory.Create<ExplicitFormula>()).Returns(new ExplicitFormula());
         _model = new Model();
         _p1 = new Parameter().WithName("p1").WithFormula(new ConstantFormula(1));
         _p1.AddTag("toto");
         _p2 = new Parameter().WithName("p2").WithFormula(new ConstantFormula(2));
         _p2.AddTag("toto");
         var root = new Container();
         _model.Root = root;
         _parameter = new Parameter().WithName("Dynamic");
         root.Add(_parameter);
         root.Add(_p1);
         root.Add(_p2);
         root.Add(new Parameter().WithName("p3").WithFormula(new ConstantFormula(3)));
         var sumFormula = new SumFormula
         {
            Criteria = Create.Criteria(x => x.With("toto"))
         };
         _parameter.Formula = sumFormula;

         root.Add(_parameter);
      }

      protected override void Because()
      {
         sut.ExpandDynamicFormulaIn(_model);
      }

      [Observation]
      public void should_replace_all_dynamic_formula_with_the_corresponding_explicit_formula()
      {
         _parameter.Formula.IsExplicit().ShouldBeTrue();
      }

      [Observation]
      public void should_have_created_an_explicit_formula_that_is_the_sum_of_the_defined_parameters()
      {
         var explicitFormula = _parameter.Formula.DowncastTo<ExplicitFormula>();
         explicitFormula.FormulaString.ShouldBeEqualTo("P_1 + P_2");
         explicitFormula.ObjectPaths.Count().ShouldBeEqualTo(2);
      }
   }

   internal class When_expanding_all_dynamic_formula_in_a_model_using_in_parent_criteria : concern_for_FormulaTask
   {
      private IModel _model;
      private IParameter _parameter;
      private IParameter _p1;
      private IParameter _p2;
      private Container _container1;
      private Container _container2;
      private Container _subContainer1;
      private Container _subContainer2;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _objectBaseFactory.Create<ExplicitFormula>()).Returns(new ExplicitFormula());
         _model = new Model();
         _container1 = new Container().WithName("Container1");
         _container2 = new Container().WithName("Container2");
         _subContainer1 = new Container().WithName("SubContainer1").WithParentContainer(_container1);
         _subContainer2 = new Container().WithName("SubContainer2").WithParentContainer(_container2);

         _p1 = new Parameter().WithName("p1").WithFormula(new ConstantFormula(1));
         _p1.AddTag("toto");
         _p2 = new Parameter().WithName("p2").WithFormula(new ConstantFormula(2));
         _p2.AddTag("toto");
         //simulate a real ROOT Container
         var root = new Container().WithContainerType(ContainerType.Simulation).WithName("root");
         _model.Root = root;
         root.Add(_container1);
         root.Add(_container2);
         _parameter = new Parameter().WithName("Dynamic");
         _container1.Add(_parameter);
         _subContainer1.Add(_p1);
         //this parameter is not in the same parent and should not be used
         _subContainer2.Add(_p2);
         root.Add(new Parameter().WithName("p3").WithFormula(new ConstantFormula(3)));
         var sumFormula = new SumFormula
         {
            Criteria = Create.Criteria(x => x.With("toto").InParent())
         };
         _parameter.Formula = sumFormula;
      }

      protected override void Because()
      {
         sut.ExpandDynamicFormulaIn(_model);
      }

      [Observation]
      public void should_replace_all_dynamic_formula_with_the_corresponding_explicit_formula()
      {
         _parameter.Formula.IsExplicit().ShouldBeTrue();
      }

      [Observation]
      public void should_have_created_an_explicit_formula_that_is_the_sum_of_the_defined_parameters()
      {
         var explicitFormula = _parameter.Formula.DowncastTo<ExplicitFormula>();
         explicitFormula.FormulaString.ShouldBeEqualTo("P_1");
         explicitFormula.ObjectPaths.Count().ShouldBeEqualTo(1);
      }
   }

   internal class When_expanding_all_dynamic_formula_in_a_model_using_in_parent_criteria_but_the_criteria_is_OR : concern_for_FormulaTask
   {
      private IModel _model;
      private IParameter _parameter;
      private IParameter _p1;
      private IParameter _p2;
      private Container _container1;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _objectBaseFactory.Create<ExplicitFormula>()).Returns(new ExplicitFormula());
         _model = new Model();
         _container1 = new Container().WithName("Container1");

         _p1 = new Parameter().WithName("p1").WithFormula(new ConstantFormula(1));
         _p1.AddTag("toto");
         _p2 = new Parameter().WithName("p2").WithFormula(new ConstantFormula(2));
         _p2.AddTag("toto");
         var root = new Container();
         _model.Root = root;
         root.Add(_container1);
         _parameter = new Parameter().WithName("Dynamic");
         _container1.Add(_parameter);
         _container1.Add(_p1);
         var sumFormula = new SumFormula
         {
            Criteria = Create.Criteria(x => x.With("toto").InParent())
         };
         sumFormula.Criteria.Operator = CriteriaOperator.Or;
         _parameter.Formula = sumFormula;
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.ExpandDynamicFormulaIn(_model)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   internal class When_expanding_all_dynamic_formula_in_a_model_using_in_children_criteria : concern_for_FormulaTask
   {
      private IModel _model;
      private IParameter _sumVolume;
      private IParameter _volume1_1;
      private IParameter _volume1_2;
      private IParameter _volume2_1;
      private Container _container1;
      private Container _container2;
      private Container _subContainer1_1;
      private Container _subContainer1_2;
      private Container _subContainer2_1;
      private Parameter _volume1;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _objectBaseFactory.Create<ExplicitFormula>()).Returns(new ExplicitFormula());
         _model = new Model();
         _container1 = new Container().WithName("Container1");
         _container2 = new Container().WithName("Container2");
         _subContainer1_1 = new Container().WithName("SubContainer1_1").WithParentContainer(_container1);
         _subContainer1_2 = new Container().WithName("SubContainer1_2").WithParentContainer(_container1);
         _subContainer2_1 = new Container().WithName("SubContainer2_1").WithParentContainer(_container2);

         _volume1_1 = new Parameter().WithName("volume").WithFormula(new ConstantFormula(1));
         _volume1_2 = new Parameter().WithName("volume").WithFormula(new ConstantFormula(2));
         _volume2_1 = new Parameter().WithName("volume").WithFormula(new ConstantFormula(3));
         _volume1 = new Parameter().WithName("volume").WithFormula(new ConstantFormula(4));
         _sumVolume = new Parameter().WithName("dynamic").WithFormula(new ConstantFormula(3));

         //simulate a real ROOT Container
         var root = new Container().WithContainerType(ContainerType.Simulation).WithName("root");
         _model.Root = root;

         //creating the following structure
         //root
         //  |Container1
         //  |  |Dynamic = sum of all volume in children of Container 1
         //  |  |volume = 4 (this parameter should not be used)
         //  |  |SubContainer1_1
         //  |  |  |volume = 1
         //  |  |SubContainer1_2
         //  |  |  |volume = 2
         //  |Container2
         //  |  |SubContainer2_2
         //  |  |  |volume = 3 (this parameter should not be used)


         root.Add(_container1);
         root.Add(_container2);

         _container1.Add(_sumVolume);
         _container1.Add(_volume1);
         _subContainer1_1.Add(_volume1_1);
         _subContainer1_2.Add(_volume1_2);
         //this parameter is not in the same parent and should not be used
         _subContainer2_1.Add(_volume2_1);
         var sumFormula = new SumFormula
         {
            Criteria = Create.Criteria(x => x.With("volume").InChildren())
         };
         _sumVolume.Formula = sumFormula;
      }

      protected override void Because()
      {
         sut.ExpandDynamicFormulaIn(_model);
      }

      [Observation]
      public void should_replace_all_dynamic_formula_with_the_corresponding_explicit_formula()
      {
         _sumVolume.Formula.IsExplicit().ShouldBeTrue();
      }

      [Observation]
      public void should_have_created_an_explicit_formula_that_is_the_sum_of_the_defined_parameters()
      {
         var explicitFormula = _sumVolume.Formula.DowncastTo<ExplicitFormula>();
         //volume_1_1 + volume_1_2 but not volume_2_1 and not volume1
         explicitFormula.FormulaString.ShouldBeEqualTo("P_1 + P_2");
         explicitFormula.ObjectPaths.Count().ShouldBeEqualTo(2);
      }
   }

   internal class When_replacing_the_neighborhood_keyword_in_a_well_defined_path : concern_for_FormulaTask
   {
      private IParameter _liverCellParameter;
      private IContainer _rootContainer;
      private FormulaUsablePath _objectPath;
      private Container _liver;
      private Container _liverCell;
      private Container _liverPlasma;
      private Neighborhood _neighborhood_liver_cell_liver_pls;
      private IParameter _neighborhoodParameter;
      private Model _model;

      protected override void Context()
      {
         base.Context();
         _model = new Model();
         _rootContainer = new Container().WithName("ROOT");
         _liver = new Container().WithName("Liver");
         _liverCell = new Container().WithName("Intracellular").WithParentContainer(_liver);
         _liverPlasma = new Container().WithName("Plasma").WithParentContainer(_liver);

         _neighborhood_liver_cell_liver_pls = new Neighborhood
         {
            FirstNeighbor = _liverCell,
            SecondNeighbor = _liverPlasma
         }.WithName("_neighborhood_liver_cell_liver_pls");

         _model.Root = _rootContainer;
         _model.Neighborhoods = new Container().WithName(NEIGHBORHOODS);
         _model.Neighborhoods.Add(_neighborhood_liver_cell_liver_pls);
         _neighborhoodParameter = DomainHelperForSpecs.ConstantParameterWithValue(10).WithName("K").WithParentContainer(_neighborhood_liver_cell_liver_pls);
         _liverCellParameter = new Parameter().WithName("P").WithParentContainer(_liverCell);

         _rootContainer.Add(_liver);
         _liverCellParameter.Formula = new ExplicitFormula("K+10");
         //..|<NBH>|..|..|Plasma|<NBH>|K
         _objectPath = new FormulaUsablePath(PARENT_CONTAINER, NBH, PARENT_CONTAINER, PARENT_CONTAINER, "Plasma", NBH, _neighborhoodParameter.Name) {Alias = "K"};
         _liverCellParameter.Formula.AddObjectPath(_objectPath);
      }

      protected override void Because()
      {
         sut.ExpandNeighborhoodReferencesIn(_model.Root);
      }

      [Observation]
      public void should_have_replaced_the_nbh_with_the_actual_path_to_the_neighborhood()
      {
         _liverCellParameter.Value.ShouldBeEqualTo(20);
      }
   }

   internal class When_replacing_the_neighborhood_keyword_in_a_path_missing_one_container : concern_for_FormulaTask
   {
      private IParameter _liverCellParameter;
      private IContainer _rootContainer;
      private FormulaUsablePath _objectPath;
      private Container _liver;
      private Container _liverCell;
      private Container _liverPlasma;
      private Neighborhood _neighborhood_liver_cell_liver_pls;
      private IParameter _neighborhoodParameter;
      private Model _model;

      protected override void Context()
      {
         base.Context();
         _model = new Model();
         _rootContainer = new Container().WithName("ROOT");
         _liver = new Container().WithName("Liver");
         _liverCell = new Container().WithName("Intracellular").WithParentContainer(_liver);
         _liverPlasma = new Container().WithName("Plasma").WithParentContainer(_liver);

         _neighborhood_liver_cell_liver_pls = new Neighborhood
         {
            FirstNeighbor = _liverCell,
            SecondNeighbor = _liverPlasma
         };

         _model.Root = _rootContainer;
         _model.Neighborhoods = new Container().WithName(NEIGHBORHOODS);
         _model.Neighborhoods.Add(_neighborhood_liver_cell_liver_pls);
         _neighborhoodParameter = DomainHelperForSpecs.ConstantParameterWithValue(10).WithName("K").WithParentContainer(_neighborhood_liver_cell_liver_pls);
         _liverCellParameter = new Parameter().WithName("P").WithParentContainer(_liverCell);

         _rootContainer.Add(_liver);
         _liverCellParameter.Formula = new ExplicitFormula("K+10");
         //..|<NBH>|..|..|Interstitial|<NBH>|K_<==_does_not_exist_in_the_model
         _objectPath = new FormulaUsablePath(PARENT_CONTAINER, NBH, PARENT_CONTAINER, PARENT_CONTAINER, "Interstitial", NBH, _neighborhoodParameter.Name) {Alias = "K"};
         _liverCellParameter.Formula.AddObjectPath(_objectPath);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.ExpandNeighborhoodReferencesIn(_model.Root)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   internal class When_replacing_the_neighborhood_keyword_in_between_two_containers_without_neighborhood : concern_for_FormulaTask
   {
      private IParameter _liverCellParameter;
      private IContainer _rootContainer;
      private FormulaUsablePath _objectPath;
      private Container _liver;
      private Container _liverCell;
      private Neighborhood _neighborhood_liver_cell_liver_pls;
      private IParameter _neighborhoodParameter;
      private Model _model;
      private Container _plasmaCEll;

      protected override void Context()
      {
         base.Context();
         _model = new Model();
         _rootContainer = new Container().WithName("ROOT");
         _liver = new Container().WithName("Liver");
         _liverCell = new Container().WithName("Intracellular").WithParentContainer(_liver);
         _plasmaCEll = new Container().WithName("Plasma").WithParentContainer(_liver);

         _neighborhood_liver_cell_liver_pls = new Neighborhood();

         _model.Root = _rootContainer;
         _model.Neighborhoods = new Container().WithName(NEIGHBORHOODS);
         _model.Neighborhoods.Add(_neighborhood_liver_cell_liver_pls);
         _neighborhoodParameter = DomainHelperForSpecs.ConstantParameterWithValue(10).WithName("K").WithParentContainer(_neighborhood_liver_cell_liver_pls);
         _liverCellParameter = new Parameter().WithName("P").WithParentContainer(_liverCell);

         _rootContainer.Add(_liver);
         _liverCellParameter.Formula = new ExplicitFormula("K+10");
         //..|<NBH>|..|..|Plasma|<NBH>|K_<==_does_not_exist_in_the_model
         _objectPath = new FormulaUsablePath(PARENT_CONTAINER, NBH, PARENT_CONTAINER, PARENT_CONTAINER, "Plasma", NBH, _neighborhoodParameter.Name) {Alias = "K"};
         _liverCellParameter.Formula.AddObjectPath(_objectPath);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.ExpandNeighborhoodReferencesIn(_model.Root)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   internal abstract class concern_for_lumen_segment_path_resolution : concern_for_FormulaTask
   {
      protected FormulaUsablePath _objectPath;
      protected Container _smallIntestine;
      protected Model _model;
      protected Container _mucosa;
      protected Container _duodenumMucosa;
      protected Container _organism;
      protected Container _lumen;
      protected Container _duodenumLumen;
      protected IParameter _volumeDuodenumLumen;
      protected Container _duodenumMucosaIntracellular;
      protected IParameter _height;
      private Container _root;
      protected ModelConfiguration _modelConfiguration;

      protected override void Context()
      {
         base.Context();
         _root = new Container().WithName(ROOT);
         _model = new Model();
         _organism = new Container().WithName(ORGANISM).Under(_root);
         _height = DomainHelperForSpecs.ConstantParameterWithValue(1).WithName("Height").Under(_organism);
         _smallIntestine = new Container().WithName("SmallIntestine").Under(_organism);
         _mucosa = new Container().WithName(MUCOSA).Under(_smallIntestine);
         _duodenumMucosa = new Container().WithName(Compartment.DUODENUM).Under(_mucosa);
         _duodenumMucosaIntracellular = new Container().WithName("Intracellular").Under(_duodenumMucosa);

         _lumen = new Container().WithName(LUMEN).Under(_organism);
         _duodenumLumen = new Container().WithName(Compartment.DUODENUM).Under(_lumen);
         _volumeDuodenumLumen = DomainHelperForSpecs.ConstantParameterWithValue(10).WithName(VOLUME).Under(_duodenumLumen);


         _model.Root = _root;
         var simulationConfiguration = new SimulationConfiguration();
         var simulationBuilder = A.Fake<SimulationBuilder>();

         _modelConfiguration = new ModelConfiguration(_model, simulationConfiguration, simulationBuilder);
      }
   }

   internal class When_replacing_the_lumen_segment_keyword_in_a_well_defined_absolute_path : concern_for_lumen_segment_path_resolution
   {
      protected Parameter _parameterReferencingAbsoluteLumenSegment;

      protected override void Context()
      {
         base.Context();

         _parameterReferencingAbsoluteLumenSegment = new Parameter().WithName("P").WithParentContainer(_organism);
         _parameterReferencingAbsoluteLumenSegment.Formula = new ExplicitFormula("V+10");
         //Organism|SmallIntestine|Mucosa|Duodenum|LumenSegment|V
         _objectPath = new FormulaUsablePath(ORGANISM, _smallIntestine.Name, _mucosa.Name, _duodenumMucosa.Name, LUMEN_SEGMENT, _volumeDuodenumLumen.Name) {Alias = "V"};
         _parameterReferencingAbsoluteLumenSegment.Formula.AddObjectPath(_objectPath);
         _parameterReferencingAbsoluteLumenSegment.Formula.AddObjectPath(new FormulaUsablePath(ORGANISM, _height.Name) {Alias = "H"});

         //to mimic what happens in the model construction, we should expand keywords
         _keywordReplacerTask.ReplaceIn(_modelConfiguration);
      }

      protected override void Because()
      {
         sut.ExpandLumenSegmentReferencesIn(_model.Root);
      }

      [Observation]
      public void should_have_replaced_the_lumen_segment_with_the_actual_path_to_the_lumen_segment()
      {
         _parameterReferencingAbsoluteLumenSegment.Value.ShouldBeEqualTo(20);
      }
   }

   internal class When_replacing_the_lumen_segment_keyword_in_a_well_defined_relative_path : concern_for_lumen_segment_path_resolution
   {
      protected Parameter _parameterReferencingRelativeLumenSegment;

      protected override void Context()
      {
         base.Context();

         _parameterReferencingRelativeLumenSegment = new Parameter().WithName("P").WithParentContainer(_duodenumMucosaIntracellular);
         _parameterReferencingRelativeLumenSegment.Formula = new ExplicitFormula("V+10");
         //..|..|LumenSegment|V
         _objectPath = new FormulaUsablePath(PARENT_CONTAINER, PARENT_CONTAINER, LUMEN_SEGMENT, _volumeDuodenumLumen.Name) {Alias = "V"};
         _parameterReferencingRelativeLumenSegment.Formula.AddObjectPath(_objectPath);

         //to mimic what happens in the model construction, we should expand keywords
         _keywordReplacerTask.ReplaceIn(_modelConfiguration);
      }

      protected override void Because()
      {
         sut.ExpandLumenSegmentReferencesIn(_model.Root);
      }

      [Observation]
      public void should_have_replaced_the_lumen_segment_with_the_actual_path_to_the_lumen_segment()
      {
         _parameterReferencingRelativeLumenSegment.Value.ShouldBeEqualTo(20);
      }
   }

   internal class When_replacing_the_lumen_segment_keyword_in_using_a_relative_path_to_a_container_that_does_not_exist_in_lumen : concern_for_lumen_segment_path_resolution
   {
      protected Parameter _parameterReferencingRelativeLumenSegment;

      protected override void Context()
      {
         base.Context();
         //we put the parameter in smallIntestine and the path will be looking for this
         _parameterReferencingRelativeLumenSegment = new Parameter().WithName("P").WithParentContainer(_smallIntestine);
         _parameterReferencingRelativeLumenSegment.Formula = new ExplicitFormula("V+10");
         //..|LumenSegment|V which will be SmallIntestine|LumenSegment which will become Organism|Lumen|SmallIntestine which does not exist in our context
         _objectPath = new FormulaUsablePath(PARENT_CONTAINER, LUMEN_SEGMENT, _volumeDuodenumLumen.Name) {Alias = "V"};
         _parameterReferencingRelativeLumenSegment.Formula.AddObjectPath(_objectPath);

         //to mimic what happens in the model construction, we should expand keywords
         _keywordReplacerTask.ReplaceIn(_modelConfiguration);
      }

      protected override void Because()
      {
         sut.ExpandLumenSegmentReferencesIn(_model.Root);
      }

      [Observation]
      public void evaluating_the_formula_should_throw_an_exception()
      {
         The.Action(() =>
         {
            var x = _parameterReferencingRelativeLumenSegment.Value;
         }).ShouldThrowAn<OSPSuiteException>();
      }
   }

   internal class When_replacing_the_lumen_segment_keyword_in_using_absolute_path_to_a_container_that_does_not_exist_in_lumen : concern_for_lumen_segment_path_resolution
   {
      protected Parameter _parameterReferencingAbsoluteLumenSegment;

      protected override void Context()
      {
         base.Context();
         //we put the parameter in smallIntestine and the path will be looking for this
         _parameterReferencingAbsoluteLumenSegment = new Parameter().WithName("P").WithParentContainer(_smallIntestine);
         _parameterReferencingAbsoluteLumenSegment.Formula = new ExplicitFormula("V+10");
         //..|LumenSegment|V which will be SmallIntestine|LumenSegment which will become Organism|Lumen|SmallIntestine which does not exist in our context
         _objectPath = new FormulaUsablePath(ORGANISM, _smallIntestine.Name, LUMEN_SEGMENT, _volumeDuodenumLumen.Name) {Alias = "V"};
         _parameterReferencingAbsoluteLumenSegment.Formula.AddObjectPath(_objectPath);

         //to mimic what happens in the model construction, we should expand keywords
         _keywordReplacerTask.ReplaceIn(_modelConfiguration);
      }

      protected override void Because()
      {
         sut.ExpandLumenSegmentReferencesIn(_model.Root);
      }

      [Observation]
      public void evaluating_the_formula_should_throw_an_exception()
      {
         The.Action(() =>
         {
            var x = _parameterReferencingAbsoluteLumenSegment.Value;
         }).ShouldThrowAn<OSPSuiteException>();
      }
   }

   internal class When_using_the_lumen_segment_keyword_in_path_multiple_times : concern_for_lumen_segment_path_resolution
   {
      protected Parameter _parameterReferencingAbsoluteLumenSegment;

      protected override void Context()
      {
         base.Context();

         _parameterReferencingAbsoluteLumenSegment = new Parameter().WithName("P").WithParentContainer(_organism);
         _parameterReferencingAbsoluteLumenSegment.Formula = new ExplicitFormula("V+10");
         //Organism|SmallIntestine|Mucosa|Duodenum|LumenSegment|V
         _objectPath = new FormulaUsablePath(ORGANISM, _smallIntestine.Name, LUMEN_SEGMENT, _duodenumMucosa.Name, LUMEN_SEGMENT, _volumeDuodenumLumen.Name) {Alias = "V"};
         _parameterReferencingAbsoluteLumenSegment.Formula.AddObjectPath(_objectPath);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.ExpandLumenSegmentReferencesIn(_model.Root)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   internal class When_using_the_lumen_segment_keyword_at_the_beginning_of_a_path : concern_for_lumen_segment_path_resolution
   {
      protected Parameter _parameterReferencingAbsoluteLumenSegment;

      protected override void Context()
      {
         base.Context();

         _parameterReferencingAbsoluteLumenSegment = new Parameter().WithName("P").WithParentContainer(_organism);
         _parameterReferencingAbsoluteLumenSegment.Formula = new ExplicitFormula("V+10");
         //Organism|SmallIntestine|Mucosa|Duodenum|LumenSegment|V
         _objectPath = new FormulaUsablePath(LUMEN_SEGMENT, _smallIntestine.Name, _volumeDuodenumLumen.Name) {Alias = "V"};
         _parameterReferencingAbsoluteLumenSegment.Formula.AddObjectPath(_objectPath);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.ExpandLumenSegmentReferencesIn(_model.Root)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   internal abstract class concern_for_lumen_next_previous_segment_path_resolution : concern_for_FormulaTask
   {
      protected FormulaUsablePath _objectPath;
      protected Model _model;
      protected Container _organism;
      protected Container _lumen;
      protected IParameter _volumeDuodenumLumen;
      protected IParameter _height;
      protected Container _stomachLumen;
      protected Container _duodenumLumen;
      protected Container _upperJejunumLumen;
      protected Container _rectumLumen;
      protected Container _liver;
      private Container _root;

      protected override void Context()
      {
         base.Context();
         _root = new Container().WithName(ROOT);
         _model = new Model();
         _organism = new Container().WithName(ORGANISM).Under(_root);
         _height = DomainHelperForSpecs.ConstantParameterWithValue(1).WithName("Height").Under(_organism);
         _lumen = new Container().WithName(LUMEN).Under(_organism);
         _liver = new Container().WithName("Liver").Under(_organism);
         _stomachLumen = new Container().WithName(Compartment.STOMACH).Under(_lumen);
         _duodenumLumen = new Container().WithName(Compartment.DUODENUM).Under(_lumen);
         _upperJejunumLumen = new Container().WithName(Compartment.UPPER_JEJUNUM).Under(_lumen);
         _rectumLumen = new Container().WithName(Compartment.RECTUM).Under(_lumen);
         _volumeDuodenumLumen = DomainHelperForSpecs.ConstantParameterWithValue(10).WithName(VOLUME).Under(_duodenumLumen);


         _model.Root = _root;
      }
   }

   internal class When_replacing_the_lumen_next_segment_keyword_in_a_well_defined_relative_path : concern_for_lumen_next_previous_segment_path_resolution
   {
      protected Parameter _parameterReferencingRelativeLumenNextSegment;

      protected override void Context()
      {
         base.Context();

         _parameterReferencingRelativeLumenNextSegment = new Parameter().WithName("P").WithParentContainer(_stomachLumen);
         _parameterReferencingRelativeLumenNextSegment.Formula = new ExplicitFormula("V+10");
         //..|LUMEN_NEXT_SEGMENT|V
         _objectPath = new FormulaUsablePath(PARENT_CONTAINER, LUMEN_NEXT_SEGMENT, _volumeDuodenumLumen.Name) {Alias = "V"};
         _parameterReferencingRelativeLumenNextSegment.Formula.AddObjectPath(_objectPath);
      }

      protected override void Because()
      {
         sut.ExpandLumenSegmentReferencesIn(_model.Root);
      }

      [Observation]
      public void should_have_replaced_the_lumen_segment_with_the_actual_path_to_the_lumen_segment()
      {
         _parameterReferencingRelativeLumenNextSegment.Value.ShouldBeEqualTo(20);
      }
   }

   internal class When_replacing_the_lumen_previous_segment_keyword_in_a_well_defined_relative_path : concern_for_lumen_next_previous_segment_path_resolution
   {
      protected Parameter _parameterReferencingRelativeLumenNextSegment;

      protected override void Context()
      {
         base.Context();

         _parameterReferencingRelativeLumenNextSegment = new Parameter().WithName("P").WithParentContainer(_upperJejunumLumen);
         _parameterReferencingRelativeLumenNextSegment.Formula = new ExplicitFormula("V+10");
         //..|LUMEN_PREVIOUS_SEGMENT|V
         _objectPath = new FormulaUsablePath(PARENT_CONTAINER, LUMEN_PREVIOUS_SEGMENT, _volumeDuodenumLumen.Name) {Alias = "V"};
         _parameterReferencingRelativeLumenNextSegment.Formula.AddObjectPath(_objectPath);
      }

      protected override void Because()
      {
         sut.ExpandLumenSegmentReferencesIn(_model.Root);
      }

      [Observation]
      public void should_have_replaced_the_lumen_segment_with_the_actual_path_to_the_lumen_segment()
      {
         _parameterReferencingRelativeLumenNextSegment.Value.ShouldBeEqualTo(20);
      }
   }

   internal class When_replacing_the_lumen_previous_segment_keyword_in_a_well_defined_relative_path_in_a_formula_for_RHS : concern_for_lumen_next_previous_segment_path_resolution
   {
      protected Parameter _parameterReferencingRelativeLumenNextSegment;

      protected override void Context()
      {
         base.Context();

         _parameterReferencingRelativeLumenNextSegment = new Parameter().WithName("P").WithParentContainer(_upperJejunumLumen);
         _parameterReferencingRelativeLumenNextSegment.WithFormula(new ConstantFormula(5));
         _parameterReferencingRelativeLumenNextSegment.RHSFormula = new ExplicitFormula("V+10");
         //..|LUMEN_PREVIOUS_SEGMENT|V
         _objectPath = new FormulaUsablePath(PARENT_CONTAINER, LUMEN_PREVIOUS_SEGMENT, _volumeDuodenumLumen.Name) {Alias = "V"};
         _parameterReferencingRelativeLumenNextSegment.RHSFormula.AddObjectPath(_objectPath);
      }

      protected override void Because()
      {
         sut.ExpandLumenSegmentReferencesIn(_model.Root);
      }

      [Observation]
      public void should_have_replaced_the_lumen_segment_with_the_actual_path_to_the_lumen_segment()
      {
         _parameterReferencingRelativeLumenNextSegment.RHSFormula.Calculate(_parameterReferencingRelativeLumenNextSegment).ShouldBeEqualTo(20);
      }
   }

   internal class When_replacing_the_lumen_next_segment_keyword_in_a_container_that_has_no_next_segment_such_as_rectum : concern_for_lumen_next_previous_segment_path_resolution
   {
      protected Parameter _parameterReferencingRelativeLumenNextSegment;

      protected override void Context()
      {
         base.Context();

         _parameterReferencingRelativeLumenNextSegment = new Parameter().WithName("P").WithParentContainer(_rectumLumen);
         _parameterReferencingRelativeLumenNextSegment.Formula = new ExplicitFormula("V+10");
         //..|LUMEN_NEXT_SEGMENT|V
         _objectPath = new FormulaUsablePath(PARENT_CONTAINER, LUMEN_NEXT_SEGMENT, _volumeDuodenumLumen.Name) {Alias = "V"};
         _parameterReferencingRelativeLumenNextSegment.Formula.AddObjectPath(_objectPath);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.ExpandLumenSegmentReferencesIn(_model.Root)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   internal class When_replacing_the_lumen_next_segment_keyword_in_a_container_that_is_not_a_lumen_segment : concern_for_lumen_next_previous_segment_path_resolution
   {
      protected Parameter _parameterReferencingRelativeLumenNextSegment;

      protected override void Context()
      {
         base.Context();

         _parameterReferencingRelativeLumenNextSegment = new Parameter().WithName("P").WithParentContainer(_liver);
         _parameterReferencingRelativeLumenNextSegment.Formula = new ExplicitFormula("V+10");
         //..|LUMEN_NEXT_SEGMENT|V
         _objectPath = new FormulaUsablePath(PARENT_CONTAINER, LUMEN_NEXT_SEGMENT, _volumeDuodenumLumen.Name) {Alias = "V"};
         _parameterReferencingRelativeLumenNextSegment.Formula.AddObjectPath(_objectPath);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.ExpandLumenSegmentReferencesIn(_model.Root)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   internal class When_replacing_the_lumen_previous_segment_keyword_in_a_container_that_has_no_next_segment_such_as_stomach : concern_for_lumen_next_previous_segment_path_resolution
   {
      protected Parameter _parameterReferencingRelativeLumenNextSegment;

      protected override void Context()
      {
         base.Context();

         _parameterReferencingRelativeLumenNextSegment = new Parameter().WithName("P").WithParentContainer(_stomachLumen);
         _parameterReferencingRelativeLumenNextSegment.Formula = new ExplicitFormula("V+10");
         //..|LUMEN_PREVIOUS_SEGMENT|V
         _objectPath = new FormulaUsablePath(PARENT_CONTAINER, LUMEN_PREVIOUS_SEGMENT, _volumeDuodenumLumen.Name) {Alias = "V"};
         _parameterReferencingRelativeLumenNextSegment.Formula.AddObjectPath(_objectPath);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.ExpandLumenSegmentReferencesIn(_model.Root)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   internal class When_expanding_a_dynamic_formula_in_a_model_at_a_formula_useable_satisfying_their_own_criteria :
      concern_for_FormulaTask
   {
      private IModel _model;
      private IParameter _parameter;
      private IParameter _p1;
      private IParameter _p2;
      private Exception _exception;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _objectBaseFactory.Create<ExplicitFormula>()).Returns(new ExplicitFormula());
         _model = new Model();
         _p1 = new Parameter().WithName("p1").WithFormula(new ConstantFormula(1));
         _p1.AddTag("toto");
         _p2 = new Parameter().WithName("p2").WithFormula(new ConstantFormula(2));
         _p2.AddTag("toto");
         var root = new Container();
         _model.Root = root;
         _parameter = new Parameter().WithName("Dynamic");
         root.Add(_parameter);
         root.Add(_p1);
         root.Add(_p2);
         root.Add(new Parameter().WithName("p3").WithFormula(new ConstantFormula(3)));

         var sumFormula = new SumFormula();
         sumFormula.Criteria = Create.Criteria(x => x.With("toto"));
         _parameter.Formula = sumFormula;
         _parameter.AddTag("toto");
         root.Add(_parameter);
      }

      protected override void Because()
      {
         try
         {
            sut.ExpandDynamicFormulaIn(_model);
         }
         catch (Exception e)
         {
            _exception = e;
         }
      }

      [Observation]
      public void should_throw_circular_reference_from_sum_formula_Exception()
      {
         _exception.ShouldBeAnInstanceOf<CircularReferenceInSumFormulaException>();
      }
   }

   internal class When_adding_a_reference_to_the_volume_of_parent_container_for_a_formula_without_any_reference_to_volume : concern_for_FormulaTask
   {
      private IFormula _explicitFormula;
      private string _alias;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula();
      }

      protected override void Because()
      {
         _alias = sut.AddParentVolumeReferenceToFormula(_explicitFormula);
      }

      [Observation]
      public void should_simply_add_it_and_return_the_volume_alias()
      {
         _alias.ShouldBeEqualTo(VOLUME_ALIAS);
         var volumeReference = _explicitFormula.ObjectPaths.Find(x => x.Alias == VOLUME_ALIAS);
         volumeReference.ShouldNotBeNull();
         volumeReference.PathAsString.ShouldBeEqualTo(new[] {PARENT_CONTAINER, VOLUME}.ToPathString());
      }
   }

   internal class When_adding_a_reference_to_the_volume_of_parent_container_for_an_existing_reference_to_volume : concern_for_FormulaTask
   {
      private IFormula _explicitFormula;
      private string _alias;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula();
         _explicitFormula.AddObjectPath(new FormulaUsablePath(PARENT_CONTAINER, VOLUME).WithAlias(VOLUME_ALIAS));
      }

      protected override void Because()
      {
         _alias = sut.AddParentVolumeReferenceToFormula(_explicitFormula);
      }

      [Observation]
      public void should_simply_use_it()
      {
         _alias.ShouldBeEqualTo(VOLUME_ALIAS);
      }
   }

   internal class When_adding_a_reference_to_the_volume_of_parent_container_for_an_existing_reference_to_volume_but_with_a_different_path : concern_for_FormulaTask
   {
      private IFormula _explicitFormula;
      private string _alias;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula();
         //another path
         _explicitFormula.AddObjectPath(new FormulaUsablePath(PARENT_CONTAINER, PARENT_CONTAINER, VOLUME, VOLUME).WithAlias(VOLUME_ALIAS));
      }

      protected override void Because()
      {
         _alias = sut.AddParentVolumeReferenceToFormula(_explicitFormula);
      }

      [Observation]
      public void should_not_use_the_default_volume_alias()
      {
         _alias.ShouldNotBeEqualTo(VOLUME_ALIAS);
      }
   }

   internal class When_validating_the_formula_origin_id : concern_for_FormulaTask
   {
      private IModel _model;
      private ExplicitFormula _rhsFormula1;
      private ExplicitFormula _rhsFormula2;

      protected override void Context()
      {
         base.Context();
         _model = new Model {Root = new Container()};
         var parameter1 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(1));
         _rhsFormula1 = new ExplicitFormula("A+B").WithOriginId("ORIGIN");
         _rhsFormula1.AddObjectPath(new FormulaUsablePath("A", "B"));
         parameter1.RHSFormula = _rhsFormula1;

         var parameter2 = new Parameter().WithName("P2").WithFormula(new ConstantFormula(2));
         _rhsFormula2 = new ExplicitFormula("A+B").WithOriginId("ORIGIN");
         _rhsFormula2.AddObjectPath(new FormulaUsablePath("A", "C"));

         parameter2.RHSFormula = _rhsFormula2;

         _model.Root.Add(parameter1);
         _model.Root.Add(parameter2);
      }

      protected override void Because()
      {
         sut.CheckFormulaOriginIn(_model);
      }

      [Observation]
      public void should_reset_the_origin_id_of_rhs_formula_with_different_object_path()
      {
         _rhsFormula2.OriginId.ShouldBeEmpty();
      }
   }
}
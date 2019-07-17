using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_TransportBuilder : ContextSpecification<ITransportBuilder>
   {

      protected override void Context()
      {
         var dim = new Dimension();
         dim.AddUnit("mg", 1, 0);

         sut = new TransportBuilder()
            .WithName("T1")
            .WithDimension(dim)
            .WithKinetic(new ConstantFormula(99));

         sut.AddParameter(new Parameter().WithName("P1").WithId("P1"));
         sut.Description = "Tralala";

         sut.SourceCriteria = new DescriptorCriteria();
         sut.SourceCriteria.Add(new MatchTagCondition("x"));
         sut.SourceCriteria.Add(new NotMatchTagCondition("y"));

         sut.TargetCriteria = new DescriptorCriteria();
         sut.TargetCriteria.Add(new MatchTagCondition("z"));
         sut.TargetCriteria.Add(new NotMatchTagCondition("t"));
         sut.TargetCriteria.Add(new NotMatchTagCondition("t2"));

         sut.TransportType = TransportType.Diffusion;
      }
   }

   public class When_checking_if_molecule_should_be_transported_and_ForAll_is_true : concern_for_TransportBuilder

   {
      protected override void Context()
      {
         base.Context();

         sut.AddMoleculeName("A");
         sut.AddMoleculeNameToExclude("B");

         sut.ForAll = true;
      }

      [Observation]
      public void should_transport_molecule_which_is_not_contained_in_any_list()
      {
         sut.TransportsMolecule("X").ShouldBeTrue();
      }

      [Observation]
      public void should_transport_molecule_contained_in_include_list()
      {
         sut.TransportsMolecule("A").ShouldBeTrue();
      }

      [Observation]
      public void should_not_transport_molecule_contained_in_exclude_list()
      {
         sut.TransportsMolecule("B").ShouldBeFalse();
      }
   }

   public class When_checking_if_molecule_should_be_transported_and_ForAll_is_false : concern_for_TransportBuilder
   {
      protected override void Context()
      {
         base.Context();

         sut.AddMoleculeName("A");
         sut.AddMoleculeNameToExclude("B");

         sut.ForAll = false;
      }

      [Observation]
      public void should_not_transport_molecule_which_is_not_contained_in_any_list()
      {
         sut.TransportsMolecule("X").ShouldBeFalse();
      }

      [Observation]
      public void should_transport_molecule_contained_in_include_list()
      {
         sut.TransportsMolecule("A").ShouldBeTrue();
      }

      [Observation]
      public void should_not_transport_molecule_contained_in_exclude_list()
      {
         sut.TransportsMolecule("B").ShouldBeFalse();
      }
   }

   public class When_setting_molecule_names : concern_for_TransportBuilder
   {
      protected override void Context()
      {
         base.Context();

         sut.AddMoleculeName("A");
         sut.AddMoleculeNameToExclude("B");
      }

      [Observation]
      public void adding_existing_molecule_name_should_do_nothing()
      {
         sut.AddMoleculeName("A");
         sut.MoleculeNames().Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void adding_existing_molecule_name_to_exclude_should_not_add_the_molecule_tweice()
      {
         sut.AddMoleculeNameToExclude("B");
         sut.MoleculeNamesToExclude().Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void adding_molecule_name_already_defined_in_exclude_list_should_throw_an_exception()
      {
         The.Action(() => sut.AddMoleculeName("B")).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void adding_molecule_name_to_exclude_already_defined_in_include_list_should_throw_an_exception()
      {
         The.Action(() => sut.AddMoleculeNameToExclude("A")).ShouldThrowAn<OSPSuiteException>();
      }
   }

   public class when_cloning_transport_builder_for_building_block : concern_for_TransportBuilder
   {
      protected ITransportBuilder _clone;
      protected ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      private IDataRepositoryTask _dataRepositoryTask;

      protected override void Context()
      {
         base.Context();
         var container = A.Fake<Utility.Container.IContainer>();
         var noDimension = A.Fake<IDimension>();
         var dimensionFactory = A.Fake<IDimensionFactory>();
         A.CallTo(() => dimensionFactory.NoDimension).Returns(noDimension);

         var objectBaseFactory = new ObjectBaseFactory(container, dimensionFactory, new IdGenerator(), A.Fake<ICreationMetaDataFactory>());
         _dataRepositoryTask = A.Fake<IDataRepositoryTask>();
         _cloneManagerForBuildingBlock = new CloneManagerForBuildingBlock(objectBaseFactory, _dataRepositoryTask);

         sut.ForAll = false;
         sut.AddMoleculeName("A");
         sut.AddMoleculeName("B");
         sut.AddMoleculeNameToExclude("C");
         sut.AddMoleculeNameToExclude("D");
      }

      protected override void Because()
      {
         _clone = _cloneManagerForBuildingBlock.Clone(sut, new FormulaCache());
      }

      [Observation]
      public void cloned_object_should_have_all_properties_cloned()
      {
         _clone.Description.ShouldBeEqualTo(sut.Description);
         _clone.Dimension.Units.Count().ShouldBeEqualTo(sut.Dimension.Units.Count());
         _clone.Dimension.Units.ToArray()[0].Name.ShouldBeEqualTo(sut.Dimension.Units.ToArray()[0].Name);

         _clone.Formula.Calculate(null).ShouldBeEqualTo(sut.Formula.Calculate(null));
         _clone.Name.ShouldBeEqualTo(sut.Name);

         _clone.Parameters.ToArray()[0].Name.ShouldBeEqualTo(sut.Parameters.ToArray()[0].Name);

         _clone.SourceCriteria.Count().ShouldBeEqualTo(sut.SourceCriteria.Count());
         _clone.TargetCriteria.Count().ShouldBeEqualTo(sut.TargetCriteria.Count());

         _clone.MoleculeList.ForAll.ShouldBeEqualTo(sut.ForAll);
         _clone.MoleculeList.MoleculeNames.Count().ShouldBeEqualTo(sut.MoleculeNames().Count());
         _clone.MoleculeList.MoleculeNamesToExclude.Count().ShouldBeEqualTo(sut.MoleculeNamesToExclude().Count());
      }
   }
}
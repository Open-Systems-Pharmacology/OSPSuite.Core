﻿using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_SimulationPKAnalysesImporter : ContextSpecification<ISimulationPKAnalysesImporter>
   {
      private IDimensionFactory _dimensionFactory;
      protected string _fileName;
      protected ImportLogger _importLogger = new ImportLogger();
      protected List<QuantityPKParameter> _results;
      private IModelCoreSimulation _simulation;

      protected override void Context()
      {
         _simulation = A.Fake<IModelCoreSimulation>();   
         _dimensionFactory = new DimensionFactoryForIntegrationTests();
         _dimensionFactory.AddDimension(DomainHelperForSpecs.TimeDimensionForSpecs());
         _dimensionFactory.AddDimension(DomainHelperForSpecs.ConcentrationDimensionForSpecs());
         sut = new SimulationPKAnalysesImporter(_dimensionFactory);
      }

      protected override void Because()
      {
         _results = sut.ImportPKParameters(_fileName,_simulation, _importLogger).ToList();
      }

      protected QuantityPKParameter ParameterFor(string output, string para)
      {
         return _results.FirstOrDefault(x => x.QuantityPath == output && x.Name == para);
      }
   }

   public class When_importing_a_file_containing_pk_analyses : concern_for_SimulationPKAnalysesImporter
   {
      protected override void Context()
      {
         base.Context();
         _fileName = DomainHelperForSpecs.PKAnalysesFilePathFor("many_outputs_and_para");
      }

      [Observation]
      public void should_have_imported_the_expected_number_of_pk_parameters()
      {
         _results.Count.ShouldBeEqualTo(6);
      }

      [Observation]
      public void should_have_imported_the_expected_number_of_values_for_the_defined_pk_parameters()
      {
         ParameterFor("Output1", "Para1").Count.ShouldBeEqualTo(2);
         ParameterFor("Output1", "Para2").Count.ShouldBeEqualTo(2);
         ParameterFor("Output2", "Para1").Count.ShouldBeEqualTo(3);
         ParameterFor("Output2", "Para2").Count.ShouldBeEqualTo(2);
         ParameterFor("Output3", "Para2").Count.ShouldBeEqualTo(5);
         ParameterFor("Output4", "Para2").Count.ShouldBeEqualTo(6);
      }

      [Observation]
      public void should_have_converted_the_value_in_the_base_unit()
      {
         //in base unit already
         var output1Para1 = ParameterFor("Output1", "Para1");
         output1Para1.ValueCache.ShouldContain(10, 11);

         //in h need to be converted to min
         var outputPara2 = ParameterFor("Output1", "Para2");
         outputPara2.ValueCache.ShouldContain(12 * 60, 13 * 60);
      }
   }

   public class When_importing_a_file_that_does_not_have_the_expected_format : concern_for_SimulationPKAnalysesImporter
   {
      protected override void Context()
      {
         base.Context();
         _fileName = DomainHelperForSpecs.SimulationResultsFilePathFor("res_10");
      }

      [Observation]
      public void should_notify_an_error()
      {
         _importLogger.Status.Is(NotificationType.Error).ShouldBeTrue();
      }
   }

   public class When_importing_a_file_with_some_unknown_unit : concern_for_SimulationPKAnalysesImporter
   {
      protected override void Context()
      {
         base.Context();
         _fileName = DomainHelperForSpecs.PKAnalysesFilePathFor("unit_not_found");
      }

      [Observation]
      public void should_have_been_able_to_import_the_file_by_creating_user_defined_units_but_a_warning_will_be_created()
      {
         _importLogger.Status.Is(NotificationType.Warning).ShouldBeTrue();
      }
   }

   public class When_importing_a_file_where_some_individual_values_are_missing : concern_for_SimulationPKAnalysesImporter
   {
      protected override void Context()
      {
         base.Context();
         _fileName = DomainHelperForSpecs.PKAnalysesFilePathFor("missing_individuals");
      }

      [Observation]
      public void should_have_no_errors_to_report()
      {
         _importLogger.Status.Is(NotificationType.Error).ShouldBeFalse(_importLogger.Log.ToString(";"));
      }

      [Observation]
      public void should_have_imported_the_expected_number_of_values()
      {
         var parameter = ParameterFor("Output1", "Para1");
         parameter.Count.ShouldBeEqualTo(3);
         parameter.ValueCache.ShouldOnlyContainInOrder(10, 11, 12);
      }
   }
}
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Helpers;
using OSPSuite.Core.Importer.Services;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_PKParameterSensitivitiesImporter : ContextSpecification<IPKParameterSensitivitiesImporter>
   {
      protected string _fileName;
      protected ImportLogger _importLogger = new ImportLogger();
      private IModelCoreSimulation _simulation;
      protected List<PKParameterSensitivity> _results;

      protected override void Context()
      {
         sut = new PKParameterSensitivitiesImporter();
         _simulation = A.Fake<IModelCoreSimulation>().WithName("Sim");
      }

      protected override void Because()
      {
         _results = sut.ImportFrom(_fileName, _simulation, _importLogger).ToList();
      }
   }

   public class When_importing_a_file_containing_valid_pk_parameter_sensitivity_results : concern_for_PKParameterSensitivitiesImporter
   {
      protected override void Context()
      {
         base.Context();
         _fileName = DomainHelperForSpecs.SensitivityAnalysisResultsFilePathFor("sa_liver_volume");
      }

      [Observation]
      public void should_have_created_one_pk_parameter_sensitivity_value_for_each_entry_in_the_file()
      {
         _results.Count.ShouldBeEqualTo(55);
      }
   }
}
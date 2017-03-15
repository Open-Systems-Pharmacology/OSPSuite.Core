using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ClassificationTypeToRootNodeTypeMapper : ContextSpecification<IClassificationTypeToRootNodeTypeMapper>
   {
      protected override void Context()
      {
         sut = new ClassificationTypeToRootNodeTypeMapper();
      }
   }

   public class When_mapping_a_classification_type_to_a_root_not_type : concern_for_ClassificationTypeToRootNodeTypeMapper
   {
      [Observation]
      public void should_return_the_expected_mapping_for_all_know_classification()
      {
         sut.MapFrom(ClassificationType.ObservedData).ShouldBeEqualTo(RootNodeTypes.ObservedDataFolder);
         sut.MapFrom(ClassificationType.Simulation).ShouldBeEqualTo(RootNodeTypes.SimulationFolder);
         sut.MapFrom(ClassificationType.Comparison).ShouldBeEqualTo(RootNodeTypes.ComparisonFolder);
         sut.MapFrom(ClassificationType.ParameterIdentification).ShouldBeEqualTo(RootNodeTypes.ParameterIdentificationFolder);
         sut.MapFrom(ClassificationType.SensitiviyAnalysis).ShouldBeEqualTo(RootNodeTypes.SensitivityAnalysisFolder);
      }

      [Observation]
      public void should_throw_an_expection_otherwise()
      {
         The.Action(() => sut.MapFrom(ClassificationType.Unknown)).ShouldThrowAn<ArgumentException>();
      }
   }

}
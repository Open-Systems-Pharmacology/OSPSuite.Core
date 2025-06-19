using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Helpers;
using Classification = OSPSuite.Core.Domain.Classification;
using DataRepository = OSPSuite.Core.Domain.Data.DataRepository;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_ClassificationSnapshotTask : ContextSpecificationAsync<ClassificationSnapshotTask>
   {
      protected ClassificationMapper _classificationMapper;
      protected Classification _modelClassification;
      protected Snapshots.Classification _snapshotClassification;
      private Snapshots.Classification _subClassification;
      protected Classification _subModelClassification;
      protected TestProject _project;
      protected SnapshotContext _snapshotContext;

      protected override Task Context()
      {
         _classificationMapper = A.Fake<ClassificationMapper>();
         sut = new ClassificationSnapshotTask(_classificationMapper);

         _modelClassification = new Classification().WithName("classification");
         _subModelClassification = new Classification().WithName("subModelClassification");
         _modelClassification.ClassificationType = ClassificationType.ObservedData;
         _subModelClassification.ClassificationType = ClassificationType.ObservedData;
         _snapshotClassification = new Snapshots.Classification().WithName("classification");
         _snapshotClassification.Classifiables = new[] { "subject" };
         _subClassification = new Snapshots.Classification().WithName("subClassification");
         _snapshotClassification.Classifications = new[] { _subClassification };

         A.CallTo(() => _classificationMapper.MapToSnapshot(_modelClassification, A<ClassificationContext>._)).Returns(_snapshotClassification);
         A.CallTo(() => _classificationMapper.MapToModel(_snapshotClassification, A<ClassificationSnapshotContext>.That.Matches(x => x.ClassificationType == ClassificationType.ObservedData)))
            .Returns(_modelClassification);

         A.CallTo(() => _classificationMapper.MapToModel(_subClassification, A<ClassificationSnapshotContext>.That.Matches(x => x.ClassificationType == ClassificationType.ObservedData)))
            .Returns(_subModelClassification);


         _project = new TestProject();

         _snapshotContext = new SnapshotContext(_project, SnapshotVersions.Current);
         return _completed;
      }
   }

   public class When_updating_project_classifications_from_snapshot : concern_for_ClassificationSnapshotTask
   {
      private IReadOnlyCollection<DataRepository> _subjects;

      protected override async Task Context()
      {
         await base.Context();

         _subjects = new List<DataRepository> { DomainHelperForSpecs.ObservedData().WithName("subject") };
      }

      protected override async Task Because()
      {
         await sut.UpdateProjectClassifications<ClassifiableObservedData, DataRepository>(new[] { _snapshotClassification }, _snapshotContext, _subjects);
      }

      [Observation]
      public void the_hierarchy_should_be_configured()
      {
         _subModelClassification.Parent.ShouldBeEqualTo(_modelClassification);
      }

      [Observation]
      public void the_snapshot_mapper_should_be_used_to_map_models()
      {
         _project.AllClassificationsByType(ClassificationType.ObservedData).ShouldContain(_modelClassification);
         _project.AllClassificationsByType(ClassificationType.ObservedData).ShouldContain(_subModelClassification);
      }
   }

   public class When_updating_project_classification_for_non_existing_classification_snapshot : concern_for_ClassificationSnapshotTask
   {
      private IReadOnlyCollection<DataRepository> _subjects;
      private DataRepository _observedData;
      private ClassifiableObservedData _originalClassifiable;

      protected override async Task Context()
      {
         await base.Context();

         _observedData = DomainHelperForSpecs.ObservedData().WithName("subject");
         _originalClassifiable = _project.GetOrCreateClassifiableFor<ClassifiableObservedData, DataRepository>(_observedData);
         _subjects = new List<DataRepository> { _observedData };
      }

      protected override async Task Because()
      {
         await sut.UpdateProjectClassifications<ClassifiableObservedData, DataRepository>(null, _snapshotContext, _subjects);
      }

      [Observation]
      public void should_not_crash_nor_update_the_existing_classifiable()
      {
         _project.GetOrCreateClassifiableFor<ClassifiableObservedData, DataRepository>(_observedData).ShouldBeEqualTo(_originalClassifiable);
      }
   }

   public class When_mapping_project_classifications_to_snapshots : concern_for_ClassificationSnapshotTask
   {
      private Snapshots.Classification[] _result;
      private DataRepository _obsData;

      protected override async Task Context()
      {
         await base.Context();
         _obsData = DomainHelperForSpecs.ObservedData().WithName("subject");
         _project.AddClassifiable(new ClassifiableObservedData { Subject = _obsData });
         _project.AddClassification(_modelClassification);
      }

      protected override async Task Because()
      {
         _result = await sut.MapClassificationsToSnapshots<ClassifiableObservedData>(_project);
      }

      [Observation]
      public void should_use_the_classification_mapper_to_map_snapshots()
      {
         _result.ShouldContain(_snapshotClassification);
      }
   }
}
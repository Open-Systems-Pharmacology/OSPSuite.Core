using System;
using System.IO;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Utility;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_RelatedItemFactory : ContextSpecification<IRelatedItemFactory>
   {
      protected IOSPSuiteExecutionContext _executionContext;
      protected IApplicationConfiguration _applicationConfiguration;
      protected IProjectRetriever _projectRetriever;
      protected IApplicationDiscriminator _applicationDiscriminator;
      protected IRelatedItemSerializer _relatedItemSerializer;
      protected IRelatedItemDescriptionCreator _relatedItemDescriptionCreator;
      protected IRelatedItemTypeRetriever _relatedItemTypeRetriever;
      protected IFileExtensionToIconMapper _iconMapper;

      protected override void Context()
      {
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         _applicationConfiguration = A.Fake<IApplicationConfiguration>();
         _projectRetriever = A.Fake<IProjectRetriever>();
         _applicationDiscriminator = A.Fake<IApplicationDiscriminator>();
         _relatedItemSerializer = A.Fake<IRelatedItemSerializer>();
         _relatedItemDescriptionCreator = A.Fake<IRelatedItemDescriptionCreator>();
         _relatedItemTypeRetriever = A.Fake<IRelatedItemTypeRetriever>();
         _iconMapper= A.Fake<IFileExtensionToIconMapper>();
         sut = new RelatedItemFactory(_executionContext, _applicationConfiguration, _projectRetriever, 
            _applicationDiscriminator, _relatedItemSerializer, _relatedItemDescriptionCreator,
            _relatedItemTypeRetriever,_iconMapper);
      }
   }

   public class When_creating_a_new_realated_item_for_a_given_object : concern_for_RelatedItemFactory
   {
      private IObjectBase _relatedObject;
      private byte[] _data;
      private RelatedItem _result;
      private DateTime _utcNow;
      private Func<DateTime> _oldUtcNow;
      private IProject _project;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _utcNow = DateTime.UtcNow;
         _oldUtcNow = SystemTime.UtcNow;
         SystemTime.UtcNow = () => _utcNow;
      }

      protected override void Context()
      {
         base.Context();

         _project = A.Fake<IProject>();
         _project.FilePath = "ABC";

         _relatedObject = A.Fake<IObjectBase>()
            .WithName("Toto")
            .WithIcon("Icon");

         A.CallTo(() => _relatedItemTypeRetriever.TypeFor(_relatedObject)).Returns("MyType");

         _data = new byte[] {15, 05, 24};

         A.CallTo(() => _relatedItemSerializer.Serialize(_relatedObject)).Returns(_data);
         A.CallTo(() => _relatedItemDescriptionCreator.DescriptionFor(_relatedObject)).Returns("DESC");

         A.CallTo(() => _applicationConfiguration.FullVersion).Returns("123");
         A.CallTo(() => _applicationConfiguration.Product).Returns(Origins.PKSim);

         A.CallTo(() => _projectRetriever.CurrentProject).Returns(_project);
      }

      protected override void Because()
      {
         _result = sut.Create(_relatedObject);
      }

      [Observation]
      public void should_save_the_serialized_data_of_the_related_item()
      {
         _result.Content.Data.ShouldBeEqualTo(_data);
      }

      [Observation]
      public void should_have_created_a_transient_object()
      {
         _result.IsTransient.ShouldBeTrue();
      }

      [Observation]
      public void should_use_the_description_creator_to_retrieve_a_new_description_for_the_related_item()
      {
         _result.Description.ShouldBeEqualTo("DESC");
      }

      [Observation]
      public void should_save_the_expected_meta_data_of_the_related_object_into_the_result()
      {
         _result.IconName.ShouldBeEqualTo(_relatedObject.Icon);
         _result.Name.ShouldBeEqualTo(_relatedObject.Name);
         _result.ItemType.ShouldBeEqualTo("MyType");
      }

      [Observation]
      public void should_save_the_expected_application_meta_data()
      {
         _result.Origin.ShouldBeEqualTo(_applicationConfiguration.Product);
         _result.Version.ShouldBeEqualTo(_applicationConfiguration.FullVersion);
         _result.FullPath.ShouldBeEqualTo(_project.FilePath);
      }

      [Observation]
      public void should_have_set_the_created_at_date_to_the_current_utc_time()
      {
         _result.CreatedAt.Date.ShouldBeEqualTo(_utcNow.Date);
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         SystemTime.UtcNow = _oldUtcNow;
      }
   }

   public class When_creating_a_related_item_from_file : concern_for_RelatedItemFactory
   {
      private string _fileFullPath;
      private RelatedItem _relatedItem;
      private string _fileNameWithExtension;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _fileFullPath = FileHelper.GenerateTemporaryFileName();
         _fileNameWithExtension = new FileInfo(_fileFullPath).Name;
         string[] lines = {"First line", "Second line", "Third line"};
         File.WriteAllLines(_fileFullPath, lines);
      }

      protected override void Context()
      {
         base.Context();
         A.CallTo(_iconMapper).WithReturnType<ApplicationIcon>().Returns(ApplicationIcons.Excel);
      }

      protected override void Because()
      {
         _relatedItem = sut.CreateFromFile(_fileFullPath);
      }

      [Observation]
      public void should_return_a_related_item_containing_the_content_of_the_file_in_bytes()
      {
         _relatedItem.Content.Data.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_created_a_transient_object()
      {
         _relatedItem.IsTransient.ShouldBeTrue();
      }

      [Observation]
      public void should_save_the_expected_meta_data_of_file_into_the_related_item()
      {
         _relatedItem.IconName.ShouldBeEqualTo(ApplicationIcons.Excel.IconName);
         _relatedItem.Name.ShouldBeEqualTo(_fileNameWithExtension);
         _relatedItem.ItemType.ShouldBeEqualTo(Constants.RELATIVE_ITEM_FILE_ITEM_TYPE);
         _relatedItem.FullPath.ShouldBeEqualTo(_fileFullPath);
      }

      [Observation]
      public void should_save_the_expected_application_meta_data()
      {
         _relatedItem.Origin.ShouldBeEqualTo(Origins.Other);
         _relatedItem.Version.ShouldBeEqualTo(_applicationConfiguration.FullVersion);
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.DeleteFile(_fileFullPath);
      }
   }
}
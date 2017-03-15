using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_GroupRepositoryPersistor : ContextForIntegration<IGroupRepositoryPersistor>
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         sut = IoC.Resolve<IGroupRepositoryPersistor>();
      }
   }

   public class When_deserializing_a_group_file_containg_groups_with_parent_child_relation : concern_for_GroupRepositoryPersistor
   {
      private GroupRepositoryForSpecs _groupRepository;
      private IGroup _parentGroup, _childGroup, _otherGroup;
      private string _tmpFile;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _groupRepository = new GroupRepositoryForSpecs();
         _parentGroup = new Group {Name = "Parent", Id = "1"};
         _childGroup = new Group { Name = "Child", Id = "2" };
         _otherGroup = new Group { Name = "Other", Id = "3" };
         _parentGroup.AddChild(_childGroup);
         _groupRepository.AddGroup(_parentGroup);
         _groupRepository.AddGroup(_childGroup);
         _groupRepository.AddGroup(_otherGroup);

         _tmpFile = FileHelper.GenerateTemporaryFileName();
         sut.Save(_groupRepository, _tmpFile);
         sut.Load(_groupRepository,_tmpFile);
      }

      [Observation]
      public void should_be_able_to_save_and_load_the_repository_from_file()
      {
         _groupRepository.All().Count().ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_have_set_the_parent_child_relation_as_expected()
      {
         var parent = _groupRepository.GroupByName("Parent");
         var child = _groupRepository.GroupByName("Child");
         var other = _groupRepository.GroupByName("Other");
         parent.ShouldNotBeNull();
         child.ShouldNotBeNull();
         other.ShouldNotBeNull();
         child.Parent.ShouldBeEqualTo(parent);
         parent.Children.ShouldOnlyContain(child);
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.DeleteFile(_tmpFile);
      }
   }
}	
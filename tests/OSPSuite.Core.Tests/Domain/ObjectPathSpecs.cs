using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ObjectPath : ContextSpecification<ObjectPath>
   {
      protected override void Context()
      {
         sut = new ObjectPath();
      }
   }

   public class When_adding_elements_to_the_end_of_a_path : concern_for_ObjectPath
   {
      private string _entry;

      protected override void Context()
      {
         base.Context();
         _entry = "toto|tata";
         sut.Add("first");
      }

      protected override void Because()
      {
         sut.Add(_entry);
      }

      [Observation]
      public void should_return_the_entry()
      {
         sut.ToString().ShouldBeEqualTo($"first|{_entry}");
      }

      [Observation]
      public void the_path_should_contain_two_elements()
      {
         sut.Count.ShouldBeEqualTo(3);
      }

      [Observation]
      public void the_path_elements_should_match_strings_without_pipe()
      {
         sut[0].ShouldBeEqualTo("first");
         sut[1].ShouldBeEqualTo("toto");
         sut[2].ShouldBeEqualTo("tata");
      }
   }

   public class When_adding_elements_to_the_front_of_a_path : concern_for_ObjectPath
   {
      private string _entry;

      protected override void Context()
      {
         base.Context();
         _entry = "toto|tata";
         sut.Add("last");
      }

      protected override void Because()
      {
         sut.AddAtFront(_entry);
      }

      [Observation]
      public void should_return_the_entry()
      {
         sut.ToString().ShouldBeEqualTo($"{_entry}|last");
      }

      [Observation]
      public void the_path_should_contain_two_elements()
      {
         sut.Count.ShouldBeEqualTo(3);
      }

      [Observation]
      public void the_path_elements_should_match_strings_without_pipe()
      {
         sut[0].ShouldBeEqualTo("toto");
         sut[1].ShouldBeEqualTo("tata");
         sut[2].ShouldBeEqualTo("last");
      }
   }

   public class When_returning_the_path_as_string_from_an_object_path_with_not_entry : concern_for_ObjectPath
   {
      [Observation]
      public void should_return_an_empty_string()
      {
         string.IsNullOrEmpty(sut.ToString()).ShouldBeTrue();
      }
   }

   public class When_resolving_an_object_from_an_absolute_object_path_regarding_the_ref_entity : concern_for_ObjectPath
   {
      protected IContainer _organ;
      protected IParameter _parameter;

      protected override void Context()
      {
         _organ = new Container().WithName("organ");
         var comp = new Container().WithName("comp").WithParentContainer(_organ);
         _parameter = new Parameter().WithName("P1").WithParentContainer(comp);
         var objectPathFactory = new ObjectPathFactory(new AliasCreator());
         sut = objectPathFactory.CreateAbsoluteFormulaUsablePath(_parameter);
      }

      [Observation]
      public void should_return_the_desired_entity()
      {
         sut.Resolve<IParameter>(_organ).ShouldBeEqualTo(_parameter);
      }
   }

   public class When_resolving_an_object_from_an_entity_with_parent_path : When_resolving_an_object_from_an_absolute_object_path_regarding_the_ref_entity
   {
      protected override void Context()
      {
         base.Context();

         var lstPaths = new List<string> { "Path1", "Path2", "Path3" };

         _organ.RootContainer.ParentPath = new ObjectPath(lstPaths);
         lstPaths.Reverse();
         foreach (var path in lstPaths)
         {
            sut.AddAtFront(path);
         }
      }
   }

   public class When_setting_the_path_element_with_index : concern_for_ObjectPath
   {
      private IContainer _organ;
      private IParameter _parameter;

      protected override void Context()
      {
         _organ = new Container().WithName("organ");
         var comp = new Container().WithName("comp").WithParentContainer(_organ);
         _parameter = new Parameter().WithName("P1").WithParentContainer(comp);
         var objectPathFactory = new ObjectPathFactory(new AliasCreator());
         sut = objectPathFactory.CreateAbsoluteFormulaUsablePath(_parameter);
      }

      [Observation]
      public void should_be_able_to_set_and_retrieve_a_path_with_index()
      {
         sut[1].ShouldBeEqualTo("comp");
         sut[1] = "toto";
         sut[1].ShouldBeEqualTo("toto");
      }

      [Observation]
      public void should_throw_an_exception_if_accessing_a_value_outside_of_the_defined_range()
      {
         The.Action(() => { sut[10] = "toto"; }).ShouldThrowAn<ArgumentOutOfRangeException>();
      }
   }

   public class When_removing_the_first_element_of_the_path : concern_for_ObjectPath
   {
      [Observation]
      public void should_have_remove_the_element()
      {
         sut = new ObjectPath("A", "B");
         sut.RemoveFirst();
         sut.PathAsString.ShouldBeEqualTo("B");
      }
   }

   public class When_replacing_the_content_of_a_path_with_another_path : concern_for_ObjectPath
   {
      [Observation]
      public void should_have_updated_the_path()
      {
         sut = new ObjectPath("A", "B");
         sut.ReplaceWith(new[] { "C", "D" });
         sut.PathAsString.ShouldBeEqualTo("C|D");
      }
   }

   public class When_removing_the_first_element_of_an_empty_path : concern_for_ObjectPath
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => new ObjectPath().RemoveFirst()).ShouldThrowAn<ArgumentOutOfRangeException>();
      }
   }

   public class When_removing_the_element_of_an_path_by_index : concern_for_ObjectPath
   {
      [Observation]
      public void should_have_remove_the_element()
      {
         sut = new ObjectPath("A", "B", "C");
         sut.RemoveAt(1);
         sut.ShouldOnlyContain("A", "C");
      }
   }

   public class When_concatenating_two_object_paths : concern_for_ObjectPath
   {
      [Observation]
      public void should_modify_the_first_object_path_and_add_the_entries_of_the_second_object_path()
      {
         sut = new ObjectPath("A", "B", "C");
         sut.ConcatWith(new ObjectPath("D", "E"));
         sut.ShouldOnlyContainInOrder("A", "B", "C", "D", "E");
      }
   }

   public class When_testing_if_one_path_starts_with_another : concern_for_ObjectPath
   {
      [Observation]
      public void if_the_first_path_is_not_long_enough_then_false()
      {
         sut = new ObjectPath("A", "B", "C");
         sut.StartsWith(new ObjectPath("A", "B", "C", "D")).ShouldBeFalse();
      }

      [Observation]
      public void if_the_paths_are_the_same_then_true()
      {
         sut = new ObjectPath("A", "B", "C");
         sut.StartsWith(new ObjectPath("A", "B", "C")).ShouldBeTrue();
      }

      [Observation]
      public void if_the_paths_is_contained_then_true()
      {
         sut = new ObjectPath("A", "B", "C");
         sut.StartsWith(new ObjectPath("A", "B")).ShouldBeTrue();
      }

      [Observation]
      public void if_the_paths_is_not_contained_then_false()
      {
         sut = new ObjectPath("A", "B", "C");
         sut.StartsWith(new ObjectPath("A", "C")).ShouldBeFalse();
      }
   }
}
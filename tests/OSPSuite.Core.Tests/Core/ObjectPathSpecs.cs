using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_ObjectPath : ContextSpecification<IObjectPath>
   {
      protected override void Context()
      {
         sut = new ObjectPath();
      }
   }

   public class When_returning_the_path_as_string_from_an_object_path_with_only_one_entry : concern_for_ObjectPath
   {
      private string _entry;

      protected override void Context()
      {
         base.Context();
         _entry = "toto";
         sut.AddAtFront(_entry);
      }

      [Observation]
      public void should_return_the_entry()
      {
         sut.ToString().ShouldBeEqualTo(_entry);
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
      public void should_return_the_desired_entity()
      {
         sut.Resolve<IParameter>(_organ).ShouldBeEqualTo(_parameter);
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
         sut = new ObjectPath("A", "B","C");
         sut.RemoveAt(1);
         sut.ShouldOnlyContain("A", "C");
      }
   }
}
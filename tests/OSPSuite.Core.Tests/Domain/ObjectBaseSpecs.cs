using System;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ObjectBase : ContextSpecification<IObjectBase>
   {
      protected bool _eventNotified;
      protected string _id;
      protected string _propertyName;
      protected string _defaultName;

      protected override void Context()
      {
         _id = "toto";
         _defaultName = "DefaultName";
         sut = new MyDerivedObject(_id);
         sut.Name = _defaultName;
         sut.PropertyChanged += (o, e) =>
         {
            _eventNotified = true;
            _propertyName = e.PropertyName;
         };
      }
   }

   
   public class When_setting_the_name_ : concern_for_ObjectBase
   {
      [Observation]
      public void should_notify_that_the_property_name_has_changed()
      {
         _eventNotified.ShouldBeTrue();
         _propertyName.ShouldBeEqualTo("Name");
      }

      protected override void Because()
      {
         sut.Name = "tutu";
      }
   }

   
   public class When_setting_the_name_equal_to_the_name_already_set : concern_for_ObjectBase
   {
      protected override void Because()
      {
         sut.Name = _defaultName;
      }
      
      [Observation]
      public void should_notify_that_the_property_name_has_changed()
      {
         _eventNotified.ShouldBeFalse();
      }
   }

   
   public class When_checking_if_two_objects_with_the_same_id_are_equals : concern_for_ObjectBase
   {
      private IObjectBase _newObject;

      protected override void Context()
      {
         base.Context();
         _newObject = A.Fake<IObjectBase>();
         _newObject.Id = _id;
      }

      [Observation]
      public void should_return_true()
      {
         sut.Equals(_newObject).ShouldBeTrue();
      }
   }

   
   public class When_checking_if_two_objects_of_different_types_with_the_same_id_are_equals : concern_for_ObjectBase
   {
      private IObjectBase _newObject;

      protected override void Context()
      {
         base.Context();
         _newObject = new MyDerivedObject(_id);
      }

      [Observation]
      public void should_return_true()
      {
         sut.Equals(_newObject).ShouldBeTrue();
      }
   }

   
   public class When_checking_if_two_objects_with_id_not_set_are_equals : concern_for_ObjectBase
   {
      private IObjectBase _anotherObject;
      private IObjectBase _oneObject;
      private IObjectBase _sameObject;

      protected override void Context()
      {
         base.Context();
         _oneObject = new MyDerivedObject();
         _sameObject = _oneObject;
         _anotherObject = new MyDerivedObject();
      }

      [Observation]
      public void should_return_true_when_the_objects_share_the_same_reference()
      {
         _sameObject.Equals(_oneObject).ShouldBeTrue();
         (_sameObject == _oneObject).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_when_the_objects_do_not_share_the_same_reference()
      {
         _sameObject.Equals(_anotherObject).ShouldBeFalse();
         (_sameObject != _anotherObject).ShouldBeTrue();
      }
   }

   
   public class When_class_is_inherited_from_object_base
   {

      [Observation]
      public void should_implement_update_properties_from_method()
      {
         var missingClassNames = from type in Assembly.GetExecutingAssembly().GetTypes()
                                 where mustImplementUpdatePropertiesFrom(type)
                                 where type.GetMethod("UpdatePropertiesFrom").DeclaringType != type
                                 select type.Name;

         if (missingClassNames.Count()>0)
         {
            string info = 
               "\n    \"UpdatePropertiesFrom\"-method not overriden in "+
               missingClassNames.Count().ToString()+" classes:\n\n" +
               string.Join("\n", missingClassNames.ToArray());

            throw new OSPSuiteException(info);
         }
      }

      private bool mustImplementUpdatePropertiesFrom(Type type)
      {
         if (!type.IsSubclassOf(typeof(ObjectBase)))
            return false; //not an ObjectBase implementation

         if (type.IsNested)
            return false; //ignore nested test classes

         if (type.IsAbstract)
            return false;

         //exclude some test classes
         if (type.Equals(typeof(ARootContainer)) ||
             type.Equals(typeof(MyDerivedObject)) ||
             type.Equals(typeof(TestProject))
            )
            return false;

         //ignore basic class for which an implementation is not necessary so far
         //remove line as soon as type needs a real implementation
         if (type.Equals(typeof(BuildingBlock))) return false;
         if (type.Equals(typeof(DistributionFormula))) return false;
         if (type.Equals(typeof(UniformDistributionFormula))) return false;
         if (type.Equals(typeof(Parameter))) return false;
         if (type.Equals(typeof(EventGroup))) return false;
         if (type.Equals(typeof(Observer))) return false;
         if (type.Equals(typeof(EventGroupBuildingBlock))) return false;
         if (type.Equals(typeof(ReactionBuildingBlock))) return false;
         if (type.Equals(typeof(PassiveTransportBuildingBlock))) return false;
         if (type.Equals(typeof(ContainerObserverBuilder))) return false;
         if (type.Equals(typeof(NormalDistributionFormula))) return false;
         if (type.Equals(typeof(MoleculeBuildingBlock))) return false;
         if (type.Equals(typeof(LogNormalDistributionFormula))) return false;
         if (type.Equals(typeof(MoleculeStartValuesBuildingBlock))) return false;
         if (type.Equals(typeof(NeighborhoodBuilder))) return false;
         if (type.Equals(typeof(ObserverBuildingBlock))) return false;
         if (type.Equals(typeof(DiscreteDistributionFormula))) return false;
         return true;
      }
   }

   public class MyDerivedObject : ObjectBase
   {
      public MyDerivedObject()
      {
      }

      public MyDerivedObject(string id) : base(id)
      {
      }
   }
}
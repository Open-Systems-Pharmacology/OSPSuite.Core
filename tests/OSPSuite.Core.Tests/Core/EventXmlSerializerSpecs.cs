using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class EventAssignmentXmlSerializerSpecs : MiniEnvironmentSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationEventAssignment()
      {
         EventAssignment x1 = CreateObject<EventAssignment>().WithName("Eva").WithDimension(DimensionLength);
         x1.ParentContainer = C0;
         x1.UseAsValue = true;
         x1.ObjectPath = ObjectPathFactory.CreateAbsoluteObjectPath(P);
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);
         x1.ResolveChangedEntity();

         IEventAssignment x2 = SerializeAndDeserialize(x1);
         x2.ParentContainer = C0;
         x2.ResolveChangedEntity();
         AssertForSpecs.AreEqualEventAssignment(x2, x1);
      }
   }

   public class EventXmlSerializerSpecs : MiniEnvironmentSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationEvent()
      {
         Event x1 = CreateObject<Event>().WithName("Eve").WithDimension(DimensionLength);
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(1.0);
         x1.ParentContainer = C0;

         EventAssignment ea1 = CreateObject<EventAssignment>().WithName("Eva").WithDimension(DimensionLength);
         ea1.UseAsValue = true;
         ea1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);
         ea1.ObjectPath = ObjectPathFactory.CreateAbsoluteObjectPath(P);
         x1.AddAssignment(ea1);
         ea1.ResolveChangedEntity();

         IEvent x2 = SerializeAndDeserialize(x1);
         x2.ParentContainer = C0;

         var refResolver = new ReferencesResolver();
         refResolver.ResolveReferencesIn(x2);

         AssertForSpecs.AreEqualEvent(x2, x1);
      }
   }
}
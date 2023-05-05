using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class InitialConditionXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = new InitialCondition
         {
            ContainerPath = new ObjectPath("aa", "bb"), 
            Name = "H2", 
            Dimension = DimensionTime,
            IsPresent = true,
            Value = 2.5,
            ScaleDivisor = 1e-2
         };

         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualInitialConditions(x1, x2);
      }
   }

   public class ParameterValueXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = new ParameterValue {ContainerPath = new ObjectPath("A", "B"), Value = 3.6, Dimension = DimensionLength};
         x1.ValueOrigin.Description = "Hello";
         x1.ValueOrigin.Method  = ValueOriginDeterminationMethods.Assumption;
         x1.ValueOrigin.Id = 5;
         x1.IsDefault = true;

         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualParameterValue(x1, x2);
      }
   }
}
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class MoleculeStartValueXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = new MoleculeStartValue {ContainerPath = new ObjectPath("aa", "bb"), Name = "H2", Dimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.AMOUNT, "mol")};

         x1.IsPresent = true;
         x1.StartValue = 2.5;
         x1.ScaleDivisor = 1e-2;

         IMoleculeStartValue x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualMoleculeStartValue(x1, x2);
      }
   }

   public class ParameterStartValueXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = new ParameterStartValue {ContainerPath = new ObjectPath("A", "B"), StartValue = 3.6, Dimension = DimensionLength};
         x1.ValueOrigin.Description = "Hello";
         x1.ValueOrigin.Method  = ValueOriginDeterminationMethods.Assumption;
         x1.ValueOrigin.Id = 5;
         x1.IsDefault = true;

         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualParameterStartValue(x1, x2);
      }
   }
}
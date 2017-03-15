using System;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class DataInfoXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         DataInfo x1 = new DataInfo(ColumnOrigins.Observation, AuxiliaryType.ArithmeticStdDev, "cm", new DateTime(2010, 10, 22), "Study1", "Dog", 2.4);
         x1.ExtendedProperties.Add(new ExtendedProperty<int>() { Name = "Age", Value = 34 });
         DataInfo x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualMcDataInfo(x1, x2);
      }
   }
}
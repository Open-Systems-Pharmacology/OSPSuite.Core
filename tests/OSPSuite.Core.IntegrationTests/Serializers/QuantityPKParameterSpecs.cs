﻿using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Helpers;
using OSPSuite.Core.Serializers;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Serializers
{

   public class QuantityPKParameterSpecs : ModellingXmlSerializerBaseSpecs
   {

      [Observation]
      public void TestSerialization()
      {
         var x1 = new QuantityPKParameter
         {
            Name = "C_max",
            Dimension = DimensionMassConcentration,
            QuantityPath = "A|B|C"
         };
         x1.SetValue(1, 5.0f);
         x1.SetValue(2, 4.0f);
         x1.SetValue(8, 2.0f);
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualQuantity(x2, x1);
      }
   }
}
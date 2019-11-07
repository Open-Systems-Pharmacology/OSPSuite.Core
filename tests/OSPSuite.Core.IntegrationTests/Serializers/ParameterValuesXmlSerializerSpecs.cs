﻿using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Core.Helpers;

namespace OSPSuite.Core.Serializers
{

   public class ParameterValuesXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Observation]
      public void TestSerialization()
      {
         var x1 = new ParameterValues("Path");
         x1.Values = new List<double>{1d, 2d, 3d};
         x1.Percentiles = new List<double>{0.1, 0.2, 0.3};
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualParameterValues(x2, x1);
      }
   }

}
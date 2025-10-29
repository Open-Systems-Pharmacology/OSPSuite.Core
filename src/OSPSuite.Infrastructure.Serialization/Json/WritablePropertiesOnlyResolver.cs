using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OSPSuite.Infrastructure.Serialization.Json
{
   public class WritablePropertiesOnlyResolver : DefaultContractResolver
   {
      protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) => base.CreateProperties(type, memberSerialization).Where(p => p.Writable).ToList();
   }
}
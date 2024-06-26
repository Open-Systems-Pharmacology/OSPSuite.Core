﻿using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Services
{
   public interface IObservedDataConfiguration
   {
      IEnumerable<string> PredefinedValuesFor(string metaData);
      IReadOnlyList<string> DefaultMetaDataCategories { get; }
      IReadOnlyList<string> ReadOnlyMetaDataCategories { get; }
      bool MolWeightAlwaysEditable { get; }
      bool MolWeightVisible { get; }
   }
}
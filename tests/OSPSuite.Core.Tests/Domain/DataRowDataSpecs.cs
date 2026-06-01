using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_DataRowDataSpecs : ContextSpecification<DataRowData>
   {
      protected override void Context()
      {
         base.Context();
         sut = new DataRowData();
      }
   }
}

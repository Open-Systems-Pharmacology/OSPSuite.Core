﻿using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Commands
{
   public abstract class ObservedDataCommandBase : OSPSuiteReversibleCommand<IOSPSuiteExecutionContext>
   {
      protected DataRepository _observedData;
      protected string _observedDataId;
      private readonly NameValueUnitsFormatter _nameValueUnitFormatter = new NameValueUnitsFormatter();

      protected ObservedDataCommandBase(DataRepository observedData)
      {
         _observedData = observedData;
         _observedDataId = _observedData.Id;
         ObjectType = ObjectTypes.ObservedData;
      }

      protected void SetBuildingBlockParameters(IOSPSuiteExecutionContext context)
      {
         BuildingBlockName = _observedData.Name;
         BuildingBlockType = context.TypeFor(_observedData);
      }

      protected override void ClearReferences()
      {
         _observedData = null;
      }

      public override void RestoreExecutionData(IOSPSuiteExecutionContext context)
      {
         _observedData = context.Get<DataRepository>(_observedDataId);
      }

      protected string GetDisplayFor(string columnId, float value)
      {
         var displayValue = getColumnValueConvertedToDisplayUnits(columnId, value);
         var unit = getColumnUnit(columnId);
         var name = getColumnName(columnId);
         return _nameValueUnitFormatter.Format(displayValue, unit, name);
      }

      private string getColumnUnit(string key)
      {
         return _observedData[key].DisplayUnit.Name;
      }

      private double getColumnValueConvertedToDisplayUnits(string key, float value)
      {
         return _observedData[key].ConvertToDisplayUnit(value);
      }

      private string getColumnName(string key)
      {
         return _observedData[key].Name;
      }
   }
}
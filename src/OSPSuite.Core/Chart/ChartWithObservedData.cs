using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Chart
{
   public abstract class ChartWithObservedData : CurveChart, IWithObservedData
   {
      private readonly Cache<string, DataRepository> _allDataRepositories = new Cache<string, DataRepository>(x => x.Id);

      public void AddObservedData(DataRepository dataRepository)
      {
         if (dataRepository == null) return;
         if (UsesObservedData(dataRepository))
            return;

         _allDataRepositories.Add(dataRepository);
      }

      public IEnumerable<DataRepository> AllObservedData()
      {
         return _allDataRepositories;
      }

      public bool UsesObservedData(DataRepository dataRepository)
      {
         return dataRepository != null && _allDataRepositories.Contains(dataRepository.Id);
      }

      public void RemoveObservedData(DataRepository dataRepository)
      {
         if (!UsesObservedData(dataRepository))
            return;

         _allDataRepositories.Remove(dataRepository.Id);
         RemoveCurvesForDataRepository(dataRepository);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceChartWithObservedData = source as ChartWithObservedData;
         if (sourceChartWithObservedData == null) return;
         sourceChartWithObservedData.AllObservedData().Each(AddObservedData);
      }
   }
}
using System.Collections.Generic;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.PKAnalyses
{
   public interface IPKParameterRepository : IRepository<PKParameter>
   {
      void Add(PKParameter pkParameter);
      string DisplayNameFor(string pkParameterName);
      string DescriptionFor(string pkParameterName);
   }

   public class PKParameterRepository : IPKParameterRepository
   {
      private readonly ICache<string, PKParameter> _allParameters;

      public PKParameterRepository()
      {
         _allParameters = new Cache<string, PKParameter>(x => x.Name, x => null);
      }

      public IEnumerable<PKParameter> All()
      {
         return _allParameters;
      }

      public void Add(PKParameter pkParameter)
      {
         _allParameters.Add(pkParameter);
      }

      public string DisplayNameFor(string pkParameterName)
      {
         var pkParameter = _allParameters[pkParameterName];
         return pkParameter == null ? pkParameterName : pkParameter.DisplayName;
      }

      public string DescriptionFor(string pkParameterName)
      {
         var pkParameter = _allParameters[pkParameterName];
         return pkParameter == null ? string.Empty : pkParameter.Description;
      }
   }
}
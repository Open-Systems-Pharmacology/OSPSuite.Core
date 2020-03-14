using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Services
{
   public interface IPKParameterTask
   {
      void AddUserDefinedPKParameter(UserDefinedPKParameter userDefinedPKParameter);
      void RemoveUserDefinedPKParameterByName(string name);
      void RemoveUserDefinedPKParameter(UserDefinedPKParameter userDefinedPKParameter);
      PKParameter PKParameterByName(string name);
      UserDefinedPKParameter UserDefinedPKParameterByName(string name);
      void RemoveAllUserDefinedPKParameters();
      string[] AllPKParameterNames();
   }

   public class PKParameterTask : IPKParameterTask
   {
      private readonly IPKParameterRepository _pkParameterRepository;
      private readonly ILogger _logger;

      public PKParameterTask(IPKParameterRepository pkParameterRepository, ILogger logger)
      {
         _pkParameterRepository = pkParameterRepository;
         _logger = logger;
      }

      public void AddUserDefinedPKParameter(UserDefinedPKParameter userDefinedPKParameter)
      {
         var existingUserDefinedPKParameter = UserDefinedPKParameterByName(userDefinedPKParameter.Name);
         if (existingUserDefinedPKParameter != null)
         {
            _logger.AddWarning(Warning.UserDefinedPKParameterAlreadyExistsAndWillBeReplaced(userDefinedPKParameter.Name));
            return;
         }
         _pkParameterRepository.Add(userDefinedPKParameter);
      }

      public void RemoveUserDefinedPKParameterByName(string name)
      {
         var userDefinedPKParameter = UserDefinedPKParameterByName(name);
         if (userDefinedPKParameter == null)
         {
            _logger.AddError(Error.UserDefinedPKParameterNotFound(name));
            return;
         }

         _pkParameterRepository.Remove(userDefinedPKParameter);
      }

      public void RemoveUserDefinedPKParameter(UserDefinedPKParameter userDefinedPKParameter)
      {
         if (userDefinedPKParameter == null)
            return;

         _pkParameterRepository.Remove(userDefinedPKParameter);
      }

      public PKParameter PKParameterByName(string name) => _pkParameterRepository.FindByName(name);

      public UserDefinedPKParameter UserDefinedPKParameterByName(string name) => PKParameterByName(name) as UserDefinedPKParameter;

      public void RemoveAllUserDefinedPKParameters()
      {
         _pkParameterRepository.All().OfType<UserDefinedPKParameter>().ToList().Each(RemoveUserDefinedPKParameter);
      }

      public string[] AllPKParameterNames() => _pkParameterRepository.All().AllNames().ToArray();
   }
}
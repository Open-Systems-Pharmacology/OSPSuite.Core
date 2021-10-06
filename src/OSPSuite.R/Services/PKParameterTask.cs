using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Services
{
   public interface IPKParameterTask
   {
      UserDefinedPKParameter CreateUserDefinedPKParameter(string name, int standardPKParameter, string displayName, string displayUnit);

      UserDefinedPKParameter CreateUserDefinedPKParameter(string name, StandardPKParameter standardPKParameter, string displayName,
         string displayUnit);

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
      private readonly IDimensionTask _dimensionTask;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IOSPSuiteLogger _logger;

      public PKParameterTask(
         IPKParameterRepository pkParameterRepository,
         IDimensionTask dimensionTask,
         IDimensionFactory dimensionFactory,
         IOSPSuiteLogger logger)
      {
         _pkParameterRepository = pkParameterRepository;
         _dimensionTask = dimensionTask;
         _dimensionFactory = dimensionFactory;
         _logger = logger;
      }

      public UserDefinedPKParameter CreateUserDefinedPKParameter(string name, int standardPKParameter, string displayName, string displayUnit)
      {
         return CreateUserDefinedPKParameter(name, (StandardPKParameter) (standardPKParameter), displayName, displayUnit);
      }

      public UserDefinedPKParameter CreateUserDefinedPKParameter(
         string name,
         StandardPKParameter standardPKParameter,
         string displayName,
         string displayUnit)
      {
         var defaultDimension = _dimensionTask.DimensionForStandardPKParameter(standardPKParameter);
         var userDefinedPKParameters = new UserDefinedPKParameter
         {
            Name = name,
            DisplayName = displayName,
            Dimension = defaultDimension,
            DisplayUnit = displayUnit,
            StandardPKParameter = standardPKParameter
         };

         //No display unit defined, we return the user defined pk parameter
         if (string.IsNullOrEmpty(displayUnit))
            return userDefinedPKParameters;

         var dimensionForDisplayUnit = _dimensionTask.DimensionForUnit(displayUnit);
         //Display unit belongs to the dimension. We can use this
         if (Equals(defaultDimension, dimensionForDisplayUnit))
            return userDefinedPKParameters;

         //We have two dimensions. Do we have a converter between those dimensions?
         if (_dimensionFactory.HasMergingInformation(defaultDimension, dimensionForDisplayUnit))
            return userDefinedPKParameters;

         //There is no converter defined between those dimensions.
         //We use the dimension defined for the display unit or create a user defined dimension otherwise
         userDefinedPKParameters.Dimension = dimensionForDisplayUnit ??  _dimensionFactory.CreateUserDefinedDimension(name, displayUnit);
         return userDefinedPKParameters;
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
using System;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain
{
   public class BothNeighborsSatisfyingCriteriaException : OSPSuiteException
   {
      public BothNeighborsSatisfyingCriteriaException(INeighborhood neighborhood) : base(Error.BothNeighborsSatisfying(neighborhood.Name))
      {
      }
   }

   public class NotUniqueIdException : ArgumentException
   {
      public NotUniqueIdException(string id) : base($"Id {id} not unique")
      {
      }
   }

   public class NotUniqueNameException : OSPSuiteException
   {
      public NotUniqueNameException(string childName, string containerName) : this($"Name '{childName}' is not unique in parent container '{containerName}'")
      {
      }

      public NotUniqueNameException(string message)
         : base(message)
      {
      }
   }

   public class ValuePointAlreadyExistsForPointException : OSPSuiteException
   {
      public ValuePointAlreadyExistsForPointException(ValuePoint point)
         : base($"A point for x={point.X} was already added with y={point.Y}")
      {
      }
   }

   public class NotFoundException : OSPSuiteException
   {
      public NotFoundException(string notFoundObjectName)
         : base("Object " + notFoundObjectName + " not found")
      {
      }
   }

   public class InvalidArgumentException : OSPSuiteException
   {
      public InvalidArgumentException(string message) : base(message)
      {
      }
   }

   public class InvalidTypeException : OSPSuiteException
   {
      public InvalidTypeException(Type givenType, Type expectedType)
         : base("Object has type = " + givenType.Name + " instead of expected type = " + expectedType.Name)
      {
      }

      public InvalidTypeException(object obj, Type expectedType)
         : this(obj.GetType(), expectedType)
      {
      }
   }

   public class CircularReferenceException : OSPSuiteException
   {
      private const string _errorMessage = "Action will create a circular reference\nAncestor '{0}' cannot be added as child to '{1}'";

      public CircularReferenceException(string childName, string containerName) : base(string.Format(_errorMessage, childName, containerName))
      {
      }
   }

   public class CircularReferenceInSumFormulaException : OSPSuiteException
   {
      private const string _errorMessage = "SumFormula '{0}' used in '{1}' will circular reference '{1}'";

      public CircularReferenceInSumFormulaException(string formulaName, string entityName)
         : base(string.Format(_errorMessage, formulaName, entityName))
      {
      }
   }

   internal class OSPSuiteAliasException : OSPSuiteException
   {
      public OSPSuiteAliasException() : this("Alias not allowed")
      {
      }

      public OSPSuiteAliasException(string message) : base(message)
      {
      }
   }

   public class UnableToRemoveChildException : OSPSuiteException
   {
      private const string _errorMessage = "Unable to remove {0} from {1}";

      public UnableToRemoveChildException(IEntity child, IContainer container) : base(string.Format(_errorMessage, child.Name, container.Name))
      {
      }
   }

   public class MissingMoleculeAmountException : Exception
   {
      public MissingMoleculeAmountException(string containerName, string moleculeName)
         : base("Missing amount for molecule " + moleculeName + " in " + containerName)
      {
      }
   }

   public class UnableToResolvePathException : OSPSuiteException
   {
      private const string _errorMessage = "Unable to resolve path '{0}' for entity '{1}'";

      public UnableToResolvePathException(IObjectPath objectPath, IEntity currentEntity)
         : base(string.Format(_errorMessage, objectPath, currentEntity.Name))

      {
      }
   }

   public class MissingMoleculeContainerException : OSPSuiteException
   {
      public MissingMoleculeContainerException(string moleculeName)
         : base(Error.MissingMoleculeContainerFor(moleculeName))
      {
      }
   }

   public class CannotLockFileException : OSPSuiteException
   {
      public CannotLockFileException(Exception exception) : base(exception.Message)
      {
      }
   }
}
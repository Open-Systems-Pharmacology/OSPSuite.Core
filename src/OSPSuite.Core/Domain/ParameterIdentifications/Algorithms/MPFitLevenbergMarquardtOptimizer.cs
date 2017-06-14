using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using MPFitLib;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Extensions;
using PropertyNames = OSPSuite.Assets.Captions.ParameterIdentification.AlgorithmProperties.Names;
using PropertyDescriptions = OSPSuite.Assets.Captions.ParameterIdentification.AlgorithmProperties.Descriptions;

namespace OSPSuite.Core.Domain.ParameterIdentifications.Algorithms
{
   public class MPFitLevenbergMarquardtOptimizer : OptimizationAlgorithm<MPFitLevenbergMarquardtOptimizer>
   {
      private const int SUCCESS = 0;
      private const int ERROR = -1;
      private string _objectiveFunctionErrorMessage = string.Empty;

      public MPFitLevenbergMarquardtOptimizer() : base(Constants.OptimizationAlgorithm.MPFIT, Captions.ParameterIdentification.Algorithms.LevenberMarquardtMPFit)
      {
         ftol = 1e-3;
         xtol = 1e-6;
         gtol = 1e-10;
         stepfactor = 100.0;
         maxiter = 200;
         maxfev = 0;

         epsfcn = 1e-9;

         configureProperties();
      }

      private void configureProperties()
      {
         _extendedPropertyStore.ConfigureProperty(x => x.epsfcn, PropertyNames.Epsfcn, PropertyDescriptions.Epsfcn);
         _extendedPropertyStore.ConfigureProperty(x => x.ftol, PropertyNames.RelativeChiSquareConvergenceCriteriumFtol, PropertyDescriptions.RelativeChiSquareConvergenceCriteriumFtol);
         _extendedPropertyStore.ConfigureProperty(x => x.xtol, PropertyNames.RelativeParameterConvergenceCriteriumXtol, PropertyDescriptions.RelativeParameterConvergenceCriteriumXtol);
         _extendedPropertyStore.ConfigureProperty(x => x.gtol, PropertyNames.OrthoganalityConvergenceCriteriumGtol, PropertyDescriptions.OrthoganalityConvergenceCriteriumGtol);
         _extendedPropertyStore.ConfigureProperty(x => x.stepfactor, PropertyNames.InitialStepBoundFactor, PropertyDescriptions.InitialStepBoundFactor);
         _extendedPropertyStore.ConfigureProperty(x => x.maxiter, PropertyNames.MaximumNumberOfIterations, PropertyDescriptions.MaximumNumberOfIterations);
         _extendedPropertyStore.ConfigureProperty(x => x.maxfev, PropertyNames.MaximumNumberOfFunctionEvaluations, PropertyDescriptions.MaximumNumberOfFunctionEvaluations);
      }

      protected override OptimizationRunProperties RunOptimization()
      {
         try
         {
            var result = new mp_result(_constraints.Count);

            var pars = createMPConstraints();

            var startValues = CreateVectorFor(x => x.StartValue);

            var config = new mp_config
            {
               ftol = ftol,
               xtol = xtol,
               gtol = gtol,
               stepfactor = stepfactor,
               maxiter = maxiter,
               maxfev = maxfev,
               epsfcn = epsfcn,
               nofinitecheck = 0
            };

            _objectiveFunctionErrorMessage = string.Empty;
            int status = MPFit.Solve(objectiveFunction, _numberOfResiduals, _constraints.Count, startValues, pars, config, null, ref result);
            evaluateStatus(status);
            
            return new OptimizationRunProperties(result.nfev);
         }

         finally
         {
            _constraints = null;
            _objectiveFunc = null;
         }
      }

      private void evaluateStatus(int status)
      {
         string errorMsg;

         // success status codes
         switch (status)
         {
            case MPFit.MP_OK_CHI: //Convergence in chi-square value
            case MPFit.MP_OK_PAR: //Convergence in parameter value
            case MPFit.MP_OK_BOTH: //Both MP_OK_PAR and MP_OK_CHI hold
            case MPFit.MP_OK_DIR: //Convergence in orthogonality
            case MPFit.MP_MAXITER: //Maximum number of iterations reached
            case MPFit.MP_FTOL: //ftol is too small; no further improvement
            case MPFit.MP_XTOL: //xtol is too small; no further improvement
            case MPFit.MP_GTOL: //gtol is too small; no further improvement
               return;
            
         }

         //special case when max iteration is set to zero
         if (status == MPFit.MP_ERR_INPUT && maxiter == 0)
            return;

         // in case of failure: first check if objective function returned an error
         if(!string.IsNullOrEmpty(_objectiveFunctionErrorMessage))
            throw new OSPSuiteException(Error.MPFit.OptimizationFailed(_objectiveFunctionErrorMessage));

         // otherwise(objective error function error is empty): check error code
         switch (status)
         {
            case MPFit.MP_ERR_INPUT:
               errorMsg = Error.MPFit.GeneralInputError;
               break;
            case MPFit.MP_ERR_NAN:
               errorMsg = Error.MPFit.UserFunctionNonFinite;
               break;
            case MPFit.MP_ERR_FUNC:
               errorMsg = Error.MPFit.NoUserFunctionSupplied;
               break;
            case MPFit.MP_ERR_NPOINTS:
               errorMsg = Error.MPFit.NoUserDataPoints;
               break;
            case MPFit.MP_ERR_NFREE:
               errorMsg = Error.MPFit.NoFreeParameters;
               break;
            case MPFit.MP_ERR_MEMORY:
               errorMsg = Error.MPFit.MemoryAllocationError;
               break;
            case MPFit.MP_ERR_INITBOUNDS:
               errorMsg = Error.MPFit.InitialValuesInconsistent;
               break;
            case MPFit.MP_ERR_BOUNDS:
               errorMsg = Error.MPFit.InitialConstraintsInconsistent;
               break;
            case MPFit.MP_ERR_PARAM:
               errorMsg = Error.MPFit.GeneralInputParameterError;
               break;
            case MPFit.MP_ERR_DOF:
               errorMsg = Error.MPFit.NotEnoughDegreesOfFreedom;
               break;
            default:
               errorMsg = Error.MPFit.UnknownStatus(status.ToString());
               break;
         }

         throw new OSPSuiteException(Error.MPFit.OptimizationFailed(errorMsg));
      }

      private mp_par[] createMPConstraints()
      {
         mp_par[] pars = new mp_par[_constraints.Count];
         var minValues = CreateVectorFor(x => x.Min);
         var maxValues = CreateVectorFor(x => x.Max);

         for (int i = 0; i < _constraints.Count; i++)
         {
            pars[i] = mpParFrom(minValues[i], maxValues[i]);
         }
         return pars;
      }

      private mp_par mpParFrom(double minValue, double maxValue)
      {
         return new mp_par
         {
            limited =
            {
               [0] = 1,
               [1] = 1
            },
            limits =
            {
               [0] = minValue,
               [1] = maxValue
            }
         };
      }

      /// <summary>
      /// </summary>
      /// <param name="p">Input: array of fit parameters </param>
      /// <param name="fvec">Output: array of residuals to be returned</param>
      /// <param name="dvec">Output: function derivatives (optional)</param>
      /// <param name="prv">function private data (cast to object type in user function)</param>
      /// <returns></returns>
      private int objectiveFunction(double[] p, double[] fvec, IList<double>[] dvec, object prv)
      {
         var values = ParameterValuesFrom(p);
         var error = _objectiveFunc(values);
         var residuals = error.AllResidualValues;

         if (error.ResidualsResult.ExceptionOccured)
         {
            _objectiveFunctionErrorMessage = error.ResidualsResult.ExceptionMessage;
            return ERROR;
         }

         for (int i = 0; i < fvec.Length; i++)
         {
            fvec[i] = residuals[i];
         }

         return SUCCESS;
      }

      /* Relative chi-square convergence criterium */
      private double ftol
      {
         get { return _extendedPropertyStore.Get(x => x.ftol); }
         set { _extendedPropertyStore.Set(x => x.ftol, value); }
      }

      /* Relative parameter convergence criterium */
      private double xtol
      {
         get { return _extendedPropertyStore.Get(x => x.xtol); }
         set { _extendedPropertyStore.Set(x => x.xtol, value); }
      }

      /* Orthogonality convergence criterium */
      private double gtol
      {
         get { return _extendedPropertyStore.Get(x => x.gtol); }
         set { _extendedPropertyStore.Set(x => x.gtol, value); }
      }

      /* Initial step bound */
      private double stepfactor
      {
         get { return _extendedPropertyStore.Get(x => x.stepfactor); }
         set { _extendedPropertyStore.Set(x => x.stepfactor, value); }
      }

      /* Maximum number of iterations.  If maxiter == 0,
                             then basic error checking is done, and parameter
                             errors/covariances are estimated based on input
                             parameter values, but no fitting iterations are done. */
      private int maxiter
      {
         get { return _extendedPropertyStore.Get(x => x.maxiter); }
         set { _extendedPropertyStore.Set(x => x.maxiter, value); }
      }

      /* Maximum number of function evaluations */
      private int maxfev
      {
         get { return _extendedPropertyStore.Get(x => x.maxfev); }
         set { _extendedPropertyStore.Set(x => x.maxfev, value); }
      }

      /// <summary>
      ///    Finite derivative step size
      /// </summary>
      private double epsfcn
      {
         get { return _extendedPropertyStore.Get(x => x.epsfcn); }
         set { _extendedPropertyStore.Set(x => x.epsfcn, value); }
      }

    
   }
}
namespace OSPSuite.Core.Domain
{
   public class SolverSettings : Container
   {
      public virtual bool UseJacobian
      {
         get { return this.Parameter(Constants.Parameters.USE_JACOBIAN).Value == 1; }
         set { this.Parameter(Constants.Parameters.USE_JACOBIAN).Value = value ? 1 : 0; }
      }

      public virtual double H0
      {
         get { return this.Parameter(Constants.Parameters.H0).Value; }
         set { this.Parameter(Constants.Parameters.H0).Value = value; }
      }

      public virtual double HMin
      {
         get { return this.Parameter(Constants.Parameters.H_MIN).Value; }
         set { this.Parameter(Constants.Parameters.H_MIN).Value = value; }
      }

      public virtual double HMax
      {
         get { return this.Parameter(Constants.Parameters.H_MAX).Value; }
         set { this.Parameter(Constants.Parameters.H_MAX).Value = value; }
      }

      public virtual int MxStep
      {
         get { return (int) this.Parameter(Constants.Parameters.MX_STEP).Value; }
         set { this.Parameter(Constants.Parameters.MX_STEP).Value = value; }
      }

      public virtual double RelTol
      {
         get { return this.Parameter(Constants.Parameters.REL_TOL).Value; }
         set { this.Parameter(Constants.Parameters.REL_TOL).Value = value; }
      }

      public virtual double AbsTol
      {
         get { return this.Parameter(Constants.Parameters.ABS_TOL).Value; }
         set { this.Parameter(Constants.Parameters.ABS_TOL).Value = value; }
      }
   }
}
namespace OSPSuite.Core.Domain
{
   public class OutputInterval : Container
   {
      public OutputInterval()
      {
         Name = Constants.OUTPUT_INTERVAL;
      }

      public virtual IParameter StartTime => this.Parameter(Constants.Parameters.START_TIME);

      public virtual IParameter EndTime => this.Parameter(Constants.Parameters.END_TIME);

      public virtual IParameter Resolution => this.Parameter(Constants.Parameters.RESOLUTION);
   }
}
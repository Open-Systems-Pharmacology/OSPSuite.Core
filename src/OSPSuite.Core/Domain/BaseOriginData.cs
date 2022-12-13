namespace OSPSuite.Core.Domain
{
   public abstract class BaseOriginData : IWithValueOrigin
   {
      protected BaseOriginData()
      {
         //Weight is the only parameter that should be defined for all species
         Weight = new OriginDataParameter();
      }

      public ValueOrigin ValueOrigin { get; } = new ValueOrigin();
      public OriginDataParameter Age { get; set; }

      public OriginDataParameter Height { get; set; }
      public OriginDataParameter BMI { get; set; }
      public OriginDataParameter Weight { get; set; }
      public string Comment { get; set; }

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         ValueOrigin.UpdateFrom(sourceValueOrigin);
      }

      protected virtual void UpdateProperties(BaseOriginData clone)
      {
         //Weight is the only required parameter
         clone.Weight = Weight.Clone();

         clone.Age = Age?.Clone();
         clone.Comment = Comment;
         clone.Height = Height?.Clone();
         clone.BMI = BMI?.Clone();
         clone.UpdateValueOriginFrom(ValueOrigin);
      }
   }
}
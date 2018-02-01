using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IProcessRateParameterCreator
   {
      IParameter CreateProcessRateParameterFor(IProcessBuilder processBuilder, IBuildConfiguration buildConfiguration);
   }

   public class ProcessRateParameterCreator : IProcessRateParameterCreator
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;

      public ProcessRateParameterCreator(IObjectBaseFactory objectBaseFactory, IFormulaBuilderToFormulaMapper formulaMapper)
      {
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
      }

      public IParameter CreateProcessRateParameterFor(IProcessBuilder processBuilder, IBuildConfiguration buildConfiguration)
      {
         var parameter = _objectBaseFactory
            .Create<IParameter>()
            .WithName(Constants.Parameters.PROCESS_RATE)
            .WithDimension(processBuilder.Dimension)
            .WithFormula(_formulaMapper.MapFrom(processBuilder.Formula, buildConfiguration));

         parameter.Visible = false;
         parameter.Editable = false;
         parameter.IsDefault = true;

         addAdditionalParentReference(parameter.Formula);

         buildConfiguration.AddBuilderReference(parameter, processBuilder);

         if (processBuilder.ProcessRateParameterPersistable)
            parameter.Persistable = true;

         parameter.AddTag(processBuilder.Name);
         parameter.AddTag(Constants.Parameters.PROCESS_RATE);

         return parameter;
      }

      private void addAdditionalParentReference(IFormula formula)
      {
         foreach (var objectPath in formula.ObjectPaths)
         {
            if (isRelativePath(objectPath))
               objectPath.AddAtFront(ObjectPath.PARENT_CONTAINER);
         }
      }

      private bool isRelativePath(IObjectPath objectPath)
      {
         if (!objectPath.Any())
            return false;

         if (objectPath[0] == ObjectPath.PARENT_CONTAINER)
            return true;

         if (objectPath.Count == 1)
            return true;

         return false;
      }
   }
}
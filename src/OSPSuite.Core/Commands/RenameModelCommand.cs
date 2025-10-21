using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class RenameModelCommand : OSPSuiteReversibleCommand<IOSPSuiteExecutionContext>
   {
      private IModel _model;

      private readonly string _modelId;
      private readonly string _newName;
      private readonly string _oldName;

      public RenameModelCommand(IModel model, string newName)
      {
         _model = model;
         _newName = newName;
         _oldName = model.Name;
         _modelId = model.Id;
         ObjectType = ObjectTypes.Model;
         CommandType = Command.CommandTypeRename;
         Description = Command.RenameSimulation(_oldName, _newName);
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         _model.Name = _newName;
         var root = _model.Root;
         root.Name = _newName;

         updateFormulasInContainer(root);
         updateFormulasInContainer(_model.Neighborhoods);

         var allEventAssignments = root.GetAllChildren<EventAssignment>();
         allEventAssignments.Each(ea => updateObjectPath(ea.ObjectPath));
      }

      protected override void ClearReferences()
      {
         _model = null;
      }

      private void updateFormulasInContainer(IContainer root)
      {
         var allFormulas = root.GetAllChildren<IUsingFormula>(uf => !uf.Formula.IsConstant()).Select(uf => uf.Formula);
         allFormulas.Each(updateObjectPaths);
         allFormulas = root.GetAllChildren<IParameter>(p => p.RHSFormula != null).Select(p => p.RHSFormula);
         allFormulas.Each(updateObjectPaths);
      }

      private void updateObjectPaths(IFormula formula)
      {
         formula.ObjectPaths.Each(updateObjectPath);
      }

      private void updateObjectPath(ObjectPath path)
      {
         if (!path.First().Equals(_oldName))
            return;

         path.Remove(_oldName);
         path.AddAtFront(_newName);
      }

      protected override ICommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context) =>
         new RenameModelCommand(_model, _oldName).AsInverseFor(this);

      public override void RestoreExecutionData(IOSPSuiteExecutionContext context)
      {
         _model = context.Get<IModel>(_modelId);
      }
   }
}
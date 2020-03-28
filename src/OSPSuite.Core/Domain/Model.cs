using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   public interface IModel : IObjectBase, IMolWeightFinder
   {
      IContainer Root { get; set; }
      IContainer Neighborhoods { set; get; }
   }

   public class Model : ObjectBase, IModel
   {
      private IContainer _neighborhoods;

      private IContainer _root;

      public IContainer Neighborhoods
      {
         set
         {
            if (Root != null && _neighborhoods != null)
            {
               Root.RemoveChild(_neighborhoods);
            }
            _neighborhoods = value;
            Root?.Add(_neighborhoods);
         }
         get => _neighborhoods;
      }

      public double? MolWeightFor(IQuantity quantity)
      {
         if (Root == null || quantity == null)
            return null;

         var moleculeName = quantity.IsAnImplementationOf<IMoleculeAmount>() ? 
            quantity.Name : 
            quantity.ParentContainer?.Name;

         if (string.IsNullOrEmpty(moleculeName))
            return null;

         //try to find the molweight parameter in the global molecule container
         var molWeightParameter = Root.EntityAt<IParameter>(moleculeName, Constants.Parameters.MOL_WEIGHT);
         return molWeightParameter?.Value;
      }

      public double? MolWeightFor(string quantityPath)
      {
         var quantity = Root?.EntityAt<IQuantity>(quantityPath.ToPathArray());
         return MolWeightFor(quantity);
      }

      public IContainer Root
      {
         get => _root;
         set
         {
            _root = value;
            if (_neighborhoods != null && !_root.ContainsName(Constants.NEIGHBORHOODS))
            {
               _root.Add(_neighborhoods);
            }
         }
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         Root.AcceptVisitor(visitor);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var sourceModel = source as IModel;
         if (sourceModel == null) return;

         //root container must be cloned BEFORE neighborhoods
         Root = cloneManager.Clone(sourceModel.Root);

         //Neighborhoods-subnode is already cloned (as one of the Root-children)
         // so only internal property must be set
         _neighborhoods = Root.Container(sourceModel.Neighborhoods.Name);
      }
   }
}

using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    A Reaction converts molecules to other
   /// </summary>
   public class Reaction : Process
   {
      private readonly List<ReactionPartner> _educts;
      private readonly List<ReactionPartner> _products;
      private readonly List<string> _modifier;

      public Reaction()
      {
         _educts = new List<ReactionPartner>();
         _products = new List<ReactionPartner>();
         _modifier = new List<string>();
         ContainerType = ContainerType.Reaction;
         Icon = IconNames.REACTION;
      }

      /// <summary>
      ///    Gets the educts of the reaction.
      /// </summary>
      /// <value>The educts.</value>
      public IEnumerable<ReactionPartner> Educts => _educts;

      /// <summary>
      ///    Adds the modifier name.
      /// </summary>
      /// <param name="modifierName">Name of the modifier.</param>
      public void AddModifier(string modifierName) => _modifier.Add(modifierName);

      /// <summary>
      ///    Adds the educt to the reactions educt list.
      /// </summary>
      /// <param name="newEduct">The new educt.</param>
      public void AddEduct(ReactionPartner newEduct) => _educts.Add(newEduct);

      /// <summary>
      ///    Gets the products of the reaction.
      /// </summary>
      /// <value>The products.</value>

      public IEnumerable<ReactionPartner> Products => _products;

      /// <summary>
      ///    Gets the Modifier Names.
      /// </summary>
      /// <value>The products.</value>
      public IEnumerable<string> ModifierNames => _modifier;

      /// <summary>
      ///    Adds the product to the reactions product list.
      /// </summary>
      /// <param name="newProduct">The new product.</param>
      public void AddProduct(ReactionPartner newProduct) => _products.Add(newProduct);

      public override bool Uses(MoleculeAmount amount)
      {
         return
            _educts.Any(x => Equals(x.Partner, amount)) ||
            _products.Any(x => Equals(x.Partner, amount));
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcReaction = source as Reaction;
         if (srcReaction == null) return;
         srcReaction.ModifierNames.Each(AddModifier);
         //Educts/Products should NOT be cloned
         //Instead, some Model-Finalizer must be called
      }
   }
}
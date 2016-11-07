using System;
using System.Linq;
using NLog;
using VectorImageEdit.Modules.GraphicsCompositing;
using VectorImageEdit.Modules.Interfaces;

namespace VectorImageEdit.Modules.LayerManagement.LayerModifiers
{
     /// <summary>
     /// Base class for a modifier action that can affect multiple layers.
     /// </summary>
    abstract class LayerModifierBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected readonly ILayerHandler HandlerLayers;
        protected readonly IGraphicsHandler HandlerGraphics;

        protected LayerModifierBase(ILayerHandler manager, IGraphicsHandler updater)
        {
            HandlerLayers = manager;
            HandlerGraphics = updater;
        }

         /// <summary>
         /// Wrapper for the concrete modifier action, used for error handling and preconditions checking.
         /// </summary>
         /// <param name="concreteBehavior"> External method to execute </param>
         /// <param name="policy"> The update policy to employ </param>
         /// <param name="preconditions"> Predicates to check before executing the action </param>
        protected void ApplyModifier(Action concreteBehavior, IRenderingPolicy policy, params Func<bool>[] preconditions)
        {
            try
            {
                if (HasLessThanTwoLayers()) return;
                if (BreaksPreconditions(preconditions)) return;

                concreteBehavior();

                HandlerGraphics.UpdateFrame(HandlerLayers.WorkspaceLayers, policy);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString());
            }
        }

        private bool BreaksPreconditions(params Func<bool>[] preconditions)
        {
            return preconditions.Any(condition => condition() == false);
        }
        private bool HasLessThanTwoLayers()
        {
            return HandlerLayers.WorkspaceLayers.Count < 2;
        }
    }
}

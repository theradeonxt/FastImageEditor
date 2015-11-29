using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NLog;
using VectorImageEdit.Modules.Interfaces;

namespace VectorImageEdit.Modules.LayerManagement.LayerModifiers
{
    public class GenericModifier
    {
        private readonly Dictionary<ModifierType, ILayerModifier> _modifierActionMap;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public GenericModifier(ILayerHandler layerMan, IGraphicsHandler graphicsMan)
        {
            // Initialize the default modifier mapping
            _modifierActionMap = new Dictionary<ModifierType, ILayerModifier>
            {
                {ModifierType.BringToFront, new BringToFrontModifier(layerMan, graphicsMan)},
                {ModifierType.SendToBack, new SendToBackModifier(layerMan, graphicsMan)},
                {ModifierType.SendBackward, new SendBackwardModifier(layerMan, graphicsMan)}
            };
        }

        public void ApplyModifier([NotNull]Layer layer, [CanBeNull]string modifierName)
        {
            ModifierType type;
            if (!string.IsNullOrEmpty(modifierName) && Enum.TryParse(modifierName, out type))
            {
                _modifierActionMap[type].ApplyModifier(layer);
            }
            else
            {
                Logger.Warn(string.Format("Modifier name was invalid: {0}{1}", (modifierName ?? ""), 
                    Environment.NewLine), Environment.StackTrace);
            }
        }
        public void ApplyModifier(Layer layer, ModifierType modifier)
        {
            _modifierActionMap[modifier].ApplyModifier(layer);
        }
    }
}

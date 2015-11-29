using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;  // TODO: Fix
using JetBrains.Annotations;
using VectorImageEdit.Modules.GraphicsCompositing;
using VectorImageEdit.Modules.Interfaces;
using VectorImageEdit.Modules.LayerManagement.LayerModifiers;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules.LayerManagement
{
    public enum InsertionPolicy
    {
        BringToFront,   // Ensure layer is added to the top-most visible level
        KeepAsRequested // Layer is added on the level it was explicitly set to
    };

    // TODO: Separate LayerManager and GraphicsManager

    ///// <summary>
    /// LayerManager Module
    ///
    /// - add & remove objects
    /// - object query functions
    /// 
    ///// </summary>
    public class LayerManager : GraphicsManager, ILayerHandler
    {
        private readonly SortedContainer<Layer> _activeLayers;
        private readonly GenericModifier _modifiers;

        public LayerManager(Control formControl,
            Action<GraphicsProfiler> graphicsUpdateCallback)
            : base(formControl, graphicsUpdateCallback)
        {
            _modifiers = new GenericModifier(this, this);

            _activeLayers = new SortedContainer<Layer>();
            
            InsertionPolicy = InsertionPolicy.BringToFront;
        }

        public InsertionPolicy InsertionPolicy { get; set; }
        public SortedContainer<Layer> WorkspaceLayers { get { return _activeLayers; } }

        public void Add([NotNull]Layer newLayer)
        {
            if (InsertionPolicy == InsertionPolicy.BringToFront) ApplyModifier("BringToFront", newLayer);

            _activeLayers.Add(newLayer);
            //_layerListChangedCallback();

            UpdateFrame(_activeLayers, RenderingPolicyFactory.MinimalUpdatePolicy(newLayer.Region));
        }
        public void AddRange([NotNull]List<Layer> newLayers)
        {
            BoundingRectangle boundRect = new BoundingRectangle();
            
            foreach (Layer layer in newLayers)
            {
                if (InsertionPolicy == InsertionPolicy.BringToFront) ApplyModifier("BringToFront", layer);
                _activeLayers.Add(layer);

                boundRect.EnlargeToFit(layer.Region);
            }

            //_layerListChangedCallback();

            UpdateFrame(_activeLayers, RenderingPolicyFactory.MinimalUpdatePolicy(boundRect.Region));
        }
        public void Remove([NotNull]Layer existingLayer)
        {
            Rectangle oldRegion = existingLayer.Region;

            // Do necessary cleanup operations
            _activeLayers.Remove(existingLayer);
            //_layerListChangedCallback();
            existingLayer.Dispose();

            UpdateFrame(_activeLayers, RenderingPolicyFactory.MinimalUpdatePolicy(oldRegion));
        }
        public void RemoveAll()
        {
            BoundingRectangle boundRect = new BoundingRectangle();

            foreach (Layer layer in _activeLayers)
            {
                boundRect.EnlargeToFit(layer.Region);
                layer.Dispose();
            }
            _activeLayers.Clear();
            //_layerListChangedCallback();

            UpdateFrame(_activeLayers, RenderingPolicyFactory.MinimalUpdatePolicy(boundRect.Region));
        }

        public void ApplyModifier([CanBeNull]string modifierName, [NotNull]Layer layer)
        {
            _modifiers.ApplyModifier(layer, modifierName);
        }
        
        [NotNull]
        public List<Layer> GetLayers()
        {
            return _activeLayers.ToList();
        }
        [NotNull]
        public List<Layer> GetShapesList()
        {
            return _activeLayers.Where(layer => !(layer is Picture)).ToList();
        }

        private void ObjectUpdateCallback(Rectangle objectRegion, IRenderingPolicy renderPolicy)
        {
            // Update the invaidated frame region
            UpdateFrame(_activeLayers, renderPolicy);

            // Redraw the selection rectangle over the frame
            UpdateSelection(objectRegion);
        }
    }
}

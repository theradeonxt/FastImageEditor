using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;
using VectorImageEdit.Modules.BasicShapes;
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

    /// <summary>
    /// LayerManager Module
    ///
    /// - add & remove objects
    /// - object query functions
    /// 
    /// </summary>
    public class LayerManager : GraphicsManager, ILayerHandler
    {
        private readonly SortedContainer<Layer> activeLayers;
        private readonly GenericModifier modifiers;

        public LayerManager(IGraphicsSurface surfaceInfo, Action<GraphicsProfiler> graphicsUpdateCallback)
            : base(surfaceInfo, graphicsUpdateCallback)
        {
            modifiers = new GenericModifier(this, this);

            activeLayers = new SortedContainer<Layer>();

            InsertionPolicy = InsertionPolicy.BringToFront;
        }

        #region Properties

        public InsertionPolicy InsertionPolicy { get; set; }

        public SortedContainer<Layer> WorkspaceLayers { get { return activeLayers; } }

        [NotNull]
        public List<Layer> LayersList
        {
            get { return activeLayers.ToList(); }
        }

        [NotNull]
        public List<Layer> ShapesList
        {
            get { return activeLayers.Where(layer => !(layer is Picture)).ToList(); }
        }

        #endregion

        public void Add([NotNull]Layer newLayer)
        {
            if (InsertionPolicy == InsertionPolicy.BringToFront) ApplyModifier("BringToFront", newLayer);

            activeLayers.Add(newLayer);
            //_layerListChangedCallback();

            UpdateFrame(activeLayers, RenderingPolicy.MinimalUpdatePolicy(newLayer.Region));
        }

        public void AddRange([NotNull]List<Layer> newLayers)
        {
            BoundingRectangle boundRect = new BoundingRectangle();

            foreach (Layer layer in newLayers)
            {
                if (InsertionPolicy == InsertionPolicy.BringToFront) ApplyModifier("BringToFront", layer);
                activeLayers.Add(layer);

                boundRect.EnlargeToFit(layer.Region);
            }

            //_layerListChangedCallback();

            UpdateFrame(activeLayers, RenderingPolicy.MinimalUpdatePolicy(boundRect.Region));
        }

        public void Remove([NotNull]Layer existingLayer)
        {
            Rectangle oldRegion = existingLayer.Region;

            // Do necessary cleanup operations
            activeLayers.Remove(existingLayer);
            //_layerListChangedCallback();
            existingLayer.Dispose();

            UpdateFrame(activeLayers, RenderingPolicy.MinimalUpdatePolicy(oldRegion));
        }

        public void RemoveAll()
        {
            BoundingRectangle boundRect = new BoundingRectangle();

            foreach (Layer layer in activeLayers)
            {
                boundRect.EnlargeToFit(layer.Region);
                layer.Dispose();
            }
            activeLayers.Clear();
            //_layerListChangedCallback();

            UpdateFrame(activeLayers, RenderingPolicy.MinimalUpdatePolicy(boundRect.Region));
        }

        public void ApplyModifier([CanBeNull]string modifierName, [NotNull]Layer layer)
        {
            modifiers.ApplyModifier(layer, modifierName);
        }

        private void ObjectUpdateCallback(Rectangle objectRegion, [NotNull]IRenderingPolicy renderPolicy)
        {
            // Update the invaidated frame region
            UpdateFrame(activeLayers, renderPolicy);

            // Redraw the selection rectangle over the frame
            UpdateSelection(objectRegion);
        }
    }
}

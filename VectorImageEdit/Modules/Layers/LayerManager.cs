using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;
using VectorImageEdit.Modules.Utility;

namespace VectorImageEdit.Modules.Layers
{
    public enum InsertionPolicy
    {
        BringToFront,   // Ensure layer is added to the top-most visible level
        KeepAsRequested // Layer is added on the level it was explicitly set to
    };

    /// <summary>
    /// LayerManager Module
    ///
    /// - add & remove objects
    /// - object query functions
    /// 
    /// </summary>
    public partial class LayerManager : GraphicsManager
    {
        public readonly MouseInteraction MouseHandler;

        private readonly SortedContainer<Layer> _activeLayers;
        private readonly Action _layerListChangedCallback;

        public LayerManager(Control formControl, Action layerListChangedCallback, 
            Action<GraphicsProfiler> graphicsUpdateCallback)
            : base(formControl, graphicsUpdateCallback)
        {
            _layerListChangedCallback = layerListChangedCallback;

            _activeLayers = new SortedContainer<Layer>();
            MouseHandler = new MouseInteraction(_activeLayers, ObjectUpdateCallback);

            // TODO: move to its own controller
            formControl.MouseDown += MouseHandler.MouseDown;
            formControl.MouseMove += MouseHandler.MouseMovement;
            formControl.MouseUp += MouseHandler.MouseUp;
        }

        public void Add([NotNull]Layer newLayer, InsertionPolicy policy = InsertionPolicy.BringToFront)
        {
            if (policy == InsertionPolicy.BringToFront) BringToFront(newLayer);
            _activeLayers.Add(newLayer);
            _layerListChangedCallback();
            UpdateFrame(_activeLayers);
        }
        public void Add([NotNull]List<Layer> newLayers, InsertionPolicy policy = InsertionPolicy.BringToFront)
        {
            // Does not use Add of one Layer to save a few expensive calls to UpdateFrame
            foreach (Layer layer in newLayers)
            {
                if (policy == InsertionPolicy.BringToFront) BringToFront(layer);
                _activeLayers.Add(layer);
            }
            _layerListChangedCallback();
            UpdateFrame(_activeLayers);
        }

        public void Remove([NotNull]Layer existingLayer)
        {
            if (existingLayer == MouseInteraction.DummyLayer) return;

            _activeLayers.Remove(existingLayer);
            _layerListChangedCallback();

            existingLayer.Dispose();
            UpdateFrame(_activeLayers);
        }
        public void RemoveAll()
        {
            foreach (Layer layer in _activeLayers)
            {
                layer.Dispose();
            }
            _activeLayers.Clear();
            _layerListChangedCallback();
        }

        [NotNull]
        public SortedContainer<Layer> GetSortedLayers()
        {
            return _activeLayers;
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

        private void ObjectUpdateCallback(Rectangle objectRegion)
        {
            UpdateFrame(_activeLayers);
            UpdateSelection(objectRegion);
        }
    }
}

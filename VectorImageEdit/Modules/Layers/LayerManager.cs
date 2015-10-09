using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
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
        private readonly Action _onLayerListChangedCallback;

        public LayerManager(Control formControl, Action onLayerListChangedCallback)
            : base(formControl)
        {
            _activeLayers = new SortedContainer<Layer>();
            _onLayerListChangedCallback = onLayerListChangedCallback;
            MouseHandler = new MouseInteraction(_activeLayers, ObjectUpdateCallback);

            // TODO: move to its own controller
            formControl.MouseDown += MouseHandler.MouseDown;
            formControl.MouseMove += MouseHandler.MouseMovement;
            formControl.MouseUp += MouseHandler.MouseUp;
        }

        #region Objects creation/deletion

        public void Add(Layer newLayer, InsertionPolicy policy = InsertionPolicy.BringToFront)
        {
            if (policy == InsertionPolicy.BringToFront) BringToFront(newLayer);
            _activeLayers.Add(newLayer);
            _onLayerListChangedCallback();
            UpdateFrame(_activeLayers);
        }

        public void Add(List<Layer> newLayers, InsertionPolicy policy = InsertionPolicy.BringToFront)
        {
            // Does not use Add of one Layer to save a few expensive calls to UpdateFrame
            foreach (Layer layer in newLayers)
            {
                if (policy == InsertionPolicy.BringToFront) BringToFront(layer);
                _activeLayers.Add(layer);
            }
            _onLayerListChangedCallback();
            UpdateFrame(_activeLayers);
        }

        public void Remove(Layer existingLayer)
        {
            if (existingLayer == null) return;

            _activeLayers.Remove(existingLayer);
            _onLayerListChangedCallback();

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
            _onLayerListChangedCallback();
        }

        #endregion Objects creation/deletion

        public SortedContainer<Layer> GetSortedLayers()
        {
            return _activeLayers;
        }

        public List<Layer> GetLayers()
        {
            return _activeLayers.ToList();
        }

        // TODO: List<ShapeBase>
        public List<Layer> GetShapesList()
        {
            return _activeLayers.Where(layer => !(layer is Picture)).ToList();
        }

        // TODO: this ClearMode is very ambiguous
        private void ObjectUpdateCallback(Rectangle objectRegion, ClearMode mode)
        {
            if (mode == ClearMode.FullUpdate)
            {
                UpdateFrame(_activeLayers);
                UpdateSelection(objectRegion);
            }
            else
            {
                UpdateFrame(_activeLayers); // this was not previously here
                UpdateSelection(objectRegion);
            }
        }
    }
}

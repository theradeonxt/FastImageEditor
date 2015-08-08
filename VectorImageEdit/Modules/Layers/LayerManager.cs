using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace VectorImageEdit.Modules.Layers
{
    public enum InsertionPolicy
    {
        BringToFront, // Ensure layer is added to the top-most visible level
        Default       // Layer is added on the level it was explicitly set to
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
        private readonly SortedContainer _activeLayers;
        private readonly ListBox _uiObjectList;

        public readonly MouseInteraction MouseHandler;

        public LayerManager(Control formControl, ListBox formLayerList)
            : base(formControl)
        {
            _activeLayers = new SortedContainer();
            MouseHandler = new MouseInteraction(_activeLayers, ObjectUpdateCallback);

            formControl.MouseDown += MouseHandler.MouseDown;
            formControl.MouseMove += MouseHandler.MouseMovement;
            formControl.MouseUp += MouseHandler.MouseUp;

            _uiObjectList = formLayerList;
            _uiObjectList.Items.Clear();
            _uiObjectList.SelectedIndexChanged += uiObjectList_SelectedIndexChanged;
        }

        void uiObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = _uiObjectList.SelectedIndex;
            if (index < 0 || index >= _activeLayers.Count) return;
            MouseHandler.SelectedLayer = _activeLayers[index];
        }

        #region Object(s) creation/deletion

        public void Add(Layer newLayer, InsertionPolicy policy = InsertionPolicy.BringToFront)
        {
            if (policy == InsertionPolicy.BringToFront) BringToFront(newLayer);
            _activeLayers.Add(newLayer);
            _uiObjectList.Items.Add(newLayer.ToString());
            UpdateFrame(_activeLayers);
        }

        public void Add(List<Layer> newLayers, InsertionPolicy policy = InsertionPolicy.BringToFront)
        {
            foreach (Layer layer in newLayers)
            {
                if (policy == InsertionPolicy.BringToFront) BringToFront(layer);
                _uiObjectList.Items.Add(layer.ToString());
                _activeLayers.Add(layer);
            }
            UpdateFrame(_activeLayers);
        }

        public void Remove(Layer existingLayer)
        {
            if (existingLayer == null) return;

            _activeLayers.Remove(existingLayer);
            _uiObjectList.Items.Remove(existingLayer.ToString());

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
        }

        #endregion

        public SortedContainer GetSortedLayers()
        {
            return _activeLayers;
        }

        public List<Layer> GetLayersList()
        {
            return _activeLayers.ToList();
        }

        public List<Layer> GetShapesList()
        {
            return _activeLayers.Where(layer => !(layer is Picture)).ToList();
        }

        private void ObjectUpdateCallback(Rectangle objectRegion, ClearMode mode)
        {
            if (mode == ClearMode.FullUpdate)
            {
                UpdateFrame(_activeLayers);
                UpdateSelection(objectRegion, ClearMode.NoClear);
            }
            else
            {
                UpdateSelection(objectRegion, mode);
            }
        }
    }
}

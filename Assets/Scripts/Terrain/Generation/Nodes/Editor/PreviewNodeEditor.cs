using UnityEngine;
using XNodeEditor;
// ReSharper disable UnusedType.Global

namespace Rebirth.Terrain.Generation.Nodes.Editor
{
    public abstract class PreviewNodeEditor : NodeEditor
    {
        private Texture2D _texture;

        public override void OnBodyGUI()
        {

            base.OnBodyGUI();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate"))
            {
                if (_texture != null)
                {
                    Object.DestroyImmediate(_texture);
                }

                if (target is IPreviewNode node)
                {
                    _texture = node.Generate();
                }
            }

            if (GUILayout.Button("Clear"))
            {
                Object.DestroyImmediate(_texture);
                _texture = null;
            }
            
            GUILayout.EndHorizontal();
            
            if (_texture != null)
            {
                var rect = GUILayoutUtility.GetRect(150, 150);
                GUI.DrawTexture(rect, _texture, ScaleMode.ScaleToFit);
            }
        }
    }

    [CustomNodeEditor(typeof(SlicePreviewNode))]
    public class SlicePreviewNodeEditor : PreviewNodeEditor
    {
    }

    [CustomNodeEditor(typeof(VoxelSlicePreviewNode))]
    public class VoxelPreviewNodeEditor : PreviewNodeEditor
    {
    }
    
    [CustomNodeEditor(typeof(CombinedSlicePreviewNode))]
    public class CombinedPreviewNodeEditor : PreviewNodeEditor
    {
    }
}

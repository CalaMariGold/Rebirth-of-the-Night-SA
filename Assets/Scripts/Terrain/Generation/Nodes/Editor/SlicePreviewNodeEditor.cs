using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Rebirth.Terrain.Generation.Nodes.Editor
{
    [CustomNodeEditor(typeof(SlicePreviewNode))]
    // ReSharper disable once UnusedType.Global
    public class SlicePreviewNodeEditor : NodeEditor
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

                if (target is SlicePreviewNode node)
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
}

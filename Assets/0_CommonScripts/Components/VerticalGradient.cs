using UnityEngine;
using UnityEngine.UI;

public class VerticalGradient : BaseMeshEffect
{
    [SerializeField] Color topColor = Color.white;
    [SerializeField] Color bottomColor = Color.white;

    public override void ModifyMesh(VertexHelper vertexHelper)
    {
        if (enabled)
        {
            UIVertex vertex = default;
            var t = new float[4] { 0f, 1f, 1f, 0f };

            for (int i = 0; i < vertexHelper.currentVertCount; i++)
            {
                vertexHelper.PopulateUIVertex(ref vertex, i);
                vertex.color *= Color.Lerp(bottomColor, topColor, t[i]);
                vertexHelper.SetUIVertex(vertex, i);
            }
        }
    }
}

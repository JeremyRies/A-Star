using UnityEngine;

public class NodeView : MonoBehaviour
{
    private Node _node;

    public GameObject Arrow;

    public GameObject HoeDoor;
    public GameObject Door;

    [Range(0, 0.5f)] public float BorderSize = 0.15f;

    public GameObject Tile;

    public void Init(Node node)
    {
        if (Tile != null)
        {
            gameObject.name = "Node (" + node.XIndex + "," + node.YIndex + ")";
            gameObject.transform.position = node.Position;
            Tile.transform.localScale = new Vector3(1f - BorderSize, 1f, 1f - BorderSize);
            _node = node;

            EnableObject(Arrow, false);
        }
    }

    private void ColorNode(Color color, GameObject go)
    {
        if (go != null)
        {
            var goRenderer = go.GetComponent<Renderer>();

            if (goRenderer != null) goRenderer.material.color = color;
        }
    }

    public void ColorNode(Color color)
    {
        ColorNode(color, Tile);
    }

    private void EnableObject(GameObject go, bool state)
    {
        if (go != null) go.SetActive(state);
    }

    public void ShowArrow(Color color)
    {
        if (_node != null && Arrow != null && _node.Previous != null)
        {
            EnableObject(Arrow, true);

            var dirToPrevious = (_node.Previous.Position - _node.Position).normalized;

            Arrow.transform.rotation = Quaternion.LookRotation(dirToPrevious);

            var arrowRenderer = Arrow.GetComponent<Renderer>();
            if (arrowRenderer != null) arrowRenderer.material.color = color;
        }
    }
}
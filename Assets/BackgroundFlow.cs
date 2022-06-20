using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundFlow : MonoBehaviour
{
    private float scrollSpeed;
    private float offset;
    private Tilemap tilemap;
    void Start()
    {
        offset = 1f;
        scrollSpeed = 1f;
        tilemap = GetComponent<Tilemap>();
    }
    void Update()
    {
        offset -= Time.deltaTime * scrollSpeed;
        if (offset < 0f) offset = 1;
        tilemap.tileAnchor = new Vector3(0, offset, 0);
    }
}

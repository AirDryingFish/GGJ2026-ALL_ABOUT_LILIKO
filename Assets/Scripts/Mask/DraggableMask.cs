using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DraggableMask : MonoBehaviour
{
    public Camera cam;
    private Vector3 _grabOffsetWorld;
    private Vector3 _ori;
    private Collider2D _collider;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        _ori = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 delta = cam.ScreenToWorldPoint(Input.mousePosition) - cam.ScreenToWorldPoint(_ori);
        _ori = Input.mousePosition;

        transform.Translate(new Vector3(delta.x, delta.y, transform.position.z), Space.World);
    }

    public bool isInMask(Vector2 worldPos)
    {
        return _collider.OverlapPoint(worldPos);
    }
}

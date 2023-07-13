using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class PlatformMove : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static event Action<PlatformMove> OnPlatformDragBegin;
    public static event Action OnPlatformDragEnd;
    [SerializeField]
    private Transform moveMeObject;
    [SerializeField]
    private float moveSpeed = 50f;

    [Space]
    [SerializeField]
    private Material standartMat;
    [SerializeField]
    private Material onDragMat;
    [Space]
    [SerializeField]
    private MeshRenderer meshRenderer;
    [Space]
    [SerializeField]
    private ConnectorLogic connectorLogic;
    public void OnBeginDrag(PointerEventData eventData)
    {
        OnPlatformDragBegin?.Invoke(this);
        meshRenderer.material = onDragMat;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnPlatformDragEnd?.Invoke();
        meshRenderer.material = standartMat;
    }

    public void MoveToPosition(Vector3 newPosition)
    {
        moveMeObject.position = Vector3.MoveTowards(moveMeObject.position, newPosition, moveSpeed * Time.deltaTime);
        connectorLogic.MoveEvent();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Collider))]
public class ConnectorLogic : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Space]
    [SerializeField]
    private Material standartMat;
    [SerializeField]
    private Material onSelected;
    [SerializeField]
    private Material onNotSelected;
    [Space]
    [SerializeField]
    private MeshRenderer meshRenderer;

    [Space]
    [SerializeField]
    private GameObject lineTemplate;
    [SerializeField]
    private Transform lineParent;


    public static event Action<Transform> OnDragConnectionStart;
    public static event Action OnDragConnectionEnd;

    private static event Action<ConnectorLogic> OnConnectorSelect;
    private static event Action OnConnectorDeselect;
    private event Action OnMove;

    private static ConnectorLogic currentSelected;
    private static ConnectorLogic currentDrag;
    private static ConnectorLogic currentOppositeDrag;
    private static bool dragging = false;
    private static List<LineLogic> AllConnections = new List<LineLogic>();

    private void Awake()
    {
        OnConnectorSelect += onConnectorSelect;
        OnConnectorDeselect += onConnectorDeselect;
        GroundClick.OnGroundClick += OnGroundClick;
    }
    private void OnDestroy()
    {
        OnConnectorSelect -= onConnectorSelect;
        OnConnectorDeselect -= onConnectorDeselect;
        GroundClick.OnGroundClick -= OnGroundClick;
        OnMove = null;
        AllConnections.Clear();
    }
    private void onConnectorSelect(ConnectorLogic target)
    {
        meshRenderer.material = target == this ? onSelected : onNotSelected;
    }

    private void onConnectorDeselect()
    {
        meshRenderer.material = standartMat;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentSelected != this)
        {
            if (currentSelected != null)
            {
                CreateConnection(this, currentSelected);
                currentSelected = null;
                OnConnectorDeselect?.Invoke();
            }
            else
            {
                currentSelected = this;
                OnConnectorSelect?.Invoke(this);
            }
        }
        else
        {
            currentSelected = null;
            OnConnectorDeselect?.Invoke();
        }
    }

    private void OnGroundClick()
    {
        currentSelected = null;
        meshRenderer.material = standartMat;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    { 
    
    }

    private void CreateConnection(ConnectorLogic con1, ConnectorLogic con2)
    {
        if(AllConnections.Any(t=> t.HaveConnection(con1,con2)))
        {
            return;
        }
        GameObject nLine = Instantiate(lineTemplate, lineParent);
        nLine.SetActive(true);
        LineLogic lnLog = nLine.GetComponent<LineLogic>();
        lnLog.Init(con1.transform, con2.transform, con1, con2);
        con1.InitMoveEvent(lnLog.OnMove);
        con2.InitMoveEvent(lnLog.OnMove);
        AllConnections.Add(lnLog);
    }
    public void InitMoveEvent( Action movEv)
    {
        OnMove += movEv;
    }
    public void MoveEvent()
    {
        OnMove?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentDrag = this;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentDrag = this;
        OnConnectorSelect?.Invoke(this);
        OnDragConnectionStart?.Invoke(this.transform);
        dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(currentDrag != null && currentOppositeDrag != null)
        {
            CreateConnection(currentDrag, currentOppositeDrag);
        }
        OnConnectorDeselect?.Invoke();
        OnDragConnectionEnd?.Invoke();
        dragging = false;
        currentDrag = null;
        currentOppositeDrag = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(dragging && currentDrag != this)
        {
            meshRenderer.material = onSelected;
            currentOppositeDrag = this;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(dragging)
        {
            if (currentDrag != this)
            {
                meshRenderer.material = onNotSelected;
            }
            currentOppositeDrag = null;
        }
    }
}

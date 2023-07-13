using UnityEngine;

public class InputLogic : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private LineRenderer lineDragEffect;

    private Vector3 yLineCorrection;

    private PlatformMove platformTarget;
    private Vector3 mouseGroundPoint;
    private bool draggingLine = false;
    private void Awake()
    {
        PlatformMove.OnPlatformDragBegin += OnPlatformDragBegin;
        PlatformMove.OnPlatformDragEnd += OnPlatformDragEnd;

        ConnectorLogic.OnDragConnectionEnd += OnDragConnectionEnd;
        ConnectorLogic.OnDragConnectionStart += OnDragConnectionStart;

        Main.OnRecreateAll += OnRecreateAll;
    }
    private void OnDestroy()
    {
        PlatformMove.OnPlatformDragBegin -= OnPlatformDragBegin;
        PlatformMove.OnPlatformDragEnd -= OnPlatformDragEnd;


        ConnectorLogic.OnDragConnectionEnd -= OnDragConnectionEnd;
        ConnectorLogic.OnDragConnectionStart -= OnDragConnectionStart;


        Main.OnRecreateAll -= OnRecreateAll;
    }
    private void Update()
    {
        if(Input.GetMouseButton(0) || draggingLine)
        {
            Ray r = mainCamera.ScreenPointToRay(Input.mousePosition);
            int groundMask = 1 << 9;
            RaycastHit hit;
            if(Physics.Raycast(r, out hit, 10000f, groundMask))
            {
                mouseGroundPoint = hit.point;

                if (draggingLine)
                {
                    Vector3 nPos = hit.point + yLineCorrection;
                    lineDragEffect.SetPosition(1, nPos);
                }
            }

            if(platformTarget != null)
            {
                platformTarget.MoveToPosition(mouseGroundPoint);
            }
        }
    }

    private void OnPlatformDragBegin(PlatformMove target)
    {
        platformTarget = target;
    }

    private void OnPlatformDragEnd()
    {
        platformTarget = null;
    }

    private void OnDragConnectionEnd()
    {
        lineDragEffect.gameObject.SetActive(false);
        draggingLine = false;
    }

    private void OnDragConnectionStart( Transform initial )
    {
        draggingLine = true;
        lineDragEffect.gameObject.SetActive(true);
        lineDragEffect.SetPosition(0, initial.position);
        yLineCorrection = Vector3.Scale(initial.position, new Vector3(0f, 0.4f, 0f));
    }

    private void OnRecreateAll()
    {
        OnDragConnectionEnd();
    }
}

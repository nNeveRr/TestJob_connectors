using UnityEngine;

public class LineLogic : MonoBehaviour
{
    private Transform firstTranfsorm;
    private Transform secondTranfsorm;

    private ConnectorLogic firstConnector;
    private ConnectorLogic secondConnector;

    [SerializeField]
    private LineRenderer line;

    public void Init(Transform firstTranfsorm, Transform secondTranfsorm, ConnectorLogic firstConnector, ConnectorLogic secondConnector)
    {
        this.firstTranfsorm = firstTranfsorm;
        this.secondTranfsorm = secondTranfsorm;
        this.firstConnector = firstConnector;
        this.secondConnector = secondConnector;
        OnMove();
    }
    public bool HaveConnection (ConnectorLogic firstConnector, ConnectorLogic secondConnector)
    {
        return (firstConnector.GetInstanceID() == this.firstConnector.GetInstanceID() && secondConnector.GetInstanceID() == this.secondConnector.GetInstanceID()) 
            || (firstConnector.GetInstanceID() == this.secondConnector.GetInstanceID() && secondConnector.GetInstanceID() == this.firstConnector.GetInstanceID());
    }

    public void OnMove()
    {
        line.SetPosition(0, firstTranfsorm.transform.position);
        line.SetPosition(1, secondTranfsorm.transform.position);
    }
}

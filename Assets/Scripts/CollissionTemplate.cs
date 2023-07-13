using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollissionTemplate : MonoBehaviour
{
    [SerializeField]
    private BoxCollider _platformCollider;

    public BoxCollider platformCollider => _platformCollider;
}

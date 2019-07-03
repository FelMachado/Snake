using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_Destroy : MonoBehaviour
{
    void GetDestroyed()
    {
        GameObject.Destroy(this.gameObject);
    }
}

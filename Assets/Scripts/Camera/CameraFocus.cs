using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{

    private bool _followPlayer;
    // Start is called before the first frame update
    void Start()
    {
        _followPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_followPlayer)
        {
            transform.position = new Vector3 (
                transform.position.x,
                transform.position.y,
                PlayerController.Instance.transform.position.z
            );
        }
    }
}

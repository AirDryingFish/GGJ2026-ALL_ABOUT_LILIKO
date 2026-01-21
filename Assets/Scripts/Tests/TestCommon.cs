using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCommon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("Start");
        Timer.After(1.0f, () =>
        {
            Debug.Log("Timer expired After 1 second");
            Timer.After(4.0f, () =>
            {
                Debug.Log("Timer expired After 4 seconds");
            });
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}

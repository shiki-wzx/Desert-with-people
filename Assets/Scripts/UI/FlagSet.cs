using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlagSet : MonoBehaviour
{
    // Start is called before the first frame update
    public bool flagSet;
    public float Movedistance;
    private RectTransform flagTrans;
    public float moveSpeed;
    private float num;
    public bool numStart;
    void Start()
    {
        flagTrans=GetComponent<RectTransform>();
        flagSet = true;
        num = 0;
        numStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(flagSet)
        {
            flagTrans.Translate(new Vector3(0, -moveSpeed*Time.deltaTime, 0));
            numStart = true;
        }
        if(num>Movedistance)
        {
            flagSet = false;
            numStart = false;
        }
        if(numStart)
        {
             num += moveSpeed * Time.deltaTime;
        }
       
    }
}

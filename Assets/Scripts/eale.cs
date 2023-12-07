using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eale : MonoBehaviour
{
    [SerializeField]
    Transform player;
    SpriteRenderer sr;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        sr= GetComponent<SpriteRenderer>();
        startPos = transform.position;
        StartCoroutine(EagleAnimation());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x > transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    IEnumerator EagleAnimation()
    { 
        Vector3 endPos=new Vector3(startPos.x,startPos.y+2,startPos.z);
        bool isFlight = true;
        float value = 0;
        while (true)
        {
            yield return null;
            //Debug.Log("MOVE!!");

            if (isFlight)
                transform.position = Vector3.Lerp(startPos, endPos, value);
            else
                transform.position = Vector3.Lerp(endPos, startPos, value);

            value = value + Time.deltaTime * 2;

            if(value>1)
            {
                value = 0;
                isFlight = !isFlight;
            }

        }

    }
}

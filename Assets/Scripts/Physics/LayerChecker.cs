using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChecker : MonoBehaviour
{
    public enum LayerCheckerType
    {
       Ray,
       Circle

    }

    [SerializeField] LayerCheckerType layerCheckerType;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private Vector2 direction;
    [SerializeField] float distance;

    public bool isTouching;



    // Update is called once per frame
    void Update()
    {
        if (layerCheckerType == LayerCheckerType.Ray)
        {


            isTouching = Physics2D.Raycast(transform.position, direction, distance, targetMask);
        }


        if (layerCheckerType == LayerCheckerType.Circle)
        {


            isTouching = Physics2D.OverlapCircle(transform.position,  distance, targetMask);
        }
    }

#if UNITY_EDITOR


    private void OnDrawGizmos()
    {
        if (isTouching)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.yellow;
            if(layerCheckerType == LayerCheckerType.Circle)
            {
                Gizmos.DrawWireSphere(transform.position, distance);
            }
            if(layerCheckerType == LayerCheckerType.Ray)
            {
                Gizmos.DrawRay(this.transform.position, direction * distance);
            }
        }
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public LayerMask detectLayer;
    public bool CanMoveDir(Vector3 moveDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 1f, detectLayer);
        Debug.Log("hit");
        if (!hit)
        {
            transform.Translate(moveDir);
            return true;
        }
        else
        {
            if (hit.collider.GetComponent<Collectable>() != null)
            {
                return hit.collider.GetComponent<Collectable>().CanMoveDir(moveDir);
            }
            //这时候就应该被吃掉
            else return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private Animator animator;
    public bool on;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.CollectableNum <=0)
        {
            on = true;
            SwitchAnim();
        }
        if (GameManager.instance.gamePass)
        {
            animator.SetBool("GameVic", false);
        }
    }

    public void SwitchAnim()
    {
        animator.SetBool("GameVic", true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        //animator.SetBool("IsWalking", player.IsWalking());
    }
}

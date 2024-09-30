using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "isWalking";
    
    private Animator animator;
    private Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}

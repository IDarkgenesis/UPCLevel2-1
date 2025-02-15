using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] private Player player;
    const string PLAYER_WALK = "IsMoving";
    const string PLAYER_ATTACK = "IsAttacking";
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();    
    }

    private void Update()
    {
        anim.SetBool(PLAYER_WALK, player.IsWalking());
        anim.SetBool(PLAYER_ATTACK, player.IsAttacking());
    }
}

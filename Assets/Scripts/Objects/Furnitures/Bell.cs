using System;
using UnityEngine;

public class Bell : BaseFurniture
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    protected override void InteractFixedForniture(PlayerController player)
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    protected override void InteractBrokenForniture(PlayerController player)
    {
        RepairForniture();
    }

    public override void Release(PlayerController player)
    {
        
    }

    public override void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController)
    {
        if (!_player.HasInteractableObject())
        {
            _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
        }
    }
    
}
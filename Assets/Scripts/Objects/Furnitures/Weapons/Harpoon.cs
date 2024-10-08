using System;
using UnityEngine;

public class Harpoon : BaseWeapon
{
    [SerializeField] private float cooldownDuration = 20f;
    private float cooldownTimer = 0f; 
    private bool isInCooldown = false;

    private void Start()
    {
        SetHasBullet(true);
    }

    private void Update()
    {
        if (isInCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isInCooldown = false;
                cooldownTimer = 0f;
                SetHasBullet(true);
            }
        }
    }

    protected override void InteractFixedForniture(PlayerController player)
    {
        if (!player.GetIsPilot() && !GetHasPilot())
        {
            EnterPilot(player.transform);
            SetOriginalParent(this.transform.parent);
            
            this.transform.SetParent(player.GetInteractableObjectFollowTransform());
            player.SetIsPilot(true);

            GetSelectedFurnitureVisual().SetCanSeeVisuals(false);
            GetSelectedFurnitureVisual().Hide();

            ShowNeededInputHint(player, player.hintController);
        }
        else if (player.GetIsPilot())
        {
            ExitPilot();
            this.transform.SetParent(GetOriginalParent());
            player.SetIsPilot(false);
            
            GetSelectedFurnitureVisual().SetCanSeeVisuals(true);
        }
    }

    protected override void InteractBrokenForniture(PlayerController player)
    {
        RepairForniture();
    }

    public override void Release(PlayerController player)
    {
        
    }
    public override void Activate(PlayerController player)
    {
        if (GetHasBullet() && !isInCooldown)
        {
            GameObject projectile = Instantiate(GetProjectilePrefab(), GetBulletSpawner().position + GetBulletSpawner().forward, Quaternion.identity);
            projectile.transform.forward = transform.forward;
            Bullet bulletScript = projectile.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.SetDirection(GetBulletSpawner().forward.normalized);
            }

            isInCooldown = true;
            cooldownTimer = cooldownDuration;
            SetHasBullet(false);
            
            ShowNeededInputHint(player, player.hintController);
        }
    }

    public override void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController)
    {
        if (!_player.GetIsPilot())
        {
            //Mostrar que puede pilotar el ca�on
            _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
        }
        else if (GetHasBullet())
        {
            //Si no esta pilotando y tiene una bala en el ca�on mostrar que puede usarlo
            _hintController.UpdateActionType(PlayerHintController.ActionType.USE);
        }
        else
        {
            //Si no tiene bala y esta pilotando mostrar que puede desmontar
            _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannon : BaseWeapon
{
    protected override void InteractFixedForniture(PlayerController player)
    {
        if (isReloading) return;

        if (player.HasInteractableObject())
        {
            if (player.GetInteractableObject().GetInteractableObjectScriptable() == GetAcceptedObject() && !GetHasBullet())
            {
                player.SetCanMove(false);
                ProgressBarManager.instance.AddFurniture(this);
                ProgressBarManager.instance.AddPlayer(player, this);
                player.hintController.isInteracting = true;
                
                isReloading = true;
                currentRepairTime = 0f;

                ShowNeededInputHint(player, player.GetPlayerHintController());
            }
        }
        else
        {
            if (!player.GetIsPilot() && !GetHasPilot())
            {
                EnterPilot(player.transform);
                SetOriginalParent(transform.parent);
                
                transform.SetParent(player.GetInteractableObjectFollowTransform());
                player.SetIsPilot(true, this);

                GetSelectedFurnitureVisual().SetCanSeeVisuals(false);
                GetSelectedFurnitureVisual().Hide();

                ShowNeededInputHint(player, player.hintController);
            }
            else if (player.GetIsPilot())
            {
                ExitPilot();
                player.SetIsPilot(false, null);
            }
        }
    }
    public override void FinishRepair(PlayerController player)
    {
        // Recargar con el tiempo de reparacion 
        if (isReloading)
        {
            SetHasBullet(true);
            isReloading = false;
            
            if (player.GetInteractableObject() != null)
                Destroy(player.GetInteractableObject().gameObject);
    
            player.ClearInteractableObject();
    
            Release(player);
        }
    }
    protected override void InteractBrokenForniture(PlayerController player)
    {
        if (!player.GetInteractableObject())
        {
            player.SetCanMove(false);
            ProgressBarManager.instance.AddPlayer(player, this);
            player.hintController.isInteracting = true;
        }
    }

    public override void Release(PlayerController player)
    {
        if (isReloading)
        {
            ProgressBarManager.instance.RemoveFurniture(this);
            isReloading = false;
        }

        ProgressBarManager.instance.RemovePlayer(player, this);
        player.hintController.isInteracting = false;
        player.SetCanMove(true);
        
        ShowNeededInputHint(player, player.GetPlayerHintController());
    }
    public override void Activate(PlayerController player)
    {
        if (GetHasBullet())
        {
            GameObject projectile = Instantiate(GetProjectilePrefab(), GetBulletSpawner().position + GetBulletSpawner().forward, Quaternion.identity);
            Bullet bulletScript = projectile.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.SetDirection(GetBulletSpawner().forward.normalized);
            }

            SetHasBullet(false);
            SetInteractableObject(null);
            ShowNeededInputHint(player, player.hintController);
            Instantiate(shootParticles, GetBulletSpawner().position, Quaternion.identity);
        }
    }

    public override void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController)
    {
        //Comprobamos si la forniture esta rota y no tenemos ningun item en la mano el boton para interactuar
        if (isFornitureBroke && !_player.HasInteractableObject())
        {
            if (_hintController.isInteracting)
            {
                _hintController.SetProgressBar(repairDuration, currentRepairTime);
                _hintController.UpdateActionType(PlayerHintController.ActionType.HOLDING);
            }
            else
            {
                _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
            }
        }
        else if (_player.HasInteractableObject())
        {
            if (_hintController.isInteracting)
            {
                _hintController.SetProgressBar(repairDuration, currentRepairTime);
                _hintController.UpdateActionType(PlayerHintController.ActionType.HOLDING);
            }
            else if (_player.GetInteractableObject().GetInteractableObjectScriptable() == GetAcceptedObject() && !GetHasBullet())
            {
                _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
            }
        }
        else
        {
            if (!_player.GetIsPilot() && !isFornitureBroke)
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
}

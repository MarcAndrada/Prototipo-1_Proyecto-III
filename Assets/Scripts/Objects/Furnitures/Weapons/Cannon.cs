using UnityEngine;

public class Cannon : BaseWeapon
{
    protected override void InteractFixedForniture(PlayerController player)
    {
        if (isReloading) return;

        if (player.HasInteractableObject() 
            && player.GetInteractableObject().GetInteractableObjectScriptable() == GetAcceptedObject() 
            && !GetHasBullet()
            ) //Comprobamos si el player tiene un objeto y este es el que necesitamos cargar el cañon
        {
            
            player.SetCanMove(false);
            ProgressBarManager.instance.AddFurniture(this);
            ProgressBarManager.instance.AddPlayer(player, this);
            player.hintController.isInteracting = true;
            
            isReloading = true;
            currentRepairTime = 0f;

            ShowNeededInputHint(player, player.GetPlayerHintController());
            AudioManager.instance.Play2dOneShotSound(reloadCannonClip, "Master", 0.4f, 0.8f, 1.2f);
        }
        else 
        {
            if (!player.GetIsPilot() && !GetHasPilot()) //Comprobamos si el player se puede montar en el cañon
            {
                EnterPilot(player.transform);
                
                SetOriginalParent(transform.parent);

                transform.SetParent(player.GetInteractableObjectFollowTransform());
                player.SetIsPilot(true, this);

                GetSelectedFurnitureVisual().SetCanSeeVisuals(false);
                GetSelectedFurnitureVisual().Hide();

                ShowNeededInputHint(player, player.hintController);
            }
            else if (player.GetIsPilot()) //Comprobamos si el player se puede desmontar en el cañon
            {
                ExitPilot();
                player.SetIsPilot(false, null);
            }
        }
    }

    public override void FinishRepair(PlayerController player)
    {
        if (isReloading)
        {
            SetHasBullet(true);
            projectileParticles.Play(true);
            isReloading = false;
            
            if (player.GetInteractableObject() != null)
                Destroy(player.GetInteractableObject().gameObject);
    
            player.ClearInteractableObject();
    
            Release(player);
        }
        else
        {
            base.FinishRepair(player);
        }
    }
    protected override void InteractBrokenForniture(PlayerController player)
    {
        if (!player.GetInteractableObject())
        {
            player.SetCanMove(false);
            ProgressBarManager.instance.AddPlayer(player, this);
            player.hintController.isInteracting = true;
            
            if (!repairAudioSource)
                repairAudioSource = AudioManager.instance.Play2dLoop(repairClip, "Master", 0.7f, 0.95f, 1.05f);
        }
    }

    public override void Release(PlayerController player)
    {
        if (isReloading)
        {
            ProgressBarManager.instance.RemoveFurniture(this);
            isReloading = false;
            player.animator.SetBool("Interacting", false);
        }

        ProgressBarManager.instance.RemovePlayer(player, this);
        player.hintController.isInteracting = false;
        player.SetCanMove(true);
        
        ShowNeededInputHint(player, player.GetPlayerHintController());
        
        AudioManager.instance.StopLoopSound(repairAudioSource);
        repairAudioSource = null;
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
            
            projectileParticles.Stop(true);
            SetHasBullet(false);
            SetInteractableObject(null);
            ShowNeededInputHint(player, player.hintController);

            Instantiate(shootParticles, GetBulletSpawner().position, Quaternion.identity);

            player.animator.SetTrigger("Shoot");
            AudioManager.instance.Play2dOneShotSound(cannonShootClip, "Master", 0.6f);
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

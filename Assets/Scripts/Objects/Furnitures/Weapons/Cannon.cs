using UnityEngine;

public class Cannon : BaseWeapon
{
    
    public override void Interact(PlayerController player)
    {
        if (player.HasInteractableObject())
        {
            if (player.GetInteractableObject().GetInteractableObjectScriptable() == GetAcceptedObject() && !GetHasBullet())
            {
                // Animacion de la bala
                Destroy(player.GetInteractableObject().gameObject);
                player.ClearInteractableObject();

                SetHasBullet(true);
            }
        }
        else
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
                Debug.Log("Player Entered The Cannon");
            }
            else if (player.GetIsPilot())
            {
                ExitPilot();
                this.transform.SetParent(GetOriginalParent());
                player.SetIsPilot(false);
                
                GetSelectedFurnitureVisual().SetCanSeeVisuals(true);
                
                Debug.Log("Player Exited The Cannon");
            }
        }
    }
    public override void Release(PlayerController player)
    {
        
    }
    public override void Activate(PlayerController player)
    {
        if (GetHasBullet())
        {
            GameObject projectile = Instantiate(GetProjectilePrefab(), GetBulletSpawner().position + GetBulletSpawner().forward, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().isKinematic = false;
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                projectileRb.AddForce(GetBulletSpawner().forward * GetShootForce(), ForceMode.Impulse);
            }
            SetHasBullet(false);
            SetInteractableObject(null);
            ShowNeededInputHint(player, player.hintController);
        }
    }

    public override void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController)
    {
        if (_player.HasInteractableObject())
        {
            if (_player.GetInteractableObject().GetInteractableObjectScriptable() == GetAcceptedObject() && !GetHasBullet())
            {
                // Poner la bala dentro del ca�on
                _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);                
            }
        }
        else
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
}

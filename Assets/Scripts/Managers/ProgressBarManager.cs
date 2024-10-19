using System.Collections.Generic;
using UnityEngine;

public class ProgressBarManager : MonoBehaviour
{
    public static ProgressBarManager instance;


    struct ProgressBar
    {
        public BaseFurniture furniture;
        public List<PlayerController> players;
    }

    [SerializeField] private float repairSpeed;
    private List<ProgressBar> progressBars;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }
        
        instance = this;
        progressBars = new List<ProgressBar>();
    }

    private void Update()
    {
        if (progressBars.Count > 0)
        {
            for (int i = 0; i < progressBars.Count; i++)
            {
                ProgressBar item = progressBars[i];
                item.furniture.currentRepairTime += repairSpeed * item.players.Count * Time.deltaTime;

                for (int j = 0; j < item.players.Count; j++)
                {
                    PlayerController player = item.players[j];
                    player.GetPlayerHintController().SetProgressBar( item.furniture.repairDuration, item.furniture.currentRepairTime);
                    item.furniture.ShowNeededInputHint(player, player.GetPlayerHintController());

                    if (item.furniture.currentRepairTime >= item.furniture.repairDuration)
                    {
                        item.furniture.FinishRepair(player);
                    }
                }
                if (item.furniture.currentRepairTime >= item.furniture.repairDuration)
                {
                    item.furniture.RepairForniture();
                    item.furniture.currentRepairTime = 0f;
                    return;
                }
            }
        }
    }

    public void AddFurniture(BaseFurniture furniture)
    {
        ProgressBar progressBar = new ProgressBar();
        progressBar.furniture = furniture;
        progressBar.players = new List<PlayerController>();
        progressBars.Add(progressBar);
    }
    public void RemoveFurniture(BaseFurniture furniture)
    {
        for (int i = 0; i < progressBars.Count; i++)
        {
            if (progressBars[i].furniture == furniture)
            {
                progressBars.RemoveAt(i);
            }
        }

        furniture.ToggleRepairParticles(false);
    }
    public void AddPlayer(PlayerController player, BaseFurniture furniture)
    {
        for (int i = 0; i < progressBars.Count; i++)
        {
            if (progressBars[i].furniture == furniture)
            {
                player.animator.SetBool("Interacting", true);
                progressBars[i].players.Add(player);
                break;
            }
        }

        furniture.ToggleRepairParticles(true);
    }
    public void RemovePlayer(PlayerController player, BaseFurniture furniture)
    {
        for (int i = 0; i < progressBars.Count; i++)
        {
            if (progressBars[i].furniture == furniture)
            {
                player.animator.SetBool("Interacting", false);
                progressBars[i].players.Remove(player);

                if (progressBars[i].players.Count <= 0)
                    furniture.ToggleRepairParticles(false);

                break;
            }
        }

    }
}

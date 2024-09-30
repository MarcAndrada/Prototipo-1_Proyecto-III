using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab; 
    public int numberOfPlayers = 4;

    private List<Player> players = new List<Player>();
}

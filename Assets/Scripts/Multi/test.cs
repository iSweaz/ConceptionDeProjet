using System.Linq;
using PurrNet;
using UnityEngine;

public class test : NetworkIdentity
{
    [SerializeField] private NetworkIdentity playerPefab, managerPrefab;
    [SerializeField] private Vector3 pos;

    [SerializeField] private NI_Manager manager;
    [SerializeField] private NI_Timer timer;
    [SerializeField] private NI_Selection playerSelection;
  


    protected override void OnSpawned()
    {
        base.OnSpawned();


        NetworkIdentity newPlayer = Instantiate(playerPefab, pos, Quaternion.identity);
        playerSelection = newPlayer.gameObject.GetComponentInChildren<NI_Selection>();
        
        if(isServer)
        {
            Instantiate(managerPrefab);

            managerPrefab.Spawn(managerPrefab.gameObject);

            manager = managerPrefab.GetComponent<NI_Manager>();
            timer = managerPrefab.GetComponent<NI_Timer>();

            playerSelection.name = "Server";
            manager.players.Add(playerSelection);
            manager.Answers.Add(playerSelection.playerAnswer);
        }
        else
        {
            manager = FindFirstObjectByType<NI_Manager>();
            timer = FindFirstObjectByType<NI_Timer>();
        }

        timer.createCanvas();  

        if(isClient)
        {
            RegisterClient(playerSelection,playerSelection.playerAnswer);
            playerSelection.name = "Client";
        }
    }


    /// <summary>
    /// Enregistre les clients auprès des List du serveur lors de leur création.
    /// </summary>
    /// <param name="Player">Référence du player</param>
    /// <param name="playerAnswer">Réponse du joueur à l'initialisation, par défaut "i"</param>
    [ServerRpc]
    void RegisterClient(NI_Selection Player, string playerAnswer)
    {
        if(!manager)
            return;
        manager.players.Add(Player);
        manager.Answers.Add(playerAnswer);
        Debug.Log($"Serveur : joueur {Player.name} ajouté avec réponse {playerAnswer}");
    }


}
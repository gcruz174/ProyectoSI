using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class TankManager : MonoBehaviour
{
    [SerializeField]
    private FishPreyAgent[] m_PreyAgents;
    [SerializeField]
    private FishPredatorAgent[] m_PredatorAgents;
    
    [SerializeField]
    private TMP_Text m_PreyCountText;

    private void Awake()
    {
        m_PreyAgents = GetComponentsInChildren<FishPreyAgent>();
        m_PredatorAgents = GetComponentsInChildren<FishPredatorAgent>();
    }

    private void Update()
    {
        bool allPreyDead = true, allPredatorsDead = true;
        
        if (m_PreyAgents.Any(preyAgent => preyAgent.gameObject.activeSelf))
        {
            allPreyDead = false;
        }
        if (m_PredatorAgents.Any(predatorAgent => predatorAgent.gameObject.activeSelf))
        {
            allPredatorsDead = false;
        }
        
        if (allPreyDead || allPredatorsDead)
        {
            ResetScene();
        }
        
        // Update text
        var finalText = "";
        finalText += $"Prey alive: {m_PreyAgents.Count(preyAgent => preyAgent.gameObject.activeSelf)}";
        finalText += $"\nPredators alive: {m_PredatorAgents.Count(predatorAgent => predatorAgent.gameObject.activeSelf)}";
        finalText += $"\nPress R to restart camera rotation.";
        m_PreyCountText.text = finalText;
    }
    
    private void ResetScene()
    {
        foreach (var preyAgent in m_PreyAgents)
        {
            preyAgent.gameObject.SetActive(true);
            preyAgent.EndEpisode();
        }
        foreach (var predatorAgent in m_PredatorAgents)
        {
            predatorAgent.gameObject.SetActive(true);
            predatorAgent.EndEpisode();
        }
    }
}

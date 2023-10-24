using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSM : AbstractStateMachine<IState>
{

    [SerializeField]
    protected Camera m_gameplayCamera;

    [SerializeField]
    protected Camera m_cinematicCamera;

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<IState>();
        m_possibleStates.Add(new GamePlayState(m_gameplayCamera));
        m_possibleStates.Add(new CinematicState(m_cinematicCamera));
    }

}

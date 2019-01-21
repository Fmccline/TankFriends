using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMode
{
    bool IsEndOfRound(TankManager[] tanks);
    TankManager GetRoundWinner(TankManager[] tanks);
    void StartRound(TankManager[] tanks);
}

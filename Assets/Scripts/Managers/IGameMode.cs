﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMode
{
    void StartRound(TankManager[] tanks);
    List<float> GetRoundScores(TankManager [] tanks);
    bool IsEndOfRound(TankManager[] tanks);
    TankManager GetRoundWinner(TankManager[] tanks);
}

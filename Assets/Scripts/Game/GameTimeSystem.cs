using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public partial struct GameTimeSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        foreach(var gameTime in SystemAPI.Query<RefRW<GameTime>>())
        {
            gameTime.ValueRW.time += SystemAPI.Time.DeltaTime;
            Panel_GameTime.Instance.SetTime(gameTime.ValueRO.time);
        }
    }
}

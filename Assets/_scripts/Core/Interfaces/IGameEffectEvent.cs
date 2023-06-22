using Core;
using System;
using UnityEngine;

namespace Interfaces
{
    public interface IGameEffectEvent: IGlobalSubscriber
    {
        void AddEffect(string effectID, Vector3 startPos, Vector3 endPos, out GameObject result, Action callback = null);

        event Action<float> OnUpdateEvent;
    }
}

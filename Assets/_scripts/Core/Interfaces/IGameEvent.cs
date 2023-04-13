using Core;
using System;
using UnityEngine;

namespace Interfaces
{
    public interface IGameEvent: IGlobalSubscriber
    {
        void AddEffect(string effectID, Vector3 startPos, Vector3 endPos, Action callback = null);

        event Action<float> OnUpdateEvent;
    }
}

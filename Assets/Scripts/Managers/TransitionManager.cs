using UnityEngine;

namespace Managers
{
    public class TransitionManager : Singleton<TransitionManager>
    {
        [field:SerializeField] public Transitions TransitionScript { get; private set; }
        
        protected override void InternalAwake()
        {
            
        }
    }
}
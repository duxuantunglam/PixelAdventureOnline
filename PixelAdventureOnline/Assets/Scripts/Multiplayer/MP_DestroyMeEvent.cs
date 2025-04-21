using Fusion;
using UnityEngine;

public class MP_DestroyMeEvent : NetworkBehaviour
{
    [SerializeField] private float destroyDelay = 1f;

    private void Start()
    {
        if (Object.HasStateAuthority)
        {
            Invoke(nameof(DestroyMe), destroyDelay);
        }
    }

    public void DestroyMe()
    {
        if (Object != null && Object.HasStateAuthority)
        {
            Runner.Despawn(Object);
        }
    }
}
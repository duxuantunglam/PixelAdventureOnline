using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector2 direction;
    public NetworkBool jump;
    public NetworkBool doubleJump;
    public NetworkBool wallJump;
}
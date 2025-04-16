// using Fusion;
// using UnityEngine;

// public class PlayerController : NetworkBehaviour
// {
//     [Networked] public Vector2 Direction { get; set; }
//     [Networked] public bool IsMoving { get; set; }

//     public override void FixedUpdateNetwork()
//     {
//         if (GetInput(out NetworkInputData data))
//         {
//             Direction = data.direction;
//             IsMoving = data.isMoving;
//         }
//     }

//     private void Update()
//     {
//         if (Object.HasInputAuthority)
//         {
//             // Xử lý input local
//             // Đồng bộ với Networked properties
//         }
//     }
// }
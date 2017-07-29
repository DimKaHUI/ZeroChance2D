using UnityEngine;
using UnityEngine.Networking;

namespace ZeroChance2D.Assets.Scripts.Mechanics
{
    [NetworkSettings(channel = 1, sendInterval = 0.01f)]
    public class PlayerTransformSync : NetworkBehaviour
    {
        [SyncVar] public Vector3 SyncedPos;
        [SyncVar] public float SyncedZRotation;
        public float LerpRate = 15;

        void FixedUpdate()
        {
            if (!isLocalPlayer)
            {
                LerpTransform();
            }
            else
            {
                TransmitMyTransform();
            }
        }

        void LerpTransform()
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, SyncedPos,
                Time.deltaTime * LerpRate);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation,
                Quaternion.Euler(0, 0, SyncedZRotation), Time.deltaTime * LerpRate);
        }

        [Command]
        void CmdTellServerMyTransform(Vector3 position, float rotation)
        {
            SyncedPos = position;
            SyncedZRotation = rotation;
        }

        [Client]
        void TransmitMyTransform()
        {
            CmdTellServerMyTransform(gameObject.transform.position, gameObject.transform.rotation.eulerAngles.z);
        }
    }
}
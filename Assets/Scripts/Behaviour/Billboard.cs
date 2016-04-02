using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class Billboard : MonoBehaviour {
    
        void Update () {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}

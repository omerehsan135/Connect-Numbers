using UnityEngine;

namespace Ilumisoft.Hex
{
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField]
        float lifeTime = 1;

        private void Start()
        {
            Destroy(this.gameObject, lifeTime);
        }
    }
}
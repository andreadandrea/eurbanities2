using UnityEngine;

public class PlayerSpawnpoint_ : MonoBehaviour
{
    private GlobalStateManager_ GSM;
    private void OnEnable()
    {
        GSM = GlobalStateManager_.Instance;
        if (ReferenceEquals(GSM, null)) return;
        
        Vector3 pos = GSM.GetSpawnPointInThisScene(gameObject.scene.name);
        if (pos != Vector3.zero)
        {
            transform.position = pos;
        }
    }
}

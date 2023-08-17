using UnityEngine;
using System.Collections;

public class ParticleEffectAutoDestroy : MonoBehaviour
{
    [SerializeField] bool OnlyDeactivate;

	
	void OnEnable()
	{
		StartCoroutine(CheckIfAlive());
	}
	
    /// <summary>
    /// Periodically checks if particle system is still playing.
    /// </summary>
	private IEnumerator CheckIfAlive()
	{
        WaitForSeconds delay = new WaitForSeconds(0.5f);

        while (true)
		{
			if(!GetComponent<ParticleSystem>().IsAlive(true))
			{
                // Try returning the object to the object pool, if it's not in there destroy it
                if (ObjectPool.ReturnPooledObject(gameObject.name , gameObject))
                {
                    break;
                }
                else
                {
                    Destroy(gameObject);
                    break;
                }
			}

            yield return delay;
        }
	}
}

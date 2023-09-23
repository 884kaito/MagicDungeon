using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//storage magic circle data
[System.Serializable]
public class MagicCircleData : MonoBehaviour
{
    public string type;
    public float mp;

    [HideInInspector] public bool isMiddle = false;


    //get middle of duration and destroy object on final
    public IEnumerator DeathTimer()
    {
        float duration = GetComponent<ParticleSystem>().main.duration;
        //wait for some seconds
        yield return new WaitForSeconds(duration / 2);

        isMiddle = true;

        yield return new WaitForSeconds(duration / 2);

        Destroy(this.gameObject);
    }
}

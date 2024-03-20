using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.VFX;
//using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class Transitions : MonoBehaviour
{
    public float FadeTime = 1;
    public float BeamTime = 3;

    public GameObject BeamFx;
    public GameObject Fader;

    public Volume PostPro;
    private PaniniProjection paniniProj;

    // Valeurs fixes ça bouge pas
    float fadeMax = 0.55f;
    float fadeMin = -0.3f;

    float paniniDistance;
    float paniniCrop;

    private void Awake()
    {
        PostPro.profile.TryGet(out paniniProj);

        paniniDistance = (float)paniniProj.distance;
        paniniCrop = (float)paniniProj.cropToFit;
    }

    public void TransitionOut()
    {
        Fader.GetComponent<Material>().DOFloat(fadeMin, "_PowerLevel", FadeTime);

        
    }

    public void TransitionIn()
    {
        paniniProj.distance.Override(paniniDistance);
        Fader.GetComponent<Material>().DOFloat(fadeMax, "_PowerLevel", FadeTime);
    }

    public IEnumerator Defeat(GameObject reactor)
    {
        GameObject beam = Instantiate(BeamFx, reactor.transform);
        paniniProj.distance.Override(1);
        yield return new WaitForSeconds(BeamTime);

        TransitionOut();
        yield return new WaitForSeconds(FadeTime);
        
        Destroy(beam);
        TransitionIn();
    }

}

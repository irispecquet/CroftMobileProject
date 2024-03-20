using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class Transitions : MonoBehaviour
{
    public float FadeTime = 1;
    public float BeamTime = 3;

    public GameObject BeamFx;
    public GameObject Fader;

    public PaniniProjection PaniniProj;

    // Valeurs fixes �a bouge pas
    float fadeMax = 0.55f;
    float fadeMin = -0.3f;

    float paniniDistance;
    float paniniCrop;

    private void Awake()
    {
        paniniDistance = (float)PaniniProj.distance;
        paniniCrop = (float)PaniniProj.cropToFit;
    }

    public void TransitionOut()
    {
        Fader.GetComponent<Material>().DOFloat(fadeMin, "_PowerLevel", FadeTime);
    }

    public void TransitionIn()
    {
        PaniniProj.distance.Override(paniniDistance);
        Fader.GetComponent<Material>().DOFloat(fadeMax, "_PowerLevel", FadeTime);
    }

    public IEnumerator Defeat(GameObject reactor) // quand on perd
    {
        GameObject beam = Instantiate(BeamFx, reactor.transform);
        PaniniProj.distance.Override(1);
        yield return new WaitForSeconds(BeamTime);

        TransitionOut();
        yield return new WaitForSeconds(FadeTime);
        
        Destroy(beam);
        TransitionIn();
    }

}

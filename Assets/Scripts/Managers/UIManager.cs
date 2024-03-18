using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [Space(8)]
        [SerializeField] private float _punchDuration;
        [SerializeField] private Vector3 _punchForce;

        public float PunchDuration => _punchDuration;

        public Vector3 PunchForce => _punchForce;

        protected override void InternalAwake()
        {
        }

        public IEnumerator GoToScene(string nameScene)
        {
            // mets ta transi ici

            float seconds = 0; // mets le temps de ton anim
            yield return new WaitForSeconds(seconds);

            SceneManager.LoadScene(nameScene);
        }
    }
}
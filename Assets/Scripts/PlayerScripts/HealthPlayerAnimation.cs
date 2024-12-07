using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class HealthPlayerAnimation : MonoBehaviour
{
    PlayableDirector _playableDirector;
    private void Awake() => _playableDirector = GetComponent<PlayableDirector>();
    public void PlayHit() => _playableDirector.Play();

}

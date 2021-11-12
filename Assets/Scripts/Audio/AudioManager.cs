using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField]
	private Sound[] sounds;

	private void Awake () {
		foreach (Sound sound in sounds) {
			sound.Source = gameObject.AddComponent<AudioSource>();
			sound.Source.clip = sound.audioClip;
			sound.Source.mute = sound.mute;
			sound.Source.bypassEffects = sound.bypassEffects;
			sound.Source.bypassListenerEffects = sound.bypassListenerEffects;
			sound.Source.bypassReverbZones = sound.bypassReverbZones;
			sound.Source.playOnAwake = sound.playOnAwake;
			sound.Source.loop = sound.loop;

			sound.Source.priority = sound.priority;
			sound.Source.volume = sound.volume;
			sound.Source.pitch = sound.pitch;
			sound.Source.panStereo = sound.stereoPan;
			sound.Source.spatialBlend = sound.spatialBlend;
			sound.Source.reverbZoneMix = sound.reverbZoneMix;
		}
	}

	public void Play (string name) {
		Sound sound = Array.Find(sounds, Sound => Sound.name == name);
		if (sound != null)
			sound.Source.Play();
		else
			Debug.Log("Sound named " + name + " doesn't exist.");
	}
}
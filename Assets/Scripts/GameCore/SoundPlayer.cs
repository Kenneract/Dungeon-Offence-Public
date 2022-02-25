/*
* Author: Kennan
* Date: Apr.7.2021
* Summary: Plays a given sound until its done, then destroy self.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

	#region Variables
	public AudioSource selfSource;
	private AudioClip curClip;
	private bool toLoop = false;
	//private bool isSpatial = false; //Stores if we're a spatial sound. NOT CURRENT USED, JUST FOR FUTURE
	private bool startedPlaying = false; //If we've started playing
	private float lifeTime = 0.00f; //How long until this object destroys itself (how long until sound is done playing)
	#endregion

	#region Unity Methods
	void Update()
	{// Update is called once per frame
		if (startedPlaying && !toLoop)
		{//If we're playing & not supposed to loop, note the passage of time
			lifeTime -= Time.deltaTime;
			if (lifeTime <= 0.00f)
			{//If the clip is over, destroy this object
				Destroy(this.gameObject);
			}//if
		}//if
	}//Update()
	#endregion

	public void SetVolume(float volume)
	{//Sets the volume of this sound source
		selfSource.volume = volume;
	}//SetVolume(float)

	public void SetToSpatial()
	{//Sets this sound source to use 3D stereo spatial sound
		//isSpatial = true;
    	selfSource.spatialBlend = 1.0f;
		//selfSource.dopplerLevel = 0.0; //(set as constant on prefab; ignore)
	}//SetToSpatial()

	public void SetPitch(float pitch)
	{//Sets the pitch of this sound source
		selfSource.pitch = pitch;
	}//SetPitch(float)

	public void SetToLoop()
	{//Sets this sound player to run in a loop
		toLoop = true;
		selfSource.loop = true;
	}//SetToLoop

	public void PlayClip(AudioClip clip)
	{//Plays the given source
		selfSource.clip = clip;
		selfSource.Play();
		startedPlaying = true;
		lifeTime = clip.length;
	}//PlayClip(AudioClip)

}//SoundPlayer




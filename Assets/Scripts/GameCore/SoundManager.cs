/*
* Author: Kennan (Rewrite) (Original by Joshua)
* Date: Apr.9.2021
* Purpose: Manages all sounds; Can play a given sound as a 3D local or 2D global sound, or can play a sound from a given object within the level (3D).
*/

//Using callbacks and global variables, we could also use this to change the volume of all spatial / global sounds during runtime (and independently).

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    public AudioClip levelMusic;
    public GameObject soundPlayerPrefab;
    [Space]
    [Range(0.0f, 1.0f)]
    public float globalVolume = 0.75f;
    [Range(0.0f, 1.0f)]
    public float spatialVolume = 1.00f;

    #region Singleton
    public static SoundManager instance;
    void SetSingleton()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of SoundManager found.");
            //Destroy(self);
        } else {
            instance = this;
            //DontDestroyOnLoad(this); // Make instance persistent.
        }//if
    }//SetSingleton()
    #endregion

    void Awake()
    {
        SetSingleton();
    }//Awake()

    void Start()
    {// Start is called before the first frame update
        PlayGlobal(levelMusic, true); //Start the level music (set to loop)
    }//Start()


    // [ONLY IMPLEMENT THIS IF ITS ACTUALLY NEEDED SOMEHOW]
    //void StopSound(AudioClip clip) //Runs through the list of sounds and stops the one provided
    //void StopAll() //Use a callback or something to stop all sounds
    //void PlayGlobalPersistent(AudioClip clip) //Stays loaded across levels; would need callback to stop it


    public void PlaySpatial(AudioClip clip, Vector3 pos, float minPitch, float maxPitch, bool loop)
    {//Plays a given sound effect at the given position (3D stereo effects).
        //Divert if no sound clip given
        if (clip == null) return;

        //Instantiate SoundPlayer at correct position
        GameObject soundPlayerObj = Instantiate(soundPlayerPrefab, pos, Quaternion.identity);
        SoundPlayer soundPlayer = soundPlayerObj.GetComponent<SoundPlayer>();
        //spatialPlayerList.Add(soundPlayer);

        //Set parameters (2D/3D, pitch, etc)
        soundPlayer.SetPitch(Random.Range(minPitch, maxPitch));
        soundPlayer.SetToSpatial();
        soundPlayer.SetVolume(spatialVolume);
        if (loop) soundPlayer.SetToLoop();

        //Load Sound(the clip)
        soundPlayer.PlayClip(clip);
    }//PlaySpatial(AudioClip, Vector3, float, float, bool)
    
    public void PlaySpatial(AudioClip clip, Vector3 pos, float minPitch, float maxPitch)
    {//Plays a given sound effect at the given position (3D stereo effects). Assumption: no loop
        PlaySpatial(clip, pos, minPitch, maxPitch, false);
    }//PlaySpatial(AudioClip, Vector3, float, float)
    
    public void PlaySpatial(AudioClip clip, Vector3 pos, bool loop)
    {//Plays a given sound effect at the given position (3D stereo effects). Assumption: No pitch shift
        PlaySpatial(clip, pos, 1.0f, 1.0f, loop);
    }//PlaySpatial(AudioClip, Vector3, bool)
    
    public void PlaySpatial(AudioClip clip, Vector3 pos)
    {//Plays a given sound effect at the given position (3D stereo effects). Assumption: No pitch shift, no loop
        PlaySpatial(clip, pos, 1.0f, 1.0f, false);
    }//PlaySpatial(AudioClip, Vector3)



    public void PlaySpatialOnObject(AudioClip clip, GameObject target, float minPitch, float maxPitch, bool loop)
    {//Plays a given sound effect (with 3D stereo effects) coming from a given target (constantly moves with target)
        //Divert if no sound clip given
        if (clip == null) return;

        //Instantiate SoundPlayer at correct position
        GameObject soundPlayerObj = Instantiate(soundPlayerPrefab, target.transform.position, Quaternion.identity, target.transform); //As child to object
        SoundPlayer soundPlayer = soundPlayerObj.GetComponent<SoundPlayer>();
        //spatialPlayerList.Add(soundPlayer);

        //Set parameters (2D/3D, pitch, etc)
        soundPlayer.SetPitch(Random.Range(minPitch, maxPitch));
        soundPlayer.SetToSpatial();
        soundPlayer.SetVolume(spatialVolume);
        if (loop) soundPlayer.SetToLoop();

        //Load Sound(the clip)
        soundPlayer.PlayClip(clip);
    }//PlaySpatialOnObject(AudioClip, GameObject, float, float, bool)

    public void PlaySpatialOnObject(AudioClip clip, GameObject target, float minPitch, float maxPitch)
    {//Plays a given sound effect (with 3D stereo effects) coming from a given target (constantly moves with target) Assumption: no loop
        PlaySpatialOnObject(clip, target, minPitch, maxPitch, false);
    }//PlaySpatialOnObject(AudioClip, GameObject, float, float)
    
    public void PlaySpatialOnObject(AudioClip clip, GameObject target, bool loop)
    {//Plays a given sound effect (with 3D stereo effects) coming from a given target (constantly moves with target) Assumption: No pitch shift
        PlaySpatialOnObject(clip, target, 1.0f, 1.0f, loop);
    }//PlaySpatialOnObject(AudioClip, GameObject, bool)
    
    public void PlaySpatialOnObject(AudioClip clip, GameObject target)
    {//Plays a given sound effect (with 3D stereo effects) coming from a given target (constantly moves with target) Assumption: No pitch shift, no loop
        PlaySpatialOnObject(clip, target, 1.0f, 1.0f, false);
    }//PlaySpatialOnObject(AudioClip, GameObject)



    public void PlayGlobal(AudioClip clip, float minPitch, float maxPitch, bool loop)
    {//Plays an audioclip globally (Constant global volume).
        //Divert if no sound clip given
        if (clip == null) return;
        
        //Instantiate SoundPlayer at correct position
        GameObject soundPlayerObj = Instantiate(soundPlayerPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, this.transform);
        SoundPlayer soundPlayer = soundPlayerObj.GetComponent<SoundPlayer>();
        //globalPlayerList.Add(soundPlayer);

        //Set parameters (2D/3D, pitch, etc)
        soundPlayer.SetPitch(Random.Range(minPitch, maxPitch));
        soundPlayer.SetVolume(globalVolume);
        if (loop) soundPlayer.SetToLoop();

        //Load Sound(the clip)
        soundPlayer.PlayClip(clip);
    }//PlayGlobal(AudioClip, float, float, bool)

    public void PlayGlobal(AudioClip clip, float minPitch, float maxPitch)
    {//Plays an audioclip globally (Constant global volume). Assumption: no loop
        PlayGlobal(clip, minPitch, maxPitch, false);
    }//PlayGlobal(AudioClip, float, float)
    
    public void PlayGlobal(AudioClip clip, bool loop)
    {//Plays an audioclip globally (Constant global volume). Assumption: No pitch shift
        PlayGlobal(clip, 1.0f, 1.0f, loop);
    }//PlayGlobal(AudioClip, bool)

    public void PlayGlobal(AudioClip clip)
    {//Plays an audioclip globally (Constant global volume). Assumption: No pitch shift, no loop
        PlayGlobal(clip, 1.0f, 1.0f, false);
    }//PlayGlobal(AudioClip)

}//SoundManager
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorSFX : MonoBehaviour {

    private FMOD.Studio.EventInstance generatorSFX;


    void OnEnable ()
    {
        generatorSFX = FMODUnity.RuntimeManager.CreateInstance(FMODPaths.GENERATOR_SFX);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(generatorSFX, transform.parent, GetComponent<Rigidbody>());
        generatorSFX.start();
    }

    void OnDisable ()
    {
        generatorSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        generatorSFX.release();
    }
}

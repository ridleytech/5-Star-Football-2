using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : MonoBehaviour
{
    int blendShapeCount;
    int blendShapeCount2;
        int blendShapeCount3;


    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;
    float blendOne = 0f;
    float blendTwo = 0f;
    float blendSpeed = 1f;
    bool blendOneFinished = false;

    public Slider fatSlider;
    public Slider muscleSlider;
    public Slider heightSlider;
    public GameObject vc;

    public SkinnedMeshRenderer tShirtRenderer;
    public SkinnedMeshRenderer shortsRenderer;

Mesh shirtMesh;
Mesh shortsMesh;
	
	// Invoked when the value of the slider changes.
	public void FatValueChangeCheck()
	{
		//Debug.Log ("val: "+fatSlider.value);

        skinnedMeshRenderer.SetBlendShapeWeight (blendShapeCount-2,fatSlider.value);
        tShirtRenderer.SetBlendShapeWeight (blendShapeCount2-2,fatSlider.value);
        shortsRenderer.SetBlendShapeWeight (blendShapeCount3-2,fatSlider.value);
	}

    public void MuscleValueChangeCheck()
	{
		//Debug.Log ("val: "+muscleSlider.value);

        skinnedMeshRenderer.SetBlendShapeWeight (blendShapeCount-1,muscleSlider.value);
        tShirtRenderer.SetBlendShapeWeight (blendShapeCount2-1,muscleSlider.value);
        shortsRenderer.SetBlendShapeWeight (blendShapeCount3-1,muscleSlider.value);
    }

    public void HeightValueChangeCheck()
	{
		//Debug.Log ("val: "+heightSlider.value);

        //skinnedMeshRenderer.SetBlendShapeWeight (blendShapeCount-5,muscleSlider.value);

        transform.parent.transform.localScale = new Vector3(heightSlider.value,heightSlider.value, heightSlider.value);
        //vc.transform.localScale = new Vector3(vc.transform.position.x,heightSlider.value, vc.transform.position.z);

    }

    void Awake ()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer> ();
        skinnedMesh = GetComponent<SkinnedMeshRenderer> ().sharedMesh;
    }

    void Start ()
    {
        fatSlider = GameObject.Find("fatSlider").GetComponent<Slider>();
        muscleSlider = GameObject.Find("muscleSlider").GetComponent<Slider>();
        heightSlider = GameObject.Find("heightSlider").GetComponent<Slider>();

        fatSlider.onValueChanged.AddListener (delegate {FatValueChangeCheck ();});
        muscleSlider.onValueChanged.AddListener (delegate {MuscleValueChangeCheck ();});
        heightSlider.onValueChanged.AddListener (delegate {HeightValueChangeCheck ();});

        //vc = GameObject.Find("CM vcam1");


        blendShapeCount = skinnedMesh.blendShapeCount; 

        tShirtRenderer = GameObject.Find("T_shirt").GetComponent<SkinnedMeshRenderer> ();
        shortsRenderer = GameObject.Find("Classic_Shorts").GetComponent<SkinnedMeshRenderer> ();

        shirtMesh = GameObject.Find("T_shirt").GetComponent<SkinnedMeshRenderer> ().sharedMesh;
        shortsMesh = GameObject.Find("Classic_Shorts").GetComponent<SkinnedMeshRenderer> ().sharedMesh;


        blendShapeCount2 = shirtMesh.blendShapeCount; 
        blendShapeCount3 = shortsMesh.blendShapeCount; 

        //Debug.Log("blendShapeCount: " + blendShapeCount);
    }
}

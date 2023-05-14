using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class ExerciseManager : MonoBehaviour
{
    Animator anim;
    GameObject player;
        public TextMeshProUGUI repsTxt;
public int lRepCount = 0;
public int lRepTotal = 8;

public int repCount = 0;
public int repTotal = 8;


float strength = 10;
    public Slider strengthMeter;
public GameObject buttonPrefab;

public int currentExercise = 1;
public CinemachineTargetGroup targetGroup;
    GameObject ui;
    GameObject exerciseUI;

    public GameObject dbR;
    public GameObject dbL;
    public GameObject straightBar;

        public GameObject vc;
public GameObject currentExerciseProp;
public GameObject Spotter;
public GameObject repBtn;
public float currentFrame = 0f;
float timer = 0.0f;

public GameObject lHand;

public GameObject rHand;


class Lift 
{
 public string name;
  public int liftid;
  public string reps;
}

   SkinnedMeshRenderer skinnedMeshRenderer;
     Mesh   shirtMesh;


      int  blendShapeCount; 


//string[] lifts = {"Bench Press", "Dumbbell Curl", "Military Press", "Squat"};

List<Lift> lifts = new List<Lift>();


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim = player.GetComponent<Animator>();
        repsTxt = GameObject.Find("repsTxt").GetComponent<TextMeshProUGUI>();
        strengthMeter = GameObject.Find("strengthMeter").GetComponent<Slider>();
        repsTxt.text = "REPS: "+ lRepCount.ToString() + "/"+lRepTotal.ToString();
targetGroup = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();
ui = GameObject.Find("UI");
exerciseUI = GameObject.Find("ExerciseUI");
exerciseUI.SetActive(false);

dbR = GameObject.Find("RubberDumbellL");
dbL = GameObject.Find("RubberDumbellR");
straightBar = GameObject.Find("StraightBarPlayer");
vc = GameObject.Find("CM vcam2");
Spotter = GameObject.Find("Spotter");

repBtn = GameObject.Find("repBtn");
            repBtn.GetComponent<Button>().onClick.AddListener(delegate {doRep(); });

            lHand = GameObject.Find("CC_Base_L_Hand_Player");

rHand = GameObject.Find("CC_Base_R_Hand_Player");


//Spotter.SetActive(false);
dbR.SetActive(false);
dbL.SetActive(false);
straightBar.SetActive(false);
repBtn.SetActive(false);

    skinnedMeshRenderer = GameObject.Find("CC_Base_Body").GetComponent<SkinnedMeshRenderer> ();
        shirtMesh = GameObject.Find("CC_Base_Body").GetComponent<SkinnedMeshRenderer> ().sharedMesh;


        blendShapeCount = shirtMesh.blendShapeCount; 



Lift bp = new Lift();
bp.name = "Bench Press";
bp.liftid = 2;
bp.reps = "100 x 8 100 x 8 100 x 8";
lifts.Add(bp);

bp = new Lift();
bp.name = "Dumbbell Curl";
bp.liftid = 1;
bp.reps = "25 x 8 25 x 8 25 x 8";

lifts.Add(bp);


bp = new Lift();
bp.name = "Squat";
bp.liftid = 3;
bp.reps = "150 x 8 150 x 8 150 x 8";

lifts.Add(bp);

        generateProgram();
    }

    void generateProgram () {
        GameObject content = GameObject.Find("Content");

        int buttonYPos = -25;


        for (int i = 0; i < lifts.Count; i++) 
        {
            // var button = Object.Instantiate(Button, Vector3.zero, Quaternion.identity) as Button;
            // var rectTransform = button.GetComponent<RectTransform>();
            // rectTransform.SetParent(content.transform);
            // rectTransform.anchorMax = cornerTopRight;
            // rectTransform.anchorMin = cornerBottomLeft;
            // // rectTransform.offsetMax = Vector2.zero;
            // // rectTransform.offsetMin = Vector2.zero;

            // content.AddChild(button);

           Vector3 position = new Vector3(0,buttonYPos,0);
           //Vector2 size = new Vector3(380,50);


            GameObject button = Object.Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);

            //GameObject button = new GameObject();
            //button.transform.parent = content.transform;
            button.transform.SetParent(content.transform);
            // button.AddComponent<RectTransform>();
            // button.AddComponent<Button>();
            //button.transform.position = position;
            //button.GetComponent<RectTransform>().sizeDelta = size;

            string lift = lifts[i].name;

            button.name = lift;

            int liftid = lifts[i].liftid;
            string reps = lifts[i].reps;


            //button.GetComponent<RectTransform>().SetSize(size);
            //button.GetComponent<Button>().onClick.AddListener(chooseEx("hello"));
            button.GetComponent<Button>().onClick.AddListener(delegate {chooseEx(liftid); });

            TextMeshProUGUI exerciseTxt = button.transform.Find("exerciseTxt").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI repsTxt = button.transform.Find("repsTxt").GetComponent<TextMeshProUGUI>();

            exerciseTxt.text = lift;
            repsTxt.text = reps;

            //Debug.Log("Y: "+buttonYPos);

            buttonYPos-=50;
        }
    }

    void chooseEx (int id){
         

        Debug.Log("chooseEx:  "+ id);

        currentExercise = id;

        anim.SetInteger("exerciseid",currentExercise);
//manageUI(0);

ui.SetActive(false);
            exerciseUI.SetActive(true);
repBtn.SetActive(true);

if(id == 1)
{
dbR.SetActive(true);
dbL.SetActive(true);
straightBar.SetActive(false);
}
else if(id == 2){

        anim.PlayInFixedTime ("Bench Press.Bench Press 6 Up 2", 0, currentFrame);

     anim.speed=0.0f;



    dbR.SetActive(false);
dbL.SetActive(false);
straightBar.SetActive(true);

player.transform.position = new Vector3(-0.21f,0.155f,1.8f);
player.transform.rotation = Quaternion.Euler(0, -180, 0);
}


updateTargetGroup(id);
    }

    void manageUI(int status){

        if(status == 0){
ui.SetActive(false);
exerciseUI.SetActive(true);
        }
        else{
            ui.SetActive(true);
            exerciseUI.SetActive(false);
        }

           updateTargetGroup(status);


    }

    void doRep(){

//blow air animation
                skinnedMeshRenderer.SetBlendShapeWeight (40,100);

        anim.speed=0.5f;

        anim.PlayInFixedTime ("Bench Press.Bench Press 6 Up 2", 0, currentFrame);

        currentFrame+=0.5f;
        //Debug.Log("cf: "+currentFrame);
        timer = 0.0f;
    }

    public float xOffset = .0f;
        public float yOffset = .08f;
    public float zOffset = .03f;


   void Update()
    {


        Vector3 pos = lHand.transform.position + (rHand.transform.position - lHand.transform.position) / 2;


        straightBar.transform.position = new Vector3(pos.x,lHand.transform.position.y+yOffset,lHand.transform.position.z-zOffset);
        straightBar.transform.rotation = Quaternion.Euler(-90, 140, -140);



timer += Time.deltaTime;
    int seconds = (int)(timer % 60);

        //Debug.Log("t: "+seconds);

        //Debug.Log("t: "+timer);

        if(timer > 0.5f){
            Debug.Log("failingRep");
            anim.SetBool("failingRep",true);
            currentFrame = 0f;
        }
        else{
                            skinnedMeshRenderer.SetBlendShapeWeight (blendShapeCount-4,0);

                        anim.SetBool("failingRep",false);

        }


         if(Input.GetKeyDown(KeyCode.R)){
           // Debug.Log("start L");
            anim.SetBool("right up 0",true);
            anim.SetBool("right down 0",false);
        }

        if(Input.GetKeyDown(KeyCode.R)){
           // Debug.Log("start L");
            anim.SetBool("right up 0",false);
            anim.SetBool("right down 0",true);
        }


if(currentExercise == 1){
    if(Input.GetKeyDown(KeyCode.L)){
           // Debug.Log("start L");
            anim.SetBool("left up 0",true);
            anim.SetBool("left down 0",false);
        }

        if(Input.GetKeyUp(KeyCode.L)){
           // Debug.Log("end L");
            anim.SetBool("left down 0",true);
            anim.SetBool("left up 0",false);
        }

         if(Input.GetKeyDown(KeyCode.R)){
           // Debug.Log("start R");
            anim.SetBool("right up 0",true);
            anim.SetBool("right down 0",false);

        }

        if(Input.GetKeyUp(KeyCode.R)){
           // Debug.Log("end R");
            anim.SetBool("right down 0",true);
            anim.SetBool("right up 0",false);

        }
}
    else if(currentExercise == 2){
    if(Input.GetKeyDown(KeyCode.L)){
           // Debug.Log("start L");
            anim.SetBool("left up 0",false);
            anim.SetBool("left down 0",true);
        }

        if(Input.GetKeyUp(KeyCode.L)){
           // Debug.Log("end L");
            anim.SetBool("left down 0",false);
            anim.SetBool("left up 0",true);
        }}   
        
    
    
    if(Input.GetKeyUp(KeyCode.T)){
           updateTargetGroup(0);
        }
    
    
    }

    public void lBicepCurlFinished(string s)
    {
       // Debug.Log("PrintEvent: " + s + " called at: " + Time.time);
        lRepCount++;
        repsTxt.text = "REPS: "+ lRepCount.ToString() + "/"+lRepTotal.ToString();

        if(lRepCount % 4 == 0){
            strength++;
strengthMeter.value = strength;

        }
    }


    public void benchPressUp(string s)
    {
       // Debug.Log("PrintEvent: " + s + " called at: " + Time.time);
        repCount++;
        repsTxt.text = "REPS: "+ repCount.ToString() + "/"+repTotal.ToString();

       // if(repCount == repTotal){
                    if(repCount == repTotal){

            //manageUI(1);

          
                    currentExerciseProp.SetActive(true);

            ui.SetActive(true);
            exerciseUI.SetActive(false);

            repCount = 0;
                    repsTxt.text = "REPS: "+ repCount.ToString() + "/"+repTotal.ToString();

        }

//         if(repCount % 4 == 0){
//             strength++;
// strengthMeter.value = strength;

//         }
    }



    public void updateTargetGroup(int status){
        Debug.Log("updateTargetGroup");

//remove teleprompter from focus
        targetGroup.m_Targets[3].weight = 0;

        if(currentExercise == 2){

//              float desired_play_time = 2.3f;
//  anim["Bench Press Idle"].time = desired_play_time;
//  anim["Bench Press Idle"].speed = 0.0f;
//  anim.Play("Bench Press Idle");

        //anim.PlayInFixedTime ("Bench Press.Bench Press 6 Up 2", 0, currentFrame);


            //bench press, focus on head

                   targetGroup.m_Targets[2].weight = 3.0f;
                   vc.transform.position = new Vector3(-0.2f,2.7f,1f) ;

                    currentExerciseProp = GameObject.Find("StraightBarRemove");
                    currentExerciseProp.SetActive(false);
 Spotter.SetActive(true);

        }


//         Cinemachine.CinemachineTargetGroup.Target target = targetGroup.m_Targets[3];
// // target.target = "Target to add";
// // target.weight = "the weight";
// // target.radius = "the radius";
 
//         target.weight = 0;


//   for (int i = 0; i < targetGroup.m_Targets.Length; i++)
//         {
//             Debug.Log("target: "+targetGroup.m_Targets[i].target);

//             if(i==3){
//                 targetGroup.m_Targets[i].weight = 0;
//             }
//         }

    }

}

using UnityEngine;

public class Cannon2 : MonoBehaviour {
    [SerializeField] private Projection _projection;

    private void Update() {
        HandleControls();
        _projection.SimulateTrajectory(_ballPrefab, _ballSpawn.position, _ballSpawn.forward * _force);
    }

    #region Handle Controls

    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private float _force = 20;
    [SerializeField] private Transform _ballSpawn;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private ParticleSystem _launchParticles;


     public float rotateSpeed = 5.0f;

    private void HandleControls() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            var spawned = Instantiate(_ballPrefab, _ballSpawn.position, _ballSpawn.rotation);

            spawned.Init(_ballSpawn.forward * _force, false);
            //_launchParticles.Play();
            //_source.PlayOneShot(_clip);
        }

        if (Input.GetKey(KeyCode.U)) {
           increaseThrow();
        }

        if (Input.GetKey(KeyCode.I)) {
           decreaseThrow();
        }

        if (Input.GetKey(KeyCode.O)) {
           rotateThrowUp();
        }

        if (Input.GetKey(KeyCode.P)) {
           rotateThrowDown();
        }
    }

    public void rotateThrowUp(){

        _ballSpawn.transform.Rotate(-1 * rotateSpeed,0.0f,  0.0f);

    }

    public void rotateThrowDown(){

        _ballSpawn.transform.Rotate(1 * rotateSpeed,0.0f,  0.0f);

    }

    public void increaseThrow(){
        _force +=5;

        Debug.Log("force: " + _force);
    }

    public void decreaseThrow(){
        _force-=5;

        Debug.Log("force: " + _force);
    }

    #endregion
}
using UnityEngine;

public class BoomerangBehaviour : MonoBehaviour
{
    public bool hasReturned = true;
    public Rigidbody boomerangRigidbody;
     public Vector3 returnDirection;
    [SerializeField] float returnDelay;
    [SerializeField] AudioSource flyingAudioSource;
    [SerializeField] AudioSource hittingAudioSource;
    private bool _isReturning;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isReturning == false)
        {
            hittingAudioSource.Play();
            flyingAudioSource.Stop();

            Invoke(nameof(ReturnBoomerang), returnDelay);
            _isReturning = true;
        }
        else
        {
            flyingAudioSource.Stop();

            hasReturned = true;
            //Debug.Log("Boomerang has returned");
        }
    }
    void ReturnBoomerang()
    {
        flyingAudioSource.Play();
        boomerangRigidbody.AddForce(returnDirection, ForceMode.Impulse);
    }
}

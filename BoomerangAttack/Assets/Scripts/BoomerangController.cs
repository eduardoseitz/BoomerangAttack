using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float throwMinForce = 50f;
    [SerializeField] private float throwMaxForce = 250f;
    [SerializeField] private float maxChargeDuration = 3f;
    [SerializeField] GameObject boomerangPrefab;
    [SerializeField] BoomerangBehaviour boomerang;
    [SerializeField] private AudioSource boomerangSource;
    private float _chargeStartTime; 
    private float _chargeEndTime;
    private float _chargeDuration;
    private float _throwForce;
    private Vector3 _throwDirection;

    // Start is called before the first frame update
    //void Start(){}

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsGameRunning)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _chargeStartTime = Time.time;
            }
            if (Input.GetButton("Fire1"))
            {
                _chargeEndTime = Time.time;
                _chargeDuration = Mathf.Clamp(_chargeEndTime - _chargeStartTime, 0, maxChargeDuration);

                UIManager.instance.UpdateChargeHud(_chargeDuration / maxChargeDuration);
            }
            if (Input.GetButtonUp("Fire1"))
            {
                UIManager.instance.UpdateChargeHud(0);

                ShootBoomerang();
            }
        }
    }

    void ShootBoomerang()
    {
        if (GameManager.instance.BoomerangCount > 0)
        {
            // Raycast towards the mouse click
            Ray _clickRay = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Get the poin where the ray hits
            RaycastHit _rayHit;
            if (Physics.Raycast(_clickRay, out _rayHit, 100f))
            {
                // Instantiate boomerang
                boomerang = Instantiate(boomerangPrefab).GetComponent<BoomerangBehaviour>();
                boomerang.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

                // Get a line from the screen center until the hit point
                //Debug.DrawLine(transform.position, _rayHit.point, Color.red);

                // Get throwing direction
                _throwDirection = (_rayHit.point - transform.position);

                // Add force to the boomerang towards the target
                _throwForce = Mathf.Max(_chargeDuration / maxChargeDuration * throwMaxForce, throwMinForce);
                boomerang.GetComponent<Rigidbody>().AddForce(_throwDirection * _throwForce, ForceMode.Impulse);
                boomerang.GetComponent<BoomerangBehaviour>().returnDirection = _throwDirection * _throwForce * -2;

                // Play sound
                boomerangSource.Play();

                GameManager.instance.UseBoomerang();
            }
        }
    }
}

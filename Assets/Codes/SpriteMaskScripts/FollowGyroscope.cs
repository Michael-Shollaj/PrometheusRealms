using UnityEngine;
using UnityEngine.UI;
using System.Threading;
//using Ardity; // This is the namespace for the SerialPort asset

public class FollowGyroscope : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float smoothTime = 0.1f;
    public float followDuration = 10f;
    public float rechargeDuration = 15f;
    public Slider followTimeSlider;
    public Image sliderFill;
    public ParticleSystem trailEffect;

    [Header("Spawn Settings")]
    public GameObject objectToSpawn;
    public float spawnInterval = 0.05f;

    [Header("Arduino Settings")]
   // public SerialController serialController;

    private bool followGyro = false;
    private bool isRecharging = false;
    private float timer = 0f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private Vector3 lastPosition;
    private float spawnTimer = 0f;

    private float gyroX, gyroY;

    private void Start()
    {
        SetupSlider();
        if (trailEffect != null) trailEffect.Stop();
        lastPosition = transform.position;

  //      if (serialController == null)
  //      {
   //         Debug.LogError("SerialController is not assigned to the FollowGyroscope script!");
  //      }
    }

    void Update()
    {
        HandleInput();
        UpdateTimerAndSlider();
        if (followGyro)
        {
            MoveSmoothly();
            UpdateSpawn();
        }
    }

    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        string[] values = msg.Split(',');
        if (values.Length == 2)
        {
            float.TryParse(values[0], out gyroX);
            float.TryParse(values[1], out gyroY);
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.G) && !isRecharging)
        {
            ToggleFollowGyro();
        }
    }

    void ToggleFollowGyro()
    {
        followGyro = !followGyro;
        timer = followGyro ? followDuration : 0f;
        followTimeSlider.gameObject.SetActive(followGyro);
        sliderFill.color = followGyro ? Color.green : Color.red;
        if (followGyro)
        {
            if (trailEffect != null) trailEffect.Play();
        }
        else
        {
            if (trailEffect != null) trailEffect.Stop();
            if (!isRecharging) StartRecharge();
        }
    }

    void UpdateTimerAndSlider()
    {
        if (followGyro || isRecharging)
        {
            timer -= Time.deltaTime;
            followTimeSlider.value = timer;
            if (timer <= 0)
            {
                if (followGyro)
                {
                    ToggleFollowGyro();
                }
                else if (isRecharging)
                {
                    isRecharging = false;
                    followTimeSlider.gameObject.SetActive(false);
                }
            }
        }
    }

    void MoveSmoothly()
    {
        // Map gyroscope data to screen coordinates
        float screenX = Mathf.Clamp(gyroX, -90, 90) / 180f * Screen.width + Screen.width / 2f;
        float screenY = Mathf.Clamp(gyroY, -90, 90) / 180f * Screen.height + Screen.height / 2f;

        targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenX, screenY, 0));
        targetPosition.z = 0;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, moveSpeed);
    }

    void StartRecharge()
    {
        isRecharging = true;
        timer = rechargeDuration;
        followTimeSlider.maxValue = rechargeDuration;
        followTimeSlider.value = rechargeDuration;
        followTimeSlider.gameObject.SetActive(true);
        sliderFill.color = Color.red;
    }

    void SetupSlider()
    {
        followTimeSlider.maxValue = followDuration;
        followTimeSlider.value = followDuration;
        sliderFill.color = Color.green;
        followTimeSlider.gameObject.SetActive(false);
    }

    public bool IsFollowingGyro()
    {
        return followGyro;
    }

    void UpdateSpawn()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && Vector3.Distance(transform.position, lastPosition) > 0.01f)
        {
            SpawnObject();
            spawnTimer = 0f;
            lastPosition = transform.position;
        }
    }

    void SpawnObject()
    {
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class FollowCursor : MonoBehaviour
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

    private bool followMouse = false;
    private bool isRecharging = false;
    private float timer = 0f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private Vector3 lastPosition;
    private float spawnTimer = 0f;

    private void Start()
    {
        Cursor.visible = false;
        SetupSlider();
        if (trailEffect != null) trailEffect.Stop();
        lastPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        UpdateTimerAndSlider();
        if (followMouse)
        {
            MoveSmoothly();
            UpdateSpawn();
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isRecharging)
        {
            ToggleFollowMouse();
        }
    }

    void ToggleFollowMouse()
    {
        followMouse = !followMouse;
        timer = followMouse ? followDuration : 0f;
        followTimeSlider.gameObject.SetActive(followMouse);
        sliderFill.color = followMouse ? Color.green : Color.red;

        if (followMouse)
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
        if (followMouse || isRecharging)
        {
            timer -= Time.deltaTime;
            followTimeSlider.value = timer;

            if (timer <= 0)
            {
                if (followMouse)
                {
                    ToggleFollowMouse();
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
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

    public bool IsFollowingMouse()
    {
        return followMouse;
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
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossPlayerController : MonoBehaviour
{
    /// <summary>
    /// Boss �� PC�� �ൿ �ʵ� ����
    /// </summary>
    [Header("Movement")]
    public float moveSpeed = 7f; // �̵� �ӵ�
    private Vector2 moveInput; // �̵� �Է� ����
    private Rigidbody2D rb; // ������ٵ� ������Ʈ

    /// <summary>
    /// Boss �� PC�� ���� �ʵ� ����
    /// </summary>
    [Header("Combat")]
    public GameObject bulletPrefab; // �Ѿ� ������
    public Transform firePoint; // �Ѿ� �߻� ��ġ
    public float fireRate = 0.2f; // �߻� �ӵ�
    private float nextFireTime = 0f; // ���� �߻� ���� �ð�


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // �̵� ó��
        var keyboard = Keyboard.current;
        if (keyboard != null) return;

        if (keyboard.aKey.isPressed) moveInput.x = -1;
        else if (keyboard.dKey.isPressed) moveInput.x = 1;
        else if(keyboard.wKey.isPressed) moveInput.y = 1;
        else if(keyboard.sKey.isPressed) moveInput.y = -1;
        rb.angularVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);



        // ���� �Է� ó��
        if (Keyboard.current.spaceKey.isPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    // �Ѿ� �߻� �޼���
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}

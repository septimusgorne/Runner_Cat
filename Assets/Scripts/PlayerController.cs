using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class PlayerController : SingletonBehaviour<RoadGenerator>

public class PlayerController : MonoBehaviour// public modificator
{
    public static PlayerController instance;

    void Awake() 
    { 
        instance = this; 
    }

    Animator animator;//link animator
    Vector3 startGamePosition;
    Quaternion startGameRotation;
    Vector3 targetPos;
    float laneOffset = 100f;//2.5
    public float laneChangeSpeed = 15;// Поле скорости "публичное"
    /*float timeElapsed;
    float lerpDuration = 0.5f;*/
    Rigidbody rb;// run physics
    Vector3 targetVelocity;
    float pointStart;
    float pointFinish;
    bool isMoving = false;
    Coroutine movingCoroutine;
    float lastVectorX;
    bool isJumping = false;
    public float jumpPower = 15;
    public float jumpGravity = -40;
    float realGravity = -9.8f;

    [SerializeField] private float _speed;

    
    void Start()
    {
        laneOffset = MapGenerator.Instance.laneOffset;///I
        animator = GetComponent<Animator>();// прописываем в методе Start
        rb = GetComponent<Rigidbody>();// link RB
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;//memory StartGamePosition
        //targetPos = transform.position;
    }

    void Update() // перемещение игрока 13.04
    {
        if (Input.GetKeyDown(KeyCode.A) && pointFinish > -laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);//optimization function left
        }
        if (Input.GetKeyDown(KeyCode.D) && pointFinish < laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);//optimization function right
        }
        if (Input.GetKeyDown(KeyCode.W) && isJumping == false)//определение на прыжок клавиши W
        {
            Jump();//if don't isJumping будем прыгать
        }
        //transform.position = Vector3.MoveTowards(transform.position, targetPos, laneChangeSpeed * Time.deltaTime);*/
    }

   /* void MovePlayer(bool[] swipes)
    {
        if (swipes[(int)SwipeManager.Direction.Left] && pointFinish > -laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);//optimization function left
        }
        if (swipes[(int)SwipeManager.Direction.Right] && pointFinish > -laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);//optimization function right
        }
        if (swipes[(int)SwipeManager.Direction.Up] && isJumping == false)//определение на прыжок клавиши W
        {
            Jump();//if don't isJumping будем прыгать
        }
    } */ //realization swipes

    void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);//AddForce прикладывает силу с Power + режим Impulse
        Physics.gravity = new Vector3(0, jumpGravity, 0);
        StartCoroutine(StopJumpCoroutine());//Coroutina на проверку остановки падения
    }

    IEnumerator StopJumpCoroutine()
    {
        do
        {
            yield return new WaitForSeconds(0.02f);//Ожидать секунд
        } while (rb.velocity.y != 0);
        isJumping = false;
        Physics.gravity = new Vector3(0, realGravity, 0);// reset real gravity
    }

    /*private void FixUpdate()
    {
        rb.velocity = targetVelocity;
        if((transform.position.x > pointFinish && targetVelocity.x > 0) ||
            (transform.position.x < pointFinish && targetVelocity.x < 0))// определение остановки объекта
        {
            targetVelocity = Vector.zero;//Двигаемся с заданной скорость в позицию, пока ее не преодолеем
            rb.velocity = targetVelocity;//Сбрасываем в ноль при достижении
            rb.position = new Vector3(pointFinish, rb.position.y, rb.position.z);
        }*/
    /*tranfsorm.position = Vector3.MoveTowards(transform.position, targetPos, laneChangeSpeed * Time.deltaTime);
    if (timeElapsed < lerpDuration)
    {
        tranfsorm.position = Vector3.Lerp(transform.position, targetPos, timeElapsed / lerpDuration);
        timeElapsed += Time.deltaTime;
    }
    else
    {
        transform.position = targetPos;// завершение для lerp*/
    //}
    //}

    void MoveHorizontal(float speed)
    {
        animator.applyRootMotion = false;//На время движения в фолс
        pointStart = pointFinish;// прошлое значение
        pointFinish += Mathf.Sign(speed) * laneOffset;// текущее значение

        if (isMoving) { StopCoroutine(movingCoroutine); isMoving = false; }// Если сейчас в движении, сбрасываем флаг в фолс.

        movingCoroutine = StartCoroutine(MoveCoroutine(speed));
        //timeElapsed = 0;
        //targetPos = new Vector3(target.x - laneOffset, transform.position.y, transform.position.z);
        //animator.applyRootMotion = false;


    }

    IEnumerator MoveCoroutine(float vectorX)
    {
        isMoving = true;
        while (Mathf.Abs(pointStart - transform.position.x) < laneOffset)
        {
            yield return new WaitForFixedUpdate();// пока не перешли на другую полосу выполняем ожидание FixUpdate

            rb.velocity = new Vector3(vectorX, rb.velocity.y, 0);
            lastVectorX = vectorX;

            float x = Mathf.Clamp(transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));

        }
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(pointFinish, transform.position.y, transform.position.z);
        if (transform.position.y > 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);//Если позиция по y больше 1, то приравниваем к 10
        }
        isMoving = false;
    }

    public void StartGame()
    {
        RoadGenerator.Instance.StartLevel();//запускаем StartLevel в RoadGenerator
    }

    public void StartLevel()
    {
        animator.SetTrigger("Run");//call Trigger "Run"
    }
    public void ResetGame()
    {
        rb.velocity = Vector3.zero;
        pointStart = 0;
        pointFinish = 0;
        animator.applyRootMotion = true;
        //animator.SetTrigger("idle");
        transform.position = startGamePosition;//reset transform position
        transform.rotation = startGameRotation;
        RoadGenerator.Instance.ResetLevel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;//для Enter назначаем флаг
        }
        if (other.gameObject.tag == "Lose")
        {
            ResetGame();
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;//для Exit снимаем флаг
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")//Если соприкоснулись с землей
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        if (collision.gameObject.tag == "NotLose")
        {
            MoveHorizontal(-lastVectorX);//вызываем метод с значением -скорости
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "RampPlane")//для спрыгивания определяем Collision, если RampPlane
        {
            if (rb.velocity.x == 0 && isJumping == false)
            {
                rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);//чтобы по x двигался вниз быстрее
            }
        }
    }
}
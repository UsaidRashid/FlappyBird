using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class BirdController : MonoBehaviour
{
    public float jumpForce = 5f;
    public float horizontalSpeed = 3f;
    private Rigidbody rb;
    public float topLimit = 33f;
    public float bottomLimit = 7f;
    private AudioSource audio;
    public float scoreUpdateInterval = 0.1f;
    private float timeSinceLastUpdate = 0.0f;
    public TextMesh scoreText;
    public TextMesh gameOverText;
    private int myScore = 0;
    private Animator wingAnimator;
    private bool isGameOver = false;
    public Button RestartButton;
    public Button QuitButton;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
{
#if UNITY_EDITOR
    EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
}

    void Start()
    {
        RestartButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        myScore = 0;
        updateScoreText();

        wingAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isGameOver)
        {
             if (Input.GetKeyUp(KeyCode.Escape))    
            {
                RestartGame();
            }
            else if (Input.GetKeyUp(KeyCode.Return))
            {
                ExitGame();
            }
        }
        else
        {
            if (enabled)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                }

                rb.velocity = new Vector3(horizontalSpeed, rb.velocity.y, rb.velocity.z);
                Vector3 clampedPosition = transform.position;
                clampedPosition.y = Mathf.Clamp(clampedPosition.y, bottomLimit, topLimit);
                transform.position = clampedPosition;
                timeSinceLastUpdate += Time.deltaTime;
                if (timeSinceLastUpdate >= scoreUpdateInterval)
                {
                    timeSinceLastUpdate -= scoreUpdateInterval;
                    updateScore();
                }
            }
        }
    }

    private void updateScore()
    {
        myScore++;
        updateScoreText();
    }

    private void updateScoreText()
    {
        scoreText.text = myScore.ToString();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Finish")
        {
            isGameOver = true;
            enabled = false;
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            audio.enabled = false;
            scoreText.gameObject.SetActive(false);

            if (wingAnimator != null)
            {
                wingAnimator.enabled = false;
            }

            string gameoverstring = gameOverText.text + myScore;
            gameOverText.text = gameoverstring;
            gameOverText.gameObject.SetActive(true);
            RestartButton.gameObject.SetActive(true);
            QuitButton.gameObject.SetActive(true);
        }
    }

    
}

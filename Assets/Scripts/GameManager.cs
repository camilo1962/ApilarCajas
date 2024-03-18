using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform blockPrefab;
    [SerializeField] private Transform blockHolder;
    
    [SerializeField] private TMPro.TextMeshProUGUI livesText;
    
    private Transform currentBlock = null;
    private Rigidbody2D currentRigidbody;
    
    private int score;
    private int finalScore;

    public TMP_Text puntosText;
    public TMP_Text finalPuntosText;
    public TMP_Text record;
    public TMP_Text otroRecord;
    public GameObject panelGameOver;
    private Vector2 blockStartPosition = new Vector2(0f, 3f);

    private float blockSpeed = 6f;
    private float blockSpeedIncrement = 0.3f;
    private int blockDirection = 1;
    private float xLimit = 5;
    
    private float timeBetweenRounds = 1f;
    
    // Variables to handle the game state.
    private int startingLives = 5;
    private int livesRemaining;
    private bool playing = true;
    
    // Start is called before the first frame update
    void Start()
    {
       livesRemaining = startingLives;
       livesText.text = $"{livesRemaining}";
       SpawnNewBlock();
       otroRecord.text = PlayerPrefs.GetInt("record", score).ToString();
    }

    private void SpawnNewBlock()
    {
      // Create a block with the desired properties.
      currentBlock = Instantiate(blockPrefab, blockHolder);
      currentBlock.position = blockStartPosition;
      currentBlock.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
      currentRigidbody = currentBlock.GetComponent<Rigidbody2D>();
      // Aumenta la velocidad del bloque cada vez para hacerlo más difícil..
      blockSpeed += blockSpeedIncrement;
    }

    private IEnumerator DelayedSpawn() {
      yield return new WaitForSeconds(timeBetweenRounds);
      SpawnNewBlock();
    
    }
    
    // Update is called once per frame
    void Update()
      {
          // Si tenemos un bloque de espera, muévelo.
          if (currentBlock && playing)
          {
             float moveAmount = Time.deltaTime * blockSpeed * blockDirection;
             currentBlock.position += new Vector3(moveAmount, 0, 0);
              // Si hemos llegado tan lejos como queremos, invertimos la dirección.
             if (Mathf.Abs(currentBlock.position.x) > xLimit)
             {
               // Ponlo al límite para que no vaya más allá.
               currentBlock.position = new Vector3(blockDirection * xLimit, currentBlock.position.y, 0);
               blockDirection = -blockDirection;
             }
    
               // Si presionamos la barra espaciadora soltamos el bloque.
               if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
               {
                 // Stop it moving.
                 currentBlock = null;
                 // Activate the RigidBody to enable gravity to drop it.
                 currentRigidbody.simulated = true;
                 score++;
                 puntosText.text = score.ToString();        
                 // Spawn the next block.
                 StartCoroutine(DelayedSpawn());
               }
          }
    
          // signa temporalmente una clave para reiniciar el juego.
          if (Input.GetKeyDown(KeyCode.Escape)) 
          {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
          }
    }
    
      //Llamado desde LoseLife cada vez que detecta que se ha caído un bloque.
    public void RemoveLife()
    {
        // Update the lives remaining UI element.
        livesRemaining = Mathf.Max(livesRemaining - 1, 0);
        livesText.text = $"{livesRemaining}";
        // Check for end of game.
        if (livesRemaining == 0)
        {
          playing = false;
          panelGameOver.SetActive(true);
          finalScore = score;
          finalPuntosText.text = finalScore.ToString();
          
          if (score > PlayerPrefs.GetInt("record", 0))
          {
              PlayerPrefs.SetInt("record", score);            
                            
          }
          record.text = PlayerPrefs.GetInt("record", score).ToString();
          
        }
    }
    
}

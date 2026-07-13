// Contains the information common to all checkpoints that inherit from it

// Base class for all checkpoints (cinematic + mission)
[CreateAssetMenu(fileName = "CheckPointBase", menuName = "Episodes/CheckPointBase")]
public class CheckPointBase : ScriptableObject
{
    // Position and rotation where players should be placed
    // at the start of the checkpoint
    public SpawnPlayerData[] m_playerSpawnData;

    public EnumScene m_scene;
    public EnumMusic m_music;

    // Instantiated objects for this checkpoint. They will be destroyed
    // when it ends		
    public List<GameObject> m_checkpointObjects;

    public virtual void StartCheckPoint()
    {
        GameManager gameManagerInstance = GameManager.m_gameManager;

        if (gameManagerInstance.SceneManager.IsSceneLoaded(m_scene))
        {
            InitializeCheckPoint();
        }
        else
        {
            // Subscribe to callback and trigger scene load (handled externally)
            gameManagerInstance.SceneManager.onSceneLoaded += OnSceneLoaded;
            gameManagerInstance.SceneManager.LoadScene(m_scene);
        }
    }

    public virtual void EndCheckPoint()
    {
        DestroyObjectsForCheckpoint();
    }

    private void OnSceneLoaded(EnumScene scene)
    {
        if (scene == m_scene)
        {
            GameManager.m_gameManager.SceneManager.onSceneLoaded -= OnSceneLoaded;
            InitializeCheckPoint();
        }
    }

    public virtual void InitializeCheckPoint()
    {
        GameManager gameManagerInstance = GameManager.m_gameManager;

        gameManagerInstance.AudioManager.PlayMusic(m_music);
        SetUpPlayersForCheckPoint();
        InstantiateObjectsForCheckPoint();
        gameManagerInstance.SaveManager.SaveEpisodeState();
    }
}

public struct SpawnPlayerData
{
    public EnumPlayer m_playerID;
    public Vector3 m_position;
    public Quaternion m_rotation;
}
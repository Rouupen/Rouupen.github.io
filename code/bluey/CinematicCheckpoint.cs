// Contains the necessary information to play different types of cinematics

[CreateAssetMenu(fileName = "CinematicCheckPoint", menuName = "Episodes/CinematicCheckPoint")]
public class CinematicCheckPoint : CheckPointBase
{
    public PlayableAsset m_cinematic;

    // Additional variables depending on the cinematic 
    // Example:
    public GameObject m_objectToEquipInPlayerHands;
    public List<GameObject> m_objectsToHide;
    public bool m_needBlackBars;

    public override void InitializeCheckPoint()
    {
        base.InitializeCheckPoint();

        // Instantiate, position, or hide specific objects
        PrepareCinematic();

        // When everything is ready, the animation clip is played
        GameManager.m_gameManager.CinematicManager.PlayCinematic(m_cinematic);
    }

    // The CinematicManager determines when the cinematic has ended and calls EndCheckPoint to continue the episode flow
    public override void EndCheckPoint()
    {
        base.EndCheckPoint();

        SavePlayerPositionsAfterCinematic();

        // Restore the scene to its original state
        DestroyCinematicObjects();
        DisableCinematicPlayers();
    }
}
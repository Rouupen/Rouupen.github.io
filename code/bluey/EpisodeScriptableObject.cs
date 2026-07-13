// Contains all information regarding an episode

// Basic data used to define a NPC
public struct NPCData
{
    public EnumNPC m_id;
    public Vector3 m_position;
    public Quaternion m_rotation;
}

// Data container for each checkpoint inside an episode
public struct CheckPointData
{
    public CheckPointBase m_checkPoint;

    // NPC data for this checkpoint
    public List<NPCData> m_npcData;
}

// Main Scriptable Object that defines an episode
[CreateAssetMenu(fileName = "EpisodeSO", menuName = "Episodes/EpisodeSO")]
public class EpisodeScriptableObject : ScriptableObject
{
    // Ordered list of the checkpoint flow
    public List<CheckPointData> m_checkPoints;

    // Advance to next checkpoint (implementation handled externally)
    public void StartCheckPoint(int checkPointIndex)
    {
        if (checkPointIndex < 0 || checkPointIndex >= m_checkPoints.Count)
        {
            Debug.LogError($"[EpisodeSO] Checkpoint index {checkPointIndex} is out of range. Flow execution stopped.");
            return;
        }

        // Trigger checkpoint start logic
        m_checkPoints[checkPointIndex].m_checkPoint.StartCheckPoint();
        SetUpNpcs(m_checkPoints[checkPointIndex].m_npcData);
    }

    // Ends active checkpoint (implementation handled externally)
    public void EndCheckPoint(int checkPointIndex)
    {
        if (checkPointIndex < 0 || checkPointIndex >= m_checkPoints.Count)
        {
            Debug.LogError($"[EpisodeSO] Checkpoint index {checkPointIndex} is out of range. Flow execution stopped.");
            return;
        }
        m_checkPoints[checkPointIndex].m_checkPoint.EndCheckPoint();
    }

    // Additional methods required for the episode

    public void SetUpNpcs(List<NPCData> npcData)
    {
        // Use stored NPC info to position them in the scene
    }

    public CheckPointBase GetCheckPoint(int checkPointIndex)
    {
        if (checkPointIndex < 0 || checkPointIndex >= m_checkPoints.Count)
        {
            Debug.LogError($"[EpisodeSO] Checkpoint index {checkPointIndex} is out of range. Flow execution stopped.");
            return null;
        }

        return m_checkPoints[checkPointIndex].m_checkPoint;
    }
}
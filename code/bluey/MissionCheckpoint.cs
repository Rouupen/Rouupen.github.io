// Contains the information and methods required to play a mission in the episode

[CreateAssetMenu(fileName = "MissionCheckPoint", menuName = "Episodes/MissionCheckPoint")]
public class MissionCheckPoint : CheckPointBase
{
    public EnumMissionType m_missionType;

    // Variables were shown based on the mission type. Example:
    [ShowIf("@(this.m_missionType == EnumMissionType.PlayActivity)")]
    public ActivityMissionData m_activityData;

    public override void InitializeCheckPoint()
    {
        base.InitializeCheckPoint();

        // When starting a mission, we passed the mission type and the checkpoint reference so it could read the associated data, among other common values and methods
        GameManager.m_gameplayManager.MissionManager.StartMission(m_missionType, this);
    }

    // The MissionManager processed and evaluated the mission. If completed, it called EndCheckPoint to continue the episode flow
    public override void EndCheckPoint()
    {
        base.EndCheckPoint();
        SavePlayerProgressPosition();
    }

    // Additional methods required for the mission
}
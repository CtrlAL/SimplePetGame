namespace Assets.Scripts.Enums
{
    public enum SoundType
    {
        #region Actions
        PlayerKick,
        EnemyKick,
        Jump,
        Throw,
        #endregion

        #region Events
        Death,
        FallDown,
        BottleHit,
        BottleDropOnFloor,
        #endregion

        #region Game States
        GameOver,
        Win,
        #endregion
    }
}
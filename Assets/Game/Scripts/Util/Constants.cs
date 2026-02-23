namespace SpaceInvaders
{
    public static class Constants
    {
        public const string TimestampFormat = "yyyyMMddHHmmss";
        public const string ProjectilePoolObjectName = "ProjectilePool";
    }

    public static class Strings
    {
        public const string ResultsScorePrefix = "SCORE: ";

        public const string ScoreInfoPrefix = ": ";
        public const string ScoreInfoSuffix = " POINTS";
        public const string ScoreInfoMystery = ": ???????";

        public const string PLAY_BUTTON_TEXT = "PLAY";
        public const string HIGH_SCORES_BUTTON_TEXT = "HIGH SCORES";
        public const string CONTROLS_BUTTON_TEXT = "CONTROLS";

        public const string STAT_DIVIDER = ":";
        public const string STAT_SCORE = "SCORE";
        public const string STAT_WAVE = "WAVE";
        public const string STAT_LIVES = "LIVES";
    }

    public static class PlayerPrefsKeys
    {
        public const string HighScores = "HighScores";
    }

    public static class SortingLayers
    {
        public const string Default = "Default";
        public const string Overlay = "Overlay";
    }

    public static class SceneBuildIndices
    {
        public static int LoadingScene = 0;
        public static int GameScene = 1;
    }
}
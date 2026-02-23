namespace SpaceInvaders
{
    public static class Constants
    {
        public const string TimestampFormat = "yyyyMMddHHmmss";
        public const string ProjectilePoolObjectName = "ProjectilePool";
    }

    public static class DisplayStrings
    {
        public const string ResultsScorePrefix = "SCORE: ";

        public const string ScoreInfoPrefix = ": ";
        public const string ScoreInfoSuffix = " POINTS";
        public const string ScoreInfoMystery = ": ? MYSTERY";
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
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    [CreateAssetMenu(menuName = "Config/Gameplay Assets Config")]
    public class GameplayAssetsConfig : ScriptableObject
    {
        public Player Player;
        public Enemy Enemy1;
        public Enemy Enemy2;
        public Enemy Enemy3;
        public EnemyUFO UFO;
        public EnemyWave EnemyWave;
        public Projectile Projectile;

        public Enemy GetEnemyPrefab(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Enemy1:
                    return Enemy1;
                case EnemyType.Enemy2:
                    return Enemy2;
                case EnemyType.Enemy3:
                    return Enemy3;
                case EnemyType.UFO:
                    return UFO;
                default:
                    // Fallback, shouldn't happen.
                    return Enemy1;
            }
        }
    }
}
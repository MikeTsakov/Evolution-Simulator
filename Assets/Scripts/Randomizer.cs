using System;

public class Randomizer {

    readonly static Random rnd = new Random();
    
    #region SetupTimers
    public double RandomizeDirection(float min, float max) {
        double randomR = rnd.NextDouble() * (max - min) + min;
        return randomR;
    }

    public double RandomizeSpeed(double moveSpeedF, float min, float max) {
        moveSpeedF = rnd.NextDouble() * ((moveSpeedF + max) - (moveSpeedF - min)) + (moveSpeedF - min);
        if (moveSpeedF < 0) {
            moveSpeedF = 0 - moveSpeedF;
        }
        return moveSpeedF;
    }
    
    public float RandomizeAngle(float angle1, float angle2) {
        return (float) rnd.NextDouble() * ((angle1) - (angle2)) + angle2;
    }

    public double RandomizeDeathTimer(double deathTimerF, float min, float max) {
        deathTimerF = rnd.NextDouble() * ((deathTimerF + max) - (deathTimerF - min)) + (deathTimerF - min);
        if (deathTimerF < 0) {
            deathTimerF = 0 - deathTimerF;
        }
        return deathTimerF;
    }

    public double RandomizeLayEggTimer(double layEggTimerF, float min, float max) {
        layEggTimerF = rnd.NextDouble() * ((layEggTimerF + max) - (layEggTimerF - min)) + (layEggTimerF - min);
        if (layEggTimerF < 0) {
            layEggTimerF = 0 - layEggTimerF;
        }
        return layEggTimerF;
    }

    public double RandomizeEggSpawnTimer(double eggSpawnTimerF, float min, float max) {
        eggSpawnTimerF = rnd.NextDouble() * ((eggSpawnTimerF + max) - (eggSpawnTimerF - min)) + (eggSpawnTimerF - min);
        if (eggSpawnTimerF < 0) {
            eggSpawnTimerF = 0 - eggSpawnTimerF;
        }
        return eggSpawnTimerF;
    }

    public double RandomizeConsumeFoodTimer(double consumeFoodTimerF, float min, float max) {
        consumeFoodTimerF = rnd.NextDouble() * ((consumeFoodTimerF + max) - (consumeFoodTimerF - min)) + (consumeFoodTimerF - min);
        if (consumeFoodTimerF < 0) {
            consumeFoodTimerF = 0 - consumeFoodTimerF;
        }
        return consumeFoodTimerF;
    }

    public double RandomizeSight(double sightF, float number) {
        sightF = sightF * number;
        return sightF;
    }
    #endregion
}

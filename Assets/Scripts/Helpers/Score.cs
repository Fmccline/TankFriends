
public class Score
{
    private float damageDone;
    private float damageTaken;
    private int kills;
    private int deaths;
    private int wins;
    private int tankNumber;

    public Score(int tankNumber)
    {
        this.tankNumber = tankNumber;
        ResetScore();
    }

    public void ResetScore()
    {
        damageDone = 0;
        damageTaken = 0;
        deaths = 0;
        kills = 0;
        wins = 0;
    }

    public float GetDamageDone()
    {
        return damageDone;
    }

    public float GetDamageTaken()
    {
        return damageTaken;
    }

    public int GetKills()
    {
        return kills;
    }

    public int GetDeaths()
    {
        return deaths;
    }

    public int GetWins()
    {
        return wins;
    }

    public int GetTankNumber()
    {
        return tankNumber;
    }

    public void AddDamageDone(float damageDone)
    {
        damageDone += damageDone;
    }

    public void AddDamageTaken(float damageTaken)
    {
        damageTaken += damageTaken;
    }

    public void AddWin()
    {
        wins++;
    }

    public void AddKill()
    {
        kills++;
    }

    public void AddDeath()
    {
        deaths++;
    }
}

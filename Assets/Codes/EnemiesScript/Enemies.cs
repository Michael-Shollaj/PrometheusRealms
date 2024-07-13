using UnityEngine;

public class Enemies : MonoBehaviour
{
    // Base class so it can be inherited from other scripts

    int baseMoveSpeed;
    int baseAttackDamage;
    int baseLifePoints;
    float baseAttackRadius;

    // Movement
    float baseFollowRadius;

    public void setMoveSpeed(int speed)
    {
        baseMoveSpeed = speed;
    }

    public void setAttackDamage(int attdmg)
    {
        baseAttackDamage = attdmg;
    }

    public void setLifePoints(int lp)
    {
        baseLifePoints = lp;
    }

    public int getMoveSpeed()
    {
        return baseMoveSpeed;
    }

    public int getAttackDamage()
    {
        return baseAttackDamage;
    }

    public int getLifePoints()
    {
        return baseLifePoints;
    }

    // Movement toward a player
    public void setFollowRadius(float r)
    {
        baseFollowRadius = r;
    }

    // Attack radius 
    public void setAttackRadius(float r)
    {
        baseAttackRadius = r;
    }

    // If player in radius move toward him 
    public bool checkFollowRadius(float playerPosition, float enemyPosition)
    {
        if (Mathf.Abs(playerPosition - enemyPosition) < baseFollowRadius)
        {
            // Player in range
            return true;
        }
        else
        {
            return false;
        }
    }

    // If player in radius attack him
    public bool checkAttackRadius(float playerPosition, float enemyPosition)
    {
        if (Mathf.Abs(playerPosition - enemyPosition) < baseAttackRadius)
        {
            // In range for attack
            return true;
        }
        else
        {
            return false;
        }
    }
}

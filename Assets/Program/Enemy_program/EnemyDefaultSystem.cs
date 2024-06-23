using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static Enemy_List;

public class EnemyDefaultSystem : MonoBehaviour
{
    [SerializeField]private Enemy_List enemyList;
    public void TakeDamage(Collider other, Vector3 damageTextPosition, Transform thisObject, Quaternion hpSliderQuaternion, float damage,
        ref GameObject oldDamageText, ref GameObject damageTextObject, ref float oldDamage, ref float currenthp, ref bool isDeath)
    {
        Debug.Log("‹N“®‚Q");
        if (oldDamageText == null)
        {
            GameObject damageText = Instantiate(damageTextObject, damageTextPosition, hpSliderQuaternion, thisObject.transform.Find("EnemyCanvas"));
            oldDamageText = damageText;
            damageText.GetComponent<Text>().text = damage.ToString();
            oldDamage = damage;
        }
        else if (oldDamageText != null)
        {
            oldDamage += damage;
            oldDamageText.GetComponent<UnityEngine.UI.Text>().text = oldDamage.ToString();
            oldDamageText.GetComponent<Damage_Text>().TextReset();
            oldDamageText.transform.localEulerAngles = new Vector3(0, 180, 180);
        }
        if (currenthp > 0)
        {
            currenthp -= damage;
        }
        if (currenthp <= 0)
        {
            isDeath = true;
        }
    }
    public void EnemyStatsReset(ref bool isDeath, int enemy_number, ref float hp, ref float atk, ref float agi, ref float currenthp, ref float currentatk, ref float currentagi,
        ref Slider hpSlider , ref Enemy_Manager enemyManager , ref GameObject playerObject)
    {
        if (enemyManager == null) enemyManager = transform.parent.parent.GetComponent<Enemy_Manager>();
        if (playerObject == null) playerObject = enemyManager.player_system;
        isDeath = false;
        hp = enemyList.data[enemy_number].hp;
        atk = enemyList.data[enemy_number].atk;
        agi = enemyList.data[enemy_number].agi;
        currenthp = hp;
        currentatk = atk;
        currentagi = agi;
        hpSlider.maxValue = hp;
        hpSlider.value = currenthp;
    }
    public void NavMeshAgentReset( int enemy_number, float currentagi, ref NavMeshAgent navMeshAgent)
    {
        navMeshAgent.speed = currentagi;
        navMeshAgent.acceleration = currentagi / 2;
        navMeshAgent.angularSpeed = currentagi * 10;
        navMeshAgent.stoppingDistance = enemyList.data[enemy_number].stoppingDistance;
    }
    public void IsBulletTypeJuge(Collider other, Vector3 damageTextPosition, Transform thisObject, Quaternion hpSliderQuaternion,
        ref GameObject oldDamageText, ref GameObject damageTextObject, ref float oldDamage, ref float currenthp, ref bool isDeath)
    {
        if (other.GetComponent<NormalBulletSystem>() != null && other.GetComponent<NormalBulletSystem>().targetTag == "Enemy")
        {
            TakeDamage(other,damageTextPosition,thisObject,hpSliderQuaternion, other.GetComponent<NormalBulletSystem>().bulletDamage,ref oldDamageText,ref damageTextObject, ref oldDamage, ref currenthp, ref isDeath);
            other.GetComponent<NormalBulletSystem>().BulletDestroy();
        }
        else if (other.GetComponent<FollowingBulletSystem>() != null && other.GetComponent<FollowingBulletSystem>().targetTag == "Enemy")
        {
            TakeDamage(other, damageTextPosition, thisObject, hpSliderQuaternion, other.GetComponent<FollowingBulletSystem>().bulletDamage, ref oldDamageText, ref damageTextObject, ref oldDamage, ref currenthp, ref isDeath);
            other.GetComponent<NormalBulletSystem>().BulletDestroy();
        }
        else if (other.GetComponent<ParabolaBulletSystem>() != null && other.GetComponent<ParabolaBulletSystem>().targetTag == "Enemy")
        {
            TakeDamage(other, damageTextPosition, thisObject, hpSliderQuaternion, other.GetComponent<ParabolaBulletSystem>().bulletDamage, ref oldDamageText, ref damageTextObject, ref oldDamage, ref currenthp, ref isDeath);
            other.GetComponent<NormalBulletSystem>().BulletDestroy();
        }
        else if (other.GetComponent<SplitBulletSystem>() != null && other.GetComponent<SplitBulletSystem>().targetTag == "Enemy")
        {
            TakeDamage(other, damageTextPosition, thisObject, hpSliderQuaternion, other.GetComponent<SplitBulletSystem>().bulletDamage, ref oldDamageText, ref damageTextObject, ref oldDamage, ref currenthp, ref isDeath);
            other.GetComponent<NormalBulletSystem>().BulletDestroy();
        }
    }
}

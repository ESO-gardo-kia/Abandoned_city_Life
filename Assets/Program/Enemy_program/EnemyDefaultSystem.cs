using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static Enemy_List;

public class EnemyDefaultSystem : MonoBehaviour
{
    [SerializeField]private Enemy_List enemyList;
    public void TakeDamage(Bullet_System bulleySystem, Collider other, Vector3 damageTextPosition, Transform thisObject, Quaternion hpSliderQuaternion, float damage,
        ref GameObject oldDamageText, ref GameObject damageTextObject, ref float oldDamage, ref float currenthp, ref bool isDeath)
    {
        if (other.gameObject.CompareTag("Bullet") && other.GetComponent<Bullet_System>().target_tag == "Enemy")
        {
            if (oldDamageText == null)
            {
                GameObject damageText = Instantiate(damageTextObject, damageTextPosition, hpSliderQuaternion, thisObject.transform.Find("EnemyCanvas"));
                oldDamageText = damageText;
                damageText.GetComponent<Text>().text = bulleySystem.damage.ToString();
                oldDamage = bulleySystem.damage;
            }
            else if (oldDamageText != null)
            {
                oldDamage += bulleySystem.damage;
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
            bulleySystem.BulletDestroy();
        }
    }
    public void EnemyStatsReset(ref bool isDeath, int enemy_number, ref float hp, ref float atk, ref float agi, ref float currenthp, ref float currentatk, ref float currentagi,
        ref Slider hpSlider , ref Enemy_Manager enemyManager , ref GameObject playerObject)
    {
        if (enemyManager == null) enemyManager = transform.parent.parent.GetComponent<Enemy_Manager>();
        if (playerObject == null) playerObject = enemyManager.player_system;
        isDeath = false;
        hp = enemyList.Status[enemy_number].hp;
        atk = enemyList.Status[enemy_number].atk;
        agi = enemyList.Status[enemy_number].agi;
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
        navMeshAgent.stoppingDistance = enemyList.Status[enemy_number].stoppingDistance;
    }
}

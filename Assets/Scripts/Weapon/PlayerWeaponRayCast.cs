using System.Collections.Generic;
using UnityEngine;

namespace Assets.Weapon
{
    public class PlayerWeaponRayCast : MonoBehaviour
    {
        public Transform pointA;
        public Transform pointB;
        public LayerMask layer;
        public Transform[] Points; //射线发射点
        public Dictionary<int, Vector3> dic_lastPoints = new Dictionary<int, Vector3>(); //存放上个位置信息

        public Transform root;
        public PlayerParam playerParam;

        private void Start()
        {
            if (dic_lastPoints.Count == 0)
            {
                for (int i = 0; i < Points.Length; i++)
                {
                    dic_lastPoints.Add(Points[i].GetHashCode(), Points[i].position);
                }
            }

            root = transform.root;
            playerParam = root.GetComponent<PlayerParam>();
        }

        private void LateUpdate()
        {
            var newA = pointA.position;
            var newB = pointB.position;
            //Debug.DrawLine(newA, newB, Color.red, 1f);
            SetPostion(Points);
        }

        private void SetPostion(Transform[] points)
        {
            if (!playerParam.isAttacking) return;

            for (int i = 0; i < points.Length; i++)
            {
                if (playerParam.hitCount != 0) continue;

                var nowPos = points[i];
                dic_lastPoints.TryGetValue(nowPos.GetHashCode(), out Vector3 lastPos);
                //Debug.DrawLine(nowPos.position, lastPos, Color.blue, 1f); ;
                //Debug.DrawRay(lastPos, nowPos.position - lastPos, Color.blue, 1f);

                Ray ray = new Ray(lastPos, nowPos.position - lastPos);
                float distance = Vector3.Distance(lastPos, nowPos.position);
                float radius = 0.2f; // 设定球体半径
                RaycastHit[] raycastHits = Physics.SphereCastAll(ray, radius, distance, layer, QueryTriggerInteraction.Ignore);

                //Debug.DrawLine(lastPos, nowPos.position, Color.blue, 1f);
                //float debugRadius = 0.1f;
                //Debug.DrawLine(lastPos + Vector3.up * debugRadius, nowPos.position + Vector3.up * debugRadius, Color.cyan, 1f);
                //Debug.DrawLine(lastPos - Vector3.up * debugRadius, nowPos.position - Vector3.up * debugRadius, Color.cyan, 1f);

                foreach (var item in raycastHits)
                {
                    if (item.collider == null) continue;
                    //下面做击中后的一些判断和处理
                    //比如扣血之类的,

                    if (playerParam.hitCount == 0)
                    {
                        if (Vector3.Dot(playerParam.PlayerModel.forward, item.transform.position) < 0)
                        {
                            item.transform.GetComponent<EnemyController>().BeAttacked(playerParam.AttackDamage, PlayerParam.ATKDIR.B);
                        }
                        else
                        {
                            item.transform.GetComponent<EnemyController>().BeAttacked(playerParam.AttackDamage, playerParam.atkDirection);
                        }

                        Debug.Log("Hit enemy");
                        playerParam.hitCount++;
                    }

                    break;
                }

                if (nowPos.position != lastPos)
                {
                    dic_lastPoints[nowPos.GetHashCode()] = nowPos.position;//存入上个位置信息
                }
            }
        }
    }
}
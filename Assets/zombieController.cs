using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NavMesh
using UnityEngine.AI;

public class zombieController : MonoBehaviour
{
    //��ԁi�X�e�[�g�p�^�[���j
    private int stateNumber = 0;
    //�ėp�^�C�}�[
    private float timeCounter = 0f;
    private Animator myanimator;
    private float enemyLength = 2.0f;
    private GameObject player;

    private Rigidbody myRigidbody;
    private NavMeshAgent nav;
    //�n�_�i�e�I�u�W�F�N�g�j
    public GameObject navPoints;
    public GameObject attackPrefab;
    private GameObject attackPoint;
    public GameObject effectPrefab;

    //�z��Ń����_���Ɏw�肷����@ ���C���X�y�N�^�[�Ō���ݒ肷��
    //public GameObject[] navRandomPoints = new GameObject[7];
    //��: GetComponent<NavMeshAgent>().destination = navRandomPoints[0].transform.position;

    //�I�u�W�F�N�g��
    private int number = 0;

    //------------------------------------------------------------------------------------------------------------------
    //�X�^�[�g
    //------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        this.myanimator = GetComponent<Animator>();

        this.myRigidbody = GetComponent<Rigidbody>();

        this.nav = GetComponent<NavMeshAgent>();

        this.player = GameObject.Find("player");

        //�q�I�u�W�F�N�g�̐�
        number = navPoints.GetComponentInChildren<Transform>().childCount;

        int randomStart = Random.Range(0, number);

        attackPoint = transform.Find("attackPoint").gameObject;

        //if (number > 0)
        //{
        //�A�j���[�V����
        myanimator.SetInteger("speed", 1);
        //���̖ڕW�n�_��
        nav.destination = navPoints.transform.GetChild(randomStart).transform.position;
        //}

        //�ŏ��̖ڕW�n�_ �������`�F�b�N
        /*if (number > 0)
        {
            nav.destination = navPoints.transform.GetChild(0).transform.position;
        } */
    }

    //------------------------------------------------------------------------------------------------------------------
    //�I���W�i���֐�
    //------------------------------------------------------------------------------------------------------------------

    //���������߂�
    float getLength(Vector3 current, Vector3 target)
    {
        return Mathf.Sqrt(((current.x - target.x) * (current.x - target.x)) + ((current.z - target.z) * (current.z - target.z)));
    }

    //���������߂� ���I�C���[�i-180�`0�`+180)
    float getEulerAngle(Vector3 current, Vector3 target)
    {
        Vector3 value = target - current;
        return Mathf.Atan2(value.x, value.z) * Mathf.Rad2Deg; //���W�A�����I�C���[
    }

    //���������߂� �����W�A��
    float getRadian(Vector3 current, Vector3 target)
    {
        Vector3 value = target - current;
        return Mathf.Atan2(value.x, value.z);
    }

    //------------------------------------------------------------------------------------------------------------------
    //�A�b�v�f�[�g
    //------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        if (playerController.isEnd == false)
        {
            //�^�C�}�[���Z
            timeCounter += Time.deltaTime;
            //���������߂�
            float direction = getEulerAngle(this.transform.position, player.transform.position);
            //���������߂�
            float length = getLength(this.transform.position, player.transform.position);


            //**************************************************************************************************************
            //���������ԏ���
            //**************************************************************************************************************
            int randomNav = Random.Range(0, number);
            //int stepPattern = Random.Range(1, 3);
            //int enemyPattern = Random.Range(4, 9);

            //�ڕW�n�_�܂ōs�����H ���u���O�ł悭�����������
            //if (GetComponent<NavMeshAgent>().remainingDistance == 0.0f)
            //�҂�����0.0f�ɂȂ�Ȃ����Ƃ�����̂ŁA0.5f�ȉ����ǂ�����


            //�ҋ@
            if (stateNumber == 0)
            {
                if (length > enemyLength && nav.remainingDistance < 0.25f)
                {
                    //���̖ڕW�n�_ �������`�F�b�N
                    //if (number > 0)
                    //{
                    //myanimator.SetInteger("speed", 1);
                    //���̖ڕW�n�_��
                    nav.destination = navPoints.transform.GetChild(randomNav).transform.position;
                    //}
                }

                if(Mathf.Abs(Mathf.DeltaAngle(direction,transform.eulerAngles.y))<30f)
                {
                    //�v���[���[���߂���
                    if (length < enemyLength)
                    {
                        //�^�C�}�[���Z�b�g
                        timeCounter = 0f;

                        //�A�j���[�V�����@�U��
                        nav.isStopped = true;

                        myanimator.SetTrigger("attack");
                        //myanimator.SetInteger("speed", 0);

                        //Debug.Log("�U��: " + Time.time);

                        //��Ԃ̑J�ځi�؂�ւ��X�e�[�g�j
                        stateNumber = 2;
                    }
                }
                
            }

            if (stateNumber == 1)
            {
                //�v���C���[�����ꂽ�H
                if (length > enemyLength)
                {
                    nav.isStopped = false;
                    myanimator.SetInteger("speed", 1);
                    //���̖ڕW�n�_��
                    nav.destination = navPoints.transform.GetChild(randomNav).transform.position;
                    stateNumber = 0;

                }

                if (Mathf.Abs(Mathf.DeltaAngle(direction, transform.eulerAngles.y)) < 30f)
                {
                    //�v���[���[���߂���
                    if (length < enemyLength)
                    {
                        //�^�C�}�[���Z�b�g
                        timeCounter = 0f;

                        //�A�j���[�V�����@�U��
                        nav.isStopped = true;

                        myanimator.SetTrigger("attack");
                        //myanimator.SetInteger("speed", 0);

                        //Debug.Log("�U��: " + Time.time);

                        //��Ԃ̑J�ځi�؂�ւ��X�e�[�g�j
                        stateNumber = 2;
                    }
                }                    
            }

            //���[�V�����؂�ւ�
            else if (stateNumber == 2)
            {
                //myanimator.SetTrigger("attack");
                myanimator.SetInteger("speed", 0);

                //���[�V�����I���
                if (timeCounter > 1.0f)
                {
                    //�^�C�}�[���Z�b�g
                    timeCounter = 0f;
                    //��Ԃ̑J�ځi�ҋ@�j
                    stateNumber = 1;
                }
            }
        }

        //**************************************************************************************************************
        //�Q�[���]�I�[�o�[�Ď��i�ǉ��j
        //**************************************************************************************************************

        if (playerController.isEnd)
        {
            nav.isStopped = true;
            //�A�j���[�V�����@�ҋ@
            this.myanimator.SetInteger("speed", 0);
            //�X�e�[�g�p�^�[�����~
            stateNumber = -1;
            //myRigidbody.velocity = Vector3.zero;
            //myRigidbody.isKinematic = true;
            //���R�������~
            myRigidbody.useGravity = false;
            //�Փ˂��Ȃ���
            GetComponent<CapsuleCollider>().enabled = false;
        }        
    }

    void attackEvent()
    {
        GameObject attack=Instantiate(attackPrefab, attackPoint.transform.position, Quaternion.identity);
        Destroy(attack.gameObject, 0.35f);
        Instantiate(effectPrefab, attackPoint.transform.position, Quaternion.identity);
    }
}

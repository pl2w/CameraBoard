using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CameraBoard
{
    public class CameraBoard : MonoBehaviourPunCallbacks
    {
        public GameObject[] players;

        public override void OnEnable()
        {
            base.OnEnable();
            players = new GameObject[transform.GetChild(0).childCount];
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                players[i] = transform.GetChild(0).GetChild(i).gameObject;
            }

            StartCoroutine(RefreshCoro());
        }

        IEnumerator RefreshCoro()
        {
            for(; ; )
            {
                RefreshBoard();
                yield return new WaitForSeconds(2.5f);
            }
        }

        void RefreshBoard()
        {
            if (!PhotonNetwork.InRoom)
            {
                foreach (GameObject p in players)
                {
                    p.SetActive(false);
                }
                return;
            }

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if(PhotonNetwork.PlayerList[i] != null)
                {
                    players[i].SetActive(true);
                    players[i].GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName.ToUpper();
                    VRRig rig = GorillaGameManager.instance.FindPlayerVRRig(PhotonNetwork.PlayerList[i]);
                    players[i].transform.GetChild(0).gameObject.GetComponent<RawImage>().texture = rig.mainSkin.material.mainTexture;
                    players[i].transform.GetChild(0).gameObject.GetComponent<RawImage>().color = rig.mainSkin.material.color;
                } else
                {
                    players[i].SetActive(false);
                }
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            RefreshBoard();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            RefreshBoard();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            RefreshBoard();
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            RefreshBoard();
        }
    }
}

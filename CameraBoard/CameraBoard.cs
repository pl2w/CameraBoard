using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
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
            foreach (GameObject p in players)
            {
                p.SetActive(false);
            }

            if (!PhotonNetwork.InRoom)
            {
                return;
            }

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if(PhotonNetwork.PlayerList[i] != null)
                {
                    players[i].SetActive(true);
                    players[i].GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName.ToUpper();
                    players[i].GetComponent<Text>().text.font = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/NameTagAnchor/NameTagCanvas/Text/")?.GetComponent<Text>().font;
                    VRRig rig = GorillaGameManager.instance.FindPlayerVRRig(PhotonNetwork.PlayerList[i]);
                    players[i].transform.GetChild(0).gameObject.GetComponent<RawImage>().texture = rig.mainSkin.material.mainTexture;
                    players[i].transform.GetChild(0).gameObject.GetComponent<RawImage>().color = rig.mainSkin.material.color;

                    foreach (GorillaPlayerScoreboardLine line in GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>())
                    {
                        if(line.linePlayer == PhotonNetwork.PlayerList[i])
                        {
                            players[i].transform.GetChild(1).gameObject.SetActive(line.speakerIcon.activeInHierarchy);
                        }
                    }
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

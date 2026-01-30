using UnityEngine;
using TMPro;

public class RoomUI : MonoBehaviour
{
    public TMP_Text roomText;

    void Start()
    {
        roomText.text = RoomData.roomName;
    }
}

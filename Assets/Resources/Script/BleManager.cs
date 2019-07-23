///////////////////////////////////////////////////////
// (c) 2019 Darwin Boomerangs                         /
// Smart -Boomerang Project                           /
// In partnership with :                              /
//   - University of St Etienne, France               /
//   - Aoyama Gakuin University / Lopez lab, Japan    /
///////////////////////////////////////////////////////

///////////////////////////////////////////////////////
// Filename    BleManager.cs                          /
// This file is intended to controll BLE connection   /
// and send a message to a specified BLE device and   /
// recieve a message from a specified BLE device      /
///////////////////////////////////////////////////////
// v1.0  Date July 23th, 2019     Author Takumi Kondo /
// Modifications from previous version...             /
// ...                                                /
///////////////////////////////////////////////////////


using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BleManager : MonoBehaviour
{
    public GameObject NotifyButton;
    public GameObject ReadButton;
    public GameObject WriteButton;
    public GameObject Text;
    public InputField message;

    public string sendMessage = null;
    bool isNotify = false;

    AndroidJavaObject BLE;

    // Start is called before the first frame update
    void Start()
    {
        Text = GameObject.Find("debugText");
        message = GameObject.Find("message").GetComponent<InputField>();
        //connect BLE
        BLE = new AndroidJavaObject("fr.boomerang.takumi.ble_module.BLE", this.gameObject.name, "received");
        Text.GetComponent<Text>().text = "Create BLE  AndroidJavaObject";
    }
    void received(string message)
    {
        Text.GetComponent<Text>().text = message;
    }
    private void OnApplicationPause(bool pause)
    {
        if (BLE == null)
        {
            return;
        }
        if (pause)
        {
            BLE.Call("onPause");
        }
        else
        {
            BLE.Call("onActive");
        }
    }

    public void clickButton(int num)
    {
        switch (num)
        {
            case 0:
                //push Notify
                if (!isNotify)
                {
                    BLE.Call("setCharacteristicNotification");
                    isNotify = true;
                    break;
                }
                break;
            case 1:
                //push Read
                if (isNotify) { isNotify = false; }
                Debug.Log("BLE:read");
                BLE.Call("readCharacteristic");
                break;
            //
            case 2:
                //push Write
                if (isNotify) { isNotify = false; }
                Debug.Log("BLE write");
                BLE.Call("writeCharacteristic", sendMessage);
                break;
            default:
                break;

        }
    }

    public void InputText()
    {
        sendMessage = message.text;
    }
}

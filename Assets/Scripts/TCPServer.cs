using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
    #region private members 	
    /// <summary> 	
    /// TCPListener to listen for incomming TCP connection 	
    /// requests. 	
    /// </summary> 	
    private TcpListener tcpListener;
    /// <summary> 
    /// Background thread for TcpServer workload. 	
    /// </summary> 	
    private Thread tcpListenerThread;
    /// <summary> 	
    /// Create handle to connected tcp client. 	
    /// </summary> 	
    private TcpClient connectedTcpClient;
    #endregion

    public float h_input = 0f;
    public float v_input = 0f;
    public float elevate_input = 0f;
    public float base_input = 0f;

    private string clientMessage = "0";

    private Vector2 smoothedInput;

    private bool getTimeOnce = true;


    //check if there is a connection and then send the client message
    public string getClientMessage()
    {
        if (connectedTcpClient == null)
            return "null";

        return clientMessage;
    }

    public void setClientMessage(string message)
    {
        if (connectedTcpClient == null)
            return;

        clientMessage = message;
    }

    public void ReadInputOnlyOnce(char input_a, char input_b, float currentTime, float previousTime, float deltaTime)
    {
        currentTime = Time.time;
        if ((getClientMessage()[0] == input_a || getClientMessage()[0] == input_b))
        {
            if (getTimeOnce)
            {
                previousTime = currentTime;
                getTimeOnce = false;
            }
            if (currentTime - previousTime > deltaTime)
            {
                setClientMessage("0");
                v_input = 0f;
                h_input = 0f;
                elevate_input = 0f;
                base_input = 0f;
                previousTime = currentTime;
                getTimeOnce = true;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        // Start TcpServer background thread 		
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendMessage("space pressed");
        }
    }

    private float slidingH;
    private float slidingV;

    private void FixedUpdate()
    {     
        if (getClientMessage()[0] == 'w')
        {
            v_input = 1f;
        }
        else if (getClientMessage()[0] == 's')
        {
            v_input = -1f;
        }
        else if (getClientMessage()[0] == 'a')
        {
            h_input = -1f;
        }
        else if (getClientMessage()[0] == 'd')
        {
            h_input = 1f;
        }
        else if (getClientMessage()[0] == 'j')
        {
            base_input = -1f;
        }
        else if (getClientMessage()[0] == 'l')
        {
            base_input = 1f;
        }
        else if (getClientMessage()[0] == 'i')
        {
            elevate_input = -1f;
        }
        else if (getClientMessage()[0] == 'k')
        {
            elevate_input = 1f;
        }
    }

   

    /// <summary> 	
    /// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
    /// </summary> 	
    private void ListenForIncommingRequests()
    {
        try
        {
            // Create listener on localhost port 8052. 			
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8052);
            tcpListener.Start();
            Debug.Log("Server is listening");
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    // Get a stream object for reading 					
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {
                        int length;
                        // Read incomming stream into byte arrary. 						
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            // Convert byte array to string message. 							
                            clientMessage = Encoding.ASCII.GetString(incommingData);
                            SendMessage(clientMessage);
                            Debug.Log("client message received as: " + clientMessage);
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }
    /// <summary> 	
    /// Send message to client using socket connection. 	
    /// </summary> 	
    private void SendMessage(string messageSent)
    {
        if (connectedTcpClient == null)
        {
            return;
        }

        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = connectedTcpClient.GetStream();
            if (stream.CanWrite)
            {
                string serverMessage = messageSent;
                // Convert string message to byte array.                 
                byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
                // Write byte array to socketConnection stream.               
                stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                Debug.Log("Server sent:" + messageSent);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
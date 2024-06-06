using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomsPlacer : MonoBehaviour
{

    public Room[] roomPrefabs; // Массив заготовленных комнат
    public Room startRoom; // Префаб начальной комнаты
    public Room endRoom;

    public int roomCount = 5;

    private List<Room> generatedRooms = new List<Room>();

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        Debug.Log("Generating start room...");
        Room currentRoom = Instantiate(startRoom);
        currentRoom.transform.position = new Vector3(0, 0, 0);
        currentRoom.DoorD.SetActive(false);
        generatedRooms.Add(currentRoom);
        Debug.Log("Start room generated.");

        int c = 0;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Transform spawnPoint = currentRoom.transform.Find("PlayerSpawnPoint");
        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
            Debug.Log("Player spawned at position: " + spawnPoint.position);
        }
        else
        {
            Debug.LogError("Player spawn point not found in the start room!");
        }

        for (int i = 0; i < roomCount; i++)
        {
            Debug.Log("Generating room " + (i + 1) + "...");
            // Get a random room prefab
            Room newRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)]);
            generatedRooms.Add(newRoom);

            string doorName = OpenRandomDoor(newRoom, currentRoom);

            ConnectRooms(newRoom, doorName, currentRoom);

            Debug.Log("Room " + (i + 1) + " generated.");

            currentRoom = newRoom;
        }

        // Connect the last room to the final room
        Room finalRoom = Instantiate(endRoom);
        generatedRooms.Add(finalRoom);
        ConnectFinalRoom(currentRoom, finalRoom);

    }

    string OpenRandomDoor(Room room, Room currentRoom)
    {
        GameObject doorA;
        if (currentRoom.DoorD.activeSelf == false)
        {
            doorA = room.DoorU;
        }
        else if (currentRoom.DoorL.activeSelf == false)
        {
            doorA = room.DoorR;
        }
        else if (currentRoom.DoorR.activeSelf == false)
        {
            doorA = room.DoorL;
        }
        else doorA = room.DoorD;


        GameObject[] doors = { room.DoorU, room.DoorL, room.DoorD, room.DoorR };

        List<GameObject> activeDoors = new List<GameObject>();

        // Фильтрация активных дверей
        foreach (GameObject door in doors)
        {
            if (door.activeSelf && door!=doorA)
            {
                activeDoors.Add(door);
            }
        }

        if (activeDoors.Count > 0)
        {
            int randomIndex = Random.Range(0, activeDoors.Count);
            GameObject chosenDoor = activeDoors[randomIndex];
            chosenDoor.SetActive(false);
            Debug.Log("Door " + chosenDoor.name + " opened.");
            return chosenDoor.name;
        }
        else
        {
            Debug.LogWarning("No active doors found in the room!");
            return null; // Возвращаем null, если нет активных дверей
        }
    }


    void ConnectRooms(Room newRoom, string doorToConnect, Room currentRoom)
    {
        string doorName;
        if (currentRoom.DoorD.activeSelf == false)
        {
            Debug.Log(1);
            doorName = "DoorD";
        }
        else if (currentRoom.DoorL.activeSelf == false)
        {
            Debug.Log(2);

            doorName = "DoorL";
        }
        else if (currentRoom.DoorR.activeSelf == false)
        {
            Debug.Log(3);

            doorName = "DoorR";
        }
        else
        {
            Debug.Log(4);

            doorName = "DoorU";
        }
        Debug.Log(doorName);

        switch (doorName)
        {
            case "DoorR":
                Debug.Log(currentRoom.DoorR.transform.position);
                Vector3 newRoomRPosition = currentRoom.DoorR.transform.position - newRoom.DoorL.transform.localPosition;
                newRoom.transform.position = newRoomRPosition;
                newRoom.DoorL.SetActive(false);
                break;
            case "DoorU":
                Debug.Log(currentRoom.DoorU.transform.position);
                Vector3 newRoomUPosition = currentRoom.DoorU.transform.position - newRoom.DoorD.transform.localPosition;
                newRoom.transform.position = newRoomUPosition;
                newRoom.DoorU.SetActive(false);
                break;
            case "DoorD":
                Debug.Log(currentRoom.DoorD.transform.position);
                Vector3 newRoomDPosition = currentRoom.DoorD.transform.position - newRoom.DoorU.transform.localPosition;
                newRoom.transform.position = newRoomDPosition;
                newRoom.DoorU.SetActive(false);
                break;
            case "DoorL":
                Debug.Log(currentRoom.DoorL.transform.position);
                Vector3 newRoomLPosition = currentRoom.DoorL.transform.position - newRoom.DoorR.transform.localPosition;
                newRoom.transform.position = newRoomLPosition;
                newRoom.DoorL.SetActive(false);
                break;
        }

        /*        Vector3 newRoomPosition = currentRoom.DoorD.transform.position - newRoom.DoorU.transform.localPosition;
                newRoom.transform.position = newRoomPosition;

                currentRoom.DoorD.SetActive(false);
                newRoom.DoorU.SetActive(false);*/
    }

    void ConnectFinalRoom(Room currentRoom, Room endRoom)
    {
        Vector3 endRoomPosition = currentRoom.DoorD.transform.position - endRoom.DoorU.transform.localPosition;
        endRoom.transform.position = endRoomPosition;

        currentRoom.DoorD.SetActive(false);
        endRoom.DoorU.SetActive(false);
    }

}

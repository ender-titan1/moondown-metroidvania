/*
    Copyright (C) 2021 Moondown Project

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using Moondown.Utility;
using Moondown.Utility.Hierarchy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moondown
{
    public class GameManager : MonoBehaviour
    {

        #region JSON
        [SerializeField]
        private TextAsset asset;

        private JSONRooms roomsJSON = new JSONRooms();

        [System.Serializable]
        class JSONRoom
        {
            public string name;
        }

        [System.Serializable]
        class JSONRooms
        {
            public JSONRoom[] rooms;
        }

        #endregion

        private Room[] rooms;

        private void Awake()
        {
            roomsJSON = JsonUtility.FromJson<JSONRooms>(asset.text);

            List<Room> roomList = new List<Room>();
            foreach (JSONRoom room in roomsJSON.rooms)
            {
                roomList.Add(new Room(null, room.name));
            }

            rooms = roomList.ToArray();
            Debug.Log(rooms.Display());
        }
    }
}
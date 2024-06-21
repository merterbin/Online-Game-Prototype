package utils

import (
	"encoding/json"
	"fmt"
	"log"
	"online-game/bgs/models"

	"github.com/gofiber/contrib/websocket"
)

func WsLogin(c *websocket.Conn, mt int, dat map[string]string, Rooms *map[int]*models.Room) {
	var room map[string]interface{}
	if err := json.Unmarshal([]byte(dat["Data"]), &room); err != nil {
		panic(err)
	}

	log.Printf("room: %v", room)

	if CheckRoom(int(room["ID"].(float64)), *Rooms) {
		var isAdded = AddPlayer(int(room["ID"].(float64)), c, Rooms)
		if isAdded != -1 {
			log.Printf("player%d join %d room", isAdded+1, int(room["ID"].(float64)))
			res := &models.Response{
				Type:   "login",
				Status: "success",
				Player: fmt.Sprintf("player%d", isAdded+1),
				Data:   "You have successfully joined the room"}

			resJson, _ := json.Marshal(res)
			c.WriteMessage(mt, resJson)

		} else {
			log.Println("Room is full")
			res := &models.Response{
				Type:   "login",
				Status: "error",
				Player: "",
				Data:   "Room is full"}

			resJson, _ := json.Marshal(res)
			c.WriteMessage(mt, resJson)
		}
	} else {
		log.Println("Room not found")
		res := &models.Response{
			Type:   "login",
			Status: "error",
			Player: "",
			Data:   "Room not found"}
		resJson, _ := json.Marshal(res)
		c.WriteMessage(mt, resJson)
	}
}

func WsExit(c *websocket.Conn, mt int, dat map[string]string, Rooms map[int]*models.Room) {
	var room map[string]interface{}
	if err := json.Unmarshal([]byte(dat["Data"]), &room); err != nil {
		panic(err)
	}
	RemovePlayer(int(room["ID"].(float64)), c, Rooms)
}

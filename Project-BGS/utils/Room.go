package utils

import (
	"encoding/json"
	"online-game/bgs/models"

	"github.com/gofiber/contrib/websocket"
)

func CheckRoom(id int, Rooms map[int]*models.Room) bool {
	for _, v := range Rooms {
		if v.ID == id {
			return true
		}
	}
	return false
}

func AddPlayer(id int, p *websocket.Conn, Rooms map[int]*models.Room) int {
	for _, v := range Rooms {
		if v.ID == id {
			if v.Player1.Conn == nil {
				v.Player1.Conn = p.Conn
				return 0
			} else {
				v.Player2.Conn = p.Conn
				res := &models.Response{
					Type:   "login",
					Status: "success",
					Player: "player2",
					Data:   "You have successfully joined the room"}
				resJson, _ := json.Marshal(res)
				v.Player1.Conn.WriteMessage(1, resJson)
				return 1
			}
		}
	}

	return -1
}

func RemovePlayer(id int, p *websocket.Conn, Rooms map[int]*models.Room) {
	for _, v := range Rooms {
		if v.ID == id {
			if v.Player1.Conn == p.Conn {
				v.Player1.Conn = nil
			} else {
				v.Player2.Conn = nil
			}
		}
	}
}

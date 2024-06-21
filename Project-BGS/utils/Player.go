package utils

import (
	"encoding/json"
	"online-game/bgs/models"

	"github.com/gofiber/contrib/websocket"
)

func MovePlayer(c *websocket.Conn, mt int, dat map[string]string, Rooms *map[int]*models.Room) {
	var room map[string]interface{}
	if err := json.Unmarshal([]byte(dat["Data"]), &room); err != nil {
		panic(err)
	}

	var roomID int = int(room["RoomID"].(float64))
	if CheckRoom(roomID, *Rooms) {
		for _, v := range *Rooms {
			if v.ID == roomID {
				if v.Player1.Conn == c.Conn {
					move := &models.Movement{
						RoomID:    roomID,
						Type:      "walk",
						Player:    "player1",
						Direction: room["Direction"].(string),
					}
					moveJson, _ := json.Marshal(move)
					res := &models.Response{
						Type:   "move",
						Status: "success",
						Player: "player2",
						Data:   string(moveJson),
					}
					resJson, _ := json.Marshal(res)
					v.Player1.Conn.WriteMessage(mt, resJson)

					if v.Player2.Conn != nil {
						v.Player2.Conn.WriteMessage(mt, resJson)
					}

				} else if v.Player2.Conn == c.Conn {

					move := &models.Movement{
						RoomID:    roomID,
						Type:      "walk",
						Player:    "player2",
						Direction: room["Direction"].(string),
					}
					moveJson, _ := json.Marshal(move)
					res := &models.Response{
						Type:   "move",
						Status: "success",
						Player: "player2",
						Data:   string(moveJson),
					}
					resJson, _ := json.Marshal(res)
					v.Player2.Conn.WriteMessage(mt, resJson)

					if v.Player1.Conn != nil {
						v.Player1.Conn.WriteMessage(mt, resJson)
					}
				}
			}
		}
	}

}

package controllers

import (
	"encoding/json"
	"online-game/bgs/utils"

	"github.com/gofiber/contrib/websocket"
)

func WebSocketHandler(c *websocket.Conn) {
	var (
		mt  int
		msg []byte
		err error
	)
	for {
		if mt, msg, err = c.ReadMessage(); err != nil {
			// log.Println("read:", err)
			break
		}
		var dat map[string]string

		if err := json.Unmarshal(msg, &dat); err != nil {
			panic(err)
		}

		// log.Printf("type: %s, data: %s", dat["Type"], dat["Data"])

		if dat["Type"] == "login" {
			utils.WsLogin(c, mt, dat, &Rooms)
		} else if dat["Type"] == "exit" { // deleted
			utils.WsExit(c, mt, dat, Rooms)
		} else if dat["Type"] == "move" {
			utils.MovePlayer(c, mt, dat, &Rooms)
		} else if dat["Type"] == "moved" {
			utils.MovedPlayer(c, mt, dat, &Rooms)
		}
	}

}

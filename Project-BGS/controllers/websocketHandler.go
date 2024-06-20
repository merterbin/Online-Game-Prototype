package controllers

import (
	"encoding/json"
	"log"
	"online-game/bgs/utils"

	"github.com/gofiber/contrib/websocket"
)

func WebSocketHandler(c *websocket.Conn) {
	// c.Locals is added to the *websocket.Conn
	log.Println(c.Locals("allowed"))  // true
	log.Println(c.Params("id"))       // 123
	log.Println(c.Query("v"))         // 1.0
	log.Println(c.Cookies("session")) // ""

	var (
		mt  int
		msg []byte
		err error
	)
	for {
		if mt, msg, err = c.ReadMessage(); err != nil {
			log.Println("read:", err)
			break
		}
		var dat map[string]string

		if err := json.Unmarshal(msg, &dat); err != nil {
			panic(err)
		}

		log.Printf("type: %s, data: %s", dat["Type"], dat["Data"])

		if dat["Type"] == "login" {
			utils.WsLogin(c, mt, dat, Rooms)
		} else if dat["Type"] == "exit" {
			utils.WsExit(c, mt, dat, Rooms)
		}
	}

}

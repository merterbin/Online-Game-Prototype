package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	s "strings"

	"github.com/gorilla/websocket"
)

var upgrader = websocket.Upgrader{
	ReadBufferSize:  1024,
	WriteBufferSize: 1024,
}

var position = make(map[string]float64)

func main() {

	http.HandleFunc("/ws", wsHandler)
	log.Fatal(http.ListenAndServe(":8080", nil))
}

func wsHandler(w http.ResponseWriter, r *http.Request) {
	conn, err := upgrader.Upgrade(w, r, nil)
	if err != nil {
		log.Println(err)
		return
	}
	fmt.Println("Client connected")
	defer conn.Close()

	for {
		messageType, message, err := conn.ReadMessage()
		if err != nil {
			log.Println(err)
			return
		}

		if messageType == websocket.BinaryMessage {
			fmt.Println("Received binary message:", message)
		} else if messageType == websocket.TextMessage {
			fmt.Println("Received text message:", string(message))

			msg := string(message)

			if s.ToLower(msg) == "ping" {
				err = conn.WriteMessage(websocket.TextMessage, []byte("Pong"))
				if err != nil {
					log.Println(err)
					return
				}
			} else {
				var dat map[string]interface{}
				if err := json.Unmarshal(message, &dat); err != nil {
					panic(err)
				}

				currentX, ok := position["x"]

				if ok {
					if currentX == dat["x"] {
						fmt.Println("Sensin")
					} else {
						err = conn.WriteJSON(dat)
						if err != nil {
							log.Println(err)
							return
						}
					}
				} else {
					fmt.Println("SA")
					position["x"] = dat["x"].(float64)
				}

			}

		}

	}
}

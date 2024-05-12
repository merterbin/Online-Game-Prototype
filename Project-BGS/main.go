package main

import (
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
		}

		msg := string(message)

		if s.ToLower(msg) == "ping" {
			err = conn.WriteMessage(websocket.TextMessage, []byte("Pong"))
			if err != nil {
				log.Println(err)
				return
			}
		} else {
			err = conn.WriteMessage(messageType, message)
			if err != nil {
				log.Println(err)
				return
			} else {
				log.Println("Send")
			}
		}

	}
}

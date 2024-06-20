package models

import (
	"github.com/gofiber/contrib/websocket"
)

type Room struct {
	ID      int
	Player1 websocket.Conn
	Player2 websocket.Conn
}

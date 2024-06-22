package models

import (
	"github.com/gofiber/contrib/websocket"
)

type Room struct {
	ID              int
	Player1         websocket.Conn
	Player1Position [3]float64
	Player2         websocket.Conn
	Player2Position [3]float64
}

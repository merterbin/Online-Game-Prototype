package main

import (
	"log"
	"online-game/bgs/controllers"

	"github.com/gofiber/contrib/websocket"
	"github.com/gofiber/fiber/v2"
)

func main() {
	port := ":8080"
	app := fiber.New()

	app.Post("/room", controllers.Room)
	app.Post("/room/exit/:id", controllers.GetRoom)
	app.Post("/room/find", controllers.FindRoom)

	app.Get("/ws", websocket.New(controllers.WebSocketHandler))

	log.Fatal(app.Listen(port))
}

package controllers

import (
	"fmt"
	"online-game/bgs/models"
	"strconv"

	"github.com/gofiber/fiber/v2"
)

var Rooms = make(map[int]*models.Room)

func Room(c *fiber.Ctx) error {
	room := new(models.Room)
	if err := c.BodyParser(room); err != nil {
		return c.Status(400).JSON(err.Error())
	}
	return c.Status(200).JSON(room)
}

func FindRoom(c *fiber.Ctx) error {
	fmt.Println(len(Rooms))
	if len(Rooms) == 0 {
		r1 := new(models.Room)
		r1.ID = 1
		Rooms[0] = r1
		return c.Status(200).JSON(r1)
	} else {
		for _, v := range Rooms {
			if v.Player2.Conn == nil {
				return c.Status(200).JSON(v)
			} else {
				r := new(models.Room)
				r.ID = len(Rooms) + 1
				Rooms[len(Rooms)] = r
				return c.Status(200).JSON(r)
			}
		}
		return c.Status(400).JSON("No room found")
	}
}

func GetRoom(c *fiber.Ctx) error {
	id := c.Params("id")
	for _, v := range Rooms {
		IDc, err := strconv.Atoi(id)
		if err != nil {
			fmt.Println("Error during conversion")
			return c.Status(400).JSON("No room found")
		}

		if v.ID == IDc {
			return c.Status(200).JSON(v)
		}
	}
	return c.Status(400).JSON("No room found")
}

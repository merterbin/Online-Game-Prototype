package models

type Movement struct {
	RoomID    int    `json:"RoomID"`
	Player    string `json:"Player"`
	Type      string `json:"Type" `
	Direction string `json:"Direction"`
}

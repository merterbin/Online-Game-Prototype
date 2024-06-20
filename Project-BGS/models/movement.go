package models

type Movement struct {
	Player    string `json:"player" form:"player" query:"player"`
	Type      string `json:"type" form:"type" query:"type"`
	Direction string `json:"direction" form:"direction" query:"direction"`
}

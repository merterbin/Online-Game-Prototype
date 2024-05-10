package main

import (
	"fmt"
	"log"
	"net/http"
)

func handler(w http.ResponseWriter, r *http.Request) {
	fmt.Fprintf(w, "BGS")
}

func main() {
	port := 8080
	log.Printf("Server started on: http://localhost:%d", port)

	server := &http.Server{
		Addr:    fmt.Sprintf(":%d", port),
		Handler: http.HandlerFunc(handler),
	}

	server.ListenAndServe()

	log.Println("Server stopped")
}

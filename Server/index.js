import Koa from "koa"
import socketio from "socket.io"

let playerCount = 0

// const app = new Koa()
const io = socketio(process.env.PORT || 3000)

io.on('connect', socket => {
  console.log("connect")

  playerCount++

  for(let i=0; i < playerCount; i++)
  {
    socket.emit('spawn')
    console.log('sending spawn to new player')
  }

  socket.broadcast.emit('spawn')

  socket.on("move", () => {
    console.log("move")
  })

  socket.on('disconnect', () => {
    console.log("player disconnect")
    playerCount--
  })
})
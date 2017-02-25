import Koa from "koa"
import socketio from "socket.io"
import shortid from "shortid"

let players = new Map()
// const app = new Koa()
const io = socketio(process.env.PORT || 3000)

io.on('connect', socket => {
  const thisPlayerId = shortid.generate()

  const player = {
    id: thisPlayerId,
    x: 0,
    z: 0
  }

  players.set(thisPlayerId, player)

  socket.broadcast.emit('spawn', { id: thisPlayerId })
  socket.broadcast.emit('requestPosition')

  for (let playerId of players.keys())
  {
    console.log(playerId)
    if(playerId !== thisPlayerId){
      socket.emit('spawn', players.get(playerId))
    }
  }

  console.log(`${thisPlayerId} connect`)

  socket.on("move", (data) => {
    data.id = thisPlayerId
    player.x = data.x
    player.z = data.z

    socket.broadcast.emit('move', data)
    console.log(`move ${JSON.stringify(data)}`)
  })

  socket.on("updatePosition", (data) => {
    data.id = thisPlayerId

    socket.broadcast.emit("updatePosition", data)
  })

  socket.on('disconnect', () => {
    console.log(`player disconnect: ${thisPlayerId}`)

    players.delete(thisPlayerId)

    socket.broadcast.emit('disconnected', {id: thisPlayerId})
  })
})
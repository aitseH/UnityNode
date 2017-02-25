"use strict";

var _socket = require("socket.io");

var _socket2 = _interopRequireDefault(_socket);

var _shortid = require("shortid");

var _shortid2 = _interopRequireDefault(_shortid);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var players = new Map();
var io = (0, _socket2.default)(process.env.PORT || 3000);

io.on('connect', function (socket) {
  var thisPlayerId = _shortid2.default.generate();

  var player = {
    id: thisPlayerId,
    x: 0,
    z: 0
  };

  players.set(thisPlayerId, player);

  socket.broadcast.emit('spawn', { id: thisPlayerId });
  socket.broadcast.emit('requestPosition');

  var _iteratorNormalCompletion = true;
  var _didIteratorError = false;
  var _iteratorError = undefined;

  try {
    for (var _iterator = players.keys()[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
      var playerId = _step.value;

      console.log(playerId);
      if (playerId !== thisPlayerId) {
        socket.emit('spawn', players.get(playerId));
      }
    }
  } catch (err) {
    _didIteratorError = true;
    _iteratorError = err;
  } finally {
    try {
      if (!_iteratorNormalCompletion && _iterator.return) {
        _iterator.return();
      }
    } finally {
      if (_didIteratorError) {
        throw _iteratorError;
      }
    }
  }

  console.log(thisPlayerId + " connect");

  socket.on("move", function (data) {
    data.id = thisPlayerId;
    player.x = data.x;
    player.z = data.z;

    socket.broadcast.emit('move', data);
    console.log("move " + JSON.stringify(data));
  });

  socket.on("updatePosition", function (data) {
    data.id = thisPlayerId;

    socket.broadcast.emit("updatePosition", data);
  });

  socket.on('disconnect', function () {
    console.log("player disconnect: " + thisPlayerId);

    players.delete(thisPlayerId);

    socket.broadcast.emit('disconnected', { id: thisPlayerId });
  });
});

# tic-tac-toe-api
ASP.NET (C#) Tic Tac Toe-playing web API. Provides a single stateless endpoint (/api/executemove) which plays a move of Tic Tac Toe given a gameboard state.

Also includes an auto-gen SDK and some functional tests using the SDK.

Completed as part of CSCI-E94 at Harvard University. (Assignment 1)

## Usage
Example usage of this API (note: no longer deployed to azure, requests will not work):

### Plays moves
X's turn (inferred from gameboard - API plays X unless O has more locations on the board)
```
X ? ?
? O ?
O ? X
```
(It should block O's threat by placing in the top right position, i.e. position 2)

```
curl -X POST \
  https://tictactoe20190214125437.azurewebsites.net/api/executemove \
  -H 'cache-control: no-cache' \
  -H 'content-type: application/json' \
  -d '{
	"gameBoard": [
        "X",
        "?",
        "?",
        "?",
        "O",
        "?",
        "O",
        "?",
        "X"
    ]
}'
```

Response:
```
{
  "move": 2,
  "azurePlayerSymbol": "X",
  "winner": "inconclusive",
  "winPositions": null,
  "gameBoard": [
    "X",
    "?",
    "X",
    "?",
    "O",
    "?",
    "O",
    "?",
    "X"
  ]
}
```

Which translates to:
```
X ? X
? O ?
O ? X
```
X also has two threats (positions 1 and 5 will cause X to win). O can't block both, so victory is all but assured for X.

### Checks for win conditions
This board has X as the winner:
```
X X X
O O ?
? ? ?
```

Request:
```
curl -X POST \
  https://tictactoe20190214125437.azurewebsites.net/api/executemove \
  -H 'cache-control: no-cache' \
  -H 'content-type: application/json' \
  -d '{
	"gameBoard": [
        "X",
        "X",
        "X",
        "O",
        "O",
        "?",
        "?",
        "?",
        "?"
    ]
}'
```

Response:
```
{
  "move": null,
  "azurePlayerSymbol": "O",
  "winner": "X",
  "winPositions": [
    0,
    1,
    2
  ],
  "gameBoard": [
    "X",
    "X",
    "X",
    "O",
    "O",
    "?",
    "?",
    "?",
    "?"
  ]
}
```
This API detects that X won using the top row (0, 1, 2), so it doesn't play and merely declares "X" to be the winner.

Worth noting that the API assumes it is playing as O, since there are more X's than O's, meaning that it is O's turn.

### Invalid game board
It is impossible for a game board like the one below to occur:
```
X X ?
? ? ?
? ? ?
```
(Why? Because there are 2 X's and no O's! That means O's turn must have been skipped once. That's cheating.)

This API handles invalid cases like this one, among some trivial invalid data cases.

Sample bad request:
```
curl -X POST \
  https://tictactoe20190214125437.azurewebsites.net/api/executemove \
  -H 'cache-control: no-cache' \
  -H 'content-type: application/json' \
  -d '{
	"gameBoard": [
        "X",
        "X",
        "?",
        "?",
        "?",
        "?",
        "?",
        "?",
        "?"
    ]
}'
```

Response: 400 (Bad Request)
```
{
  "message": "Cannot skip turns. X has 2 more plays than O, indicating skipping of the latter's turn(s).",
  "code": 1
}
```

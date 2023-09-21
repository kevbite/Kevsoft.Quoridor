const canvas = document.getElementById('gridCanvas');
const ctx = canvas.getContext('2d');
const gridSize = 9;
const cellSize = Math.min(canvas.width, canvas.height) / gridSize;
const colours = {
    grid: '#000000',
    wall: '#964B00',
    players: [
        '#ff0000',
        '#00ff00',
        '#0000ff',
        '#ffff00'
    ]
}

canvas.addEventListener('mousemove', highlightSquare);
canvas.addEventListener('mouseout', clearHighlight);
canvas.addEventListener("click", onCanvasClick);

window.onGameStateChanged = (state) => {
    window.state = state;

    renderGame();
};

window.cellClicked = (position) => {
    DotNet.invokeMethodAsync('Kevsoft.Quoridor.BlazorApp', 'JsCellClicked', {
        x: position.column,
        y: 9 - position.row,
    });
};

window.wallClicked = (position) => {
    DotNet.invokeMethodAsync('Kevsoft.Quoridor.BlazorApp', 'JsWallClicked', {
        position: position.line,
        x: position.column,
        y: 9 - position.row,
    });
};

function getGamePartByPosition(x, y) {
    const column = Math.max(Math.floor(x / cellSize), 0);
    const row = Math.max(Math.floor(y / cellSize), 0);

    // Calculate which line is being hovered over
    const lineX = x - column * cellSize;
    const lineY = y - row * cellSize;

    //console.log(`column: ${column}, row: ${row}, lineX: ${lineX}, lineY: ${lineY}`)
    // Highlight the top line when not first row
    if (row > 0 && lineY < 8) {
        return {
            type: 'wall',
            column,
            row,
            line: 'horizontal'
        };
    }else if (row < 8 && lineY >= cellSize - 8){
        return {
            type: 'wall',
            column,
            row: row + 1,
            line: 'horizontal'
        };
    }else if (column > 0 && lineX < 8) {
        return {
            type: 'wall',
            column: column - 1,
            row,
            line: 'vertical'
        };
    }else if (column < 8 && lineX >= cellSize - 8){
        return {
            type: 'wall',
            column: column,
            row,
            line: 'vertical'
        };
    }
    else {
        return {
            type: 'square',
            column,
            row,
        };
    }
}

function highlightSquare(event) {
    const rect = canvas.getBoundingClientRect();
    const mouseX = event.clientX - rect.left;
    const mouseY = event.clientY - rect.top;

    ctx.reset();
    // Clear
    //ctx.clearRect(0, 0, canvas.width, canvas.height);

    // Redraw grid
    renderGame();

    const selectedGamePart = getGamePartByPosition(mouseX, mouseY);


    const {column, row} = selectedGamePart;
    
    if (selectedGamePart.type === 'wall') {
        
        ctx.strokeStyle = colours.wall;
        ctx.lineWidth = 8;
        ctx.beginPath();
        
        console.log(`column: ${column}, row: ${row}, line: ${selectedGamePart.line}`)
        if (selectedGamePart.line === 'vertical') {
            ctx.moveTo((column + 1) * cellSize, (row + 1) * cellSize);
            ctx.lineTo((column + 1) * cellSize, (row - 1) * cellSize);
        } else if (selectedGamePart.line === 'horizontal') {
            ctx.moveTo(column * cellSize, row * cellSize);
            ctx.lineTo((column + 2) * cellSize, row * cellSize);
        }
        ctx.stroke();
    } else if (selectedGamePart.type === 'square') {
        
        ctx.fillStyle = 'rgba(0, 0, 255, 0.3)';
        ctx.fillRect(column * cellSize, row * cellSize, cellSize, cellSize);
    }
}

function clearHighlight() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    renderGame();
}

function onCanvasClick(event) {
    const rect = canvas.getBoundingClientRect();
    const mouseX = event.clientX - rect.left;
    const mouseY = event.clientY - rect.top;

    const selectedGamePart = getGamePartByPosition(mouseX, mouseY);

    if (selectedGamePart.type === 'square') {
        window.cellClicked(selectedGamePart);
    }else if(selectedGamePart.type === 'wall'){
        window.wallClicked(selectedGamePart);
    }
}

function renderGame() {

    const {players, walls} = window.state;



    ctx.reset();
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    for (let column = 0; column < 9; column++) {
        for (let row = 0; row < 9; row++) {
            ctx.strokeStyle = colours.grid;
            ctx.lineWidth = 4;
            ctx.strokeRect(column * cellSize, row * cellSize, cellSize, cellSize);

            // Find the player at this position
            let player = players.find(p => p.x === column && p.y === 8 - row);

            if (player) {
                // Draw dots in the center of the square
                const rowCenter = row * cellSize + cellSize / 2;
                const columnCenter = column * cellSize + cellSize / 2;
                const dotRadius = cellSize / 4;

                ctx.fillStyle = colours.players[player.player];
                ctx.beginPath();
                ctx.arc(columnCenter, rowCenter, dotRadius, 0, Math.PI * 2);
                ctx.fill();
            }
        }
    }
    for (let column = 0; column < 9; column++) {
        for (let row = 0; row < 9; row++) {

            const wall = walls.find(p => p.x === column && p.y === 8 - row);

            if (wall) {
                ctx.strokeStyle = colours.wall;
                ctx.lineWidth = 8;
                ctx.beginPath();
                if (wall.direction === 'Vertical') {
                    ctx.moveTo((column + 1) * cellSize, (row + 1) * cellSize);
                    ctx.lineTo((column + 1) * cellSize, row * cellSize);
                } else if (wall.direction === 'Horizontal') {
                    ctx.moveTo(column * cellSize, row * cellSize);
                    ctx.lineTo((column + 1) * cellSize, row * cellSize);
                }
                ctx.stroke();
            }
        }
    }
}
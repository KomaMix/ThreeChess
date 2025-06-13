function highlightMoves(cell) {
    const piece = boardElementsState.cells[cell.id].elements.figure;

    

    cellsIds = getPossibleMoves(cell.id);
    correctIds = filterCorrectMoves(cell.id, cellsIds);

    highlightCellsArray(correctIds)

    if (correctIds.length == 0) {
        highlightStartCell(cell.id);
    }
}

function highlightCellsArray(cellIds) {
    cellIds.forEach(cellId => highlightCell(cellId))
}

function highlightStartCell(cellId) {
    const cell = boardElementsState.cells[cellId];
    if (!cell) {
        console.error(`нет клетки с id ${cellId}`)
    }
    if (cell.elements.figure) {
        cell.elements.path.classList.add('cell-selected');
    }
}

function highlightCell(targetCellId) {
    const cell = boardElementsState.cells[targetCellId];
    if (!cell) {
        console.error(`нет клетки с id ${targetCellId}`)
    }
    if (cell.elements.figure) {
        cell.elements.path.classList.add('cell-capture-highlight');
    } else {
        cell.elements.path.classList.add('cell-highlighted');
    }
}

function highlightKingRedColor(targetCellId) {
    const cell = boardElementsState.cells[targetCellId];
    cell.elements.path.classList.add('cell-king-red-highlighted');
}


function clearHighlightedCells() {
    document.querySelectorAll('path').forEach(path => {
        path.classList.remove('cell-highlighted');
        path.classList.remove('cell-capture-highlight');
        path.classList.remove('cell-selected');
        path.classList.remove('cell-king-red-highlighted');
    });
}
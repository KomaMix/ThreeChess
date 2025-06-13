function moveFigure(startId, endId) {
    console.log('Attempt to move from', startId, 'to', endId);

    const cellFrom = boardElementsState.cells[startId];
    const cellTo = boardElementsState.cells[endId];

    if (!cellFrom || !cellTo) {
        console.error('Cells not found');
        return;
    }


    // Если в исходной ячейке нет фигуры
    if (!cellFrom.elements.figure) {
        console.error('No figure in source cell');
        return;
    }

    const figure = cellFrom.elements.figure;

    // Если фигура является пешкой, проверяем, прошла ли она половину доски.
    if (figure.figureInfo.figureType === 'Pawn') {
        for (let line of movedElements.mainLines) {
            const startIndex = line.indexOf(startId);
            const endIndex = line.indexOf(endId);
            // Если обе ячейки принадлежат одной главной линии
            if (startIndex !== -1 && endIndex !== -1) {
                // Если пешка переходила между индексами 3 и 4 (в любом направлении)
                if ((startIndex === 3 && endIndex === 4) || (startIndex === 4 && endIndex === 3)) {
                    figure.hasPassedHalfBoard = true;
                    console.log('Pawn has passed half board.');
                }
                break;
            }
        }
    }

    // Логика взятия: если целевая клетка занята
    if (cellTo.elements.figure) {
        // Если фигура в целевой клетке принадлежит противнику – захватываем её
        if (cellTo.elements.figure.figureInfo.figureColor !== gameConfig.color) {
            console.log('Captured enemy figure at', endId);
            // Удаляем SVG-элемент захваченной фигуры с доски
            const enemyFigure = cellTo.elements.figure.figureImage;
            if (enemyFigure && enemyFigure.parentNode) {
                enemyFigure.parentNode.removeChild(enemyFigure);
            }
            // Убираем фигуру из состояния клетки
            cellTo.elements.figure = null;
        } else {
            console.log('Target cell occupied by friendly figure');
            return;
        }
    }

    if (figure.figureInfo.figureType === 'Rook') {
        figure.hasMoved = true;
    }

    if (figure.figureInfo.figureType === 'King') {
        figure.hasMoved = true;

        let secondaryLine = undefined;

        for (let line of movedElements.secondaryLines) {
            const index = line.indexOf(startId);

            if (index !== -1) {
                secondaryLine = line;
            }
        }

        const kingIndex = secondaryLine.indexOf(startId);
        const kingToIndex = secondaryLine.indexOf(endId);
        let rookIndex = undefined;

        if (Math.abs(kingIndex - kingToIndex) === 2) {
            if (kingIndex > kingToIndex) {
                rookIndex = 0;
            } else {
                rookIndex = 7;
            }

            const cellRook = boardElementsState.cells[secondaryLine[rookIndex]];
            const cellToRook = boardElementsState.cells[secondaryLine[kingToIndex + (kingIndex - kingToIndex > 0 ? 1 : -1)]];
            const rookFigure = cellRook.elements.figure;

            cellToRook.elements.figure = rookFigure;
            cellRook.elements.figure = null;

            cellRook.elements.label.style.visibility = 'visible';
            cellToRook.elements.label.style.visibility = 'hidden';

            rookFigure.figureImage.setAttribute('x', cellToRook.center.x - FIGURE_SIZE / 2);
            rookFigure.figureImage.setAttribute('y', cellToRook.center.y - FIGURE_SIZE / 2);
        }
    }

    // Переносим фигуру: перемещаем её из исходной ячейки в целевую
    cellTo.elements.figure = figure;
    cellFrom.elements.figure = null;

    cellFrom.elements.label.style.visibility = 'visible';
    cellTo.elements.label.style.visibility = 'hidden';

    

    // Позиционируем фигуру в новой ячейке (центруем изображение)
    cellTo.elements.figure.figureImage.setAttribute('x', cellTo.center.x - FIGURE_SIZE / 2);
    cellTo.elements.figure.figureImage.setAttribute('y', cellTo.center.y - FIGURE_SIZE / 2);

    replaceCurrentTurnColor();
    console.log('Moved successfully');
}

function localMove(startId, endId) {
    // Получаем все возможные ходы для фигуры на стартовой клетке
    const possibleMoves = getPossibleMoves(startId);

    const correctMoves = filterCorrectMoves(startId, possibleMoves);



    // Проверяем, что целевая клетка есть в списке возможных ходов
    if (!correctMoves.includes(endId)) {
        console.log(`Invalid move: ${endId} is not a valid move from ${startId}`);
        return;
    }

    //if (isKingInCheck(gameConfig.controlledColor)) {
    //    const kingCellId = findKing(gameConfig.controlledColor);

    //    highlightKingRedColor(kingCellId);
    //    return;
    //}

    // Выполняем перемещение
    moveFigure(startId, endId);

    // Отправляем ход на сервер
    gameHub.invoke("HandleMove", {
        startCellId: startId,
        endCellId: endId,
        gameId: gameId,
        userId: gameConfig.userId
    });
}

function filterCorrectMoves(startId, possibleMoves) {
    const controlledColor = gameConfig.controlledColor;
    const correctIds = [];

    possibleMoves.forEach(currId =>
    {
        let temp = boardElementsState.cells[startId];
        boardElementsState.cells[startId] = boardElementsState.cells[currId];
        boardElementsState.cells[currId] = temp;

        let result = isKingInCheck(controlledColor);

        if (!result) {
            correctIds.push(currId);
        }

        temp = boardElementsState.cells[startId];
        boardElementsState.cells[startId] = boardElementsState.cells[currId];
        boardElementsState.cells[currId] = temp;
    });

    return correctIds;
}


function isKingInCheck(kingColor) {
    const kingCellId = findKing(kingColor);
    if (!kingCellId) return false;

    // Проверяем, может ли любая вражеская фигура атаковать короля
    for (const cellId in boardElementsState.cells) {
        const cell = boardElementsState.cells[cellId];

        if (cell.elements.figure &&
            cell.elements.figure.figureInfo.figureColor !== kingColor) {

            const possibleMoves = getPossibleMoves(cellId);
            if (possibleMoves.includes(kingCellId)) {
                return true;
            }
        }
    }
    return false;
}


function findKing(color) {
    for (const cellId in boardElementsState.cells) {
        const cell = boardElementsState.cells[cellId];
        if (cell.elements.figure &&
            cell.elements.figure.figureInfo.figureType === 'King' &&
            cell.elements.figure.figureInfo.figureColor === color) {
            return cellId;
        }
    }
    return null;
}


function replaceCurrentTurnColor() {
    const colors = ["White", "Black", "Red"];
    const currentIndex = colors.indexOf(gameConfig.currentTurnColor);
    gameConfig.currentTurnColor = colors[(currentIndex + 1) % colors.length];
}
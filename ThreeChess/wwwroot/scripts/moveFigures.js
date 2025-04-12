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

    // Переносим фигуру: перемещаем её из исходной ячейки в целевую
    cellTo.elements.figure = figure;
    cellFrom.elements.figure = null;

    cellFrom.elements.label.style.visibility = 'visible';
    cellTo.elements.label.style.visibility = 'hidden';

    // Позиционируем фигуру в новой ячейке (центруем изображение)
    cellTo.elements.figure.figureImage.setAttribute('x', cellTo.center.x - 25);
    cellTo.elements.figure.figureImage.setAttribute('y', cellTo.center.y - 25);

    console.log('Moved successfully');
}

function localMove(startId, endId) {
    const cellTo = boardElementsState.cells[endId];

    // Проверяем, что целевая клетка помечена как возможный ход (подсвечена)
    if (!(cellTo.elements.path.classList.contains('cell-highlighted') ||
        cellTo.elements.path.classList.contains('cell-capture-highlight'))) {
        console.log('Invalid move: target cell is not highlighted as valid.');
        return;
    }


    moveFigure(startId, endId);
    hubConnection.invoke("HandleMove", startId, endId);
}
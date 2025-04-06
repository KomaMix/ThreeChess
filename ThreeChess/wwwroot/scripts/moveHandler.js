function highlightDiagonalMoves(cellId, figureType) {
    isKing = false;

    if (figureType === 'King') {
        isKing = true;
    }

    // Получаем массив диагоналей из глобального состояния
    const diagonals = movedElements.diagonals; // предполагается, что это массив массивов строк

    // Фильтруем только те диагонали, в которых присутствует текущая ячейка
    const relevantDiagonals = diagonals.filter(diagonal => diagonal.includes(cellId));

    console.log("relevantDiagonals:", relevantDiagonals);

    relevantDiagonals.forEach(diagonal => {
        const currentIndex = diagonal.indexOf(cellId);

        // Проходим в сторону начала диагонали
        for (let i = currentIndex - 1; i >= 0; i--) {
            const targetCellId = diagonal[i];
            const cell = boardElementsState.cells[targetCellId];
            if (!cell) break;
            // Если в ячейке есть фигура, выделяем её (как потенциальное взятие) и прекращаем поиск в этом направлении
            if (cell.elements.figure) {
                cell.elements.path.classList.add('cell-highlighted');
                console.log(`added light ${targetCellId}`);
                break;
            } else {
                cell.elements.path.classList.add('cell-highlighted');
                console.log(`added light ${targetCellId}`);
            }

            if (isKing) {
                break;
            }
        }

        // Проходим в сторону конца диагонали
        for (let i = currentIndex + 1; i < diagonal.length; i++) {
            const targetCellId = diagonal[i];
            const cell = boardElementsState.cells[targetCellId];
            if (!cell) break;
            if (cell.elements.figure) {
                cell.elements.path.classList.add('cell-highlighted');
                break;
            } else {
                cell.elements.path.classList.add('cell-highlighted');
            }

            if (isKing) {
                break;
            }
        }
    });
}

function highlightMainLinesMoves(cellId, figureType) {
    isKing = false;

    if (figureType === 'King') {
        isKing = true;
    }

    // Получаем массив диагоналей из глобального состояния
    const mainLines = movedElements.mainLines; // предполагается, что это массив массивов строк

    // Фильтруем только те диагонали, в которых присутствует текущая ячейка
    const relevantLines = mainLines.filter(line => line.includes(cellId));

    console.log("relevantLines:", relevantLines);

    relevantLines.forEach(line => {
        const currentIndex = line.indexOf(cellId);

        // Проходим в сторону начала диагонали
        for (let i = currentIndex - 1; i >= 0; i--) {
            const targetCellId = line[i];
            const cell = boardElementsState.cells[targetCellId];
            if (!cell) break;
            // Если в ячейке есть фигура, выделяем её (как потенциальное взятие) и прекращаем поиск в этом направлении
            if (cell.elements.figure) {
                cell.elements.path.classList.add('cell-highlighted');
                console.log(`added light ${targetCellId}`);
                break;
            } else {
                cell.elements.path.classList.add('cell-highlighted');
                console.log(`added light ${targetCellId}`);
            }

            if (isKing) {
                break;
            }
        }

        // Проходим в сторону конца диагонали
        for (let i = currentIndex + 1; i < line.length; i++) {
            const targetCellId = line[i];
            const cell = boardElementsState.cells[targetCellId];
            if (!cell) break;
            if (cell.elements.figure) {
                cell.elements.path.classList.add('cell-highlighted');
                break;
            } else {
                cell.elements.path.classList.add('cell-highlighted');
            }

            if (isKing) {
                break;
            }
        }
    });
}

function highlightSecondaryLinesMoves(cellId, figureType) {
    isKing = false;

    if (figureType === 'King') {
        isKing = true;
    }

    // Получаем массив диагоналей из глобального состояния
    const secondaryLines = movedElements.secondaryLines; // предполагается, что это массив массивов строк

    // Фильтруем только те диагонали, в которых присутствует текущая ячейка
    const relevantLines = secondaryLines.filter(line => line.includes(cellId));

    console.log("relevantLines:", relevantLines);

    relevantLines.forEach(line => {
        const currentIndex = line.indexOf(cellId);

        // Проходим в сторону начала диагонали
        for (let i = currentIndex - 1; i >= 0; i--) {
            const targetCellId = line[i];
            const cell = boardElementsState.cells[targetCellId];
            if (!cell) break;
            // Если в ячейке есть фигура, выделяем её (как потенциальное взятие) и прекращаем поиск в этом направлении
            if (cell.elements.figure) {
                cell.elements.path.classList.add('cell-highlighted');
                console.log(`added light ${targetCellId}`);
                break;
            } else {
                cell.elements.path.classList.add('cell-highlighted');
                console.log(`added light ${targetCellId}`);
            }

            if (isKing) {
                break;
            }
        }

        // Проходим в сторону конца диагонали
        for (let i = currentIndex + 1; i < line.length; i++) {
            const targetCellId = line[i];
            const cell = boardElementsState.cells[targetCellId];
            if (!cell) break;
            if (cell.elements.figure) {
                cell.elements.path.classList.add('cell-highlighted');
                break;
            } else {
                cell.elements.path.classList.add('cell-highlighted');
            }

            if (isKing) {
                break;
            }
        }
    });
}

function highlightKnightMoves(cellId) {
    // Массив комбинаций: сначала primary = mainLines, secondary = secondaryLines,
    // затем наоборот: primary = secondaryLines, secondary = mainLines.
    const lineCombos = [
        { primary: movedElements.mainLines, secondary: movedElements.secondaryLines },
        { primary: movedElements.secondaryLines, secondary: movedElements.mainLines }
    ];

    // Функция-помощник: подсвечивает ячейку, если она существует на доске.
    function highlightCell(targetCellId, note = '') {
        const cell = boardElementsState.cells[targetCellId];
        if (cell) {
            cell.elements.path.classList.add('cell-highlighted');
            console.log(`Подсвечена ячейка ${targetCellId} ${note}`);
        }
    }

    // Для каждой комбинации линий
    lineCombos.forEach(combo => {
        const primaryLines = combo.primary;     // линии, по которым будем двигаться первым этапом
        const secondaryLines = combo.secondary; // линии для второго этапа

        // --- Последовательность 1: 1 шаг по primary, затем 2 шага по secondary ---
        // Находим все линии primary, где присутствует исходная ячейка
        primaryLines.forEach(line => {
            const startIndex = line.indexOf(cellId);
            if (startIndex === -1) return; // если текущая линия не содержит исходную ячейку

            // Делаем 1 ход в обе стороны по текущей линии
            const oneStepIndices = [];
            if (startIndex - 1 >= 0) oneStepIndices.push(startIndex - 1);
            if (startIndex + 1 < line.length) oneStepIndices.push(startIndex + 1);

            oneStepIndices.forEach(intermediateIndex => {
                const intermediateCellId = line[intermediateIndex];
                // Для каждой найденной промежуточной ячейки ищем линии secondary, где она присутствует
                secondaryLines.forEach(secLine => {
                    const secIndex = secLine.indexOf(intermediateCellId);
                    if (secIndex === -1) return;

                    // Делаем 2 хода по линии secondary: в обе стороны, если возможно
                    const twoStepIndices = [];
                    if (secIndex - 2 >= 0) twoStepIndices.push(secIndex - 2);
                    if (secIndex + 2 < secLine.length) twoStepIndices.push(secIndex + 2);

                    twoStepIndices.forEach(targetIndex => {
                        const targetCellId = secLine[targetIndex];
                        highlightCell(targetCellId, '(вариант 1+2)');
                    });
                });
            });
        });

        // --- Последовательность 2: 2 шага по primary, затем 1 шаг по secondary ---
        primaryLines.forEach(line => {
            const startIndex = line.indexOf(cellId);
            if (startIndex === -1) return;

            // Делаем 2 последовательных хода по линии primary: проверяем возможность для обоих направлений
            const twoStepPrimaryIndices = [];
            // Влево: убедимся, что есть ячейки на 1 и 2 шаге
            if (startIndex - 2 >= 0) twoStepPrimaryIndices.push(startIndex - 2);
            // Вправо:
            if (startIndex + 2 < line.length) twoStepPrimaryIndices.push(startIndex + 2);

            twoStepPrimaryIndices.forEach(primaryTargetIndex => {
                const primaryTargetCellId = line[primaryTargetIndex];
                // Теперь для полученной ячейки ищем линии secondary, где она встречается
                secondaryLines.forEach(secLine => {
                    const secIndex = secLine.indexOf(primaryTargetCellId);
                    if (secIndex === -1) return;

                    // Делаем 1 ход по линии secondary в обе стороны
                    const oneStepSecIndices = [];
                    if (secIndex - 1 >= 0) oneStepSecIndices.push(secIndex - 1);
                    if (secIndex + 1 < secLine.length) oneStepSecIndices.push(secIndex + 1);

                    oneStepSecIndices.forEach(targetIndex => {
                        const targetCellId = secLine[targetIndex];
                        highlightCell(targetCellId, '(вариант 2+1)');
                    });
                });
            });
        });
    });
}


function clearHighlightedCells() {
    document.querySelectorAll('path').forEach(path => {
        path.classList.remove('cell-highlighted');
    });
}
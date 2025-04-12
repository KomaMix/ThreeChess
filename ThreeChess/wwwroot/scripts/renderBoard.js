function renderBoard(cells, figuresMap) {
    const cellsLayer = document.getElementById('cells-layer');
    const labelsLayer = document.getElementById('labels-layer');
    const figuresLayer = document.getElementById('figures-layer');

    // Очищаем все слои
    cellsLayer.innerHTML = '';
    labelsLayer.innerHTML = '';
    figuresLayer.innerHTML = '';


    cells.forEach(cell => {
        if (!validatePolygon(cell.polygon.points)) return;

        // Масштабирование точек
        const scaledPoints = cell.polygon.points.map(p => ({
            x: p.x * VIEWBOX_SIZE / 2 + CENTER_OFFSET,
            y: p.y * VIEWBOX_SIZE / 2 + CENTER_OFFSET
        }));

        // Создание пути
        const path = document.createElementNS('http://www.w3.org/2000/svg', 'path');
        path.setAttribute('d', generatePathData(scaledPoints));
        path.setAttribute('data-cell-id', cell.id);

        if (cell.isWhite) {
            path.classList.add('cell-white');
        } else {
            path.classList.add('cell-black');
        }

        path.addEventListener('click', async () => {
            try {
                console.log('Clicked cell:', cell.id, 'Last clicked:', last_click_id);

                // Если это первый клик
                if (last_click_id === undefined) {
                    firstClickHandler(cell);
                }
                // Если это второй клик
                else {
                    secondClickHandler(cell);
                }
            } catch (err) {
                console.error("Error sending cell click:", err);
            }
        });


        const centroid = {
            x: cell.center.x * VIEWBOX_SIZE / 2 + CENTER_OFFSET,
            y: cell.center.y * VIEWBOX_SIZE / 2 + CENTER_OFFSET,
        };

        const figureInfo = figuresMap[cell.id.toString()];

        let figureImage = undefined;
        if (figureInfo) {
            figureImage = document.createElementNS('http://www.w3.org/2000/svg', 'image');
            figureImage.setAttributeNS('http://www.w3.org/1999/xlink', 'xlink:href', figureInfo.path);
            figureImage.setAttribute('x', centroid.x - 25);
            figureImage.setAttribute('y', centroid.y - 25);
            figureImage.setAttribute('width', '50');
            figureImage.setAttribute('height', '50');
            figureImage.classList.add('cell-figure');
        }



        let labelText;
        labelText = document.createElementNS('http://www.w3.org/2000/svg', 'text');
        labelText.setAttribute('x', centroid.x);
        labelText.setAttribute('y', centroid.y);
        labelText.textContent = cell.id;
        labelText.classList.add('cell-label');

        if (figureInfo) {
            labelText.style.visibility = 'hidden';
        }


        boardElementsState.cells[cell.id] = {
            id: cell.id,
            center: { x: centroid.x, y: centroid.y },
            elements: {
                path: path,
                figure: figureInfo ? {
                    figureImage: figureImage || null,
                    figureInfo: figureInfo
                } : null,
                label: labelText
            }
        };


        cellsLayer.appendChild(path);
        if (figureInfo) {
            figuresLayer.appendChild(figureImage);
        }

        labelsLayer.appendChild(labelText);

    });

    console.log("State:", boardElementsState);
}


function generatePathData(points) {
    let path = `M${points[0].x},${points[0].y}`;
    for (let i = 1; i < points.length; i++) {
        path += ` L${points[i].x},${points[i].y}`;
    }
    return `${path} Z`;
}
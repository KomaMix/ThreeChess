function validatePolygon(points) {
    if (points.length < 3) return false;
    const first = points[0];
    const last = points[points.length - 1];
    if (first.x !== last.x || first.y !== last.y) {
        points.push({ ...first });
    }
    return true;
}
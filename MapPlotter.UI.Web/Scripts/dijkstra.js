function Dijkstra(graph) {
    this.nodes = graph.Vertices;
    this.edges = graph.Edges;
    this.settledNodes = {};
    this.unSettledNodes = {};
    this.distance = {};
    this.predecessors = {};

    this.getMinimum = function(vertices) {
        var minimum = null;

        for (var vertexKey in vertices) {
            if (vertices.hasOwnProperty(vertexKey)) {
                var vertex = vertices[vertexKey];
                if (minimum == null) {
                    minimum = vertex;
                } else {
                    if (this.getShortestDistance(vertex) < this.getShortestDistance(minimum)) {
                        minimum = vertex;
                    }
                }
            }
        }
        return minimum;
    }

    this.getShortestDistance = function(destination) {
        var d = this.distance[this.getHashKey(destination)];
        if (d == null) {
            return 9999999999;
        } else {
            return d;
        }
    }

    this.Execute = function(source) {
        this.distance[this.getHashKey(source)] = 0;

        this.unSettledNodes[this.getHashKey(source)] = source;
        while (this.getLength(this.unSettledNodes) > 0) {
            var node = this.getMinimum(this.unSettledNodes);
            this.settledNodes[this.getHashKey(node)] = node;
            delete this.unSettledNodes[this.getHashKey(node)];
            this.findMinimalDistances(node);
        }
    }


    this.findMinimalDistances = function(node) {
        var adjacentNodes = [];

        for (var n = 0; n < node.Neighbours.length; n++) {
            var neighbour = node.Neighbours[n];
            adjacentNodes.push(neighbour);
        }

        for (var t = 0; t < adjacentNodes.length; t++) {
            var target = adjacentNodes[t];
            if (this.getShortestDistance(target.Vertice) > this.getShortestDistance(node) + target.Distance) {
                this.distance[this.getHashKey(target.Vertice)] = this.getShortestDistance(node) + target.Distance;
                this.predecessors[this.getHashKey(target.Vertice)] = {
                    Vertice: node,
                    Distance: target.Distance
                }
                this.unSettledNodes[this.getHashKey(target.Vertice)] = target.Vertice;
            }
        }
    }

    this.getHashKey = function(obj)
    {
        if (obj.Point != null) {
            return obj.Point.Lat + "_" + obj.Point.Lng;
        }

        return "";
    }
    this.getLength = function(obj) {
        var count = 0;
        for (var prop in obj) {
            if (obj.hasOwnProperty(prop))
                ++count;
        }

        return count;
    }

    this.isSettled = function(vertex) {
        return this.settledNodes[this.getHashKey(vertex)] != null;
    }
    

    this.getPath = function(target) {
        var path = [];
        var distance = 0;
        var step = {
            Vertice: target,
            Distance: 0
        }

        // check if a path exists
        if (this.predecessors[this.getHashKey(step.Vertice)] == null) {
            return null;
        }
        path.push(step);
        while (this.predecessors[this.getHashKey(step.Vertice)] != null) {
            step = this.predecessors[this.getHashKey(step.Vertice)];
            path.push(step);
            distance += step.Distance;
        }
        // Put it into the correct order
        path = path.reverse(path);
        return {
            Distance: distance,
            Path: path
        }
    }
}